using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class MappedField
{
    private readonly FieldInfo field;
    private readonly string column;
    private readonly bool inserted;
    private readonly IFieldMapper mapper;
    private int offset;

    public MappedField(FieldInfo field, string column, bool inserted, IFieldMapper mapper)
    {
        this.field = field;
        this.column = column;
        this.inserted = inserted;
        this.mapper = mapper;
        this.offset = -1;
    }

    public bool Inserted => this.inserted;

    public int Offset => this.offset;

    public string Column => this.column;

    public IEnumerable<string> SetUpColumns(int offset)
    {
        this.offset = offset;
        return Maybe.Just(this.Column).Concat(this.mapper.MetaColumns);
    }

    public void ReadInto(object instance, IDataRecord record)
    {
        this.field.SetValue(instance, this.mapper.ReadValue(record, this.offset));
    }

    public IEnumerable<object?> WriteFrom(object instance)
    {
        return this.mapper.WriteValue(this.field.GetValue(instance));
    }
}

}
