using System;
using KiwiQuery.Expressions.Predicates;

namespace KiwiQuery.Mapped.Mappers.PrimaryKeys
{

internal interface IPrimaryKey
{
    public Predicate MakeEqualityPredicate(Table table, object key);
    
    void ReplaceAutoIncrementedValue(object entity, int key);

    void CheckType(Type keyType);

    string GetColumnToReference();

    Type FieldType { get; }
}

}
