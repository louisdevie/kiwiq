using KiwiQuery.Sql;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace KiwiQuery
{
    /// <summary>
    /// A generic SQL command.
    /// </summary>
    public abstract class Command
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
        protected DbCommand DbCommand => this.command;

        /// <summary>
        /// Creates a new query for a schema.
        /// </summary>
        /// <param name="schema"></param>
        protected Command(Schema schema)
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
            this.DbCommand.CommandText = this.BuildCommandText(
                QueryBuilderFactory.Current.NewQueryBuilder(this.Schema.CurrentDialect, this.DbCommand)
            );
            
            if (this.Schema.CurrentLogger != null)
            {
                this.Schema.CurrentLogger.LogDebug("Command: {}", this.DbCommand.CommandText);
                foreach (DbParameter parameter in this.DbCommand.Parameters)
                {
                    this.Schema.CurrentLogger.LogDebug("Parameter {}: {}", parameter.ParameterName, parameter.Value);
                }
            }
        }
    }
}

