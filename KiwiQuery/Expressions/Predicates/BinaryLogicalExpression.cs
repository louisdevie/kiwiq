using System;
using System.Collections.Generic;
using KiwiQuery.Sql;

namespace KiwiQuery.Expressions.Predicates
{
    /// <summary>
    /// A sequence of predicates joined by OR or AND operators.
    /// </summary>
    internal class BinaryLogicalExpression : Predicate
    {
        private readonly List<Predicate> operands;
        private readonly LogicalOperator op;

        /// <param name="op">The operator to join the predicates with.</param>
        /// <param name="operands">The operands of the expression.</param>
        /// <exception cref="ArgumentException"/>
        public BinaryLogicalExpression(LogicalOperator op, Predicate[] operands)
        {
            if (op == LogicalOperator.Not) throw new ArgumentException("NOT is not a binary operator.");
            this.op = op;
            this.operands = Flatten(operands, op);
        }

        private static List<Predicate> Flatten(Predicate[] predicates, LogicalOperator op)
        {
            List<Predicate> result = new List<Predicate>();

            foreach (Predicate predicate in predicates)
            {
                result.AddRange(predicate.RelativeTo(op));
            }

            return result;
        }

        public override IEnumerable<Predicate> RelativeTo(LogicalOperator op)
        {
            return op == this.op ? this.operands : base.RelativeTo(op);
        }

        public override void WriteTo(QueryBuilder builder)
        {
            switch (this.operands.Count)
            {
            case 0:
                switch (this.op)
                {
                case LogicalOperator.And:
                    builder.AppendTruthyConstant();
                    break;

                case LogicalOperator.Or:
                    builder.AppendFalsyConstant();
                    break;

                default:
                    throw new Exception("Should be unreachable, WTH ?");
                }

                break;

            case 1:
                this.operands[1].WriteTo(builder);
                return;

            default:
            {
                bool first = true;
                foreach (Predicate operand in this.operands)
                {
                    if (!first)
                    {
                        builder.AppendLogicalOperator(this.op);
                    }

                    builder.OpenBracket();
                    operand.WriteTo(builder);
                    builder.CloseBracket();

                    first = false;
                }
            }
                ;
                break;
            }
        }
    }
}