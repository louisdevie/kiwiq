using System;
using KiwiQuery.Expressions;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Mappers.PrimaryKeys;

namespace KiwiQuery.Mapped.Relationships
{

internal class HasOne : IRelationship
{
    private readonly string? explicitForeignColumn;

    public HasOne(string? explicitForeignColumn = null)
    {
        this.explicitForeignColumn = explicitForeignColumn;
    }

    public Column FindLocalColumn(Table foreignTable, Table localTable, IPrimaryKey primaryKey)
    {
        return localTable.Column(primaryKey.GetColumnToReference());
    }

    public Column FindForeignColumn(Table foreignTable, Table localTable)
    {
        return foreignTable.Column(
            this.explicitForeignColumn ?? throw CouldNotInferException.ForeignColumn(foreignTable.Name, localTable.Name)
        );
    }

    public bool IsReferencing => false;

    public RefActivator GetRefActivator(Type wrappedType)
    {
        return new RefActivator(
            typeof(Ref<>).MakeGenericType(wrappedType),
            typeof(HasOneFactory<>).MakeGenericType(wrappedType)
        );
    }
}

}
