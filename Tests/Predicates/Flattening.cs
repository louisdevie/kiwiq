using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Predicates
{
    public class Flattening
    {
        private static Predicate[] TestPredicates => new Predicate[]
        {
            new MockPredicate("A"),
            new BinaryLogicalExpression(LogicalOperator.And,
                new Predicate[] { new MockPredicate("B"), new MockPredicate("C") }),
            new BinaryLogicalExpression(LogicalOperator.Or,
                new Predicate[] { new MockPredicate("D"), new MockPredicate("E") })
        };

        [Fact]
        public void OrFlattening()
        {
            BinaryLogicalExpression expression = new BinaryLogicalExpression(LogicalOperator.Or, TestPredicates);
            MockQueryBuilder queryBuilder = new();

            expression.WriteTo(queryBuilder);
            
            Assert.Equal("( A ) || ( ( B ) && ( C ) ) || ( D ) || ( E )", queryBuilder.ToString());
        }

        [Fact]
        public void AndFlattening()
        {
            BinaryLogicalExpression expression = new BinaryLogicalExpression(LogicalOperator.And, TestPredicates);
            MockQueryBuilder queryBuilder = new();

            expression.WriteTo(queryBuilder);
            
            Assert.Equal("( A ) && ( B ) && ( C ) && ( ( D ) || ( E ) )", queryBuilder.ToString());
        }
    }
}