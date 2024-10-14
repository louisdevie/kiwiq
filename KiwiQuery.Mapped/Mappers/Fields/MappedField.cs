using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal abstract class MappedField
{
    private readonly FieldFlags flags;
    private readonly int constructorArgumentPosition;

    protected MappedField(FieldFlags flags, int constructorArgumentPosition)
    {
        this.flags = flags;
        this.constructorArgumentPosition = constructorArgumentPosition;
    }
    
    /// <summary>
    /// The main column containing the value of the field. It is used for comparison when used as a key and for
    /// selection in <see cref="MappedUpdateQuery{T}.SetOnly" /> and <see cref="MappedUpdateQuery{T}.SetAllExcept" />.
    /// </summary>
    public abstract string? Column { get; }

    public abstract int Offset { get; }

    public bool PrimaryKey => (this.flags & FieldFlags.PrimaryKey) == FieldFlags.PrimaryKey;

    public bool Inserted => (this.flags & FieldFlags.NotInserted) == FieldFlags.None;

    public int ConstructorArgumentPosition => this.constructorArgumentPosition;

    public abstract Type FieldType { get; }

    public abstract IEnumerable<Column> SetUpColumns(int offset);
    
    public abstract object? ReadArgument(IDataRecord record, Schema schema);

    public abstract void ReadInto(object instance, IDataRecord record, Schema schema);

    public abstract IEnumerable<object?> WriteFrom(object instance);

    public abstract void PutKeyInto(object instance, int key);
}

}
