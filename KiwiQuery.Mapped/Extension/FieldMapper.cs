using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// A generic implementation of <see cref="IFieldMapper"/>.
/// </summary>
/// <typeparam name="T">The type of values handled.</typeparam>
public abstract class FieldMapper<T> : IFieldMapper
{
    /// <inheritdoc cref="IFieldMapper.ReadValue"/>
    protected abstract T ReadValue(IDataRecord record, int offset);

    /// <inheritdoc cref="IFieldMapper.WriteValue"/>
    protected abstract object? WriteValue(T value);
    
    bool IFieldMapper.CanHandle(Type fieldType) => fieldType == typeof(T);

    IFieldMapper IFieldMapper.SpecializeFor(Type fieldType, IColumnInfo info) => this;

    object? IFieldMapper.ReadValue(IDataRecord record, int offset) => this.ReadValue(record, offset);
    
    IEnumerable<object?> IFieldMapper.WriteValue(object? value) => Maybe.Just(this.WriteValue((T)value!));

    IEnumerable<string> IFieldMapper.MetaColumns => Maybe.Nothing<string>();
    
    /// <inheritdoc />
    public virtual bool CanMapIntegerKey => false;

    /// <inheritdoc />
    public virtual object? MapIntegerKey(int key) => null;
}

}
