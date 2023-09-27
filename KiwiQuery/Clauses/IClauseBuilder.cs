using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal interface IClauseBuilder
    {
        void WriteClauseTo(QueryBuilder qb);
    }
}
