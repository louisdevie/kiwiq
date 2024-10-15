using System.Collections.Generic;
using System.Linq;
using KiwiQuery.Clauses;
using KiwiQuery.Expressions;

namespace KiwiQuery.Mapped.Mappers
{

internal class ReferenceJoin : IJoin
{
    private readonly Column joinedColumn;
    private readonly Column referencedColumn;
    private readonly IJoin[] additionalJoins;

    public ReferenceJoin(Column joinedColumn, Column referencedColumn, IEnumerable<IJoin> additionalJoins)
    {
        this.joinedColumn = joinedColumn;
        this.referencedColumn = referencedColumn;
        this.additionalJoins = additionalJoins.ToArray();
    }

    public void AddTo(SelectCommand query)
    {
        query.LeftJoin(this.joinedColumn, this.referencedColumn);

        foreach (IJoin additionalJoin in this.additionalJoins)
        {
            additionalJoin.AddTo(query);
        }
    }
}

}
