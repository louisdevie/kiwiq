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
        public static Predicate operator ==(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.Equal);

        public static Predicate operator ==(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.Equal);
        #endregion

        #region operator !=
        public static Predicate operator !=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.NotEqual);

        public static Predicate operator !=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.NotEqual);

        #endregion

        #region operator <
        public static Predicate operator <(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThan);

        public static Predicate operator <(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThan);
        #endregion

        #region operator >
        public static Predicate operator >(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThan);

        public static Predicate operator >(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThan);
        #endregion

        #region operator <=
        public static Predicate operator <=(Value lhs, Value rhs)
         => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThanOrEqual);

        public static Predicate operator <=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThanOrEqual);
        #endregion

        #region operator >=
        public static Predicate operator >=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThanOrEqual);

        public static Predicate operator >=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThanOrEqual);
        #endregion

        #region operator +
        public static Value operator +(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Addition);

        public static Value operator +(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Addition);
        #endregion

        #region operator -
        public static Value operator -(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Substraction);

        public static Value operator -(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Substraction);
        #endregion

        #region operator *
        public static Value operator *(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Multiplication);

        public static Value operator *(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Multiplication);
        #endregion

        #region operator /
        public static Value operator /(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Division);

        public static Value operator /(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Division);
        #endregion

        #region operator %
        public static Value operator %(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Modulo);

        public static Value operator %(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Modulo);
        #endregion
    }
}
