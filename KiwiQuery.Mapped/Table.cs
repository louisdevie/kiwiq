using System;
using System.Collections.Generic;
using System.Linq;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped
{

/// <summary>
/// A table of the schema mapped to a type. <br/>
/// Instances of this class should be created from a <see cref="Schema"/>.
/// </summary>
/// <typeparam name="TEntity">The type of entity to read.</typeparam>
/// <typeparam name="TKey">
/// The type of the primary key. If it is a single column, the type of that column is expected. Otherwise, you will
/// need to redefine the operations that work with a primary key.
/// </typeparam>
public class Table<TKey, TEntity>
where TEntity : notnull
where TKey : notnull
{
    private readonly Schema schema;
    private readonly Table rawTable;
    private readonly IMapper<TEntity> mapper;

    internal Table(Schema schema, Table rawTable, IMapper<TEntity> mapper)
    {
        this.schema = schema;
        this.rawTable = rawTable;
        this.mapper = mapper;
        this.mapper.PrimaryKey.CheckType(typeof(TKey));
    }

    /// <summary>
    /// Creates a SELECT query for this table.
    /// </summary>
    public MappedSelectCommand<TEntity> Select()
    {
        return new MappedSelectCommand<TEntity>(this.schema.Select(), this.mapper);
    }

    /// <summary>
    /// Queries the entity with the given key.
    /// </summary>
    /// <param name="key">The primary of the entity to fetch.</param>
    /// <returns>The first result if there is one.</returns>
    /// <exception cref="NotFoundException">If the query returns no results.</exception>
    public virtual TEntity SelectOne(TKey key)
    {
        try
        {
            return this.SelectWhere(this.mapper.PrimaryKey.MakeEqualityPredicate(this.rawTable, key)).First();
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException(typeof(TEntity), key);
        }
    }

    /// <summary>
    /// Queries the entities that match the predicate.
    /// </summary>
    /// <param name="predicate">A predicate for the query.</param>
    /// <returns>The results of the query.</returns>
    public virtual IEnumerable<TEntity> SelectWhere(Predicate predicate)
    {
        return this.Select().Where(predicate).FetchList();
    }

    /// <summary>
    /// Queries all the entities from the table.
    /// </summary>
    /// <returns>The results of the query.</returns>
    public virtual IEnumerable<TEntity> SelectAll()
    {
        return this.Select().FetchList();
    }

    /// <summary>
    /// Creates an INSERT query for this table.
    /// </summary>
    public MappedInsertCommand<TEntity> Insert()
    {
        return new MappedInsertCommand<TEntity>(this.schema.InsertInto(this.mapper.FirstTable.Name), this.mapper);
    }

    /// <summary>
    /// Executes a command to add a record to the table.
    /// </summary>
    /// <param name="entity">The data to insert.</param>
    /// <returns>
    /// The entity, with its primary key set to that of the newly inserted row if it is auto-incremented.
    /// </returns>
    public virtual TEntity InsertOne(TEntity entity)
    {
        int autoId = this.Insert().Values(entity).Apply();
        if (autoId != InsertCommand.NO_AUTO_ID) this.mapper.PrimaryKey.ReplaceAutoIncrementedValue(entity, autoId);
        return entity;
    }

    /// <summary>
    /// Create an UPDATE query for this table.
    /// </summary>
    public MappedUpdateCommand<TEntity> Update()
    {
        return new MappedUpdateCommand<TEntity>(this.schema.Update(this.mapper.FirstTable.Name), this.mapper);
    }

    /// <summary>
    /// Executes a command that updates a single record.
    /// </summary>
    /// <param name="key">The key of the record to update.</param>
    /// <param name="entity">The new values to set. Columns declared as Inserted = false will be ignored.</param>
    /// <returns><see langword="true"/> if the row was successfully updated.</returns>
    public bool UpdateOne(TKey key, TEntity entity)
    {
        return this.Update()
            .SetInserted(entity)
            .Where(this.mapper.PrimaryKey.MakeEqualityPredicate(this.rawTable, key))
            .Apply();
    }

    /// <summary>
    /// Create a DELETE query for this table.
    /// </summary>
    public MappedDeleteCommand<TEntity> Delete()
    {
        return new MappedDeleteCommand<TEntity>(this.schema.DeleteFrom(this.mapper.FirstTable.Name));
    }

    /// <summary>
    /// Executes a command that removes a single record.
    /// </summary>
    /// <param name="key">The key of the record to remove.</param>
    /// <returns><see langword="true"/> if the row was successfully removed.</returns>
    public bool DeleteOne(TKey key)
    {
        return this.DeleteWhere(this.mapper.PrimaryKey.MakeEqualityPredicate(this.rawTable, key));
    }

    /// <summary>
    /// Executes a command that removes all records matching a predicate.
    /// </summary>
    /// <param name="predicate">A predicate for the query.</param>
    /// <returns><see langword="true"/> if at least one row was removed.</returns>
    public bool DeleteWhere(Predicate predicate)
    {
        return this.Delete().Where(predicate).Apply();
    }
}

}
