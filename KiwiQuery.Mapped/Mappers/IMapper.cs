using System.Data.Common;

namespace KiwiQuery.Mapped.Mappers
{
    public interface IMapper
    {
        object RowToObject(DbDataReader reader);
    }
    
    public interface IMapper<T> : IMapper
    where T : notnull
    {
        new T RowToObject(DbDataReader reader);
    }
}