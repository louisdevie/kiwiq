using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public sealed class Null : Value
    {
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendNull();
        }
    }
}
