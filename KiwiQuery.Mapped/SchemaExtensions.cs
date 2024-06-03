using System;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Provides extension methods for the <see cref="Schema"/> class.
/// </summary>
public static class SchemaExtensions
{
    /// <summary>
    /// Creates a new mapped SELECT command for an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>A <see cref="MappedSelectQuery{T}"/> that can be further configured and then executed.</returns>
    public static MappedSelectQuery<T> Select<T>(this Schema @this) where T : notnull
    {
        return new MappedSelectQuery<T>(@this.Select(Array.Empty<string>()), CachedMapper.For<T>());
    }

    /// <summary>
    /// Creates a new mapped INSERT command for an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>An <see cref="InsertQuery"/> that can be further configured and then executed.</returns>
    public static MappedInsertQuery<T> InsertInto<T>(this Schema @this) where T : notnull
    {
        var mapper = CachedMapper.For<T>();
        return new MappedInsertQuery<T>(@this.InsertInto(mapper.TableName), mapper);
    }

    /// <summary>
    /// Creates a new mapped INSERT command for an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <param name="this"></param>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>An <see cref="InsertQuery"/> that can be further configured and then executed.</returns>
    public static MappedInsertQuery<T> InsertInto<T>(this Schema @this, string table) where T : notnull
    {
        return new MappedInsertQuery<T>(@this.InsertInto(table), CachedMapper.For<T>());
    }
    
    /// <summary>
    /// Creates a new mapped DELETE command for an entity.
    /// </summary> on the given table
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>A <see cref="DeleteQuery"/> that can be further configured and then executed.</returns>
    public static MappedDeleteQuery<T> DeleteFrom<T>(this Schema @this) where T : notnull
    {
        var mapper = CachedMapper.For<T>();
        return new MappedDeleteQuery<T>(@this.DeleteFrom(mapper.TableName));
    }
    
    /// <summary>
    /// Creates a new mapped DELETE command for an entity.
    /// </summary> on the given table
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <param name="this"></param>
    /// <param name="table">The name of the table into which the values will be inserted.</param>
    /// <returns>A <see cref="DeleteQuery"/> that can be further configured and then executed.</returns>
    public static MappedDeleteQuery<T> DeleteFrom<T>(this Schema @this, string table) where T : notnull
    {
        CachedMapper.For<T>(); // create and cache the mapper anyway
        return new MappedDeleteQuery<T>(@this.DeleteFrom(table));
    }

    /// <summary>
    /// Creates a new mapped UPDATE command for an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity to read.</typeparam>
    /// <returns>An <see cref="UpdateQuery"/> that can be further configured and then executed.</returns>
    public static  MappedUpdateQuery<T> Update<T>(this Schema @this) where T : notnull
    {
        var mapper = CachedMapper.For<T>();
        return new MappedUpdateQuery<T>(@this.Update(mapper.TableName), mapper);
    }
}

}
