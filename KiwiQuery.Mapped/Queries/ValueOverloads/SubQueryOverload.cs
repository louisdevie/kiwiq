namespace KiwiQuery.Mapped.Queries.ValueOverloads
{

internal class SubQueryOverload : IValueOverload
{
    private readonly SelectCommand subQuery;

    public SubQueryOverload(SelectCommand subQuery)
    {
        this.subQuery = subQuery;
    }

    public void AddTo(InsertCommand insertCommand, string column) => insertCommand.Value(column, this.subQuery);

    public void AddTo(UpdateCommand updateCommand, string column) => updateCommand.Set(column, this.subQuery);
}

}
