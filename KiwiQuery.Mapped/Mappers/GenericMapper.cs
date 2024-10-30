using System.Collections.Generic;
using System.Data;
using System.Reflection;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Mappers.Fields;
using KiwiQuery.Mapped.Mappers.Filters;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;

namespace KiwiQuery.Mapped.Mappers
{

internal class GenericMapper : IMapper
{
    private readonly Table firstTable;
    private readonly List<IJoin> joins;
    private readonly bool hasFreeJoins;
    private readonly ConstructorInfo constructor;
    private readonly List<MappedField> fields;
    private readonly List<Column> allColumns;
    private readonly IPrimaryKey primaryKey;

    internal GenericMapper(
        Table firstTable, List<IJoin> joins, bool hasFreeJoins, ConstructorInfo constructor, List<MappedField> fields,
        IPrimaryKey primaryKey
    )
    {
        this.firstTable = firstTable;
        this.joins = joins;
        this.hasFreeJoins = hasFreeJoins;
        this.constructor = constructor;
        this.fields = fields;
        this.primaryKey = primaryKey;

        this.allColumns = new List<Column>();
        foreach (MappedField field in this.fields)
        {
            this.allColumns.AddRange(field.SetUpColumns(this.allColumns.Count));
        }
    }

    public Table FirstTable => this.firstTable;

    public IEnumerable<IJoin> Joins => this.joins;

    public bool HasFreeJoins => this.hasFreeJoins;

    public IEnumerable<Column> Projection => this.allColumns;

    public object RowToObject(IDataRecord record, Schema schema)
    {
        var arguments = new object?[this.constructor.GetParameters().Length];
        foreach (MappedField field in this.fields)
        {
            if (field.ConstructorArgumentPosition != -1)
            {
                arguments[field.ConstructorArgumentPosition] = field.ReadArgument(record, schema);
            }
        }
        object instance = this.constructor.Invoke(arguments);
        foreach (MappedField field in this.fields)
        {
            if (field.ConstructorArgumentPosition == -1)
            {
                field.ReadInto(instance, record, schema);
            }
        }
        return instance;
    }

    public IEnumerable<(string, object?)> ObjectToValues(object obj, IColumnFilter filter)
    {
        var values = new List<(string, object?)>();
        foreach (MappedField field in this.fields)
        {
            if (!field.IsWriteable || !filter.Filter(field)) continue;

            int offset = field.Offset;
            foreach (object? mappedValue in field.WriteFrom(obj))
            {
                values.Add((this.allColumns[offset].Name, mappedValue));
                offset++;
            }
        }
        return values;
    }

    public IPrimaryKey PrimaryKey => this.primaryKey;

    public Column Attribute(string path) => this.ResolveAttributePath(path, path);

    public Column ResolveAttributePath(string path, string pathFromRoot)
    {
        string[] segments = path.Split('.', 2);
        string localSegment = segments[0];
        MappedField? localField = this.fields.Find(field => field.Name == localSegment);
        if (localField == null)
        {
            throw InvalidAttributePathException.FieldNotFound(localSegment, this.constructor.DeclaringType?.Name, pathFromRoot);
        }

        if (segments.Length == 2)
        {
            return localField.ResolveAttributePath(segments[1], pathFromRoot);
        }
        else
        {
            string? column = localField.Column;
            if (column == null)
            {
                throw InvalidAttributePathException.FieldHasNoMainColumn(localSegment, this.constructor.DeclaringType?.Name, pathFromRoot);
            }
            return this.firstTable.Column(column);
        }
    }
}

internal class GenericMapper<T> : GenericMapper, IMapper<T>
where T : notnull
{
    public GenericMapper(
        Table firstTable, List<IJoin> joins, bool hasFreeJoins, ConstructorInfo constructor, List<MappedField> fields,
        IPrimaryKey primaryKey
    ) : base(firstTable, joins, hasFreeJoins, constructor, fields, primaryKey) { }

    public new T RowToObject(IDataRecord record, Schema schema)
    {
        return (T)base.RowToObject(record, schema);
    }

    public IEnumerable<(string, object?)> ObjectToValues(T obj, IColumnFilter filter)
    {
        return base.ObjectToValues(obj, filter);
    }
}

}
