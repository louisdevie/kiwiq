using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Provides extension methods for the <see cref="Schema"/> class so that it can be used like an
/// <see cref="ExtendedSchema"/>.
/// </summary>
public static class SchemaExtensions
{
    /// <inheritdoc cref="ExtendedSchema.Select{T}()" />
    public static MappedSelectCommand<T> Select<T>(this Schema @this)
    where T : notnull
    {
        return new MappedSelectCommand<T>(
            @this.Select(),
            new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>()
        );
    }

    /// <inheritdoc cref="ExtendedSchema.InsertInto{T}()" />
    public static MappedInsertCommand<T> InsertInto<T>(this Schema @this)
    where T : notnull
    {
        var mapper = new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>();
        return new MappedInsertCommand<T>(@this.InsertInto(mapper.FirstTable.Name), mapper);
    }

    /// <inheritdoc cref="ExtendedSchema.InsertInto{T}(string)" />
    public static MappedInsertCommand<T> InsertInto<T>(this Schema @this, string table)
    where T : notnull
    {
        return new MappedInsertCommand<T>(
            @this.InsertInto(table),
            new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>()
        );
    }

    /// <inheritdoc cref="ExtendedSchema.DeleteFrom{T}()" />
    public static MappedDeleteCommand<T> DeleteFrom<T>(this Schema @this)
    where T : notnull
    {
        var mapper = new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>();
        return new MappedDeleteCommand<T>(@this.DeleteFrom(mapper.FirstTable.Name));
    }

    /// <inheritdoc cref="ExtendedSchema.DeleteFrom{T}(string)" />
    public static MappedDeleteCommand<T> DeleteFrom<T>(this Schema @this, string table)
    where T : notnull
    {
        new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>();
        return new MappedDeleteCommand<T>(@this.DeleteFrom(table));
    }

    /// <inheritdoc cref="ExtendedSchema.Update{T}()" />
    public static MappedUpdateCommand<T> Update<T>(this Schema @this)
    where T : notnull
    {
        var mapper = new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>();
        return new MappedUpdateCommand<T>(@this.Update(mapper.FirstTable.Name), mapper);
    }

    /// <inheritdoc cref="ExtendedSchema.Update{T}(string)" />
    public static MappedUpdateCommand<T> Update<T>(this Schema @this, string table)
    where T : notnull
    {
        return new MappedUpdateCommand<T>(
            @this.Update(table),
            new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<T>()
        );
    }

    /// <inheritdoc cref="ExtendedSchema.Table{TKey, TEntity}()" />
    public static Table<TKey, TEntity> Table<TKey, TEntity>(this Schema @this)
    where TKey : notnull
    where TEntity : notnull
    {
        var mapper = new GenericMapperFactory(@this, SharedMappers.Current).MakeMapper<TEntity>();
        return new Table<TKey, TEntity>(@this, mapper.FirstTable, mapper);
    }

    /// <summary>
    /// Transforms this schema into an <see cref="ExtendedSchema" /> with the given converters.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="converters">Converters to register only for this schema.</param>
    /// <returns>An extended schema based on this schema.</returns>
    public static ExtendedSchema Using(this Schema @this, params IFieldConverter[] converters)
    {
        var extended = new ExtendedSchema(@this);
        foreach (IFieldConverter converter in converters) extended.Register(converter);
        return extended;
    }

    /// <summary>
    /// Transforms this schema into an <see cref="ExtendedSchema" /> with the given mappers.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="mappers">Mappers to register only for this schema.</param>
    /// <returns>An extended schema based on this schema.</returns>
    public static ExtendedSchema Using(this Schema @this, params IFieldMapper[] mappers)
    {
        var extended = new ExtendedSchema(@this);
        foreach (IFieldMapper mapper in mappers) extended.Register(mapper);
        return extended;
    }
}

}
