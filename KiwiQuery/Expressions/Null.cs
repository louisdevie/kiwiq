using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public class Null : Value
    {
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendNull();
        }
    }
}
