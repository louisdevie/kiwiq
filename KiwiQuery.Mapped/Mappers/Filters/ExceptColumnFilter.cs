using System.Linq;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal class ExceptColumnFilter : IColumnFilter
{
    private readonly string[] columns;

    public ExceptColumnFilter(string[] columns)
    {
        this.columns = columns;
    }

    public bool FilterOut(MappedField field) => this.columns.All(col => !field.Column.Equals(col));
}

}
