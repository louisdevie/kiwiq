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

    public bool Filter(MappedField field) => field.Column != null && this.columns.Contains(field.Column);
}

}
