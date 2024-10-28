using KiwiQuery.Clauses;

namespace KiwiQuery
{

/// <summary>
/// Contains methods for commands that accept a LIMIT / OFFSET clause.
/// </summary>
public static class LimitClauseExtensions
{
    /// <summary>
    /// Adds a LIMIT statement to the query without offset. The offset can
    /// be configured by combining <br/> this method with <see cref="Offset{TSelf}"/>
    /// or using the <see cref="Limit{TSelf}(IHasLimitClause{TSelf}, int, int)"/> overload.
    /// </summary>
    /// <param name="query">The query to add the limit to.</param>
    /// <param name="limit">The maximum number of rows the query should return.</param>
    public static TSelf Limit<TSelf>(this IHasLimitClause<TSelf> query, int limit)
    {
        query.LimitClause.SetLimit(limit);
        return query.Downcast();
    }

    /// <summary>
    /// Adds a LIMIT statement to the query with an offset. Calling
    /// <code>Limit(a, b)</code>
    /// is the same as using
    /// <code>Limit(a).Offset(b)</code>
    /// </summary>
    /// <param name="query">The query to add the limit to.</param>
    /// <param name="limit">The maximum number of rows the query should return.</param>
    /// <param name="offset">The numbers of rows to skip in the results.</param>
    public static TSelf Limit<TSelf>(this IHasLimitClause<TSelf> query, int limit, int offset)
    {
        query.LimitClause.SetLimitAndOffset(limit, offset);
        return query.Downcast();
    }

    /// <summary>
    /// Add an offset to the LIMIT statement.
    /// </summary>
    /// <param name="query">The query to add the offset to.</param>
    /// <param name="offset">The numbers of rows to skip in the results.</param>
    public static TSelf Offset<TSelf>(this IHasLimitClause<TSelf> query, int offset)
    {
        query.LimitClause.AddOffset(offset);
        return query.Downcast();
    }
}

}
