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
        private Table table;
        private Column firstColumn;
        private Column secondColumn;

        public JoinClause(Table table, Column firstColumn, Column secondColumn)
        {
            this.table = table;
            this.firstColumn = firstColumn;
            this.secondColumn = secondColumn;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendJoinKeyword();
            this.table.WriteTo(builder);

            builder.AppendOnKeyword();
            this.firstColumn.WriteTo(builder);
            builder.AppendComparisonOperator(ComparisonOperator.Equal);
            this.secondColumn.WriteTo(builder);
        }
    }
}
