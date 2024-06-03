namespace KiwiQuery.Mapped.Queries
{

internal interface IValueOverload
{
    void AddTo(InsertQuery insertQuery, string column);
    
    void AddTo(UpdateQuery updateQuery, string column);
}

}
