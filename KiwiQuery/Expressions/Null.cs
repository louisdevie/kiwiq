using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// The SQL NULL value.
    /// </summary>
    public sealed class Null : Value
    {
        /// <inheritdoc />
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendNull();
        }
    }
}
