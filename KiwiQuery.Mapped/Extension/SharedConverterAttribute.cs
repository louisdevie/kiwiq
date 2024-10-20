using System;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// An attribute for registering shared converters.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SharedConverterAttribute : Attribute
{
    /// <summary>
    /// Indicates that this class should be used as a converter for all mapped queries. <see cref="IFieldConverter"/> must be
    /// implemented.
    /// </summary>
    public SharedConverterAttribute() { }
}

}
