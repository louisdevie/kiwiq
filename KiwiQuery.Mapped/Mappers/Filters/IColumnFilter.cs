using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal interface IColumnFilter
{
    bool Filter(MappedField field);
}

}
