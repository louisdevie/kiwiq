using System;
using System.Collections.Generic;
using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped
{
    /// <summary>
    /// A SQL SELECT command with its results mapped to objects. <br/>
    /// Instances of this class should be created from a <see cref="Schema"/>.
    /// </summary>
    public class MappedSelectQuery<T>
    where T : notnull
    {
        private readonly SelectQuery rawQuery;
        private readonly IMapper<T> mapper;
        private bool explicitTables;

        internal MappedSelectQuery(SelectQuery rawQuery, IMapper<T> mapper)
        {
            this.rawQuery = rawQuery;
            this.mapper = mapper;
            this.explicitTables = false;
        }

        /// <summary>
        /// Add columns aliases to be selected. <br/>
        /// This method is useful when using JOINs to discriminate two columns with the same name or to map a field to
        /// a computed value.
        /// </summary>
        /// <param name="columns">The columns and values to select.</param>
        /// <exception cref="ArgumentException">If a column without alias was given.</exception>
        public MappedSelectQuery<T> With(params Column[] columns)
        {
            foreach (Column column in columns)
            {
                if (column.Alias == null)
                {
                    throw new ArgumentException("Expected a column alias.", nameof(columns));
                }
            }

            return this;
        }

        /// <inheritdoc cref="SelectQuery.From(string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> From(string table)
        {
            this.rawQuery.From(table);
            this.explicitTables = true;
            return this;
        }

        /// <inheritdoc cref="From(string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> From(Table table)
        {
            this.rawQuery.From(table);
            this.explicitTables = true;
            return this;
        }

        /// <summary>
        /// Build the command, execute it and fetch the results one by one.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> that returns the mapped rows.
        /// </returns>
        public IEnumerator<T> Fetch(bool buffered = true)
        {
            IEnumerator<T> enumerator;

            if (buffered)
            {
                enumerator = this.FetchBuffered().GetEnumerator();
            }
            else
            {
                enumerator = this.FetchUnbuffered();
            }

            return enumerator;
        }

        /// <summary>
        /// Build the command, execute it and fetch the results into a list.
        /// </summary>
        /// <returns>
        /// A <see cref="List{T}"/> that contains all the mapped rows.
        /// </returns>
        public List<T> FetchList()
        {
            return this.FetchBuffered();
        }

        private List<T> FetchBuffered()
        {
            List<T> results = new List<T>();
            using var reader = this.FetchRaw();
            {
                results.Add(this.mapper.RowToObject(reader));
            }
            return results;
        }

        private UnbufferedReader<T> FetchUnbuffered()
        {
            return new UnbufferedReader<T>(this.FetchRaw());
        }

        private DbDataReader FetchRaw()
        {
            this.FillQuery();
            return this.rawQuery.Fetch();
        }
        
        private void FillQuery()
        {
            
        }

        #if FUTURE // available from 0.5.0
        /// <inheritdoc cref="SelectQuery.Distinct()"/>
        public MappedSelectQuery<T> Distinct()
        {
            this.rawQuery.Distinct();
            return this;
        }
        #endif

        #region JOIN clause methods

        /// <inheritdoc cref="SelectQuery.Join(Table, Column, Column)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> Join(Table table, Column firstColumn, Column secondColumn)
        {
            this.rawQuery.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.Join(Table, string, string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> Join(Table table, string firstColumn, string secondColumn)
        {
            this.rawQuery.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.Join(string, string, string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> Join(string table, string firstColumn, string secondColumn)
        {
            this.rawQuery.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.Join(Column, Column)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> Join(Column columnToJoin, Column matchingColumn)
        {
            this.rawQuery.Join(columnToJoin, matchingColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.LeftJoin(Table, Column, Column)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> LeftJoin(Table table, Column firstColumn, Column secondColumn)
        {
            this.rawQuery.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.LeftJoin(Table, string, string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> LeftJoin(Table table, string firstColumn, string secondColumn)
        {
            this.rawQuery.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.LeftJoin(string, string, string)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> LeftJoin(string table, string firstColumn, string secondColumn)
        {
            this.rawQuery.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.LeftJoin(Column, Column)"/>
        /// <remarks>If this method is used, all tables must be declared explicitly.</remarks>
        public MappedSelectQuery<T> LeftJoin(Column columnToJoin, Column matchingColumn)
        {
            this.rawQuery.LeftJoin(columnToJoin, matchingColumn);
            return this;
        }

        #endregion

        #region WHERE clause methods

        /// <inheritdoc cref="SelectQuery.Where(Predicate)"/>
        public MappedSelectQuery<T> Where(Predicate predicate)
        {
            this.rawQuery.Where(predicate);
            return this;
        }

        #endregion

        #region LIMIT clause methods

        /// <inheritdoc cref="SelectQuery.Limit(int)"/>
        public MappedSelectQuery<T> Limit(int limit)
        {
            this.rawQuery.Limit(limit);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.Limit(int, int)"/>
        public MappedSelectQuery<T> Limit(int limit, int offset)
        {
            this.rawQuery.Limit(limit, offset);
            return this;
        }

        /// <inheritdoc cref="SelectQuery.Offset(int)"/>
        public MappedSelectQuery<T> Offset(int offset)
        {
            this.rawQuery.Offset(offset);
            return this;
        }

        #endregion
    }
}