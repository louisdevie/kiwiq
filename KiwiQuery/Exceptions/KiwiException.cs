using System;

namespace KiwiQuery.Exceptions
{

/// <summary>
/// The base class for all exceptions thrown by the KiwiQuery library (including extensions like KiwiQuery Mapped).
/// </summary>
public abstract class KiwiException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KiwiException"/> class with a specified error message.
    /// </summary>
    protected KiwiException(string message) : base(message) { }
}

}
