using System.Collections.Generic;
using System.Data.Common;

namespace KiwiQuery.Mapped.Mappers
{
    internal interface IMapper
    {
        object RowToObject(DbDataReader reader);
        
        string TableName { get; }
        
        IEnumerable<string> Columns { get; }
    }
    
    internal interface IMapper<T> : IMapper
    where T : notnull
    {
        new T RowToObject(DbDataReader reader);
        
    }
}