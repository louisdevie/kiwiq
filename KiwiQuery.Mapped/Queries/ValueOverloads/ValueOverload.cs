using KiwiQuery.Expressions;

namespace KiwiQuery.Mapped.Queries
{

internal class ValueOverload : IValueOverload
{
    private readonly Value value;

    public ValueOverload(Value value)
    {
        this.value = value;
    }

    public void AddTo(InsertQuery insertQuery, string column) => insertQuery.Value(column, this.value);

    public void AddTo(UpdateQuery updateQuery, string column) => updateQuery.Set(column, this.value);
}

}
