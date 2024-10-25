using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Thrown when an attribute path cannot be resolved to a database column.
/// </summary>
public class InvalidAttributePathException : KiwiException
{
    internal InvalidAttributePathException(string error, string pathFromRoot) 
    : base($"{error} (following '{pathFromRoot}')")
    {
    }

    internal static InvalidAttributePathException FieldNotFound(string fieldName, string? typeName, string pathFromRoot)
        => new InvalidAttributePathException($"No mapped field named '{fieldName}' found in entity {typeName ?? "unknown"}", pathFromRoot);

    internal static InvalidAttributePathException FieldHasNoMainColumn(string fieldName, string? typeName, string pathFromRoot)
        => new InvalidAttributePathException($"Field '{fieldName}' in entity {typeName ?? "unknown"} has no main column mapped to it", pathFromRoot);

    internal static InvalidAttributePathException TriedReadingFromValue(string fieldName, string? typeName, string pathFromRoot)
        => new InvalidAttributePathException($"Trying to read field '{fieldName}' from a value of type {typeName ?? "unknown"}", pathFromRoot);
}

}