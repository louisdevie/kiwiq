namespace KiwiQuery.Sql
{
    /// <summary>
    /// Something that can be written as SQL.
    /// </summary>
    public interface IWriteable
    {
        /// <summary>
        /// Writes this to a given <see cref="QueryBuilder"/>.
        /// </summary>
        /// <param name="builder">The query builder to write to.</param>
        void WriteTo(QueryBuilder builder);
    }
}
