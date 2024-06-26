using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when a type fails to be instantiated because it has no parameterless constructor.
/// </summary>
public class NoDefaultConstructorException : KiwiException
{
    internal NoDefaultConstructorException(Type type) : base(
        $"No default constructor found for {type.FullName ?? type.Name}"
    ) { }
}

}
