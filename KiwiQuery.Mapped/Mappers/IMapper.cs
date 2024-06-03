using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Mappers
{
    internal interface IMapper
    {
        object RowToObject(IDataRecord reader);
        
        string TableName { get; }
        
        IEnumerable<string> Projection { get; }

        public IEnumerable<(string, object?)> ObjectToValues(object obj, IColumnFilter filter);
    }
    
    internal interface IMapper<T> : IMapper
    where T : notnull
    {
        new T RowToObject(IDataRecord record);
        

        public IEnumerable<(string, object?)> ObjectToValues(T obj, IColumnFilter filter);
    }
}