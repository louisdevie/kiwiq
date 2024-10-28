using System;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicates that this constructor should be used when mapping objects from the database.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public class PersistenceConstructorAttribute : Attribute
{
    
}

}
