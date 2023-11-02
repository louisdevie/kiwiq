using KiwiQuery.Clauses;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    public class SelectQuery : Query, IWriteable
    {
        private string? table;
        private WhereClauseBuilder whereClauseBuilder;
        private JoinClauseBuilder joinClauseBuilder;
        private List<Value> selection;

        public SelectQuery(IEnumerable<Value> selection, Schema schema) : base(schema)
        {
            this.table = null;
            this.whereClauseBuilder = new();
            this.joinClauseBuilder = new(schema);
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

        public void WriteTo(QueryBuilder result)
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

            this.joinClauseBuilder.WriteClauseTo(result);
            this.whereClauseBuilder.WriteClauseTo(result);
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            this.WriteTo(result);
            return result.ToString();
        }

        public DbDataReader Fetch()
        {
            this.BuildCommand();
            return this.Command.ExecuteReader();
        }

        public TReader Fetch<TReader>() where TReader : DbDataReader => (TReader)this.Fetch();

        #region JOIN clause methods

        public SelectQuery Join(Table table, Column firstColumn, Column secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery Join(Table table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery Join(string table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.Join(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery Join(Column columnToJoin, Column matchingColumn)
        {
            this.joinClauseBuilder.Join(columnToJoin, matchingColumn);
            return this;
        }
        public SelectQuery LeftJoin(Table table, Column firstColumn, Column secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery LeftJoin(Table table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery LeftJoin(string table, string firstColumn, string secondColumn)
        {
            this.joinClauseBuilder.LeftJoin(table, firstColumn, secondColumn);
            return this;
        }

        public SelectQuery LeftJoin(Column columnToJoin, Column matchingColumn)
        {
            this.joinClauseBuilder.LeftJoin(columnToJoin, matchingColumn);
            return this;
        }

        #endregion

        #region WHERE clause methods

        public SelectQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }
        #endregion
    }
}
