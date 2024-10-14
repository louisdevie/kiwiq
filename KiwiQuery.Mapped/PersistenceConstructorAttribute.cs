using System;
using System.Collections.Generic;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicates that this constructor should be used when mapping objects from the database.
/// </summary>
/// <remarks>
/// Currently, this attribute does nothing and the parameterless constructor will always be used.
/// </remarks>
[AttributeUsage(AttributeTargets.Constructor)]
// ReSharper disable once UnusedType.Global
public class PersistenceConstructorAttribute : Attribute
{
    
}

}
