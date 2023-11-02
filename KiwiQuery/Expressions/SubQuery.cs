using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// Adapts a <see cref="SelectQuery"/> to be used as a <see cref="Value"/>.
    /// </summary>
    public class SubQuery : Value
    {
        private SelectQuery query;

        /// <summary>
        /// Creates a new subquery from the given SELECT query.
        /// </summary>
        /// <param name="query"></param>
        internal SubQuery(SelectQuery query)
        {
            this.query = query;
        }

        public override void WriteTo(QueryBuilder builder)
        {

            builder.OpenBracket();
            this.query.WriteTo(builder);
            builder.CloseBracket();
        }
    }
}
