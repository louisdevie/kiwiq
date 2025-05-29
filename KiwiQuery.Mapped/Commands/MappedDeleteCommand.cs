using KiwiQuery.Clauses;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped.Commands
{

/// <summary>
/// A SQL DELETE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedDeleteCommand<T> : IHasMappedWhereClause<MappedDeleteCommand<T>>
where T : notnull
{
    private readonly DeleteCommand rawQuery;
    private readonly IMapper<T> mapper;

    internal MappedDeleteCommand(DeleteCommand rawQuery, IMapper<T> mapper)
    {
        this.rawQuery = rawQuery;
        this.mapper = mapper;
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
    
    /// <inheritdoc />
    public IMappedRoot Root => this.mapper;
}

}
