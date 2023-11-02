using KiwiQuery.Expressions;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    /// <summary>
    /// Provides methods for building JOIN statements.
    /// </summary>
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

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The other table to join.</param>
        /// <param name="firstColumn">The column to join on.</param>
        /// <param name="secondColumn">The column of the other table to join on.</param>
        public void Join(Table table, Column firstColumn, Column secondColumn)
        {
            this.joins.Add(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Inner));
        }

        /// <summary>
        /// Performs a LEFT JOIN with another table.
        /// </summary>
        /// <param name="table">The other table to join.</param>
        /// <param name="firstColumn">The column to join on.</param>
        /// <param name="secondColumn">The column of the other table to join on.</param>
        public void LeftJoin(Table table, Column firstColumn, Column secondColumn)
        {
            this.joins.Add(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Left));
        }

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The other table to join.</param>
        /// <param name="firstColumn">The name of the column to join on.</param>
        /// <param name="secondColumn">The name of the column of the other table to join on.</param>
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

        /// <summary>
        /// Performs a LEFT JOIN with another table.
        /// </summary>
        /// <param name="table">The other table to join.</param>
        /// <param name="firstColumn">The name of the column to join on.</param>
        /// <param name="secondColumn">The name of the column of the other table to join on.</param>
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

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the other table to join.</param>
        /// <param name="firstColumn">The name of the column to join on.</param>
        /// <param name="secondColumn">The name of the column of the other table to join on.</param>
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

        /// <summary>
        /// Performs a LEFT JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the other table to join.</param>
        /// <param name="firstColumn">The name of the column to join on.</param>
        /// <param name="secondColumn">The name of the column of the other table to join on.</param>
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

        /// <summary>
        /// Performs an INNER JOIN with the table of <paramref name="columnToJoin"/>.
        /// </summary>
        /// <param name="columnToJoin">The column of the table to join.</param>
        /// <param name="matchingColumn">The column already in present to join on.</param>
        public void Join(Column columnToJoin, Column matchingColumn)
        {
            if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
            this.joins.Add(new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Inner));
        }

        /// <summary>
        /// Performs a LEFT JOIN with the table of <paramref name="columnToJoin"/>.
        /// </summary>
        /// <param name="columnToJoin">The column of the table to join.</param>
        /// <param name="matchingColumn">The column already in present to join on.</param>
        public void LeftJoin(Column columnToJoin, Column matchingColumn)
        {
            if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
            this.joins.Add(new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Left));
        }

        #endregion
    }
}
