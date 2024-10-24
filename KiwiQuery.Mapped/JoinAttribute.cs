using System;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Join an additional table when reading this entity.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class JoinAttribute : Attribute, IJoin
{
    private readonly string joinedTable;
    private readonly string joinedColumn;
    private readonly string? referencedTable;
    private readonly string referencedColumn;

    /// <summary>
    /// Join an additional table when reading this entity.
    /// </summary>
    public JoinAttribute(string joinedTable, string joinedColumn, string referencedColumn)
    {
        this.joinedTable = joinedTable;
        this.joinedColumn = joinedColumn;
        this.referencedTable = null;
        this.referencedColumn = referencedColumn;
    }

    /// <summary>
    /// Join an additional table when reading this entity.
    /// </summary>
    public JoinAttribute(string joinedTable, string joinedColumn, string referencedTable, string referencedColumn)
    {
        this.joinedTable = joinedTable;
        this.joinedColumn = joinedColumn;
        this.referencedTable = referencedTable;
        this.referencedColumn = referencedColumn;
    }

    void IJoin.AddTo(SelectCommand query)
    {
        Table implicitJoinedTable = query.Schema.Table(this.joinedTable);

        query.Join(
            implicitJoinedTable.Column(this.joinedColumn),
            this.referencedTable != null
                ? query.Schema.Table(this.referencedTable).Column(this.referencedColumn)
                : query.Schema.Column(this.referencedColumn)
        );
    }
}

}
