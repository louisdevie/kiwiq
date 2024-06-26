using System;
using KiwiQuery.Exceptions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Thrown when the type of primary key (TKey) declared on a <see cref="Table{TKey,TEntity}"/> doesn't match the type of
/// the field (from TEntity).
/// </summary>
public class IncompatibleKeyException : KiwiException
{
    internal IncompatibleKeyException(Type declaredType, Type actualType) : base(
        $"Primary was expected to be of type {declaredType.Name} but the field is actually of type {actualType.Name}."
    ) { }
}

}
