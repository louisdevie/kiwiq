using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;

namespace KiwiQuery
{

/// <summary>
/// A SQL UPDATE command. <br/>
/// Instances of this class should be created from a <see cref="Schema"/>.
/// </summary>
public class UpdateQuery : Query, IHasWhereClause<UpdateQuery>
{
    private readonly string table;
    private readonly WhereClauseBuilder whereClauseBuilder;

    private class ValueToUpdate
    {
        private readonly string column;
        private readonly Value value;

        public string Column => this.column;

        public Value Value => this.value;

        public ValueToUpdate(Value value, string column)
        {
            this.column = column;
            this.value = value;
        }
    }

    private readonly List<ValueToUpdate> values;

    /// <inheritdoc />
    public UpdateQuery(string table, Schema schema) : base(schema)
    {
        this.table = table;
        this.whereClauseBuilder = new WhereClauseBuilder();
        this.values = new List<ValueToUpdate>();
    }

    /// <summary>
    /// Sets a new value for a column.
    /// </summary>
    /// <param name="column">The name of the column to update.</param>
    /// <param name="value">The new value to put in the database.</param>
    public UpdateQuery Set(string column, Value value)
    {
        this.values.Add(new ValueToUpdate(value, column));
        return this;
    }

    /// <inheritdoc cref="Set(string,KiwiQuery.Expressions.Value)"/>
    public UpdateQuery Set(string column, object? value)
    {
        this.values.Add(new ValueToUpdate(new Parameter(value), column));
        return this;
    }

    /// <inheritdoc cref="Set(string,KiwiQuery.Expressions.Value)"/>
    public UpdateQuery Set(string column, SelectQuery value)
    {
        this.values.Add(new ValueToUpdate(new SubQuery(value), column));
        return this;
    }

    /// <inheritdoc />
    protected override string BuildCommandText(QueryBuilder result)
    {
        result.AppendUpdateKeyword().AppendTableOrColumnName(this.table).AppendSetKeyword();

        if (this.values.Count == 0)
        {
            throw new InvalidOperationException("No values to update.");
        }

        bool firstValue = true;
        foreach (var value in this.values)
        {
            if (firstValue)
            {
                firstValue = false;
            }
            else
            {
                result.AppendComma();
            }

            result.AppendTableOrColumnName(value.Column).AppendSetClauseAssignment();
            value.Value.WriteTo(result);
        }

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
        int affectedRows = this.Command.ExecuteNonQuery();
        return affectedRows > 0;
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public UpdateQuery Downcast() => this;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.whereClauseBuilder;
}

}
