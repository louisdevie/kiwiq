using System;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;

namespace KiwiQuery.Mapped.Mappers.PrimaryKeys
{

internal class UndefinedPrimaryKey : IPrimaryKey
{
    public Predicate MakeEqualityPredicate(Table table, object key)
    {
        throw new UnavailableOperationException(
            $"No primary key was defined on {table.Name}. You can add a [Column(..., PrimaryKey=true)] attribute "
            + "to the id field or override this operation."
        );
    }

    public void ReplaceAutoIncrementedValue(object entity, int key) { }

    public void CheckType(Type keyType) { }

    public string GetColumnToReference()
    {
        throw new UnavailableOperationException("The primary key must be defined in order to use relationships.");
    }

    public Type FieldType
        => throw new UnavailableOperationException("The primary key must be defined in order to use relationships.");
}

}
