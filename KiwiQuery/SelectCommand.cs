﻿using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace KiwiQuery
{

/// <summary>
/// A SQL SELECT command. <br/>
/// Instances of this class should be created from a <see cref="Schema"/>.
/// </summary>
public class SelectCommand : Command, IWriteable, IHasJoinClause<SelectCommand>, IHasWhereClause<SelectCommand>,
    IHasLimitClause<SelectCommand>
{
    private Table? table;
    private bool distinct;
    private readonly WhereClauseBuilder whereClauseBuilder;
    private readonly JoinClauseBuilder joinClauseBuilder;
    private readonly LimitClauseBuilder limitClauseBuilder;
    private readonly List<Value> projection;

    /// <summary>
    /// Creates a new SELECT command.
    /// </summary>
    /// <param name="projection">The columns to select.</param>
    /// <param name="schema">The schema to execute the command on.</param>
    internal SelectCommand(IEnumerable<Value> projection, Schema schema) : base(schema)
    {
        this.table = null;
        this.distinct = false;
        this.whereClauseBuilder = new WhereClauseBuilder();
        this.joinClauseBuilder = new JoinClauseBuilder(schema);
        this.limitClauseBuilder = new LimitClauseBuilder();
        this.projection = projection.ToList();
    }

    /// <summary>
    /// Add more columns to be selected. <br/>
    /// This method is useful for breaking down long select statements and
    /// combining simple string columns with <see cref="Column"/> objects.
    /// </summary>
    /// <param name="columns">The columns and values to select.</param>
    public SelectCommand And(params Value[] columns)
    {
        foreach (Value column in columns)
        {
            this.projection.Add(column);
        }
        return this;
    }

    /// <inheritdoc cref="And(Value[])"/>
    public SelectCommand And(params string[] columns)
    {
        foreach (string column in columns)
        {
            this.projection.Add(this.Schema.Column(column));
        }
        return this;
    }

    /// <summary>
    /// Chooses the first table to select from. Other tables can be joined later.
    /// </summary>
    /// <param name="table">The first table to select from.</param>
    public SelectCommand From(string table)
    {
        this.table = this.Schema.Table(table);
        return this;
    }

    /// <inheritdoc cref="From(string)"/>
    public SelectCommand From(Table table)
    {
        this.table = table;
        return this;
    }

    /// <inheritdoc/>
    public void WriteTo(QueryBuilder result)
    {
        if (this.table is null) throw new InvalidOperationException("No table specified.");

        result.AppendSelectKeyword();

        if (this.distinct)
        {
            result.AppendDistinctKeyword();
        }

        if (this.projection.Count == 0)
        {
            result.AppendAllColumnsWildcard();
        }
        else
        {
            result.PushContext.WithTableAliases();
            result.AppendCommaSeparatedElements(this.projection);
            result.PopContext();
        }

        result.PushContext.DeclaringTables();
        result.AppendFromKeyword();
        this.table.WriteTo(result);
        this.joinClauseBuilder.WriteClauseTo(result);
        result.PopContext();

        result.PushContext.WithTableAliases();
        this.whereClauseBuilder.WriteClauseTo(result);
        this.limitClauseBuilder.WriteClauseTo(result);
        result.PopContext();
    }

    /// <inheritdoc/>
    protected override string BuildCommandText(QueryBuilder result)
    {
        this.WriteTo(result);
        return result.ToString();
    }

    /// <summary>
    /// Build the command, execute it and fetch the results.
    /// </summary>
    /// <returns>
    /// An open <see cref="DbDataReader"/>. If you need the data reader
    /// specific to the connector you're using, <br/> you can use the
    /// generic overload <see cref="Fetch{TReader}()"/> instead.
    /// </returns>
    public DbDataReader Fetch()
    {
        this.BuildCommand();
        return this.DbCommand.ExecuteReader();
    }

    /// <summary>
    /// Build the command, execute it and fetch the results.
    /// </summary>
    /// <typeparam name="TReader">The scpecific type of data reader returned by the connector.</typeparam>
    /// <returns>
    /// An open <typeparamref name="TReader"/>. If you don't need the data
    /// reader specific to the connector you're using, <br/> you can use
    /// the basic overload <see cref="Fetch()"/> instead.
    /// </returns>
    public TReader Fetch<TReader>()
    where TReader : DbDataReader => (TReader)this.Fetch();

    /// <summary>
    /// Remove duplicates from the query results.
    /// </summary>
    public SelectCommand Distinct()
    {
        this.distinct = true;
        return this;
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public SelectCommand Downcast() => this;

    /// <inheritdoc />
    public JoinClauseBuilder JoinClause => this.joinClauseBuilder;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.whereClauseBuilder;

    /// <inheritdoc />
    public LimitClauseBuilder LimitClause => this.limitClauseBuilder;
}

}
