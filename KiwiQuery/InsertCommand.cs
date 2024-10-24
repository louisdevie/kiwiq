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
    public class InsertCommand : Command
    {
        /// <summary>
        /// The value returned by <see cref="Apply"/> when the table doesn't use an auto-incremented primary key. 
        /// </summary>
        public const int NO_AUTO_ID = -1;
        
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
        internal InsertCommand(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.values = new List<ValueToInsert>();
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public InsertCommand Value(Value value)
        {
            this.values.Add(new ValueToInsert(value));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public InsertCommand Value(object? value)
        {
            this.values.Add(new ValueToInsert(new Parameter(value)));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted.
        /// </summary>
        /// <param name="subQuery">The subquery to insert as a value.</param>
        public InsertCommand Value(SelectCommand subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery)));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="value">The value to insert.</param>
        public InsertCommand Value(string column, Value value)
        {
            this.values.Add(new ValueToInsert(value, column));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="value">The value to insert.</param>
        public InsertCommand Value(string column, object? value)
        {
            this.values.Add(new ValueToInsert(new Parameter(value), column));
            return this;
        }

        /// <summary>
        /// Add a value to be inserted into a specific column.
        /// </summary>
        /// <param name="column">The name of the column to insert the value into.</param>
        /// <param name="subQuery">The subquery to insert as a value.</param>
        public InsertCommand Value(string column, SelectCommand subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery), column));
            return this;
        }

        /// <inheritdoc />
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
        /// <returns>
        /// The ID (value of the primary key) of the inserted row, or <see cref="NO_AUTO_ID"/> if the primary  key is
        /// not an integer.
        /// </returns>
        public int Apply()
        {
            this.BuildCommand();
            this.DbCommand.ExecuteNonQuery();

            DbCommand selectIdCommand = this.Schema.Connection.CreateCommand();
            selectIdCommand.CommandText = QueryBuilderFactory.Current
                .NewQueryBuilder(this.Schema.CurrentDialect, this.DbCommand)
                .AppendLastInsertIdQuery()
                .ToString();
            object? id = selectIdCommand.ExecuteScalar();

            return id switch
            {
                int intId => intId,
                long longId => (int)longId,
                uint uintId => (int)uintId,
                ulong ulongId => (int)ulongId,
                _ => NO_AUTO_ID
            };
        }
    }
}
