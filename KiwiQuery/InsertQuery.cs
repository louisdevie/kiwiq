using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
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
        

        public InsertQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.values = new();
        }

        public InsertQuery Value(Value value)
        {
            this.values.Add(new ValueToInsert(value));
            return this;
        }

        public InsertQuery Value(object? value)
        {
            DbParameter param = this.Command.CreateParameter();
            param.Value = value;
            this.values.Add(new ValueToInsert(new Parameter(param)));
            return this;
        }

        public InsertQuery Value(SelectQuery subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery)));
            return this;
        }

        public InsertQuery Value(string column, Value value)
        {
            this.values.Add(new ValueToInsert(value, column));
            return this;
        }

        public InsertQuery Value(string column, object? value)
        {
            DbParameter param = this.Command.CreateParameter();
            param.Value = value;
            this.values.Add(new ValueToInsert(new Parameter(param), column));
            return this;
        }

        public InsertQuery Value(string column, SelectQuery subQuery)
        {
            this.values.Add(new ValueToInsert(new SubQuery(subQuery), column));
            return this;
        }

        internal override string BuildCommandText(QueryBuilder result)
        {
            result.AppendInsertIntoKeywords()
                  .AppendTableOrColumnName(table);

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

        public int Apply()
        {
            this.BuildCommand();
            this.Command.ExecuteNonQuery();

            DbCommand selectIdCommand = this.Schema.Connection.CreateCommand();
            selectIdCommand.CommandText = this.Schema
                .QueryBuilderFactory
                .NewQueryBuilder(this.Command)
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
