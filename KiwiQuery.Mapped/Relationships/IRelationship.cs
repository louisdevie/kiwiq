using System;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;

namespace KiwiQuery.Mapped.Relationships
{

internal interface IRelationship
{
    public Column FindLocalColumn(Table foreignTable, Table localTable, IPrimaryKey primaryKey);

    public Column FindForeignColumn(Table foreignTable, Table localTable);

    bool IsReferencing { get; }

    RefActivator GetRefActivator(Type refType);
}

}
