using System;
using System.Collections.Generic;
using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Builtin;
using KiwiQuery.Mapped.Mappers.Fields;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Represents a database schema. This class is the root class from which you can build commands and other objects.
/// An <em>extended schema</em> implements <see cref="IFieldMapperCollection"/>, so you can register converters or
/// mappers for its lifetime.
/// </summary>
public class ExtendedSchema : IFieldMapperCollection
{
    #region Attributes and constructors

    private readonly Schema schema;
    private readonly List<IFieldMapper> fieldMappers;
    private readonly Dictionary<Type, IFieldMapper> resolvedFieldMappers;
    private readonly GenericMapperFactory mapperFactory;
    private readonly Dictionary<Type, IMapper> existingMappers;

    /// <inheritdoc cref="Schema(DbConnection, Dialect)"/>
    public ExtendedSchema(DbConnection connection, Dialect dialect) : this(new Schema(connection, dialect)) { }

    /// <inheritdoc cref="Schema(DbConnection)"/>
    public ExtendedSchema(DbConnection connection) : this(new Schema(connection)) { }

    /// <summary>
    /// Creates a new schema inheriting its behavior from a <see cref="Schema"/>.
    /// </summary>
    public ExtendedSchema(Schema schema)
    {
        this.schema = schema;
        this.fieldMappers = new List<IFieldMapper>();
        this.resolvedFieldMappers = new Dictionary<Type, IFieldMapper>();
        this.mapperFactory = new GenericMapperFactory(this.schema, this);
        this.existingMappers = new Dictionary<Type, IMapper>();
    }

    #endregion

    #region Mapped queries creation

    /// <summary>
    /// Creates a new mapped SELECT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>A <see cref="MappedSelectQuery{T}"/> that can be further configured and then executed.</returns>
    public MappedSelectQuery<T> Select<T>()
    where T : notnull
    {
        return new MappedSelectQuery<T>(this.schema.Select(), this.GetMapper<T>());
    }

    /// <summary>
    /// Creates a new mapped INSERT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to create.</typeparam>
    /// <returns>An <see cref="MappedInsertQuery{T}"/> that can be further configured and then executed.</returns>
    public MappedInsertQuery<T> InsertInto<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedInsertQuery<T>(this.schema.InsertInto(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new mapped INSERT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to create.</typeparam>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>An <see cref="MappedInsertQuery{T}"/> that can be further configured and then executed.</returns>
    public MappedInsertQuery<T> InsertInto<T>(string table)
    where T : notnull
    {
        return new MappedInsertQuery<T>(this.schema.InsertInto(table), this.mapperFactory.MakeMapper<T>());
    }

    /// <summary>
    /// Creates a new mapped DELETE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to delete.</typeparam>
    /// <returns>A <see cref="DeleteQuery"/> that can be further configured and then executed.</returns>
    public MappedDeleteQuery<T> DeleteFrom<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedDeleteQuery<T>(this.schema.DeleteFrom(mapper.FirstTable.Name));
    }

    /// <summary>
    /// Creates a new mapped DELETE command for a type.
    /// </summary> on the given table
    /// <typeparam name="T">The type of entity to delete.</typeparam>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>A <see cref="DeleteQuery"/> that can be further configured and then executed.</returns>
    public MappedDeleteQuery<T> DeleteFrom<T>(string table)
    where T : notnull
    {
        _ = this.GetMapper<T>(); // create and cache the mapper anyway
        return new MappedDeleteQuery<T>(this.schema.DeleteFrom(table));
    }

    /// <summary>
    /// Creates a new mapped UPDATE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <returns>An <see cref="MappedUpdateQuery{T}"/> that can be further configured and then executed.</returns>
    public MappedUpdateQuery<T> Update<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedUpdateQuery<T>(this.schema.Update(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new mapped UPDATE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <param name="table">The name of the table that will be updated.</param>
    /// <returns>An <see cref="MappedUpdateQuery{T}"/> that can be further configured and then executed.</returns>
    public MappedUpdateQuery<T> Update<T>(string table)
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedUpdateQuery<T>(this.schema.Update(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new repository for a type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to read.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the primary key. If it is a single column, the type of that column is expected. Otherwise, you will
    /// need to redefine the operations that work with a primary key.
    /// </typeparam>
    public Table<TKey, TEntity> Table<TKey, TEntity>()
    where TKey : notnull
    where TEntity : notnull
    {
        var mapper = this.GetMapper<TEntity>();
        return new Table<TKey, TEntity>(this.schema, mapper.FirstTable, mapper);
    }

    #endregion

    #region IFieldMapperCollection implementation

    /// <inheritdoc />
    public void Register(IConverter converter)
    {
        this.Register(new ConverterMapper(converter));
    }

    /// <inheritdoc />
    public void Register(IFieldMapper mapper)
    {
        this.fieldMappers.Add(mapper);
    }

    IFieldMapper IFieldMapperCollection.GetMapper(Type fieldType, IColumnInfo info)
    {
        if (!this.resolvedFieldMappers.TryGetValue(fieldType, out IFieldMapper? mapper))
        {
            mapper = DefaultMapperResolver.ResolveFromList(this.fieldMappers, fieldType, info);
            this.resolvedFieldMappers.Add(fieldType, mapper);
        }
        return mapper;
    }

    private IMapper<T> GetMapper<T>()
    where T : notnull
    {
        IMapper<T> mapper;
        Type entityType = typeof(T);
        if (this.existingMappers.TryGetValue(entityType, out IMapper? existingMapper))
        {
            mapper = (IMapper<T>)existingMapper;
        }
        else
        {
            mapper = this.mapperFactory.MakeMapper<T>();
            this.existingMappers.Add(entityType, mapper);
        }
        return mapper;
    }

    #endregion

    #region Schema proxy

    /// <inheritdoc cref="Schema.InsertInto(string)"/>
    public InsertQuery InsertInto(string table) => this.schema.InsertInto(table);

    /// <inheritdoc cref="Schema.DeleteFrom(string)"/>
    public DeleteQuery DeleteFrom(string table) => this.schema.DeleteFrom(table);

    /// <inheritdoc cref="Schema.Update(string)"/>
    public UpdateQuery Update(string table) => this.schema.Update(table);

    /// <inheritdoc cref="Schema.Select(string[])"/>
    public SelectQuery Select(params string[] columns) => this.schema.Select(columns);

    /// <inheritdoc cref="Schema.Select(Value[])"/>
    public SelectQuery Select(params Value[] columns) => this.schema.Select(columns);

    /// <inheritdoc cref="Schema.Select()"/>
    public SelectQuery Select() => this.schema.Select();

#pragma warning disable CA1822 // Static members suggestion
// ReSharper disable MemberCanBeMadeStatic.Global

    /// <inheritdoc cref="Schema.Table(string)"/>
    public Table Table(string name) => this.schema.Table(name);

    /// <inheritdoc cref="Schema.Column(string)"/>
    public Column Column(string name) => this.schema.Column(name);

    /// <inheritdoc cref="Schema.SubQuery(SelectQuery)"/>
    public SubQuery SubQuery(SelectQuery query) => this.schema.SubQuery(query);

#pragma warning restore CA1822
// ReSharper restore MemberCanBeMadeStatic.Global

    #endregion
}

}
