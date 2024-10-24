using System;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicates that this field is referenced in a one-to-one relationship. 
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class HasOneAttribute : Attribute
{
    private readonly string? foreignColumn;

    /// <summary>
    /// Indicates that this field is referenced in a one-to-one relationship. 
    /// </summary>
    public HasOneAttribute()
    {
        this.foreignColumn = null;
    }

    /// <summary>
    /// Indicates that this field is referenced in a one-to-one relationship.
    /// </summary>
    /// <param name="foreignColumn">The column from the other entity that references this one.</param>
    public HasOneAttribute(string foreignColumn)
    {
        this.foreignColumn = foreignColumn;
    }

    internal IRelationship ToRelationship()
    {
        return new HasOne(this.foreignColumn);
    }
}

}
