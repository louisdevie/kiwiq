using System;
using System.Collections.Generic;
using System.Data.Common;

namespace KiwiQuery.Mapped.Mappers
{
    internal abstract class CachedMapper : IMapper
    {
        private static readonly Dictionary<Type, CachedMapper> Instances = new Dictionary<Type, CachedMapper>();

        public static CachedMapper<T> For<T>()
        where T : notnull
        {
            CachedMapper<T> mapper;
            if (Instances.TryGetValue(typeof(T), out CachedMapper found))
            {
                mapper = (CachedMapper<T>)found;
            }
            else
            {
                mapper = new CachedMapper<T>();
                Instances.Add(typeof(T), mapper);
            }
            return mapper;
        }

        public object RowToObject(DbDataReader reader)
        {
            return null!;
        }
    }

    internal class CachedMapper<T> : CachedMapper, IMapper<T>
    where T : notnull
    {
        public new T RowToObject(DbDataReader reader)
        {
            return (T)base.RowToObject(reader);
        }
    }
}