using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    /// <summary>
    /// A generic SQL command.
    /// </summary>
    public abstract class Query
    {
        private Schema schema;
        private DbCommand command;

        /// <summary>
        /// The schema this command will be executed on.
        /// </summary>
        public Schema Schema => this.schema;

        /// <summary>
        /// The inner command.
        /// </summary>
        protected DbCommand Command => this.command;

        /// <summary>
        /// Creates a new query for a schema.
        /// </summary>
        /// <param name="schema"></param>
        protected Query(Schema schema)
        {
            this.schema = schema;
            this.command = this.schema.Connection.CreateCommand();
        }

        /// <summary>
        /// Generates the SQL for the command.
        /// </summary>
        /// <param name="qb">The query builder to use.</param>
        /// <returns>A string containing the SQL.</returns>
        protected abstract string BuildCommandText(QueryBuilder qb);

        /// <summary>
        /// Build the inner command using the appropriate query builder.
        /// </summary>
        protected void BuildCommand()
        {
            this.Command.CommandText = this.BuildCommandText(
                QueryBuilderFactory.Current.NewQueryBuilder(this.Schema.CurrentDialect, this.Command)
            );
        }
    }
}

