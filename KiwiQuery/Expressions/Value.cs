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

        public static Predicate operator ==(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.Equal);

        public static Predicate operator !=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.NotEqual);

        public static Predicate operator <(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThan);

        public static Predicate operator >(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThan);

        public static Predicate operator <=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.LessThanOrEqual);

        public static Predicate operator >=(Value lhs, Value rhs)
            => new ComparisonPredicate(lhs, rhs, ComparisonOperator.GreaterThanOrEqual);

        public static Predicate operator ==(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.Equal);

        public static Predicate operator !=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.NotEqual);

        public static Predicate operator <(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThan);

        public static Predicate operator >(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThan);

        public static Predicate operator <=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.LessThanOrEqual);

        public static Predicate operator >=(Value lhs, object rhs)
            => new ComparisonPredicate(lhs, new Parameter(rhs), ComparisonOperator.GreaterThanOrEqual);
    }
}
