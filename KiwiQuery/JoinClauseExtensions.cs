using System;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions;

namespace KiwiQuery
{

/// <summary>
/// Provides common operations for queries with JOIN clauses.
/// </summary>
public static class JoinClauseExtensions
{
    #region table - column - column

    /// <summary>
    /// Performs an INNER JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The other table to join.</param>
    /// <param name="firstColumn">The column to join on.</param>
    /// <param name="secondColumn">The column of the other table to join on.</param>
    public static TSelf Join<TSelf>(
        this IHasJoinClause<TSelf> query, Table table, Column firstColumn, Column secondColumn
    )
    {
        query.JoinClause.AddJoin(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Inner));
        return query.Downcast();
    }

    /// <summary>
    /// Performs a LEFT JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The other table to join.</param>
    /// <param name="firstColumn">The column to join on.</param>
    /// <param name="secondColumn">The column of the other table to join on.</param>
    public static TSelf LeftJoin<TSelf>(
        this IHasJoinClause<TSelf> query, Table table, Column firstColumn, Column secondColumn
    )
    {
        query.JoinClause.AddJoin(new JoinClause(table, firstColumn, secondColumn, JoinClause.JoinType.Left));
        return query.Downcast();
    }

    /// <summary>
    /// Performs an INNER JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The other table to join.</param>
    /// <param name="firstColumn">The name of the column to join on.</param>
    /// <param name="secondColumn">The name of the column of the other table to join on.</param>
    public static TSelf Join<TSelf>(
        this IHasJoinClause<TSelf> query, Table table, string firstColumn, string secondColumn
    )
    {
        query.JoinClause.AddJoin(
            new JoinClause(
                table,
                query.JoinClause.GetColumn(firstColumn),
                query.JoinClause.GetColumn(secondColumn),
                JoinClause.JoinType.Inner
            )
        );
        return query.Downcast();
    }

    /// <summary>
    /// Performs a LEFT JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The other table to join.</param>
    /// <param name="firstColumn">The name of the column to join on.</param>
    /// <param name="secondColumn">The name of the column of the other table to join on.</param>
    public static TSelf LeftJoin<TSelf>(
        this IHasJoinClause<TSelf> query, Table table, string firstColumn, string secondColumn
    )
    {
        query.JoinClause.AddJoin(
            new JoinClause(
                table,
                query.JoinClause.GetColumn(firstColumn),
                query.JoinClause.GetColumn(secondColumn),
                JoinClause.JoinType.Left
            )
        );
        return query.Downcast();
    }

    /// <summary>
    /// Performs an INNER JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The name of the other table to join.</param>
    /// <param name="firstColumn">The name of the column to join on.</param>
    /// <param name="secondColumn">The name of the column of the other table to join on.</param>
    public static TSelf Join<TSelf>(
        this IHasJoinClause<TSelf> query, string table, string firstColumn, string secondColumn
    )
    {
        query.JoinClause.AddJoin(
            new JoinClause(
                query.JoinClause.GetTable(table),
                query.JoinClause.GetColumn(firstColumn),
                query.JoinClause.GetColumn(secondColumn),
                JoinClause.JoinType.Inner
            )
        );
        return query.Downcast();
    }

    /// <summary>
    /// Performs a LEFT JOIN with another table.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="table">The name of the other table to join.</param>
    /// <param name="firstColumn">The name of the column to join on.</param>
    /// <param name="secondColumn">The name of the column of the other table to join on.</param>
    public static TSelf LeftJoin<TSelf>(
        this IHasJoinClause<TSelf> query, string table, string firstColumn, string secondColumn
    )
    {
        query.JoinClause.AddJoin(
            new JoinClause(
                query.JoinClause.GetTable(table),
                query.JoinClause.GetColumn(firstColumn),
                query.JoinClause.GetColumn(secondColumn),
                JoinClause.JoinType.Left
            )
        );
        return query.Downcast();
    }

    #endregion

    #region column - column

    /// <summary>
    /// Performs an INNER JOIN with the table of <paramref name="columnToJoin"/>.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="columnToJoin">The column of the table to join. It must come from an explicit <see cref="Table"/>.</param>
    /// <param name="matchingColumn">The column already present to join on.</param>
    public static TSelf Join<TSelf>(this IHasJoinClause<TSelf> query, Column columnToJoin, Column matchingColumn)
    {
        if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
        query.JoinClause.AddJoin(
            new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Inner)
        );
        return query.Downcast();
    }

    /// <summary>
    /// Performs a LEFT JOIN with the table of <paramref name="columnToJoin"/>.
    /// </summary>
    /// <param name="query">The query to add the join to.</param>
    /// <param name="columnToJoin">The column of the table to join.</param>
    /// <param name="matchingColumn">The column already in present to join on.</param>
    public static TSelf LeftJoin<TSelf>(this IHasJoinClause<TSelf> query, Column columnToJoin, Column matchingColumn)
    {
        if (columnToJoin.Table is null) throw new NullReferenceException("The table to join is unknown.");
        query.JoinClause.AddJoin(
            new JoinClause(columnToJoin.Table, columnToJoin, matchingColumn, JoinClause.JoinType.Left)
        );
        return query.Downcast();
    }

    #endregion
}

}
