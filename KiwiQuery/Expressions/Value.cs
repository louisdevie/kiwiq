using KiwiQuery.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Expressions
{
    public abstract class Value : IWriteable
    {
        public abstract void WriteTo(QueryBuilder builder);

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
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
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Plus);

        public static Value operator +(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Plus);
        #endregion

        #region operator -
        public static Value operator -(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Minus);

        public static Value operator -(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Minus);
        #endregion

        #region operator *
        public static Value operator *(Value lhs, Value rhs)
            => new ArithmeticExpression(lhs, rhs, ArithmeticOperator.Times);

        public static Value operator *(Value lhs, object rhs)
            => new ArithmeticExpression(lhs, new Parameter(rhs), ArithmeticOperator.Times);
        #endregion
    }
}
