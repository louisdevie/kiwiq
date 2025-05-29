using System;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicates that this field is referenced in a one-to-many relationship. 
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class HasManyAttribute : Attribute
{
    private readonly string? foreignColumn;

    /// <summary>
    /// Indicates that this field is referenced in a one-to-many relationship. 
    /// </summary>
    public HasManyAttribute()
    {
        this.foreignColumn = null;
    }

    /// <summary>
    /// Indicates that this field is referenced in a one-to-many relationship.
    /// </summary>
    /// <param name="foreignColumn">The column from the other entity that references this one.</param>
    public HasManyAttribute(string foreignColumn)
    {
        this.foreignColumn = foreignColumn;
    }

    internal IRelationship ToRelationship()
    {
        return new HasMany(this.foreignColumn);
    }
}

}
