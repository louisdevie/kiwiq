namespace KiwiQuery.Sql.Context
{
    /// <summary>
    /// Various hints on how the SQL should be generated.
    /// </summary>
    public interface IQueryContext
    {
        /// <summary>
        /// Indicates if table aliases are available at this point in the query.
        /// </summary>
        NameContext Tables { get; }
    }
}