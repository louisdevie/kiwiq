using System;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// Converts a value before it is stored, and converts it back when it's read. 
/// </summary>
public interface IFieldConverter
{
    /// <summary>
    /// Check if this can convert values of type <paramref name="fieldType"/>.
    /// </summary>
    bool CanHandle(Type fieldType);

    /// <summary>
    /// The type this converter will output.
    /// </summary>
    Type StorageType { get; }
    
    /// <summary>
    /// Returns a mapper that is configured specifically to convert values of type <paramref name="fieldType"/> using
    /// the format described by <paramref name="info"/>.
    /// </summary>
    IFieldConverter SpecializeFor(Type fieldType, IColumnInfo info);

    /// <summary>
    /// Converts a value from the model type to the storage type.
    /// </summary>
    object? ToStoredValue(object? value);

    /// <summary>
    /// Converts a value from the storage type to the model type.
    /// </summary>
    object? FromStoredValue(object? value);
}

}
