namespace KiwiQuery.Mapped.Commands.ValueOverloads
{

internal interface IValueOverload
{
    void AddTo(InsertCommand insertCommand, string column);
    
    void AddTo(UpdateCommand updateCommand, string column);
}

}
