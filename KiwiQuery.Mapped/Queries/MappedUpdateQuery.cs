using System.Collections.Generic;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Filters;
using KiwiQuery.Mapped.Queries.ValueOverloads;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL UPDATE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table"/>.
/// </summary>
public class MappedUpdateCommand<T> : IHasWhereClause<MappedUpdateCommand<T>>
where T : notnull
{
    private readonly UpdateCommand rawQuery;
    private readonly IMapper<T> mapper;
    private readonly Dictionary<string, IValueOverload> values;
    private Maybe<T> obj;
    private IColumnFilter filter;

    internal MappedUpdateCommand(UpdateCommand rawQuery, IMapper<T> mapper)
    {
        this.mapper = mapper;
        this.rawQuery = rawQuery;
        this.values = new Dictionary<string, IValueOverload>();
        this.obj = default;
        this.filter = new NoColumnFilter();
    }

    /// <summary>
    /// Set the values to be updated from all the fields of an object. You can call this method or one of
    /// <see cref="SetInserted"/>, <see cref="SetOnly"/> and <see cref="SetAllExcept"/> only once.
    /// </summary>
    /// <param name="obj">The object to insert.</param>
    public MappedUpdateCommand<T> SetAll(T obj)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseObjectIsAlreadySet();
        }
        this.obj = Maybe.Just(obj);
        this.filter = new NoColumnFilter();
        return this;
    }

    /// <summary>
    /// Set the values to be updated from the same fields inculded in INSERT INTO queries. You can call this method or
    /// one of <see cref="SetAll"/>, <see cref="SetOnly"/> and <see cref="SetAllExcept"/> only once.
    /// </summary>
    /// <param name="obj">The object to insert.</param>
    public MappedUpdateCommand<T> SetInserted(T obj)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseObjectIsAlreadySet();
        }
        this.obj = Maybe.Just(obj);
        this.filter = new InsertColumnFilter();
        return this;
    }

    /// <summary>
    /// Set the values to be updated from all the fields of an object. You can call this method or one of
    /// <see cref="SetAll"/>, <see cref="SetInserted"/> and <see cref="SetAllExcept"/> only once.
    /// </summary>
    /// <param name="obj">The object to insert.</param>
    /// <param name="columns">The object to insert.</param>
    public MappedUpdateCommand<T> SetOnly(T obj, params string[] columns)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseObjectIsAlreadySet();
        }
        this.obj = Maybe.Just(obj);
        this.filter = new IntersectColumnFilter(columns);
        return this;
    }

    /// <summary>
    /// Set the values to be updated from all the fields of an object. You can call this method or one of
    /// <see cref="SetAll"/>, <see cref="SetInserted"/> and <see cref="SetOnly"/> only once.
    /// </summary>
    /// <param name="obj">The object to insert.</param>
    /// <param name="columns">The object to insert.</param>
    public MappedUpdateCommand<T> SetAllExcept(T obj, params string[] columns)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseObjectIsAlreadySet();
        }
        this.obj = Maybe.Just(obj);
        this.filter = new ExceptColumnFilter(columns);
        return this;
    }

    /// <inheritdoc cref="UpdateCommand.Set(string,KiwiQuery.Expressions.Value)"/> 
    public MappedUpdateCommand<T> Set(string column, Value value)
    {
        this.values.Add(column, new ValueOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateCommand.Set(string,object?)"/> 
    public MappedUpdateCommand<T> Set(string column, object? value)
    {
        this.values.Add(column, new ObjectOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateCommand.Set(string,KiwiQuery.SelectCommand)"/> 
    public MappedUpdateCommand<T> Set(string column, SelectCommand value)
    {
        this.values.Add(column, new SubQueryOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateCommand.Apply"/> 
    public bool Apply()
    {
        this.CompleteQuery();
        return this.rawQuery.Apply();
    }

    private void CompleteQuery()
    {
        if (this.obj.IsSomething)
        {
            foreach ((string column, object? value) in this.mapper.ObjectToValues(this.obj.Value, this.filter))
            {
                if (!this.values.ContainsKey(column))
                {
                    this.rawQuery.Set(column, value);
                }
            }
        }

        foreach (var valueOverride in this.values)
        {
            valueOverride.Value.AddTo(this.rawQuery, valueOverride.Key);
        }
    }

    /// <summary>
    /// Downcasts this query into its precise type.
    /// </summary>
    public MappedUpdateCommand<T> Downcast() => this;

    /// <inheritdoc />
    public WhereClauseBuilder WhereClause => this.rawQuery.WhereClause;
}

}
