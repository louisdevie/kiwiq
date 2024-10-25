using System.Collections.Generic;

namespace KiwiQuery.Expressions.Predicates
{
    /// <summary>
    /// A predicate is a boolean value.
    /// </summary>
    public abstract class Predicate : Value
    {
        /// <summary>
        /// Returns this predicate as if it was a list of predicates join by the given operator.
        /// By default, this method returns one and only one predicate: itself.
        /// </summary>
        /// <param name="op">The operator wanted.</param>
        /// <returns>A read-only list of predicates.</returns>
        public virtual IEnumerable<Predicate> RelativeTo(LogicalOperator op)
        {
            yield return this;
        }

        /// <inheritdoc />
        public override bool IsNull() => false;
    }
}
