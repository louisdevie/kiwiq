using System;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped
{

/// <summary>
/// This interface indicates that a command uses a <see cref="WhereClauseBuilder"/> and provides a <see cref="IMappedRoot"/>.
/// </summary>
public interface IHasMappedWhereClause<TSelf> : IHasWhereClause<TSelf>
{
    /// <summary>
    /// The root entity mapped by this command.
    /// </summary>
    public IMappedRoot Root { get; }
}
    
/// <summary>
/// Contains methods for commands that accept a WHERE clause.
/// </summary>
public static class MappedWhereClauseExtensions
{
    /// <summary>
    /// Add a WHERE statement to the query using mapped columns. This method can only be called once.
    /// </summary>
    /// <param name="query">The query to add the statement to.</param>
    /// <param name="predicateBuilder">A function returning a predicate to use to filter the results.</param>
    /// <exception cref="InvalidOperationException">When a WHERE statement is already present in the query.</exception>
    public static TSelf Where<TSelf>(this IHasMappedWhereClause<TSelf> query, Func<IMappedRoot, Predicate> predicateBuilder)
    {
        query.Where(predicateBuilder.Invoke(query.Root));
        return query.Downcast();
    }

    /*
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
    */
}

}
