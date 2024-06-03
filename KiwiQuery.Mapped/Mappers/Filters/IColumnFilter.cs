using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers
{

internal interface IColumnFilter
{
    bool FilterOut(MappedField field);
}

}
