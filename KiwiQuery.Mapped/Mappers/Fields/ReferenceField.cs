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
    private readonly int foreignColumnOffset;
    private int offset;

    public ReferenceField(
        FieldInfo field, string columnName, FieldFlags flags, IRelationship relationship, IMapper nestedMapper,
        int foreignColumnOffset, int constructorArgumentPosition
    ) : base(field.Name, flags, constructorArgumentPosition)
    {
        this.field = field;
        this.columnName = columnName;
        this.relationship = relationship;
        this.nestedMapper = nestedMapper;
        this.foreignColumnOffset = foreignColumnOffset;
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

    public override object? ReadArgument(IDataRecord record, Schema schema)
    {
        var offsetRecord = new OffsetRecord(record, this.offset);
        return offsetRecord.IsDBNull(this.foreignColumnOffset) ? null : this.nestedMapper.RowToObject(offsetRecord, schema);
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

    public override Column ResolveAttributePath(string path, string pathFromRoot)
    {
        return this.nestedMapper.ResolveAttributePath(path, pathFromRoot);
    }
}

}
