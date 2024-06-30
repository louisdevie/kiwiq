using KiwiQuery.Clauses;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery
{

/// <summary>
/// A SQL DELETE command. <br/>
/// Instances of this class should be created from a <see cref="Schema"/>.
/// </summary>
public class DeleteCommand : Command, IHasWhereClause<DeleteCommand>
{
    private readonly string table;
    private readonly WhereClauseBuilder whereClauseBuilder;

    /// <summary>
    /// Creates a new DELETE command.
    /// </summary>
    /// <param name="table">The name of the table to delete rows from.</param>
    /// <param name="schema">The schema to execute this command on.</param>
    internal DeleteCommand(string table, Schema schema) : base(schema)
    {
        this.table = table;
        this.whereClauseBuilder = new WhereClauseBuilder();
    }

    /// <inheritdoc />
    protected override string BuildCommandText(QueryBuilder result)
    {
        result.AppendDeleteFromKeywords().AppendTableOrColumnName(this.table);

        this.whereClauseBuilder.WriteClauseTo(result);

        return result.ToString();
    }

    /// <summary>
    /// Build and execute the command.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there was at least one row affected, otherwise <see langword="false"/>.
    /// </returns>
    public bool Apply()
    {
        this.BuildCommand();
        int affectedRows = this.DbCommand.ExecuteNonQuery();
        return affectedRows > 0;
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public DeleteCommand Downcast() => this;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.whereClauseBuilder;
}

}
