using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// The column of a table. <br/>
    /// Instances of this class should be created from a <see cref="Expressions.Table"/> or a <see cref="Schema"/>.
    /// </summary>
    public sealed class Column : Value
    {
        private string name;
        private Table? table;
        private string? alias;

        /// <summary>
        /// The name of the column.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// The table the column belongs to, if explicitly stated.
        /// </summary>
        public Table? Table => this.table;

        /// <summary>
        /// The alias of the column, if it has one.
        /// </summary>
        public string? Alias => this.alias;

        /// <summary>
        /// Creates a new column from a table.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="table">The table it belongs to.</param>
        internal Column(string name, Table table) : this(name)
        {
            this.table = table;
        }

        /// <summary>
        /// Creates a new column without an explicit table.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="schema">Teh schema </param>
        internal Column(string name)
        {
            this.name = name;
            this.table = null;
            this.alias = null;
        }

        /// <summary>
        /// Use an alias for this column. This only has an effect on how the
        /// results of a SELECT query will be returned.
        /// </summary>
        /// <param name="alias">The alias for the column.</param>
        /// <returns>The column with its alias.</returns>
        public Column As(string alias)
        {
            this.alias = alias;
            return this;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            if (this.table != null)
            {
                this.table.WriteTo(builder);
                builder.AppendAccessor();
            }
            builder.AppendTableOrColumnName(this.name);
            if (this.alias != null)
            {
                builder.AppendAsKeyword()
                       .AppendTableOrColumnName(this.alias);
            }
        }
    }
}
