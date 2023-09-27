using KiwiQuery.Clauses;
using KiwiQuery.Predicates;
using KiwiQuery.Sql;
using System.Runtime.CompilerServices;

namespace KiwiQuery
{
    public class DeleteQuery : Query
    {
        private string table;
        private WhereClauseBuilder whereClauseBuilder;

        public DeleteQuery(string table, Schema schema) : base(schema) 
        {
            this.table = table;
            this.whereClauseBuilder = new();
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            result.AppendDeleteFromKeywords()
                  .AppendTableOrColumnName(this.table);

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

        public DeleteQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }

        #endregion
    }
}
