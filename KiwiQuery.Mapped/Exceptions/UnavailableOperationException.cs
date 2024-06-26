using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when an automatic operation cannot be generated because of the entity used.
/// </summary>
public class UnavailableOperationException : KiwiException
{
    internal UnavailableOperationException(string message) : base(message) { }
}

}
