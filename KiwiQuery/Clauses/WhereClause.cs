using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal class WhereClause : Clause
    {
        private Predicate predicate;

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