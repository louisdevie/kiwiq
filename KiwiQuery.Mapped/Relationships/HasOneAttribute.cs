using System;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Mapped
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

    internal IRelationship AsRelationship()
    {
        return new HasOne(this.foreignColumn);
    }
}

}
