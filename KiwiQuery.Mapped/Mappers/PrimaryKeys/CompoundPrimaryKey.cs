using System;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;

namespace KiwiQuery.Mapped.Mappers.PrimaryKeys
{

internal class CompoundPrimaryKey : IPrimaryKey
{
    public Predicate MakeEqualityPredicate(Table table, object key)
    {
        throw new UnavailableOperationException(
            $"A compound primary key was defined on {table.Name}. You may want to override this operation with "
            + "your own logic."
        );
    }

    public void ReplaceAutoIncrementedValue(object entity, int key) { }

    public void CheckType(Type keyType) { }

    public string GetColumnToReference()
    {
        throw new UnavailableOperationException("A compound primary key cannot be used in relationships.");
    }

    public Type FieldType
        => throw new UnavailableOperationException("A compound primary key cannot be used in relationships.");
}

}
