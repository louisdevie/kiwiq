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
    private IEnumerable<string> metaColumns;

    /// <inheritdoc cref="IFieldMapper.GetValue"/>
    public abstract T GetValue(IDataRecord record, int offset);
    
    bool IFieldMapper.CanHandle(Type fieldType) => fieldType == typeof(T);

    IFieldMapper IFieldMapper.SpecializeFor(Type fieldType, IColumnInfos infos) => this;

    object? IFieldMapper.GetValue(IDataRecord record, int offset) => this.GetValue(record, offset);

    IEnumerable<string> IFieldMapper.MetaColumns => Maybe.Nothing<string>();
}

}
