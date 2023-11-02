using KiwiQuery.Sql;
using System;

namespace KiwiQuery.Expressions.Predicates
{
    /// <summary>
    /// A sequence of predicates joined by OR or AND operators.
    /// </summary>
    internal class BinaryLogicalExpression : Predicate
    {
        private Predicate[] operands;
        private LogicalOperator op;

        /// <param name="op">The operator to join the predicates with.</param>
        /// <param name="operands">The operands of the expression.</param>
        /// <exception cref="ArgumentException"/>
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
            foreach (Predicate operand in this.operands)
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
