using System.Linq;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Filters
{

internal class IntersectColumnFilter : IColumnFilter
{
    private readonly string[] columns;

    public IntersectColumnFilter(string[] columns)
    {
        this.columns = columns;
    }

    public bool FilterOut(MappedField field) => this.columns.Any(col => field.Column.Equals(col));
}

}
