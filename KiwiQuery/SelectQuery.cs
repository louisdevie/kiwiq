using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Predicates;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery
{
    public class SelectQuery : Query
    {
        private string? table;
        private WhereClauseBuilder whereClauseBuilder;
        private List<Column> selection;

        public SelectQuery(IEnumerable<Column> selection, Schema schema) : base(schema)
        {
            this.table = null;
            this.whereClauseBuilder = new();
            this.selection = selection.ToList();
        }

        public SelectQuery And(params Column[] columns)
        {
            foreach (Column column in columns)
            {
                this.selection.Add(column);
            }
            return this;
        }

        public SelectQuery And(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.selection.Add(this.Schema.Column(column));
            }
            return this;
        }

        public SelectQuery From(string table)
        {
            this.table = table;
            return this;
        }
        public SelectQuery From(Table table)
        {
            this.table = table.Name;
            return this;
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            result.AppendSelectKeyword();

            if (this.selection.Count == 0)
            {
                result.AppendAllColumnsWildcard();
            }
            else
            {
                result.AppendCommaSeparatedElements(this.selection);
            }

            result.AppendFromKeyword();

            if (this.table is null) throw new InvalidOperationException("No table specified.");

            result.AppendTableOrColumnName(this.table);

            this.whereClauseBuilder.WriteClauseTo(result);

            return result.ToString();
        }

        public DbDataReader Fetch()
        {
            this.BuildCommand();
            return this.Command.ExecuteReader();
        }

        public TReader Fetch<TReader>() where TReader : DbDataReader => (TReader)this.Fetch();

        #region WHERE clause methods

        public SelectQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }

        #endregion
    }
}
