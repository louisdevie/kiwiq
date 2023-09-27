using System.Text.RegularExpressions;
using KiwiQuery.Expressions;
using KiwiQuery.Sql;

namespace KiwiQuery.Predicates
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
            this.lhs.WriteTo(builder);
            builder.AppendComparisonOperator(this.op);
            this.rhs.WriteTo(builder);
        }
    }
}
