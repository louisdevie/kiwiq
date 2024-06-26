using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal class PrimaryKeyColumnFilter : IColumnFilter
{
    public bool Filter(MappedField field) => !field.PrimaryKey;
}

}
