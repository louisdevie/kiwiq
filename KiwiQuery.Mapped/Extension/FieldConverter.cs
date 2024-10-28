using System;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// A generic implementation of <see cref="IFieldConverter"/>.
/// </summary>
/// <typeparam name="TModel">The type of model values handled.</typeparam>
/// <typeparam name="TStorage">The storage type.</typeparam>
public abstract class FieldConverter<TModel, TStorage> : IFieldConverter
{
    /// <inheritdoc cref="IFieldConverter.ToStoredValue"/>
    public abstract TStorage ToStoredValue(TModel value);

    /// <inheritdoc cref="IFieldConverter.FromStoredValue"/>
    public abstract TModel FromStoredValue(TStorage value);

    bool IFieldConverter.CanHandle(Type fieldType) => fieldType == typeof(TModel);

    Type IFieldConverter.StorageType => typeof(TStorage);

    IFieldConverter IFieldConverter.SpecializeFor(Type fieldType, IColumnInfo info) => this;

    object? IFieldConverter.ToStoredValue(object? value) => this.ToStoredValue((TModel)value!);

    object? IFieldConverter.FromStoredValue(object? value) => this.FromStoredValue((TStorage)value!);
}

}
