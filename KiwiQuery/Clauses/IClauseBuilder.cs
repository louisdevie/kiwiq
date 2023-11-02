using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// An object that provides methods for building a clause.
    /// </summary>
    internal interface IClauseBuilder
    {
        void WriteClauseTo(QueryBuilder qb);
    }
}
