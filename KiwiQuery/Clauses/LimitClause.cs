using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal class LimitClause : Clause
    {
        private readonly int limit;
        private readonly int offset;

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
