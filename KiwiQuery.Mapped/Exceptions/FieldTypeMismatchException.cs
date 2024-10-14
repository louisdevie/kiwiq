using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when the type of a field doesn't match the type of the corresponding constructor parameter.
/// </summary>
public class FieldTypeMismatchException : KiwiException
{
    internal FieldTypeMismatchException(string fieldName, Type fieldType, Type parameterType) : base(
        $"The field '{fieldName}' has type '{fieldType.FullName ?? fieldType.Name}', which is not assignable to the" 
        + $"constructor parameter of type '{parameterType.FullName ?? parameterType.Name}'"
    ) { }
}

}
