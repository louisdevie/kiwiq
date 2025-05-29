using System;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicates that this field is a reference to another entity. 
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class BelongsToAttribute : Attribute
{
    private readonly string? foreignColumn;

    /// <summary>
    /// Indicates that this field is a reference to another entity. 
    /// </summary>
    public BelongsToAttribute()
    {
        this.foreignColumn = null;
    }

    /// <summary>
    /// Indicates that this field is a reference to another entity.
    /// </summary>
    /// <param name="foreignColumn">The column from the other entity that is referenced.</param>
    public BelongsToAttribute(string foreignColumn)
    {
        this.foreignColumn = foreignColumn;
    }

    internal IRelationship ToRelationship()
    {
        return new BelongsTo(this.foreignColumn);
    }
}

}
