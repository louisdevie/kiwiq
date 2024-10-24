using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// Adapts a <see cref="SelectCommand"/> to be used as a <see cref="Value"/>.
    /// </summary>
    public class SubQuery : Value
    {
        private SelectCommand command;

        /// <summary>
        /// Creates a new subquery from the given SELECT query.
        /// </summary>
        /// <param name="command">A SELECT command to use as the subquery.</param>
        internal SubQuery(SelectCommand command)
        {
            this.command = command;
        }

        /// <inheritdoc />
        public override void WriteTo(QueryBuilder builder)
        {

            builder.OpenBracket();
            this.command.WriteTo(builder);
            builder.CloseBracket();
        }
    }
}
