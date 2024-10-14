using KiwiQuery.Expressions;
using System;
using System.Data.Common;
using System.Linq;

namespace KiwiQuery
{

/// <summary>
/// Represents a database schema. This class is the root class from which you can build commands and other objects.
/// </summary>
public class Schema
{
    private static Dialect defaultDialect = Dialect.MySql;

    /// <summary>
    /// Sets the default dialect to use when creating instances of this class. <br/>
    /// The default dialect is initially <see cref="Dialect.MySql"/>.
    /// </summary>
    public static void SetDefaultDialect(Dialect dialect) => defaultDialect = dialect;

    private readonly DbConnection connection;
    private readonly Dialect dialect;

    /// <summary>
    /// The connection to the database through which commands are executed.
    /// </summary>
    public DbConnection Connection => this.connection;

    /// <summary>
    /// The mode used for this schema.
    /// </summary>
    internal Dialect CurrentDialect => this.dialect;

    /// <summary>
    /// Creates a new schema using a specific mode.
    /// </summary>
    /// <param name="connection">The connection to the database through which commands will be executed.</param>
    /// <param name="dialect">The mode to use for this schema.</param>
    public Schema(DbConnection connection, Dialect dialect)
    {
        this.connection = connection;
        this.dialect = dialect;
    }

    /// <summary>
    /// Creates a new schema using the default dialect (<see cref="Dialect.MySql"/>
    /// or the dialect set with <see cref="SetDefaultDialect"/>).
    /// </summary>
    /// <param name="connection">The connection to the database through which commands will be executed.</param>
    public Schema(DbConnection connection) : this(connection, defaultDialect) { }

    /// <summary>
    /// Creates a new INSERT command on the given table.
    /// </summary>
    /// <param name="table">The table to insert values into.</param>
    /// <returns>An <see cref="InsertCommand"/> that can be further configured and then executed.</returns>
    public InsertCommand InsertInto(string table) => new InsertCommand(table, this);

    /// <summary>
    /// Creates a new DELETE command on the given table.
    /// </summary>
    /// <param name="table">The table to delete rows from.</param>
    /// <returns>A <see cref="DeleteCommand"/> that can be further configured and then executed.</returns>
    public DeleteCommand DeleteFrom(string table) => new DeleteCommand(table, this);

    /// <summary>
    /// Creates a new UPDATE command on the given table.
    /// </summary>
    /// <param name="table">The table to update.</param>
    /// <returns>An <see cref="UpdateCommand"/> that can be further configured and then executed.</returns>
    public UpdateCommand Update(string table) => new UpdateCommand(table, this);

    /// <summary>
    /// Creates a new SELECT command on the given table.
    /// </summary>
    /// <param name="columns">The columns to read.</param>
    /// <returns>A <see cref="SelectCommand"/> that can be further configured and then executed.</returns>
    public SelectCommand Select(params string[] columns) => new SelectCommand(columns.Select(this.Column), this);

    /// <summary>
    /// Creates a new SELECT command on the given table.
    /// </summary>
    /// <param name="columns">The columns and values to read.</param>
    /// <returns>A <see cref="SelectCommand"/> that can be further configured and then executed.</returns>
    public SelectCommand Select(params Value[] columns) => new SelectCommand(columns, this);

    /// <summary>
    /// Creates a new SELECT * command on the given table.
    /// </summary>
    /// <returns>A <see cref="SelectCommand"/> that can be further configured and then executed.</returns>
    public SelectCommand Select() => new SelectCommand(Array.Empty<Column>(), this);

#pragma warning disable CA1822 // Static members suggestion
// ReSharper disable MemberCanBeMadeStatic.Global

    /// <summary>
    /// Creates a table from this schema.
    /// </summary>
    /// <param name="name">The name of the table.</param>
    public Table Table(string name) => new Table(name);

    /// <summary>
    /// Creates a column from this schema. <br/>
    /// The returned column will not belong explicitly to any table. To get
    /// the column of a specific table, you have to do
    /// <code>
    /// db.Table("myTable").Column("myColumn")
    /// </code>
    /// </summary>
    /// <param name="name">The name of the column.</param>
    public Column Column(string name) => new Column(name, null);

    /// <summary>
    /// Turns a SELECT command into a subquery that you can then use anywhere a <see cref="Value"/> is expected.
    /// </summary>
    /// <param name="query">The subquery.</param>
    public SubQuery SubQuery(SelectCommand query) => new SubQuery(query);

#pragma warning restore CA1822
// ReSharper restore MemberCanBeMadeStatic.Global
}

}
