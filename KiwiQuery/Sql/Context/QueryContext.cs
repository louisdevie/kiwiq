namespace KiwiQuery.Sql.Context
{
    internal class QueryContext : IQueryContext
    {
        private NameContext tables;

        public QueryContext()
        {
            this.tables = NameContext.Canonical;
        }
        
        public QueryContext(IQueryContext parent)
        {
            this.tables = parent.Tables;
        }

        public NameContext Tables => this.tables;

        public QueryContext DeclaringTables()
        {
            this.tables = NameContext.Declaration;
            return this;
        }

        public QueryContext WithTableAliases()
        {
            this.tables = NameContext.Aliased;
            return this;
        }
    }
}