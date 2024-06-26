using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class ReferenceField : MappedField
{
    private readonly FieldInfo field;
    private readonly string columnName;
    private readonly IRelationship relationship;
    private readonly IMapper nestedMapper;
    private int offset;

    public ReferenceField(
        FieldInfo field, string columnName, FieldFlags flags, IRelationship relationship, IMapper nestedMapper
    ): base(flags)
    {
        this.field = field;
        this.columnName = columnName;
        this.relationship = relationship;
        this.nestedMapper = nestedMapper;
        this.offset = -1;
    }

    public override string? Column => this.relationship.IsReferencing ? this.columnName : null;

    public override int Offset => this.offset;

    public override Type FieldType => this.field.FieldType;

    public override IEnumerable<Column> SetUpColumns(int offset)
    {
        this.offset = offset;
        return this.nestedMapper.Projection;
    }

    public override void ReadInto(object instance, IDataRecord record, Schema schema)
    {
        this.field.SetValue(instance, this.nestedMapper.RowToObject(new OffsetRecord(record, this.offset), schema));
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
