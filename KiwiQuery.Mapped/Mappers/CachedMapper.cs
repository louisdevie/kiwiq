using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers
{

internal abstract class CachedMapper : IMapper
{
    private static readonly ConcurrentDictionary<Type, CachedMapper> Instances = new ConcurrentDictionary<Type, CachedMapper>();

    public static CachedMapper<T> For<T>() where T : notnull
    {
        return (CachedMapper<T>)Instances.GetOrAdd(typeof(T), (type) => new CachedMapper<T>());
    }

    internal static void InvalidateAll()
    {
        Instances.Clear();
    }

    private readonly string tableName;
    private readonly ConstructorInfo constructor;
    private readonly List<MappedField> fields;
    private readonly List<string> allColumns;

    protected CachedMapper(Type type)
    {
        this.constructor = type.GetConstructor(
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                               null,
                               Array.Empty<Type>(),
                               null
                           )
                           ?? throw new NoDefaultConstructorException(type);

        this.tableName = GetTableName(type);

        this.fields = type.GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly
            )
            .Where(NotIgnored)
            .Select(MapField)
            .ToList();

        this.allColumns = new List<string>();
        foreach (var field in this.fields)
        {
            this.allColumns.AddRange(field.SetUpColumns(this.allColumns.Count));
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
        bool shouldBeInserted;
        if (GetColumnAttribute(field) is { } columnAttribute)
        {
            columnInfos = columnAttribute;
            columnName = columnAttribute.Name ?? field.Name;
            shouldBeInserted = columnAttribute.Inserted;
        }
        else
        {
            columnInfos = new NoColumnInfos();
            columnName = field.Name;
            shouldBeInserted = true;
        }

        return new MappedField(field, columnName, shouldBeInserted, GetMapper(field, columnInfos));
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

    public IEnumerable<string> Projection => this.allColumns;

    public object RowToObject(IDataRecord record)
    {
        object instance = this.constructor.Invoke(Array.Empty<object>());
        foreach (MappedField field in this.fields)
        {
            field.ReadInto(instance, record);
        }
        return instance;
    }

    public IEnumerable<(string, object?)> ObjectToValues(object obj, IColumnFilter filter)
    {
        var values = new List<(string, object?)>();
        foreach (MappedField field in this.fields)
        {
            if (filter.FilterOut(field)) continue;
            
            int offset = field.Offset;
            foreach (object? mappedValue in field.WriteFrom(obj))
            {
                values.Add((this.allColumns[offset], mappedValue));
                offset++;
            }
        }
        return values;
    }
}

internal class CachedMapper<T> : CachedMapper, IMapper<T> where T : notnull
{
    public CachedMapper() : base(typeof(T)) { }

    public new T RowToObject(IDataRecord record)
    {
        return (T)base.RowToObject(record);
    }

    public IEnumerable<(string, object?)> ObjectToValues(T obj, IColumnFilter filter)
    {
        return base.ObjectToValues(obj, filter);
    }
}

}
