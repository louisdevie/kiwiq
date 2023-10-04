using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Clauses
{
    internal class JoinClauseBuilder : IClauseBuilder
    {
        private List<JoinClause> joins;
        private Schema schema;

        public JoinClauseBuilder(Schema schema)
        {
            this.schema = schema;
            this.joins = new();
        }

        public void WriteClauseTo(QueryBuilder qb)
        {
            foreach (JoinClause joinClause in joins)
            {
                joinClause.WriteTo(qb);
            }
        }

        #region table - column - column

        public void Join(Table table, Column firstColumn, Column secondColumn)
        {
            this.joins.Add(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Inner));
        }
        public void LeftJoin(Table table, Column firstColumn, Column secondColumn)
        {
            this.joins.Add(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Left));
        }

        public void Join(Table table, string firstColumn, string secondColumn)
        {
            this.joins.Add(
                new JoinClause(
                    table,
                    this.schema.Column(firstColumn),
                    this.schema.Column(secondColumn),
                    JoinClause.JoinType.Inner
                )
            );
        }

        public void LeftJoin(Table table, string firstColumn, string secondColumn)
        {
            this.joins.Add(
                new JoinClause(
                    table,
                    this.schema.Column(firstColumn),
                    this.schema.Column(secondColumn),
                    JoinClause.JoinType.Left
                )
            );
        }

        public void Join(string table, string firstColumn, string secondColumn)
        {
            this.joins.Add(
                new JoinClause(
                    this.schema.Table(table),
                    this.schema.Column(firstColumn),
                    this.schema.Column(secondColumn),
                    JoinClause.JoinType.Inner
                )
            );
        }

        public void LeftJoin(string table, string firstColumn, string secondColumn)
        {
            this.joins.Add(
                new JoinClause(
                    this.schema.Table(table),
                    this.schema.Column(firstColumn),
                    this.schema.Column(secondColumn),
                    JoinClause.JoinType.Left
                )
            );
        }

        #endregion

        #region column - column

        public void Join(Column columnToJoin, Column matchingColumn)
        {
            if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
            this.joins.Add(new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Inner));
        }

        public void LeftJoin(Column columnToJoin, Column matchingColumn)
        {
            if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
            this.joins.Add(new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Left));
        }

        #endregion
    }
}
