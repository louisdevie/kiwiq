using KiwiQuery.Expressions;

namespace KiwiQuery.Mapped.Mappers
{

/// <summary>
/// Represents an entity that has been mapped.
/// </summary>
public interface IMappedRoot
{
    /// <summary>
    /// Finds the column mapped to an attribute.
    /// </summary>
    /// <param name="path">The path to the attribute.</param>
    Column Attribute(string path);
}

}