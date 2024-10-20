using System;
using System.Data;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// A generic implementation of <see cref="IFieldConverter"/> that takes into account the
/// <see cref="ColumnAttribute.Format"/> option.
/// </summary>
/// <typeparam name="TModel">The type of model values handled.</typeparam>
/// <typeparam name="TStorage">The storage type.</typeparam>
public abstract class FormattedFieldConverter<TModel, TStorage> : IFieldConverter
{
    /// <inheritdoc cref="IFieldConverter.ToStoredValue"/>
    public abstract TStorage ToStoredValue(TModel value);

    /// <inheritdoc cref="IFieldConverter.FromStoredValue"/>
    public abstract TModel FromStoredValue(TStorage value);
    
    /// <summary>
    /// Returns a mapper that is configured with a specific format.
    /// </summary>
    /// <param name="format">The user-defined <see cref="ColumnAttribute.Format"/>.</param>
    public abstract FormattedFieldConverter<TModel, TStorage> SpecializeFor(string? format);

    bool IFieldConverter.CanHandle(Type fieldType) => fieldType == typeof(TModel);

    Type IFieldConverter.StorageType => typeof(TStorage);

    IFieldConverter IFieldConverter.SpecializeFor(Type fieldType, IColumnInfo info) => this.SpecializeFor(info.Format);

    object? IFieldConverter.ToStoredValue(object? value) => this.ToStoredValue((TModel)value!);

    object? IFieldConverter.FromStoredValue(object? value) => this.FromStoredValue((TStorage)value!);
}

}
