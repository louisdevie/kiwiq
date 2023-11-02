using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A C# value inserted in the query with a named parameter.
    /// </summary>
    internal class Parameter : Value
    {
        private object? inner;

        /// <summary>
        /// Creates a new parameter. The name of the parameter will be automatically generated.
        /// </summary>
        /// <param name="value">The c# value to insert in the query.</param>
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
