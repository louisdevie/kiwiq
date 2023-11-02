using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public sealed class Table : IWriteable
    {
        private string name;
        private Schema schema;

        public Table(string name, Schema schema)
        {
            this.name = name;
            this.schema = schema;
        }

        public Schema Schema => this.schema;

        public string Name => this.name;

        public Column Column(string name) => new(name, this);

        public void WriteTo(QueryBuilder builder)
        {
            builder.AppendTableOrColumnName(this.name);
        }
    }
}
