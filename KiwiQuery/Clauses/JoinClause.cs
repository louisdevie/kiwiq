using KiwiQuery.Expressions;
using KiwiQuery.Predicates;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Clauses
{
    internal class JoinClause : Clause
    {
        public enum JoinType { Inner, Left }

        private Table table;
        private Column firstColumn;
        private Column secondColumn;
        private JoinType type;

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
            this.firstColumn.WriteTo(builder);
            builder.AppendComparisonOperator(ComparisonOperator.Equal);
            this.secondColumn.WriteTo(builder);
        }
    }
}
