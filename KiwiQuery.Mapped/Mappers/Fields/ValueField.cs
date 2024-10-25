using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class ValueField : MappedField
{
    private readonly FieldInfo field;
    private readonly Column column;
    private readonly IFieldMapper mapper;
    private int offset;

    public ValueField(FieldInfo field, Column column, FieldFlags flags, IFieldMapper mapper, int constructorArgumentPosition) : base(field.Name, flags, constructorArgumentPosition)
    {
        this.field = field;
        this.column = column;
        this.mapper = mapper;
        this.offset = -1;
    }

    public override int Offset => this.offset;

    public override string Column => this.column.Name;

    public override Type FieldType => this.field.FieldType;

    public override IEnumerable<Column> SetUpColumns(int offset)
    {
        this.offset = offset;
        return Maybe.Just(this.column).Concat(this.mapper.MetaColumns.Select(col => this.column.Sibling(col)));
    }

    public override object? ReadArgument(IDataRecord record, Schema schema)
    {
        return this.mapper.ReadValue(record, this.offset);
    }

    public override void ReadInto(object instance, IDataRecord record, Schema schema)
    {
        this.field.SetValue(instance, this.ReadArgument(record, schema));
    }

    public override IEnumerable<object?> WriteFrom(object instance)
    {
        return this.mapper.WriteValue(this.field.GetValue(instance));
    }

    public override void PutKeyInto(object instance, int key)
    {
        if (this.mapper.CanMapIntegerKey) this.field.SetValue(instance, this.mapper.MapIntegerKey(key));
    }

    public override Column ResolveAttributePath(string path, string pathFromRoot)
    {
        throw InvalidAttributePathException.TriedReadingFromValue(path, this.FieldType.Name, pathFromRoot);
    }
}

}
