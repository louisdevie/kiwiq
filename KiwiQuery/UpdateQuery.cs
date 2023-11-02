using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
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
            private string column;
            private Value value;

            public bool HasColumn => this.column != null;

            public string Column => this.column;

            public Value Value => this.value;

            public ValueToUpdate(Value value, string column)
            {
                this.column = column;
                this.value = value;
            }
        }
        private List<ValueToUpdate> values;

        public UpdateQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.whereClauseBuilder = new();
            this.values = new();
        }

        public UpdateQuery Set(string column, Value value)
        {
            this.values.Add(new ValueToUpdate(value, column));
            return this;
        }

        public UpdateQuery Set(string column, object? value)
        {
            this.values.Add(new ValueToUpdate(new Parameter(value), column));
            return this;
        }

        public UpdateQuery Set(string column, SelectQuery value)
        {
            this.values.Add(new ValueToUpdate(new SubQuery(value), column));
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

                result.AppendTableOrColumnName(value.Column)
                      .AppendSetClauseAssignment();
                value.Value.WriteTo(result);
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
