namespace KiwiQuery.Mapped.Queries.ValueOverloads
{

internal class SubQueryOverload : IValueOverload
{
    private readonly SelectQuery subQuery;

    public SubQueryOverload(SelectQuery subQuery)
    {
        this.subQuery = subQuery;
    }

    public void AddTo(InsertQuery insertQuery, string column) => insertQuery.Value(column, this.subQuery);

    public void AddTo(UpdateQuery updateQuery, string column) => updateQuery.Set(column, this.subQuery);
}

}
