using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using KiwiQuery.Sql.Dialects;

namespace KiwiQuery.Sql
{
    /// <summary>
    /// Provides <see cref="QueryBuilder"/> instances.
    /// </summary>
    public class QueryBuilderFactory
    {
        #region Singleton

        private static QueryBuilderFactory? current;

        /// <summary>
        /// The current instance of the QueryBuilderFactory in use.
        /// </summary>
        public static QueryBuilderFactory Current
        {
            get
            {
                if (current == null)
                {
                    current = new QueryBuilderFactory();
                }
                return current;
            }
        }

        #endregion

        private readonly Dictionary<Dialect, ConstructorInfo> implementations;

        private QueryBuilderFactory()
        {
            this.implementations = new Dictionary<Dialect, ConstructorInfo>();
            this.RegisterQueryBuilder(Dialect.MySql, typeof(MySqlQueryBuilder));
            this.RegisterQueryBuilder(Dialect.Sqlite, typeof(SqliteQueryBuilder));
        }

        private void RegisterQueryBuilder(Dialect dialect, Type implementation)
        {
            if (!typeof(QueryBuilder).IsAssignableFrom(implementation))
            {
                throw new ArgumentException($"The query builder implementation {implementation} does not inherit from KiwiQuery.Sql.QueryBuilder.");
            }

            ConstructorInfo? constructor = implementation.GetConstructor(new Type[1] { typeof(DbCommand) })
                ?? throw new InvalidOperationException($"The query builder implementation {implementation} does not have an accessible constructor with a (DbCommand) signature.");
            
            this.implementations.Add(dialect, constructor);
        }

        /// <summary>
        /// Add an implementation of <see cref="QueryBuilder"/> to the factory.
        /// </summary>
        /// <param name="implementation">The type of the query builder to add.</param>
        /// <returns>The custom <see cref="Dialect"/> to use this query builder.</returns>
        public Dialect RegisterCustomQueryBuilder(Type implementation)
        {
            Dialect newDialect = Dialect.ForClass(implementation);
            this.RegisterQueryBuilder(newDialect, implementation);
            return newDialect;
        }

        /// <summary>
        /// Add an implementation of <see cref="QueryBuilder"/> to the factory.
        /// </summary>
        /// <typeparam name="T">The type of the query builder to add.</typeparam>
        /// <returns>The custom <see cref="Dialect"/> to use this query builder.</returns>
        public Dialect RegisterCustomQueryBuilder<T>()
        where T : QueryBuilder
        {
            return this.RegisterCustomQueryBuilder(typeof(T));
        }

        /// <summary>
        /// Check if a query builder implementation is available for a specific mode.
        /// </summary>
        /// <param name="dialect">The mode to check for.</param>
        /// <returns><see langword="true"/> if the mode is supported, <see langword="false"/> otherwise.</returns>
        public bool SupportsMode(Dialect dialect)
        {
            return this.implementations.ContainsKey(dialect);
        }

        /// <summary>
        /// Creates a new query builder for the specified <see cref="Dialect"/>.
        /// </summary>
        /// <param name="dialect">The mode used to choose the query builder.</param>
        /// <param name="command">The command to pass to the <see cref="QueryBuilder(DbCommand)"/> constructor.</param>
        /// <returns>A new query builder attached to the given <paramref name="command"/>.</returns>
        /// <exception cref="ArgumentException"/>
        public QueryBuilder NewQueryBuilder(Dialect dialect, DbCommand command)
        {
            if (this.implementations.TryGetValue(dialect, out ConstructorInfo? constructor))
            {
                return (QueryBuilder)constructor.Invoke(new object[1] { command });
            }
            else
            {
                throw new ArgumentException($"No implementation found for {dialect}.");
            }
        }
    }
}
