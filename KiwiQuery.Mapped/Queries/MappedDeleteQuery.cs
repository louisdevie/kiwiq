using System;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL DELETE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedDeleteQuery<T> : IHasWhereClause<MappedDeleteQuery<T>>
where T : notnull
{
    private readonly DeleteQuery rawQuery;

    internal MappedDeleteQuery(DeleteQuery rawQuery)
    {
        this.rawQuery = rawQuery;
    }

    /// <summary>
    /// Build and execute the command.
    /// </summary>
    /// <returns><see langword="true"/> if there was at least one row affected, otherwise <see langword="false"/>.</returns>
    public bool Apply()
    {
        return this.rawQuery.Apply();
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public MappedDeleteQuery<T> Downcast() => this;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.rawQuery.WhereClause;
}

}
