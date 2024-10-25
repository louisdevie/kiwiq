using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Filters;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;

namespace KiwiQuery.Mapped.Mappers
{

internal interface IMapper : IMappedRoot
{
    object RowToObject(IDataRecord reader, Schema schema);

    Table FirstTable { get; }

    IEnumerable<Column> Projection { get; }

    IEnumerable<IJoin> Joins { get; }

    bool HasFreeJoins { get; }

    public IEnumerable<(string, object?)> ObjectToValues(object obj, IColumnFilter filter);

    IPrimaryKey PrimaryKey { get; }
    Column ResolveAttributePath(string path, string pathFromRoot);
}

internal interface IMapper<T> : IMapper
where T : notnull
{
    new T RowToObject(IDataRecord record, Schema schema);


    public IEnumerable<(string, object?)> ObjectToValues(T obj, IColumnFilter filter);
}

}
