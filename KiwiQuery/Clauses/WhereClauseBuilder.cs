using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System;

namespace KiwiQuery.Clauses
{

/// <summary>
/// Provides methods for building WHERE statements.
/// </summary>
public class WhereClauseBuilder
{
    private WhereClause? clause;

    internal void WriteClauseTo(QueryBuilder qb) => this.clause?.WriteTo(qb);

    internal void AddPredicate(Predicate predicate)
    {
        if (this.clause is null)
        {
            this.clause = new WhereClause(predicate);
        }
        else
        {
            throw new InvalidOperationException("A where clause already exists in this query.");
        }
    }
}

/// <summary>
/// This interface indicates that a query uses a <see cref="WhereClauseBuilder"/>. 
/// </summary>
public interface IHasWhereClause<TSelf>
{
    /// <summary>
    /// The <see cref="WhereClauseBuilder"/> used by this query.
    /// </summary>
    public WhereClauseBuilder WhereClause { get; }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public TSelf Downcast();
}

}
