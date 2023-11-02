using System.Text.RegularExpressions;
using KiwiQuery.Expressions;
using KiwiQuery.Sql;

namespace KiwiQuery.Expressions.Predicates
{
    internal class ComparisonPredicate : Predicate
    {
        private Value lhs;
        private Value rhs;
        private ComparisonOperator op;

        public ComparisonPredicate(Value lhs, Value rhs, ComparisonOperator op)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            lhs.WriteTo(builder);
            builder.AppendComparisonOperator(op);
            rhs.WriteTo(builder);
        }
    }
}
