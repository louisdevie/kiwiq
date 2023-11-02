using KiwiQuery.Sql;

namespace KiwiQuery.Expressions.Predicates
{
    internal class NotExpression : Predicate
    {
        private Predicate rhs;

        public NotExpression(Predicate rhs)
        {
            this.rhs = rhs;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendLogicalOperator(LogicalOperator.Not);
            builder.OpenBracket();
            this.rhs.WriteTo(builder);
            builder.CloseBracket();
        }
    }
}
