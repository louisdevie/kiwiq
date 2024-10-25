using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A wildcard that represents all the columns in a SELECT statement.
    /// </summary>
    public sealed class AllColumns : Value
    {
        /// <inheritdoc />
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendAllColumnsWildcard();
        }

        /// <inheritdoc />
        public override bool IsNull() => false;
    }
}
