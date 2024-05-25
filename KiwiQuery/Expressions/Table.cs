using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A table of the schema. <br/>
    /// Instances of this class should be created from a <see cref="Schema"/>.
    /// </summary>
    public sealed class Table : IWriteable
    {
        private string name;
        private string? alias;

        /// <summary>
        /// The name of the table.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// The alias of the table, if it has one.
        /// </summary>
        public string? Alias => this.alias;

        /// <summary>
        /// Creates a new table.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        internal Table(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Creates a column from this table.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The new column.</returns>
        public Column Column(string name) => new Column(name, this);

        /// <summary>
        /// Use an alias for this table. A separate column with the aliased name must be created to select its columns.
        /// </summary>
        /// <param name="alias">The alias for the column.</param>
        /// <returns>The column with its alias.</returns>
        public Table As(string alias)
        {
            this.alias = alias;
            return this;
        }

        public void WriteTo(QueryBuilder builder)
        {
            builder.AppendTableOrColumnName(this.name);
            if (this.alias != null)
            {
                builder.AppendAsKeyword();
                builder.AppendTableOrColumnName(this.alias);
            }
        }
    }
}
