using System;

namespace KiwiQuery.Mapped.Relationships
{

[AttributeUsage(AttributeTargets.Field)]
public class HasOneAttribute : Attribute
{
    private readonly string? foreignColumn;

    public HasOneAttribute()
    {
        this.foreignColumn = null;
    }

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
