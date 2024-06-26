using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when an item selected by its primary key was not found.
/// </summary>
public class NotFoundException : KiwiException
{
    internal NotFoundException(Type entityType, object key) : base($"No {entityType.Name} found with key {key}.") { }
}

}
