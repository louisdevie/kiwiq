using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A binary arithmetic operations between two values.
    /// </summary>
    internal class ArithmeticExpression : Value
    {
        private Value lhs;
        private Value rhs;
        private ArithmeticOperator op;

        /// <summary>
        /// Creates a binary arithmetic operations between two values.
        /// </summary>
        /// <param name="lhs">The operand on the left side of the operator.</param>
        /// <param name="rhs">The operand on the right side of the operator.</param>
        /// <param name="op">The arithmetic operator.</param>
        public ArithmeticExpression(Value lhs, Value rhs, ArithmeticOperator op)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.OpenBracket();
            this.lhs.WriteTo(builder);
            builder.CloseBracket()
                   .AppendArithmeticOperator(this.op)
                   .OpenBracket();
            this.rhs.WriteTo(builder);
            builder.CloseBracket();
        }
    }
}
