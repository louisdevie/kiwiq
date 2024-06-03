using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal class InsertColumnFilter : IColumnFilter
{
    public bool FilterOut(MappedField field) => !field.Inserted;
}

}
