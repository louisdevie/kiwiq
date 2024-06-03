namespace KiwiQuery.Mapped.Queries
{

internal class ObjectOverload : IValueOverload
{
    private readonly object? value;

    public ObjectOverload(object? value)
    {
        this.value = value;
    }

    public void AddTo(InsertQuery insertQuery, string column) => insertQuery.Value(column, this.value);

    public void AddTo(UpdateQuery updateQuery, string column) => updateQuery.Set(column, this.value);
}

}
