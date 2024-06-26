using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System.Collections.Generic;

namespace KiwiQuery.Clauses
{

/// <summary>
/// Provides the ability to build JOIN clauses.
/// </summary>
public class JoinClauseBuilder
{
    private readonly List<JoinClause> joins;
    private readonly Schema schema;

    internal JoinClauseBuilder(Schema schema)
    {
        this.joins = new List<JoinClause>();
        this.schema = schema;
    }

    internal void WriteClauseTo(QueryBuilder qb)
    {
        foreach (JoinClause joinClause in this.joins)
        {
            joinClause.WriteTo(qb);
        }
    }

    internal void AddJoin(JoinClause joinClause)
    {
        this.joins.Add(joinClause);
    }

    internal Table GetTable(string name) => this.schema.Table(name);

    internal Column GetColumn(string name) => this.schema.Column(name);
}

/// <summary>
/// This interface indicates that a query uses a <see cref="JoinClauseBuilder"/>. 
/// </summary>
public interface IHasJoinClause<TSelf>
{
    /// <summary>
    /// The <see cref="JoinClauseBuilder"/> used by this query.
    /// </summary>
    public JoinClauseBuilder JoinClause { get; }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public TSelf Downcast();
}

}
