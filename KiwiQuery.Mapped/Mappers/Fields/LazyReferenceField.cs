using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class LazyReferenceField : MappedField
{
    private readonly FieldInfo field;
    private readonly Column column;
    private readonly IFieldMapper mapper;
    private readonly bool isReferencing;
    private readonly Column foreignColumn;
    private readonly IMapper nestedMapper;
    private readonly RefActivator activator;

    private int offset;

    public LazyReferenceField(
        FieldInfo field, Column column, FieldFlags flags, IFieldMapper mapper,
        bool isReferencing, Column foreignColumn, IMapper nestedMapper, RefActivator activator, int constructorArgumentPosition
    ) : base(flags, constructorArgumentPosition)
    {
        this.field = field;
        this.column = column;
        this.mapper = mapper;
        this.isReferencing = isReferencing;
        this.foreignColumn = foreignColumn;
        this.nestedMapper = nestedMapper;
        this.activator = activator;
        this.offset = -1;
    }

    public override string? Column => this.isReferencing ? this.column.Name : null;

    public override int Offset => this.offset;

    public override Type FieldType => this.field.FieldType;

    public override IEnumerable<Column> SetUpColumns(int offset)
    {
        this.offset = offset;
        return Maybe.Just(this.column);
    }

    public override object? ReadArgument(IDataRecord record, Schema schema)
    {
        object? refValue = this.mapper.ReadValue(record, this.offset);
        return this.activator.Activate(this.nestedMapper, this.foreignColumn == refValue!, schema);
    }

    public override void ReadInto(object instance, IDataRecord record, Schema schema)
    {
        this.field.SetValue(instance, this.ReadArgument(record, schema));
    }

    public override IEnumerable<object?> WriteFrom(object instance)
    {
        throw new NotImplementedException();
    }

    public override void PutKeyInto(object instance, int key)
    {
        throw new NotImplementedException();
    }
}

}
