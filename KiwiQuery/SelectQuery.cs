using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace KiwiQuery
{
    /// <summary>
    /// A SQL SELECT command. <br/>
    /// Instances of this class should be created from a <see cref="Schema"/>.
    /// </summary>
    public class SelectQuery : Query, IWriteable
    {
        private Table? table;
        private bool distinct;
        private readonly WhereClauseBuilder whereClauseBuilder;
        private readonly JoinClauseBuilder joinClauseBuilder;
        private readonly LimitClauseBuilder limitClauseBuilder;
        private readonly List<Value> projection;

        /// <summary>
        /// Creates a new SELECT command.
        /// </summary>
        /// <param name="projection">The columns to select.</param>
        /// <param name="schema">The schema to execute the command on.</param>
        internal SelectQuery(IEnumerable<Value> projection, Schema schema) : base(schema)
        {
            this.table = null;
            this.distinct = false;
            this.whereClauseBuilder = new WhereClauseBuilder();
            this.joinClauseBuilder = new JoinClauseBuilder(schema);
            this.limitClauseBuilder = new LimitClauseBuilder();
            this.projection = projection.ToList();
        }

        /// <summary>
        /// Add more columns to be selected. <br/>
        /// This method is useful for breaking down long select statements and
        /// combining simple string columns with <see cref="Column"/> objects.
        /// </summary>
        /// <param name="columns">The columns and values to select.</param>
        public SelectQuery And(params Value[] columns)
        {
            foreach (Value column in columns)
            {
                this.projection.Add(column);
            }
            return this;
        }

        /// <inheritdoc cref="And(Value[])"/>
        public SelectQuery And(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.projection.Add(this.Schema.Column(column));
            }
            return this;
        }

        /// <summary>
        /// Chooses the first table to select from. Other tables can be joined later.
        /// </summary>
        /// <param name="table">The first table to select from.</param>
        public SelectQuery From(string table)
        {
            this.table = this.Schema.Table(table);
            return this;
        }

        /// <inheritdoc cref="From(string)"/>
        public SelectQuery From(Table table)
        {
            this.table = table;
            return this;
        }

        /// <inheritdoc/>
        public void WriteTo(QueryBuilder result)
        {
            if (this.table is null) throw new InvalidOperationException("No table specified.");
            
            result.AppendSelectKeyword();

            if (this.distinct)
            {
                result.AppendDistinctKeyword();
            }

            if (this.projection.Count == 0)
            {
                result.AppendAllColumnsWildcard();
            }
            else
            {
                result.PushContext.WithTableAliases();
                result.AppendCommaSeparatedElements(this.projection);
                result.PopContext();
            }

            result.PushContext.DeclaringTables();
            result.AppendFromKeyword();
            this.table.WriteTo(result);
            this.joinClauseBuilder.WriteClauseTo(result);
            result.PopContext();
            
            result.PushContext.WithTableAliases();
            this.whereClauseBuilder.WriteClauseTo(result);
            this.limitClauseBuilder.WriteClauseTo(result);
            result.PopContext();
        }

        /// <inheritdoc/>
        protected override string BuildCommandText(QueryBuilder result)
        {
            this.WriteTo(result);
            return result.ToString();
        }

        /// <summary>
        /// Build the command, execute it and fetch the results.
        /// </summary>
        /// <returns>
        /// An open <see cref="DbDataReader"/>. If you need the data reader
        /// specific to the connector you're using, <br/> you can use the
        /// generic overload <see cref="Fetch{TReader}()"/> instead.
        /// </returns>
        public DbDataReader Fetch()
        {
            this.BuildCommand();
            return this.Command.ExecuteReader();
        }

        /// <summary>
        /// Build the command, execute it and fetch the results.
        /// </summary>
        /// <typeparam name="TReader">The scpecific type of data reader returned by the connector.</typeparam>
        /// <returns>
        /// An open <typeparamref name="TReader"/>. If you don't need the data
        /// reader specific to the connector you're using, <br/> you can use
        /// the basic overload <see cref="Fetch()"/> instead.
        /// </returns>
        public TReader Fetch<TReader>() where TReader : DbDataReader => (TReader)this.Fetch();

        /// <summary>
        /// Remove duplicates from the query results.
        /// </summary>
        /// <added>0.5.0</added>
        public SelectQuery Distinct()
        {
            this.distinct = true;
            return this;
        }
        
        #region JOIN clause methods

        /// <inheritdoc cref="JoinClauseBuilder.Join(Table, Column, Column)"/>
        public SelectQuery Join(Table table, Column firstColumn, Column secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.Join(Table, string, string)"/>
        public SelectQuery Join(Table table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.Join(string, string, string)"/>
        public SelectQuery Join(string table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.Join(Column, Column)"/>
        public SelectQuery Join(Column columnToJoin, Column matchingColumn)
        {
            this.joinClauseBuilder.Join(columnToJoin, matchingColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.LeftJoin(Table, Column, Column)"/>
        public SelectQuery LeftJoin(Table table, Column firstColumn, Column secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.LeftJoin(Table, string, string)"/>
        public SelectQuery LeftJoin(Table table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.LeftJoin(string, string, string)"/>
        public SelectQuery LeftJoin(string table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        /// <inheritdoc cref="JoinClauseBuilder.LeftJoin(Column, Column)"/>
        public SelectQuery LeftJoin(Column columnToJoin, Column matchingColumn)
        {
            this.joinClauseBuilder.LeftJoin(columnToJoin, matchingColumn);
            return this;
        }

        #endregion

        #region WHERE clause methods

        /// <inheritdoc cref="WhereClauseBuilder.Where(Predicate)"/>
        public SelectQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }

        #endregion

        #region LIMIT clause methods

        /// <inheritdoc cref="LimitClauseBuilder.Limit(int)"/>
        public SelectQuery Limit(int limit)
        {
            this.limitClauseBuilder.Limit(limit);
            return this;
        }

        /// <inheritdoc cref="LimitClauseBuilder.Limit(int, int)"/>
        public SelectQuery Limit(int limit, int offset)
        {
            this.limitClauseBuilder.Limit(limit, offset);
            return this;
        }

        /// <inheritdoc cref="LimitClauseBuilder.Offset(int)"/>
        public SelectQuery Offset(int offset)
        {
            this.limitClauseBuilder.Offset(offset);
            return this;
        }

        #endregion
    }
}
