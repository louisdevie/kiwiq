using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public class Column : Value
    {
        private string name;
        private Table? table;
        private Schema schema;
        public string? alias;

        public Column(string name, Table table) : this(name, table.Schema)
        {
            this.table = table;
        }

        public Column(string name, Schema schema)
        {
            this.name = name;
            this.table = null;
            this.schema = schema;
            this.alias = null;
        }

        public Column As(string alias)
        {
            this.alias = alias;
            return this;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            if (this.table is not null)
            {
                this.table.WriteTo(builder);
                builder.AppendAccessor();
            }
            builder.AppendTableOrColumnName(this.name);
            if (this.alias is not null)
            {
                builder.AppendAsKeyword()
                       .AppendTableOrColumnName(this.alias);
            }
        }
    }
}
