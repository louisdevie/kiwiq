using KiwiQuery.Clauses;
using KiwiQuery.Predicates;
using KiwiQuery.Sql;
using System.Data.Common;
using System.Runtime.ExceptionServices;

namespace KiwiQuery
{
    public class UpdateQuery : Query
    {
        private string table;
        private WhereClauseBuilder whereClauseBuilder;

        private class ValueToUpdate
        {
            string column;
            DbParameter parameter;

            public bool HasColumn => this.column != null;

            public string Column => this.column;

            public DbParameter Parameter => this.parameter;

            public ValueToUpdate(DbParameter parameter, string column)
            {
                this.column = column;
                this.parameter = parameter;
            }
        }
        private List<ValueToUpdate> values;

        public UpdateQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.whereClauseBuilder = new();
            this.values = new();
        }

        public UpdateQuery Set(string column, object? value)
        {
            DbParameter param = this.Command.CreateParameter();
            param.Value = value;
            this.values.Add(new ValueToUpdate(param, column));
            return this;
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            result.AppendUpdateKeyword()
                  .AppendTableOrColumnName(this.table)
                  .AppendSetKeyword();

            if (this.values.Count == 0)
            {
                throw new InvalidOperationException("No values to update.");
            }

            bool firstValue = true;
            foreach (var value in this.values)
            {
                if (firstValue)
                {
                    firstValue = false;
                }
                else
                {
                    result.AppendComma();
                }

                string param = result.RegisterParameter(value.Parameter);

                result.AppendTableOrColumnName(value.Column)
                      .AppendSetClauseAssignment()
                      .AppendRaw(param);
            }

            this.whereClauseBuilder.WriteClauseTo(result);

            return result.ToString();
        }

        public bool Apply()
        {
            this.BuildCommand();
            int affectedRows = this.Command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        #region WHERE clause methods

        public UpdateQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }

        #endregion
    }
}
