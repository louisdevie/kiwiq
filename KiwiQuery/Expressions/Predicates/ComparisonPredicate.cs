using KiwiQuery.Sql;

namespace KiwiQuery.Expressions.Predicates
{
    /// <summary>
    /// A comparison of two values.
    /// </summary>
    internal class ComparisonPredicate : Predicate
    {
        private Value lhs;
        private Value rhs;
        private ComparisonOperator op;

        /// <param name="lhs">The operand on the left side of the operator.</param>
        /// <param name="rhs">The operand on the right side of the operator.</param>
        /// <param name="op">The comparison operator.</param>
        public ComparisonPredicate(Value lhs, Value rhs, ComparisonOperator op)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            this.lhs.WriteTo(builder);
            builder.AppendComparisonOperator(this.op);
            this.rhs.WriteTo(builder);
        }
    }
}
