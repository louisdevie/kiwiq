using System;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped
{
    public static class SchemaExtensions
    {
        public static MappedSelectQuery<T> Select<T>(this Schema @this) where T : notnull
        {
            return new MappedSelectQuery<T>(@this.Select(Array.Empty<string>()), CachedMapper.For<T>());
        }
    }
}