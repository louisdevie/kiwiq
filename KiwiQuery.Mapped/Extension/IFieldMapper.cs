using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// Maps a field to the database.
/// </summary>
public interface IFieldMapper
{
    /// <summary>
    /// Check if this can map values of type <paramref name="fieldType"/>.
    /// </summary>
    bool CanHandle(Type fieldType);

    /// <summary>
    /// Returns a mapper that is configured specifically to convert values of type <paramref name="fieldType"/> using
    /// the format described by <paramref name="info"/>.
    /// </summary>
    IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection);

    /// <summary>
    /// Maps a value from a record.
    /// </summary>
    /// <param name="record">A record from the database.</param>
    /// <param name="offset">The position of the column(s) this field is mapped to.</param>
    object? ReadValue(IDataRecord record, int offset);

    /// <summary>
    /// Maps a value to a parameter.
    /// </summary>
    /// <param name="fieldValue">A value from this field.</param>
    IEnumerable<object?> WriteValue(object? fieldValue);

    /// <summary>
    /// Additional columns that complement the value and are required when reading or writing from the database.
    /// </summary>
    IEnumerable<string> MetaColumns { get; }

    /// <summary>
    /// Indicate if this type of field can be mapped to an integer auto-incremented primary key. If true,
    /// <see cref="MapIntegerKey"/> must be implemented. This property should always return the same value. 
    /// </summary>
    bool CanMapIntegerKey { get; }

    /// <summary>
    /// Maps an integer generated as a primary key. This method may not be implemented if <see cref="CanMapIntegerKey"/>
    /// is always false.
    /// </summary>
    /// <param name="key">The key value.</param>
    object? MapIntegerKey(int key) => null;
}

}
