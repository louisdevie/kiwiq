using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    public class InsertQuery : Query
    {
        private string table;

        private class ValueToInsert
        {
            string? column;
            DbParameter parameter;

            public bool HasColumn => this.column != null;

            public string Column => this.column ?? throw new InvalidOperationException("Found mixed named and unnamed columns.");

            public DbParameter Parameter => this.parameter;

            public ValueToInsert(DbParameter parameter, string? column = null)
            {
                this.column = column;
                this.parameter = parameter;
            }
        }
        private List<ValueToInsert> values;

        public InsertQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.values = new();
        }

        public InsertQuery Value(object? value)
        {
            DbParameter param = this.Command.CreateParameter();
            param.Value = value;
            this.values.Add(new ValueToInsert(param));
            return this;
        }

        public InsertQuery Value(string column, object? value)
        {
            DbParameter param = this.Command.CreateParameter();
            param.Value = value;
            this.values.Add(new ValueToInsert(param, column));
            return this;
        }

        protected override string BuildCommandText(QueryBuilder result)
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
                  .OpenBracket();

            string[] namedParameters = new string[this.values.Count];
            for (int i = 0; i < this.values.Count; i++)
            {
                namedParameters[i] = result.RegisterParameter(this.values[i].Parameter);
            }

            result.AppendCommaSeparatedNamedParameters(namedParameters)
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
            object? id = this.Command.ExecuteScalar();

            if (id is int intId) return intId;
            else if (id is long longId) return (int)longId;
            else return -1;
        }
    }
}
