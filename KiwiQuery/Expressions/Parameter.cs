using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery.Expressions
{
    internal class Parameter : Value
    {
        private object? inner;

        public Parameter(object? value)
        {
            this.inner = value;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            string param = builder.ResisterParameterWithValue(this.inner);
            builder.AppendRaw(param);
        }
    }
}
