using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers
{

internal abstract class CachedMapper : IMapper
{
    private static readonly Dictionary<Type, CachedMapper> Instances = new Dictionary<Type, CachedMapper>();

    public static CachedMapper<T> For<T>() where T : notnull
    {
        CachedMapper<T> mapper;
        if (Instances.TryGetValue(typeof(T), out CachedMapper found))
        {
            mapper = (CachedMapper<T>)found;
        }
        else
        {
            mapper = new CachedMapper<T>();
            Instances.Add(typeof(T), mapper);
        }

        return mapper;
    }

    private readonly string tableName;
    private readonly ConstructorInfo constructor;
    private readonly List<MappedField> fields;
    private readonly List<string> projection;

    public CachedMapper(Type type)
    {
        this.constructor = type.GetConstructor(
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                               null,
                               Array.Empty<Type>(),
                               null
                           )
                           ?? throw new ArgumentException(
                               $"No default constructor found for in {type.FullName ?? type.Name}"
                           );

        this.tableName = GetTableName(type);

        this.fields = type.GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly
            )
            .Where(NotIgnored)
            .Select(MapField)
            .ToList();

        this.projection = new List<string>();
        foreach (var field in this.fields)
        {
            this.projection.AddRange(field.SetUpProjection(this.projection.Count));
        }
    }

    private static string GetTableName(Type type)
    {
        string name = type.Name;
        foreach (TableAttribute attr in type.GetCustomAttributes<TableAttribute>())
        {
            name = attr.Name;
        }

        return name;
    }

    private static bool NotIgnored(FieldInfo field) => !field.GetCustomAttributes<NotStoredAttribute>().Any();

    private static MappedField MapField(FieldInfo field)
    {
        IColumnInfos columnInfos;
        string columnName;
        if (GetColumnAttribute(field) is { } columnAttribute)
        {
            columnInfos = columnAttribute;
            columnName = columnAttribute.Name ?? field.Name;
        }
        else
        {
            columnInfos = new NoColumnInfos();
            columnName = field.Name;
        }

        return new MappedField(field, columnName, GetMapper(field, columnInfos));
    }

    private static ColumnAttribute? GetColumnAttribute(FieldInfo field)
    {
        ColumnAttribute? columnAttribute = null;
        foreach (ColumnAttribute attr in field.GetCustomAttributes<ColumnAttribute>())
        {
            columnAttribute = attr;
        }

        return columnAttribute;
    }

    private static IFieldMapper GetMapper(FieldInfo field, IColumnInfos infos)
    {
        return SharedMappers.Current.GetMapper(field.FieldType, infos);
    }

    public string TableName => this.tableName;

    public IEnumerable<string> Projection => this.projection;

    public object RowToObject(DbDataReader reader)
    {
        object instance = this.constructor.Invoke(Array.Empty<object>());
        foreach (MappedField field in this.fields)
        {
            field.MapValue(instance, reader);
        }
        return instance;
    }
}

internal class CachedMapper<T> : CachedMapper, IMapper<T> where T : notnull
{
    public CachedMapper() : base(typeof(T)) { }

    public new T RowToObject(DbDataReader reader)
    {
        return (T)base.RowToObject(reader);
    }
}

}
