using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL SELECT command with its results mapped to objects. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
// TODO expose this API
internal class MappedSelectCommand : IHasJoinClause<MappedSelectCommand>, IHasWhereClause<MappedSelectCommand>,
    IHasLimitClause<MappedSelectCommand>
{
    private readonly SelectCommand rawQuery;
    private readonly IMapper mapper;
    private bool explicitTables;

    internal MappedSelectCommand(SelectCommand rawQuery, IMapper mapper)
    {
        this.rawQuery = rawQuery;
        this.mapper = mapper;
        this.explicitTables = false;
    }

    /// <summary>
    /// Add columns aliases to be selected. <br/>
    /// This method is useful when using JOINs to discriminate two columns with the same name or to map a field to
    /// a computed value.
    /// </summary>
    /// <param name="columns">The columns and values to select.</param>
    /// <exception cref="ArgumentException">If a column without alias was given.</exception>
    public MappedSelectCommand With(params Column[] columns)
    {
        foreach (Column column in columns)
        {
            if (column.Alias == null)
            {
                throw new ArgumentException("Expected a column alias.", nameof(columns));
            }
        }

        return this;
    }

    /// <inheritdoc cref="SelectCommand.From(string)"/>
    /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
    public MappedSelectCommand From(string table)
    {
        this.rawQuery.From(table);
        this.explicitTables = true;
        return this;
    }

    /// <inheritdoc cref="From(string)"/>
    /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
    public MappedSelectCommand From(Table table)
    {
        this.rawQuery.From(table);
        this.explicitTables = true;
        return this;
    }

    /// <summary>
    /// Build the command, execute it and fetch the results one by one.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerator{T}"/> that returns the mapped rows.
    /// </returns>
    public IEnumerator Fetch(bool buffered = true)
    {
        IEnumerator enumerator;

        if (buffered)
        {
            enumerator = this.FetchBuffered().GetEnumerator();
        }
        else
        {
            enumerator = this.FetchUnbuffered();
        }

        return enumerator;
    }

    /// <summary>
    /// Build the command, execute it and fetch the results into a list.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> that contains all the mapped rows.
    /// </returns>
    public ArrayList FetchList()
    {
        return this.FetchBuffered();
    }

    private ArrayList FetchBuffered()
    {
        var results = new ArrayList();
        using DbDataReader reader = this.FetchRaw();
        while (reader.Read())
        {
            results.Add(this.mapper.RowToObject(reader, this.rawQuery.Schema));
        }
        return results;
    }

    private UnbufferedReader FetchUnbuffered()
    {
        return new UnbufferedReader(this.FetchRaw());
    }

    private DbDataReader FetchRaw()
    {
        this.CompleteQuery(); // TODO make sure this is called only once
        return this.rawQuery.Fetch();
    }

    private void CompleteQuery()
    {
        if (!this.explicitTables)
        {
            this.rawQuery.From(this.mapper.FirstTable);

            foreach (IJoin join in this.mapper.Joins) join.AddTo(this.rawQuery);
        }

        this.rawQuery.And(this.mapper.Projection.ToArray<Value>());
    }

    /// <inheritdoc cref="SelectCommand.Distinct()"/>
    public MappedSelectCommand Distinct()
    {
        this.rawQuery.Distinct();
        return this;
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public MappedSelectCommand Downcast() => this;

    /// <inheritdoc />
    public JoinClauseBuilder JoinClause => this.rawQuery.JoinClause;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.rawQuery.WhereClause;

    /// <inheritdoc />
    public LimitClauseBuilder LimitClause => this.rawQuery.LimitClause;
}

/// <summary>
/// A SQL SELECT command with its results mapped to objects. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedSelectCommand<T> : IHasJoinClause<MappedSelectCommand<T>>, IHasWhereClause<MappedSelectCommand<T>>,
    IHasLimitClause<MappedSelectCommand<T>>
where T : notnull
{
    private readonly SelectCommand rawQuery;
    private readonly IMapper<T> mapper;
    private bool explicitTables;

    internal MappedSelectCommand(SelectCommand rawQuery, IMapper<T> mapper)
    {
        this.rawQuery = rawQuery;
        this.mapper = mapper;
        this.explicitTables = false;
    }

    /// <summary>
    /// Add columns aliases to be selected. <br/>
    /// This method is useful when using JOINs to discriminate two columns with the same name or to map a field to
    /// a computed value.
    /// </summary>
    /// <param name="columns">The columns and values to select.</param>
    /// <exception cref="ArgumentException">If a column without alias was given.</exception>
    public MappedSelectCommand<T> With(params Column[] columns)
    {
        foreach (Column column in columns)
        {
            if (column.Alias == null)
            {
                throw new ArgumentException("Expected a column alias.", nameof(columns));
            }
        }

        return this;
    }

    /// <inheritdoc cref="SelectCommand.From(string)"/>
    /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
    public MappedSelectCommand<T> From(string table)
    {
        this.rawQuery.From(table);
        this.explicitTables = true;
        return this;
    }

    /// <inheritdoc cref="From(string)"/>
    /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
    public MappedSelectCommand<T> From(Table table)
    {
        this.rawQuery.From(table);
        this.explicitTables = true;
        return this;
    }

    /// <summary>
    /// Build the command, execute it and fetch the results one by one.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerator{T}"/> that returns the mapped rows.
    /// </returns>
    public IEnumerator<T> Fetch(bool buffered = true)
    {
        IEnumerator<T> enumerator;

        if (buffered)
        {
            enumerator = this.FetchBuffered().GetEnumerator();
        }
        else
        {
            enumerator = this.FetchUnbuffered();
        }

        return enumerator;
    }

    /// <summary>
    /// Build the command, execute it and fetch the results into a list.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> that contains all the mapped rows.
    /// </returns>
    public List<T> FetchList()
    {
        return this.FetchBuffered();
    }

    private List<T> FetchBuffered()
    {
        List<T> results = new List<T>();
        using var reader = this.FetchRaw();
        while (reader.Read())
        {
            results.Add(this.mapper.RowToObject(reader, this.rawQuery.Schema));
        }
        return results;
    }

    private UnbufferedReader<T> FetchUnbuffered()
    {
        return new UnbufferedReader<T>(this.FetchRaw());
    }

    private DbDataReader FetchRaw()
    {
        this.CompleteQuery(); // TODO make sure this is called only once
        return this.rawQuery.Fetch();
    }

    private void CompleteQuery()
    {
        if (!this.explicitTables)
        {
            this.rawQuery.From(this.mapper.FirstTable);

            foreach (IJoin join in this.mapper.Joins) join.AddTo(this.rawQuery);
        }

        this.rawQuery.And(this.mapper.Projection.ToArray<Value>());
    }

    /// <inheritdoc cref="SelectCommand.Distinct()"/>
    public MappedSelectCommand<T> Distinct()
    {
        this.rawQuery.Distinct();
        return this;
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public MappedSelectCommand<T> Downcast() => this;

    /// <inheritdoc />
    public JoinClauseBuilder JoinClause => this.rawQuery.JoinClause;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.rawQuery.WhereClause;

    /// <inheritdoc />
    public LimitClauseBuilder LimitClause => this.rawQuery.LimitClause;
}

}
