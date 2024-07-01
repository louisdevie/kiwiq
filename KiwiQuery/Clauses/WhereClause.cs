using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// Represents a WHERE statement.
    /// </summary>
    internal class WhereClause : Clause
    {
        private readonly Predicate predicate;

        /// <summary>
        /// Creates a new WHERE statement.
        /// </summary>
        /// <param name="predicate">The predicate used to filter the query.</param>
        public WhereClause(Predicate predicate)
        {
            this.predicate = predicate;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendWhereKeyword();
            this.predicate.WriteTo(builder);
        }
    }
}