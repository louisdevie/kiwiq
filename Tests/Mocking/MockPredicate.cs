using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace Tests.Mocking
{
    public class MockPredicate: Predicate
    {
        private readonly string text;

        public MockPredicate(string text)
        {
            this.text = text;
        }
        
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendRaw(this.text);
        }
    }
}