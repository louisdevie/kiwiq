using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Expressions
{
    public class FunctionCall : Value
    {
        private string function;
        private Value[] arguments;

        public FunctionCall(string function, params Value[] arguments)
        {
            this.function = function;
            this.arguments = arguments;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendRaw(this.function)
                   .OpenBracket()
                   .AppendCommaSeparatedElements(this.arguments)
                   .CloseBracket();
        }
    }
}
