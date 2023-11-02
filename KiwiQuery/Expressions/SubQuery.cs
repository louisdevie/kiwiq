using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    internal class SubQuery : Value
    {
        private SelectQuery query;

        public SubQuery(SelectQuery query)
        {
            this.query = query;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            
            builder.OpenBracket();
            this.query.WriteTo(builder);
            builder.CloseBracket();
        }
    }
}
