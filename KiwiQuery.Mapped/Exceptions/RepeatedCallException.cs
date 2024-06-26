using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// An exception thrown when a builder method was not expected to be called multiple times on the same object.
/// </summary>
public class RepeatedCallException : KiwiException
{
    private RepeatedCallException(string message) : base(message) { }

    internal static RepeatedCallException BecauseObjectIsAlreadySet()
        => new RepeatedCallException("An object is already set.");

    internal static RepeatedCallException BecauseMultipleInsertAreNotSupported()
        => new RepeatedCallException("Multiple INSERT queries are not yet supported");
}

}
