using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A call to a standard SQL function. <br/>
    /// Instances of this class can be created from the <see cref="SQL"/> class.
    /// </summary>
    public sealed class FunctionCall : Value
    {
        private string function;
        private Value[] arguments;

        /// <summary>
        /// Creats a new function call.
        /// </summary>
        /// <param name="function">The name of the function to call.</param>
        /// <param name="arguments">The arguments tu pass to the function.</param>
        internal FunctionCall(string function, params Value[] arguments)
        {
            this.function = function;
            this.arguments = arguments;
        }

        /// <inheritdoc />
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendRaw(this.function)
                   .OpenBracket()
                   .AppendCommaSeparatedElements(this.arguments)
                   .CloseBracket();
        }
    }
}
