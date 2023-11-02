using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;

namespace KiwiQuery
{
    public static class SQL
    {
        #region Special values

        public static Null NULL => new();

        #endregion

        #region Functions

        public static FunctionCall COUNT() => new("COUNT", new All());

        #endregion

        #region Logical operators

        public static Predicate NOT(Predicate predicate) => new NotExpression(predicate);

        private static Predicate[] BuildPredicateArray(Predicate first, Predicate second, Predicate[] others)
        {
            Predicate[] result = new Predicate[others.Length + 2];
            result[0] = first;
            result[1] = second;
            others.CopyTo(result, 2);
            return result;
        }

        public static Predicate OR(Predicate first, Predicate second, params Predicate[] others)
        {
            return new BinaryLogicalExpression(LogicalOperator.Or, BuildPredicateArray(first, second, others));
        }

        public static Predicate AND(Predicate first, Predicate second, params Predicate[] others)
        {
            return new BinaryLogicalExpression(LogicalOperator.And, BuildPredicateArray(first, second, others));
        }

        #endregion
    }
}
