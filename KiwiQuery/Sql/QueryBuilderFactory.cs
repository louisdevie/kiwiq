using System.Data.Common;
using System.Reflection;

namespace KiwiQuery.Sql
{
    internal class QueryBuilderFactory
    {
        #region Singleton

        private static QueryBuilderFactory? current;

        public static QueryBuilderFactory Current
        {
            get
            {
                if (current == null)
                {
                    current = new();
                }
                return current;
            }
        }

        #endregion

        private Dictionary<Mode, ConstructorInfo> implementations;

        private QueryBuilderFactory()
        {
            this.implementations = new();
            this.RegisterQueryBuilder(Mode.MySql, typeof(MySqlQueryBuilder));
        }

        private void RegisterQueryBuilder(Mode mode, Type implementation)
        {
            if (!implementation.IsAssignableTo(typeof(QueryBuilder)))
            {
                throw new ArgumentException($"The query builder implementation {implementation} does not inherit from KiwiQuery.Sql.QueryBuilder.");
            }

            ConstructorInfo? constructor = implementation.GetConstructor(BindingFlags.Public, new Type[1] { typeof(DbCommand) });

            if (constructor is null)
            {
                throw new InvalidOperationException($"The query builder implementation {implementation} does not have an accessible constructor with a (DbCommand) signature.");
            }

            this.implementations.Add(mode, constructor);
        }

        /// <summary>
        /// Add an implementation of <see cref="QueryBuilder"/> to the factory.
        /// </summary>
        /// <param name="implementation">The type of the query builder to add.</param>
        /// <returns>The custom <see cref="Mode"/> to use this query builder.</returns>
        public Mode RegisterCustomQueryBuilder(Type implementation)
        {
            int customModeValue = 1;
            while (this.implementations.ContainsKey((Mode)customModeValue)) customModeValue++;
            this.RegisterQueryBuilder((Mode)customModeValue, implementation);
            return (Mode)customModeValue;
        }

        /// <summary>
        /// Add an implementation of <see cref="QueryBuilder"/> to the factory.
        /// </summary>
        /// <typeparam name="T">The type of the query builder to add.</typeparam>
        /// <returns>The custom <see cref="Mode"/> to use this query builder.</returns>
        public Mode RegisterCustomQueryBuilder<T>()
        where T : QueryBuilder
        {
            return this.RegisterCustomQueryBuilder(typeof(T));
        }

        /// <summary>
        /// Creates a new query builder for the specified <see cref="Mode"/>.
        /// </summary>
        /// <param name="mode">The mode used to choose the query builder.</param>
        /// <param name="command">The command to pass to the <see cref="QueryBuilder(DbCommand)"/> constructor.</param>
        /// <returns>A new query builder attached to the given <paramref name="command"/>.</returns>
        /// <exception cref="ArgumentException"/>
        public QueryBuilder NewQueryBuilder(Mode mode, DbCommand command)
        {
            if (this.implementations.TryGetValue(mode, out ConstructorInfo? constructor))
            {
                return (QueryBuilder)constructor.Invoke(new object[1] { command });
            }
            else
            {
                throw new ArgumentException($"No implementation found for {mode}.");
            }
        }
    }
}
