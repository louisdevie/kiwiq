using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Expressions
{
    internal class ArithmeticExpression: Value
    {
        private Value lhs;
        private Value rhs;
        private ArithmeticOperator op;

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
