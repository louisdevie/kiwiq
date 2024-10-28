using System;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.PrimaryKeys
{

internal class SimplePrimaryKey : IPrimaryKey
{
    private readonly MappedField mappedField;

    public SimplePrimaryKey(MappedField mappedField)
    {
        this.mappedField = mappedField;
    }

    public Predicate MakeEqualityPredicate(Table table, object key)
    {
        return table.Column(this.mappedField.Column ?? throw new VirtualFieldUsageException()) == key;
    }

    public void ReplaceAutoIncrementedValue(object entity, int key)
    {
        this.mappedField.PutKeyInto(entity, key);
    }

    public void CheckType(Type keyType)
    {
        if (!keyType.IsAssignableFrom(this.mappedField.FieldType))
        {
            throw new IncompatibleKeyException(keyType, this.mappedField.FieldType);
        }
    }

    public string GetColumnToReference() => this.mappedField.Column ?? throw new VirtualFieldUsageException();

    public Type FieldType => this.mappedField.FieldType;
}

}
