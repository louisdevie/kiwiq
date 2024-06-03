using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal class NoColumnFilter : IColumnFilter
{
    public bool FilterOut(MappedField field) => false;
}

}
