using KiwiQuery.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal class WhereClauseBuilder : IClauseBuilder
    {
        private WhereClause? clause;

        public void WriteClauseTo(QueryBuilder qb) => this.clause?.WriteTo(qb);

        public void Where(Predicate predicate)
        {
            if (this.clause is null)
            {
                this.clause = new WhereClause(predicate);
            }
            else
            {
                throw new InvalidOperationException("A where clause already exists in this query.");
            }
        }
    }
}
