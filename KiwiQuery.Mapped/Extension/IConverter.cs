using System;
using System.Data;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// Describes how to read a certain type of values from database rows.
/// </summary>
public interface IConverter
{
    /// <summary>
    /// Check if this can convert values of type <paramref name="fieldType"/>.
    /// </summary>
    bool CanHandle(Type fieldType);

    /// <summary>
    /// Returns a mapper that is configured specifically to convert values of type <paramref name="fieldType"/> using
    /// the format described by <paramref name="info"/>.
    /// </summary>
    IConverter SpecializeFor(Type fieldType, IColumnInfo info);

    /// <summary>
    /// Extracts the value from a result row at a specified column index.
    /// </summary>
    /// <param name="record">The current row being read.</param>
    /// <param name="ordinal"></param>
    /// <returns></returns>
    object? GetValue(IDataRecord record, int ordinal);

    /// <summary>
    /// Converts a value to be used as a query parameter.
    /// </summary>
    object? ToParameter(object? value);
}

}
