using System;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions.Predicates;

namespace KiwiQuery
{

public static class WhereClauseExtensions
{
    /// <summary>
    /// Add a WHERE statement to the query. This method can only be called once.
    /// </summary>
    /// <param name="query">The query to add the statement to.</param>
    /// <param name="predicate">The predicates </param>
    /// <exception cref="InvalidOperationException"></exception>
    public static TSelf Where<TSelf>(this IHasWhereClause<TSelf> query, Predicate predicate)
    {
        query.WhereClause.AddPredicate(predicate);
        return query.Downcast();
    }
}

}
