using System;

namespace KiwiQuery.Mapped.Exceptions.Internal
{
    
internal class MapperMustBeSpecializedException : InvalidOperationException
{
    public MapperMustBeSpecializedException() : base("This mapper must be specialized before it can be used.")
    {
    }
}
    
}