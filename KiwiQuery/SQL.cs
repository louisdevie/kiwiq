using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
// ReSharper disable InconsistentNaming
// Allow full caps for this class and its methods 

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
        public static FunctionCall COUNT() => new FunctionCall("COUNT");

        #endregion

        #region Logical operators

        /// <summary>
        /// The SQL NOT operator.
        /// </summary>
        /// <param name="predicate">The predicate to negate.</param>
        /// <returns>A new predicate that is the negation of the input predicate.</returns>
        public static Predicate NOT(Predicate predicate) => new NotExpression(predicate);

        /// <summary>
        /// The SQL OR operator. Even if you can't use it with less than two
        /// operands in SQL, if there is one predicate then that predicate <br/>
        /// alone will be the result, and if there is no predicate, a value
        /// that is <em>always false</em> will be the result.
        /// </summary>
        /// <param name="predicates">The predicates to join with OR operators.</param>
        /// <returns>A new predicate that is true when <em>any</em> input predicates are true.</returns>
        public static Predicate OR(params Predicate[] predicates)
        {
            return new BinaryLogicalExpression(LogicalOperator.Or, predicates);
        }


        /// <summary>
        /// The SQL AND operator. Even if you can't use it with less than two
        /// operands in SQL, if there is one predicate then that predicate <br/>
        /// alone will be the result, and if there is no predicate, a value
        /// that is <em>always true</em> will be the result.
        /// </summary>
        /// <param name="predicates">The predicates to join with AND operators.</param>
        /// <returns>A new predicate that is true when <em>all</em> the input predicates are true.</returns>
        public static Predicate AND(params Predicate[] predicates)
        {
            return new BinaryLogicalExpression(LogicalOperator.And, predicates);
        }

        #endregion
    }
}
