using KiwiQuery.Mapped.Helpers;

namespace KiwiQuery.Mapped.Mappers
{

internal interface IJoin
{
    void AddTo(SelectCommand query);
}

}
