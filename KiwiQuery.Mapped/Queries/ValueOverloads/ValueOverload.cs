using KiwiQuery.Expressions;

namespace KiwiQuery.Mapped.Queries.ValueOverloads
{

internal class ValueOverload : IValueOverload
{
    private readonly Value value;

    public ValueOverload(Value value)
    {
        this.value = value;
    }

    public void AddTo(InsertCommand insertCommand, string column) => insertCommand.Value(column, this.value);

    public void AddTo(UpdateCommand updateCommand, string column) => updateCommand.Set(column, this.value);
}

}
