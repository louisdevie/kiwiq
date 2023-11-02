using KiwiQuery.Clauses;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery
{
    /// <summary>
    /// A SQL DELETE command. <br/>
    /// Instances of this class should be created from a <see cref="Schema"/>.
    /// </summary>
    public class DeleteQuery : Query
    {
        private string table;
        private WhereClauseBuilder whereClauseBuilder;

        /// <summary>
        /// Creates a new DELETE command.
        /// </summary>
        /// <param name="table">The name of the table to delete rows from.</param>
        /// <param name="schema">The schema to execute this command on.</param>
        internal DeleteQuery(string table, Schema schema) : base(schema)
        {
            this.table = table;
            this.whereClauseBuilder = new WhereClauseBuilder();
        }

        protected override string BuildCommandText(QueryBuilder result)
        {
            result.AppendDeleteFromKeywords()
                  .AppendTableOrColumnName(this.table);

            this.whereClauseBuilder.WriteClauseTo(result);

            return result.ToString();
        }

        /// <summary>
        /// Build and execute the command.
        /// </summary>
        /// <returns><see langword="true"/> if there was at least one row affected, otherwise <see langword="false"/>.</returns>
        public bool Apply()
        {
            this.BuildCommand();
            int affectedRows = this.Command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        #region WHERE clause methods

        /// <inheritdoc cref="WhereClauseBuilder.Where(Predicate)"/>
        public DeleteQuery Where(Predicate predicate)
        {
            this.whereClauseBuilder.Where(predicate);
            return this;
        }

        #endregion
    }
}
