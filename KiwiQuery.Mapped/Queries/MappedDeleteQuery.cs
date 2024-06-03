using KiwiQuery.Expressions.Predicates;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL DELETE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table{T}"/>.
/// </summary>
public class MappedDeleteQuery<T>
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

    #region WHERE clause methods

    /// <inheritdoc cref="DeleteQuery.Where"/>
    public MappedDeleteQuery<T> Where(Predicate predicate)
    {
        this.rawQuery.Where(predicate);
        return this;
    }

    #endregion
}

}
