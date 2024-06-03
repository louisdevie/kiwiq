using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    /// the format described by <paramref name="infos"/>.
    /// </summary>
    IFieldMapper SpecializeFor(Type fieldType, IColumnInfos infos);

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
}

}
