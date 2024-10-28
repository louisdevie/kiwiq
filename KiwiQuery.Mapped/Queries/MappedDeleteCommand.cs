using KiwiQuery.Clauses;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL DELETE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedDeleteCommand<T> : IHasWhereClause<MappedDeleteCommand<T>>
where T : notnull
{
    private readonly DeleteCommand rawQuery;

    internal MappedDeleteCommand(DeleteCommand rawQuery)
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
    public MappedDeleteCommand<T> Downcast() => this;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.rawQuery.WhereClause;
}

}
