using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// Provides methods for building WHERE statements.
    /// </summary>
    internal class WhereClauseBuilder : IClauseBuilder
    {
        private WhereClause? clause;

        public void WriteClauseTo(QueryBuilder qb) => this.clause?.WriteTo(qb);

        /// <summary>
        /// Add a WHERE statement to the query. This method can only be called once.
        /// </summary>
        /// <param name="predicate">The predicates </param>
        /// <exception cref="InvalidOperationException"></exception>
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
