using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// A SQL statement.
    /// </summary>
    internal abstract class Clause : IWriteable
    {
        public abstract void WriteTo(QueryBuilder builder);
    }
}
