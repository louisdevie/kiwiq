using System.Collections.Generic;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Filters;

namespace KiwiQuery.Mapped.Queries
{

/// <summary>
/// A SQL UPDATE command for a mapped class. <br/>
/// Instances of this class should be created from a <see cref="Schema"/> or a mapped <see cref="Table{T}"/>.
/// </summary>
public partial class MappedUpdateQuery<T> where T : notnull
{
    private readonly UpdateQuery rawQuery;
    private readonly IMapper<T> mapper;
    private readonly Dictionary<string, IValueOverload> values;
    private Maybe<T> obj;
    private IColumnFilter filter;

    internal MappedUpdateQuery(UpdateQuery rawQuery, IMapper<T> mapper)
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
    public MappedUpdateQuery<T> SetAll(T obj)
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
    public MappedUpdateQuery<T> SetInserted(T obj)
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
    public MappedUpdateQuery<T> SetOnly(T obj, params string[] columns)
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
    public MappedUpdateQuery<T> SetAllExcept(T obj, params string[] columns)
    {
        if (this.obj.IsSomething)
        {
            throw RepeatedCallException.BecauseObjectIsAlreadySet();
        }
        this.obj = Maybe.Just(obj);
        this.filter = new ExceptColumnFilter(columns);
        return this;
    }

    /// <inheritdoc cref="UpdateQuery.Set(string,KiwiQuery.Expressions.Value)"/> 
    public MappedUpdateQuery<T> Set(string column, Value value)
    {
        this.values.Add(column, new ValueOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateQuery.Set(string,KiwiQuery.Expressions.Value)"/> 
    public MappedUpdateQuery<T> Set(string column, object? value)
    {
        this.values.Add(column, new ObjectOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateQuery.Set(string,KiwiQuery.Expressions.Value)"/> 
    public MappedUpdateQuery<T> Set(string column, SelectQuery value)
    {
        this.values.Add(column, new SubQueryOverload(value));
        return this;
    }

    /// <inheritdoc cref="UpdateQuery.Apply"/> 
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


    #region WHERE clause methods

    /// <inheritdoc cref="UpdateQuery.Where(Predicate)"/>
    public MappedUpdateQuery<T> Where(Predicate predicate)
    {
        this.rawQuery.Where(predicate);
        return this;
    }

    #endregion
}

}
