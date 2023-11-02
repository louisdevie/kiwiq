using KiwiQuery.Sql;

namespace KiwiQuery.Expressions.Predicates
{
    internal class BinaryLogicalExpression : Predicate
    {
        private Predicate[] operands;
        private LogicalOperator op;

        public BinaryLogicalExpression(LogicalOperator op, Predicate[] operands)
        {
            if (operands.Length < 2) throw new ArgumentException("Logical operators need at least two operands.");
            this.operands = operands;
            if (op == LogicalOperator.Not) throw new ArgumentException("NOT is not a logical binary operator.");
            this.op = op;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            bool first = true;
            foreach (Predicate operand in operands)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendLogicalOperator(this.op);
                }

                builder.OpenBracket();
                operand.WriteTo(builder);
                builder.CloseBracket();
            }
        }
    }
}
