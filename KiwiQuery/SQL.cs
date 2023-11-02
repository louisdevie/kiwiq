using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;

namespace KiwiQuery
{
    /// <summary>
    /// Standard SQL functions, constants and special operators.
    /// </summary>
    public static class SQL
    {
        #region Special values

        /// <inheritdoc cref="Null"/>
        public static Null NULL => new Null();

        #endregion

        #region Functions

        /// <summary>
        /// The SQL COUNT() function.
        /// </summary>
        public static FunctionCall COUNT() => new FunctionCall("COUNT", new AllColumns());

        #endregion

        #region Logical operators

        /// <summary>
        /// The SQL NOT operator.
        /// </summary>
        /// <param name="predicate">The predicate to negate.</param>
        /// <returns>A new predicate that is the negation of the input predicate.</returns>
        public static Predicate NOT(Predicate predicate) => new NotExpression(predicate);

        private static Predicate[] BuildPredicateArray(Predicate first, Predicate second, Predicate[] others)
        {
            Predicate[] result = new Predicate[others.Length + 2];
            result[0] = first;
            result[1] = second;
            others.CopyTo(result, 2);
            return result;
        }

        /// <summary>
        /// The SQL OR operator.
        /// </summary>
        /// <param name="first">The first predicate.</param>
        /// <param name="second">The second predicate.</param>
        /// <param name="others">Others predicates to join with OR operators.</param>
        /// <returns>A new predicate that is true when <em>any</em> of the input predicates are true.</returns>
        public static Predicate OR(Predicate first, Predicate second, params Predicate[] others)
        {
            return new BinaryLogicalExpression(LogicalOperator.Or, BuildPredicateArray(first, second, others));
        }


        /// <summary>
        /// The SQL AND operator.
        /// </summary>
        /// <param name="first">The first predicate.</param>
        /// <param name="second">The second predicate.</param>
        /// <param name="others">Others predicates to join with AND operators.</param>
        /// <returns>A new predicate that is true when <em>all</em> of the input predicates are true.</returns>
        public static Predicate AND(Predicate first, Predicate second, params Predicate[] others)
        {
            return new BinaryLogicalExpression(LogicalOperator.And, BuildPredicateArray(first, second, others));
        }

        #endregion
    }
}
