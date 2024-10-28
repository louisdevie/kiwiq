using System;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{

/// <summary>
/// Provides the ability to build LIMIT / OFFSET clauses.
/// </summary>
public class LimitClauseBuilder
{
    private int limitOnly;
    private LimitClause? clause;

    internal LimitClauseBuilder()
    {
        this.limitOnly = -1;
    }

    internal void WriteClauseTo(QueryBuilder qb) => this.clause?.WriteTo(qb);

    internal void AddOffset(int offset)
    {
        if (this.clause is null)
        {
            throw new InvalidOperationException("A limit must be created before calling Offset().");
        }
        this.clause = new LimitClause(this.limitOnly, offset);
    }

    internal void SetLimitAndOffset(int limit, int offset)
    {
        if (this.clause is null)
        {
            this.clause = new LimitClause(limit, offset);
        }
        else
        {
            throw new InvalidOperationException("A limit clause already exists in this query.");
        }
    }

    internal void SetLimit(int limit)
    {
        if (this.clause is null)
        {
            this.limitOnly = limit;
            this.clause = new LimitClause(limit, 0);
        }
        else
        {
            throw new InvalidOperationException("A limit clause already exists in this query, or Offset() was called before Limit().");
        }
    }
}


/// <summary>
/// This interface indicates that a query uses a <see cref="LimitClauseBuilder"/>. 
/// </summary>
public interface IHasLimitClause<TSelf>
{
    /// <summary>
    /// The <see cref="LimitClauseBuilder"/> used by this query.
    /// </summary>
    public LimitClauseBuilder LimitClause { get; }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public TSelf Downcast();
}

}
