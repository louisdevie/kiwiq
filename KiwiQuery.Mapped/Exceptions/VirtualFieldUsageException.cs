using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when a virtual field (i.e. a field that is not backed by an actual column like HasOne or HasMany
/// relationships) is used where a value is required, as a key for example.
/// </summary>
public class VirtualFieldUsageException : KiwiException
{
    internal VirtualFieldUsageException() : base("A virtual field cannot be used as a value.") { }
}

}
