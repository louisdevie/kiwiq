namespace KiwiQuery.Mapped.Mappers
{

internal interface IJoin
{
    void AddTo(SelectCommand query);
}

}
