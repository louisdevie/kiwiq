namespace KiwiQuery.Mapped.Queries.ValueOverloads
{

internal class ObjectOverload : IValueOverload
{
    private readonly object? value;

    public ObjectOverload(object? value)
    {
        this.value = value;
    }

    public void AddTo(InsertCommand insertCommand, string column) => insertCommand.Value(column, this.value);

    public void AddTo(UpdateCommand updateCommand, string column) => updateCommand.Set(column, this.value);
}

}
