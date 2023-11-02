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

        /// <summary>
        /// The name of the table.
        /// </summary>
        public string Name => this.name;

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
        public Column Column(string name) => new(name, this);

        public void WriteTo(QueryBuilder builder)
        {
            builder.AppendTableOrColumnName(this.name);
        }
    }
}
