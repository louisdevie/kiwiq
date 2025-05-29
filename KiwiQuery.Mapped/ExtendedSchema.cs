using System;
using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Commands;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Fields;

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
    private readonly FieldMapperCollection fieldMappers;
    private readonly GenericMapperFactory mapperFactory;

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
        this.fieldMappers = new FieldMapperCollection(SharedMappers.Current);
        this.mapperFactory = new GenericMapperFactory(this.schema, this);
    }

    #endregion

    #region Mapped queries creation

    /// <summary>
    /// Creates a new mapped SELECT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>A <see cref="MappedSelectCommand{T}"/> that can be further configured and then executed.</returns>
    public MappedSelectCommand<T> Select<T>()
    where T : notnull
    {
        return new MappedSelectCommand<T>(this.schema.Select(), this.GetMapper<T>());
    }

    /// <summary>
    /// Creates a new mapped INSERT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to create.</typeparam>
    /// <returns>An <see cref="MappedInsertCommand{T}"/> that can be further configured and then executed.</returns>
    public MappedInsertCommand<T> InsertInto<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedInsertCommand<T>(this.schema.InsertInto(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new mapped INSERT command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to create.</typeparam>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>An <see cref="MappedInsertCommand{T}"/> that can be further configured and then executed.</returns>
    public MappedInsertCommand<T> InsertInto<T>(string table)
    where T : notnull
    {
        return new MappedInsertCommand<T>(this.schema.InsertInto(table), this.mapperFactory.MakeMapper<T>());
    }

    /// <summary>
    /// Creates a new mapped DELETE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to delete.</typeparam>
    /// <returns>A <see cref="DeleteCommand"/> that can be further configured and then executed.</returns>
    public MappedDeleteCommand<T> DeleteFrom<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedDeleteCommand<T>(this.schema.DeleteFrom(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new mapped DELETE command for a type.
    /// </summary> on the given table
    /// <typeparam name="T">The type of entity to delete.</typeparam>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>A <see cref="DeleteCommand"/> that can be further configured and then executed.</returns>
    public MappedDeleteCommand<T> DeleteFrom<T>(string table)
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedDeleteCommand<T>(this.schema.DeleteFrom(table), mapper);
    }

    /// <summary>
    /// Creates a new mapped UPDATE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <returns>An <see cref="MappedUpdateCommand{T}"/> that can be further configured and then executed.</returns>
    public MappedUpdateCommand<T> Update<T>()
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedUpdateCommand<T>(this.schema.Update(mapper.FirstTable.Name), mapper);
    }

    /// <summary>
    /// Creates a new mapped UPDATE command for a type.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <param name="table">The name of the table that will be updated.</param>
    /// <returns>An <see cref="MappedUpdateCommand{T}"/> that can be further configured and then executed.</returns>
    public MappedUpdateCommand<T> Update<T>(string table)
    where T : notnull
    {
        var mapper = this.GetMapper<T>();
        return new MappedUpdateCommand<T>(this.schema.Update(mapper.FirstTable.Name), mapper);
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
    public void Register(IFieldMapper mapper)
    {
        this.fieldMappers.Register(mapper);
    }
    
    /// <inheritdoc />
    public void Register(IFieldConverter converter)
    {
        this.fieldMappers.Register(converter);
    }

    IFieldMapper IFieldMapperCollection.GetMapper(Type fieldType, IColumnInfo info, IFieldMapperCollection topCollection)
    {
        return ((IFieldMapperCollection) this.fieldMappers).GetMapper(fieldType, info, topCollection);
    }

    private GenericMapper<T> GetMapper<T>()
    where T : notnull
    {
        return this.mapperFactory.MakeMapper<T>();
    }

    #endregion

    #region Schema proxy

    /// <inheritdoc cref="Schema.InsertInto(string)"/>
    public InsertCommand InsertInto(string table) => this.schema.InsertInto(table);

    /// <inheritdoc cref="Schema.DeleteFrom(string)"/>
    public DeleteCommand DeleteFrom(string table) => this.schema.DeleteFrom(table);

    /// <inheritdoc cref="Schema.Update(string)"/>
    public UpdateCommand Update(string table) => this.schema.Update(table);

    /// <inheritdoc cref="Schema.Select(string[])"/>
    public SelectCommand Select(params string[] columns) => this.schema.Select(columns);

    /// <inheritdoc cref="Schema.Select(Value[])"/>
    public SelectCommand Select(params Value[] columns) => this.schema.Select(columns);

    /// <inheritdoc cref="Schema.Select()"/>
    public SelectCommand Select() => this.schema.Select();

#pragma warning disable CA1822 // Static members suggestion
// ReSharper disable MemberCanBeMadeStatic.Global

    /// <inheritdoc cref="Schema.Table(string)"/>
    public Table Table(string name) => this.schema.Table(name);

    /// <inheritdoc cref="Schema.Column(string)"/>
    public Column Column(string name) => this.schema.Column(name);

#pragma warning restore CA1822
// ReSharper restore MemberCanBeMadeStatic.Global

    #endregion
}

}
