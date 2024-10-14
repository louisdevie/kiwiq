using System.Collections.Generic;
using System.Data.Common;

namespace KiwiQuery.Sql.Dialects
{

/// <summary>
/// A query builder for SQLite.
/// </summary>
internal class SqliteQueryBuilder : QueryBuilder
{
    public SqliteQueryBuilder(DbCommand command) : base(command) { }

    public override QueryBuilder AppendTableOrColumnName(string tableOrColumn)
    {
        this.EnsureWordBoundary();
        this.Buffer.Append('"').Append(tableOrColumn).Append('"');
        this.EndsWithWordBoundary();
        return this;
    }

    public override QueryBuilder AppendNamedParameter(string name)
    {
        this.EnsureWordBoundary();
        this.Buffer.Append('@').Append(name);
        this.DoesntEndWithWordBoundary();
        return this;
    }

    public override QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns)
    {
        this.EnsureWordBoundary();
        this.Buffer.Append('"').AppendJoin("\",\"", columns).Append('"');
        this.DoesntEndWithWordBoundary();
        return this;
    }

    public override QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters)
    {
        this.EnsureWordBoundary();
        this.Buffer.AppendJoin($",", parameters);
        this.DoesntEndWithWordBoundary();
        return this;
    }

    public override QueryBuilder AppendTruthyConstant()
    {
        this.EnsureWordBoundary();
        this.Buffer.Append('1');
        this.DoesntEndWithWordBoundary();
        return this;
    }

    public override QueryBuilder AppendFalsyConstant()
    {
        this.EnsureWordBoundary();
        this.Buffer.Append('0');
        this.DoesntEndWithWordBoundary();
        return this;
    }
}

}
