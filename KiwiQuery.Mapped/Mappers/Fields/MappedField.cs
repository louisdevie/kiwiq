using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class MappedField
{
    private FieldInfo field;
    private readonly string column;
    private int ordinal;
    private IFieldMapper mapper;

    public MappedField(FieldInfo field, string column, IFieldMapper mapper)
    {
        this.field = field;
        this.column = column;
        this.ordinal = -1;
        this.mapper = mapper;
    }

    public IEnumerable<string> SetUpProjection(int offset)
    {
        this.ordinal = offset;
        return Maybe.Just(this.column).Concat(this.mapper.MetaColumns);
    }

    public void MapValue(object instance, DbDataReader reader)
    {
        this.field.SetValue(instance, this.mapper.GetValue(reader, this.ordinal));
    }
}

}
