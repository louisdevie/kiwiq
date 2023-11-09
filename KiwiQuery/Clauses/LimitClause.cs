using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal class LimitClause : Clause
    {
        private int limit;
        private int offset;

        public LimitClause(int limit, int offset)
        {
            this.limit = limit;
            this.offset = offset;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendLimitClause(this.limit, this.offset);
        }
    }
}
