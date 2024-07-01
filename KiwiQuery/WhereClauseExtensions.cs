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
    /// <param name="predicate">The predicate to use to filter the results.</param>
    /// <exception cref="InvalidOperationException">When a WHERE statement is already present in the query.</exception>
    public static TSelf Where<TSelf>(this IHasWhereClause<TSelf> query, Predicate predicate)
    {
        query.WhereClause.AddPredicate(predicate);
        return query.Downcast();
    }

    /// <summary>
    /// Add a WHERE statement to the query with a predicate that must not match. This method can only be called once.
    /// </summary>
    /// <param name="query">The query to add the statement to.</param>
    /// <param name="predicate">
    /// The predicate to use to filter the results, that will be wrapped with the NOT operator.
    /// </param>
    /// <exception cref="InvalidOperationException">When a WHERE statement is already present in the query.</exception>
    public static TSelf WhereNot<TSelf>(this IHasWhereClause<TSelf> query, Predicate predicate)
    {
        query.WhereClause.AddPredicate(SQL.NOT(predicate));
        return query.Downcast();
    }

    /// <summary>
    /// Add a WHERE statement to the query with predicates that must all match. This method can only be called
    /// once.
    /// </summary>
    /// <param name="query">The query to add the statement to.</param>
    /// <param name="predicates">
    /// The predicates to use to filter the results, that will be joined together with the AND operator.
    /// </param>
    /// <exception cref="InvalidOperationException">When a WHERE statement is already present in the query.</exception>
    public static TSelf WhereAll<TSelf>(this IHasWhereClause<TSelf> query, params Predicate[] predicates)
    {
        query.WhereClause.AddPredicate(SQL.AND(predicates));
        return query.Downcast();
    }

    /// <summary>
    /// Add a WHERE statement to the query with one predicate that must match. This method can only be called once.
    /// </summary>
    /// <param name="query">The query to add the statement to.</param>
    /// <param name="predicates">
    /// The predicates to use to filter the results, that will be joined together with the OR operator.
    /// </param>
    /// <exception cref="InvalidOperationException">When a WHERE statement is already present in the query.</exception>
    public static TSelf WhereAny<TSelf>(this IHasWhereClause<TSelf> query, params Predicate[] predicates)
    {
        query.WhereClause.AddPredicate(SQL.OR(predicates));
        return query.Downcast();
    }
}

}
