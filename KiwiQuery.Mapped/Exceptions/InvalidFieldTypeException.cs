using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when a type of field that cannot be mapped nor converted to the database is encountered.
/// </summary>
public class InvalidFieldTypeException : KiwiException
{
    private readonly Type problematicType;

    internal Type ProblematicType => this.problematicType;

    internal InvalidFieldTypeException(Type problematicType, string details = "") : base(
        $"Fields of type ${problematicType.FullName ?? problematicType.Name} cannot be mapped to the database{details}. "
        + "You can define a converter for it with a [Column(..., Converter = new MyConverter())] attribute or choose "
        + "to ignore it with a [Transient] attribute."
    )
    {
        this.problematicType = problematicType;
    }
}

}
