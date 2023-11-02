using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace KiwiQuery
{
    /// <summary>
    /// A SQL INSERT command. <br/>
    /// Instances of this class should be created from a <see cref="Schema"/>.
    /// </summary>
    public class InsertQuery : Query
    {
        private string table;

        private class ValueToInsert
        {
            private string? column;
            private Value value;

            public bool HasColumn => this.column != null;

            public string Column => this.column ?? throw new InvalidOperationException("Found mixed named and unnamed columns.");

            public Value Value => this.value;

            public ValueToInsert(Value value, string? column = null)
            {
                this.column = column;
                this.value = value;
            }
        }
        private List<ValueToInsert> values;

        /// <summary>
        /// Creates a new INSERT command.
        /// </summary>
        /// <param name="table">The table to insert into.</param>
        /// <param name="schema">The schema to execute the command on.</param>
        internal InsertQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.values = new List<ValueToInsert>();
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public InsertQuery Value(Value value)
        {
            this.values.Add(new ValueToInsert(value));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public InsertQuery Value(object? value)
        {
            this.values.Add(new ValueToInsert(new Parameter(value)));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="subQuery">The subquery to insert as a value.</param>
        public InsertQuery Value(SelectQuery subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery)));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="value">The value to insert.</param>
        public InsertQuery Value(string column, Value value)
        {
            this.values.Add(new ValueToInsert(value, column));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="value">The value to insert.</param>
        public InsertQuery Value(string column, object? value)
        {
            this.values.Add(new ValueToInsert(new Parameter(value), column));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="subQuery">The subquery to insert as a value.</param>
        public InsertQuery Value(string column, SelectQuery subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery), column));
            return this;
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            result.AppendInsertIntoKeywords()
                  .AppendTableOrColumnName(this.table);

            if (this.values.Count == 0)
            {
                throw new InvalidOperationException("No values to insert.");
            }

            if (this.values.First().HasColumn)
            {
                result.OpenBracket()
                      .AppendCommaSeparatedColumnNames(this.values.Select(valueToInsert => valueToInsert.Column))
                      .CloseBracket();
            }

            result.AppendValuesKeyword()
                  .OpenBracket()
                  .AppendCommaSeparatedElements(this.values.Select(valueToInsert => valueToInsert.Value))
                  .CloseBracket();

            return result.ToString();
        }

        /// <summary>
        /// Build and execute the command.
        /// </summary>
        /// <returns>The ID (value of the primary key) of the inserted row, or -1 of the primary key is not an integer.</returns>
        public int Apply()
        {
            this.BuildCommand();
            this.Command.ExecuteNonQuery();

            DbCommand selectIdCommand = this.Schema.Connection.CreateCommand();
            selectIdCommand.CommandText = QueryBuilderFactory.Current
                .NewQueryBuilder(this.Schema.Mode, this.Command)
                .AppendLastInsertIdQuery()
                .ToString();
            object? id = selectIdCommand.ExecuteScalar();

            if (id is int intId) return intId;
            else if (id is long longId) return (int)longId;
            else if (id is uint uintId) return (int)uintId;
            else if (id is ulong ulongId) return (int)ulongId;
            else return -1;
        }
    }
}
