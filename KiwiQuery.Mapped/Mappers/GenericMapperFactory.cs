using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Mappers.Fields;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped.Mappers
{

internal class GenericMapperFactory
{
    private const string TABLE_ALIAS_BASE = "kiwi";

    private readonly Schema schema;
    private readonly IFieldMapperCollection provider;

    public GenericMapperFactory(Schema schema, IFieldMapperCollection provider)
    {
        this.schema = schema;
        this.provider = provider;
    }

    public GenericMapper<T> MakeMapper<T>()
    where T : notnull
    {
        GenericMapperInit init = this.Init(typeof(T), TABLE_ALIAS_BASE);
        return new GenericMapper<T>(
            init.FirstTable,
            init.Joins,
            init.HasFreeJoins,
            init.Constructor,
            init.Fields,
            init.PrimaryKey
        );
    }

    private GenericMapper MakeMapper(Type type, string tableAlias)
    {
        GenericMapperInit init = this.Init(type, tableAlias);
        return new GenericMapper(
            init.FirstTable,
            init.Joins,
            init.HasFreeJoins,
            init.Constructor,
            init.Fields,
            init.PrimaryKey
        );
    }

    private struct GenericMapperInit
    {
        public readonly Table FirstTable;
        public readonly List<IJoin> Joins;
        public readonly bool HasFreeJoins;
        public readonly ConstructorInfo Constructor;
        public readonly List<MappedField> Fields;
        public IPrimaryKey PrimaryKey;

        public GenericMapperInit(Table firstTable, List<IJoin> freeJoins, ConstructorInfo constructor)
        {
            this.FirstTable = firstTable;
            this.Joins = freeJoins;
            this.HasFreeJoins = freeJoins.Count != 0;
            this.Constructor = constructor;
            this.Fields = new List<MappedField>();
            this.PrimaryKey = new UndefinedPrimaryKey();
        }
    }

    private const BindingFlags CONSTRUCTOR_BINDING_FLAGS
        = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    private const BindingFlags FIELDS_BINDING_FLAGS
        = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    private GenericMapperInit Init(Type type, string tableAlias)
    {
        ConstructorInfo[] constructors = type.GetConstructors(CONSTRUCTOR_BINDING_FLAGS);
        ConstructorInfo? constructor = null;
        if (constructors.Length > 1)
        {
            foreach (ConstructorInfo ctor in constructors)
            {
                if (ctor.GetCustomAttributes<PersistenceConstructorAttribute>().Any())
                {
                    constructor = ctor;
                }
            }
            if (constructor == null)
            {
                foreach (ConstructorInfo ctor in constructors)
                {
                    if (ctor.GetParameters().Length == 0)
                    {
                        constructor = ctor;
                    }
                }
            }
        }
        else if (constructors.Length == 1)
        {
            constructor = constructors[0];
        }

        if (constructor == null)
        {
            throw new NoDefaultConstructorException(type);
        }

        var init = new GenericMapperInit(this.GetTable(type).As(tableAlias), this.GetFreeJoins(type), constructor);

        var referenceFields = new List<(FieldInfo, IRelationship)>();

        foreach (FieldInfo field in type.GetFields(FIELDS_BINDING_FLAGS))
        {
            IRelationship? relationship = null;
            var isPersistent = true;
            foreach (Attribute attr in field.GetCustomAttributes())
            {
                switch (attr)
                {
                case HasOneAttribute hasOne:
                    relationship = hasOne.ToRelationship();
                    break;

                case TransientAttribute _:
                    isPersistent = false;
                    break;
                }
            }

            if (isPersistent)
            {
                if (relationship != null)
                {
                    referenceFields.Add((field, relationship));
                }
                else
                {
                    this.MapValueField(init.FirstTable, field, init);
                }
            }
        }

        var primaryKeyColumns = init.Fields.Where(field => field.PrimaryKey).ToArray();

        switch (primaryKeyColumns.Length)
        {
        case 0:
            break;

        case 1:
            init.PrimaryKey = new SimplePrimaryKey(primaryKeyColumns[0]);
            break;

        default:
            init.PrimaryKey = new CompoundPrimaryKey();
            break;
        }

        for (var i = 0; i < referenceFields.Count; i++)
        {
            (FieldInfo field, IRelationship relationship) = referenceFields[i];
            this.MapReferenceField(init.FirstTable, field, init, relationship, $"{tableAlias}_r{i}");
        }

        return init;
    }

    private Table GetTable(Type type)
    {
        string name = type.Name;
        foreach (TableAttribute attr in type.GetCustomAttributes<TableAttribute>())
        {
            name = attr.Name;
        }

        return this.schema.Table(name);
    }

    private List<IJoin> GetFreeJoins(Type type)
        => type.GetCustomAttributes<JoinAttribute>().Select<JoinAttribute, IJoin>(a => a).ToList();

    private struct MappedFieldInfo
    {
        public IColumnInfo ColumnInfo;
        public string ColumnName;
        public FieldFlags Flags;

        public MappedFieldInfo(FieldInfo fieldInfo)
        {
            this.ColumnInfo = new NoColumnInfo();
            this.ColumnName = fieldInfo.Name;
            this.Flags = FieldFlags.None;
        }

        public void AddColumnInfo(ColumnAttribute columnAttribute)
        {
            this.ColumnInfo = columnAttribute;
            if (columnAttribute.Name != null) this.ColumnName = columnAttribute.Name;
            if (!columnAttribute.Inserted) this.Flags |= FieldFlags.NotInserted;
        }

        public void SetPrimaryKeyFlag()
        {
            this.Flags |= FieldFlags.PrimaryKey;
        }
    }

    private MappedFieldInfo GetMappingInfo(FieldInfo field)
    {
        var info = new MappedFieldInfo(field);

        foreach (Attribute attr in field.GetCustomAttributes())
        {
            switch (attr)
            {
            case ColumnAttribute columnAttribute:
                info.AddColumnInfo(columnAttribute);
                break;

            case KeyAttribute _:
                info.SetPrimaryKeyFlag();
                break;
            }
        }

        return info;
    }

    private static int FindConstructorArgumentPosition(ConstructorInfo constructor, FieldInfo field)
    {
        int position = -1;
        ParameterInfo[] parameters = constructor.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Name == field.Name)
            {
                if (!parameters[i].ParameterType.IsAssignableFrom(field.FieldType))
                {
                    throw new FieldTypeMismatchException(field.Name, field.FieldType, parameters[i].ParameterType);
                }
                else
                {
                    position = i;
                }
            }
        }
        return position;
    }

    // TODO remove table parameter
    private void MapValueField(Table table, FieldInfo field, GenericMapperInit init)
    {
        MappedFieldInfo info = this.GetMappingInfo(field);
        init.Fields.Add(
            new ValueField(
                field,
                table.Column(info.ColumnName),
                info.Flags,
                this.provider.GetMapper(field.FieldType, info.ColumnInfo),
                FindConstructorArgumentPosition(init.Constructor, field)
            )
        );
    }

    // TODO remove table parameter
    private void MapReferenceField(
        Table table, FieldInfo field, GenericMapperInit init, IRelationship relationship, string tableAlias
    )
    {
        MappedFieldInfo info = this.GetMappingInfo(field);
        Type fieldType = field.FieldType;

        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Ref<>))
        {
            Type wrappedType = fieldType.GetGenericArguments()[0];
            GenericMapper nestedMapper = this.MakeMapper(wrappedType, tableAlias);

            Column localColumn = relationship.FindLocalColumn(nestedMapper.FirstTable, init.FirstTable, init.PrimaryKey)
                .As($"{tableAlias}_lazy");

            Type localColumnType = init.Fields.Find(f => f.Column == localColumn.Name)?.FieldType
                                   ?? throw CouldNotInferException.ColumnType(localColumn);

            init.Fields.Add(
                new LazyReferenceField(
                    field,
                    localColumn,
                    info.Flags,
                    this.provider.GetMapper(localColumnType, info.ColumnInfo),
                    relationship.IsReferencing,
                    relationship.FindForeignColumn(nestedMapper.FirstTable, init.FirstTable),
                    nestedMapper,
                    relationship.GetRefActivator(wrappedType),
                    FindConstructorArgumentPosition(init.Constructor, field)
                )
            );
        }
        else
        {
            GenericMapper nestedMapper = this.MakeMapper(fieldType, tableAlias);
            
            Column foreignColumn = relationship.FindForeignColumn(nestedMapper.FirstTable, init.FirstTable);
            int foreignColumnOffset = nestedMapper.Projection.Select((column, offset) => (column, offset)).FirstOrDefault((tuple) =>
                    tuple.column.Table?.Name == nestedMapper.FirstTable.Name && tuple.column.Name == foreignColumn.Name)
                .offset;

            init.Joins.Add(
                new ReferenceJoin(
                    foreignColumn,
                    init.FirstTable.Column(init.PrimaryKey.GetColumnToReference()),
                    nestedMapper.Joins
                )
            );
            init.Fields.Add(
                new ReferenceField(
                    field,
                    info.ColumnName,
                    info.Flags,
                    relationship,
                    nestedMapper,
                    foreignColumnOffset,
                    FindConstructorArgumentPosition(init.Constructor, field)
                )
            );
        }
    }
}

}
