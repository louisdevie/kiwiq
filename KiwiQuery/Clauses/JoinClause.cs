using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// Represents a JOIN statement.
    /// </summary>
    internal class JoinClause : Clause
    {
        public enum JoinType { Inner, Left }

        private Table table;
        private Column firstColumn;
        private Column secondColumn;
        private JoinType type;

        /// <summary>
        /// Creates a new JOIN statement.
        /// </summary>
        /// <param name="table">The table to join.</param>
        /// <param name="firstColumn">The column of the first table to join on.</param>
        /// <param name="secondColumn">The column of the other table to join on.</param>
        /// <param name="type">The type of join to perform.</param>
        public JoinClause(Table table, Column firstColumn, Column secondColumn, JoinType type)
        {
            this.table = table;
            this.firstColumn = firstColumn;
            this.secondColumn = secondColumn;
            this.type = type;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            switch (this.type)
            {
                case JoinType.Inner:
                    builder.AppendInnerKeyword();
                    break;
                case JoinType.Left:
                    builder.AppendLeftKeyword();
                    break;
            }
            builder.AppendJoinKeyword();
            this.table.WriteTo(builder);

            builder.AppendOnKeyword();
            
            builder.PushContext.WithTableAliases();
            this.firstColumn.WriteTo(builder);
            builder.AppendComparisonOperator(ComparisonOperator.Equal);
            this.secondColumn.WriteTo(builder);
            builder.PopContext();
        }
    }
}
