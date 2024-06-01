using System;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// An attribute for registering shared mappers.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SharedMapperAttribute : Attribute
{
    /// <summary>
    /// Indicates that this class should be used as a converter for all mapped queries. <see cref="IFieldMapper"/> must
    /// be implemented.
    /// </summary>
    public SharedMapperAttribute() { }
}

}
