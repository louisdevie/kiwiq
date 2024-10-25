using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    /// <summary>
    /// A generic SQL value.
    /// </summary>
    public abstract class Value : IWriteable
    {
        /// <inheritdoc />
        public abstract void WriteTo(QueryBuilder builder);

        /// <summary>Returns true if this value is always NULL.</summary>
        public abstract bool IsNull();

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        #region operator ==
        /// <summary>
        /// Compare two SQL values for equality.
        /// </summary>
        public static Predicate operator ==(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.Equal);

        /// <summary>
        /// Compare a SQL value with a constant for equality. The constant will be injected as a parameter.
        /// </summary>
        public static Predicate operator ==(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.Equal);
        #endregion

        #region operator !=
        /// <summary>
        /// Compare two SQL values for inequality.
        /// </summary>
        public static Predicate operator !=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.NotEqual);

        /// <summary>
        /// Compare a SQL value with a constant for inequality.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Predicate operator !=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.NotEqual);

        #endregion

        #region operator <
        /// <summary>
        /// Check if a SQL value is less than another.
        /// </summary>
        public static Predicate operator <(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThan);

        /// <summary>
        /// Check if a SQL value is less than a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Predicate operator <(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThan);
        #endregion

        #region operator >
        /// <summary>
        /// Check if a SQL value is greater than another.
        /// </summary>
        public static Predicate operator >(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThan);

        /// <summary>
        /// Check if a SQL value is greater than a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Predicate operator >(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThan);
        #endregion

        #region operator <=
        /// <summary>
        /// Check if a SQL value is less than or equal to another.
        /// </summary>
        public static Predicate operator <=(Value lhs, Value rhs)
         => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThanOrEqual);

        /// <summary>
        /// Check if a SQL value is less than or equal to a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Predicate operator <=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThanOrEqual);
        #endregion

        #region operator >=
        /// <summary>
        /// Check if a SQL value is greater than or equal to another.
        /// </summary>
        public static Predicate operator >=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThanOrEqual);

        /// <summary>
        /// Check if a SQL value is greater than or equal to a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Predicate operator >=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThanOrEqual);
        #endregion

        #region operator +
        /// <summary>
        /// Add two SQL values together.
        /// </summary>
        public static Value operator +(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Addition);

        /// <summary>
        /// Add a constant to a SQL value. The constant will be injected as a parameter.
        /// </summary>
        public static Value operator +(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Addition);
        #endregion

        #region operator -
        /// <summary>
        /// Subtract a SQL value from another.
        /// </summary>
        public static Value operator -(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Subtraction);

        /// <summary>
        /// Subtract a constant to a SQL value. The constant will be injected as a parameter.
        /// </summary>
        public static Value operator -(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Subtraction);
        #endregion

        #region operator *
        /// <summary>
        /// Multiply two SQL values together.
        /// </summary>
        public static Value operator *(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Multiplication);

        /// <summary>
        /// Multiply a SQL value by a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Value operator *(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Multiplication);
        #endregion

        #region operator /
        /// <summary>
        /// Divide a SQL value by another.
        /// </summary>
        public static Value operator /(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Division);

        /// <summary>
        /// Divide a SQL value by a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Value operator /(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Division);
        #endregion

        #region operator %
        /// <summary>
        /// Compute the modulo of two SQL values.
        /// </summary>
        public static Value operator %(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Modulo);

        /// <summary>
        /// Compute the modulo of a SQL value by a constant. The constant will be injected as a parameter.
        /// </summary>
        public static Value operator %(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Modulo);
        #endregion
    }
}
