using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public sealed class All : Value
    {
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendAllColumnsWildcard();
        }
    }
}
