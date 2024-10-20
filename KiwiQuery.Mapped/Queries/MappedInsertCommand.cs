using System;
using System.Collections.Generic;
using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Filters;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;
using KiwiQuery.Mapped.Queries.ValueOverloads;
using KiwiQuery.Sql;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL INSERT INTO command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedInsertCommand<T>
where T : notnull
{
    private readonly InsertCommand rawQuery;
    private readonly IMapper<T> mapper;
    private readonly IPrimaryKey primaryKey;
    private readonly Dictionary<string, IValueOverload> values;
    private Maybe<T> obj;

    internal MappedInsertCommand(InsertCommand rawQuery, IMapper<T> mapper)
    {
        this.mapper = mapper;
        this.primaryKey = this.mapper.PrimaryKey;
        this.rawQuery = rawQuery;
        this.values = new Dictionary<string, IValueOverload>();
        this.obj = Maybe.Nothing<T>();
    }

    /// <summary>
    /// Set the values to be inserted from an object. This method can only be called once.
    /// </summary>
    /// <param name="obj">The object to insert.</param>
    public MappedInsertCommand<T> Values(T obj)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseMultipleInsertAreNotSupported();
        }
        this.obj = Maybe.Just(obj);
        return this;
    }

    /// <summary>
    /// Add a value to be inserted into a specific column. It will override any column with the same name in objects
    /// passed to <see cref="Values"/>.
    /// </summary>
    /// <param name="column">The name of the column to insert the value into.</param>
    /// <param name="value">The value to insert.</param>
    public MappedInsertCommand<T> Value(string column, Value value)
    {
        this.values.Add(column, new ValueOverload(value));
        return this;
    }

    /// <summary>
    /// Add a value to be inserted into a specific column.
    /// </summary>
    /// <param name="column">The name of the column to insert the value into.</param>
    /// <param name="value">The value to insert.</param>
    public MappedInsertCommand<T> Value(string column, object? value)
    {
        this.values.Add(column, new ObjectOverload(value));
        return this;
    }

    /// <summary>
    /// Add a value to be inserted into a specific column.
    /// </summary>
    /// <param name="column">The name of the column to insert the value into.</param>
    /// <param name="subQuery">The subquery to insert as a value.</param>
    public MappedInsertCommand<T> Value(string column, SelectCommand subQuery)
    {
        this.values.Add(column, new SubQueryOverload(subQuery));
        return this;
    }

    /// <summary>
    /// Build and execute the command.
    /// </summary>
    /// <returns>The ID (value of the primary key) of the inserted row, or -1 of the primary key is not an integer.</returns>
    public int Apply()
    {
        this.CompleteQuery();
        return this.rawQuery.Apply();
    }

    private void CompleteQuery()
    {
        if (this.obj.IsSomething)
        {
            foreach ((string column, object? value) in this.mapper.ObjectToValues(
                         this.obj.Value,
                         new InsertColumnFilter()
                     ))
            {
                if (!this.values.ContainsKey(column))
                {
                    this.rawQuery.Value(column, value);
                }
            }
        }

        foreach (var valueOverride in this.values)
        {
            valueOverride.Value.AddTo(this.rawQuery, valueOverride.Key);
        }
    }
}

}
