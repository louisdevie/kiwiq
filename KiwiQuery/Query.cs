using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    public abstract class Query
    {
        private Schema schema;
        private DbCommand command;

        public Schema Schema => this.schema;

        public DbCommand Command => this.command;

        public Query(Schema schema)
        {
            this.schema = schema;
            this.command = this.schema.Connection.CreateCommand();
        }

        internal abstract string BuildCommandText(QueryBuilder qb);

        protected void BuildCommand()
        {
            this.Command.CommandText = this.BuildCommandText(
                this.Schema.QueryBuilderFactory.NewQueryBuilder(this.Command)
            );
        }
    }
}

