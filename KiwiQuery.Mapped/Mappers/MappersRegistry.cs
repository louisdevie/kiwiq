using System;
using System.Collections.Concurrent;

namespace KiwiQuery.Mapped.Mappers
{

internal class MappersRegistry
{
    private readonly ConcurrentDictionary<Type, IMapper> existingMappers;

    public MappersRegistry()
    {
        this.existingMappers = new ConcurrentDictionary<Type, IMapper>();
    }
}

}