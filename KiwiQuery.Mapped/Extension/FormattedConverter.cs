using System;
using System.Data;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// A generic implementation of <see cref="IConverter"/> that takes into account the
/// <see cref="ColumnAttribute.Format"/> option.
/// </summary>
/// <typeparam name="T">The type of values handled.</typeparam>
public abstract class FormattedConverter<T> : IConverter
{
    /// <inheritdoc cref="IConverter.GetValue"/>
    public abstract T GetValue(IDataRecord record, int ordinal);

    /// <inheritdoc cref="IConverter.ToParameter"/>
    public abstract object? ToParameter(T value);

    /// <summary>
    /// Returns a mapper that is configured with a specific format.
    /// </summary>
    /// <param name="format">The user-defined <see cref="ColumnAttribute.Format"/>.</param>
    public abstract FormattedConverter<T> SpecializeFor(string? format);

    object? IConverter.GetValue(IDataRecord record, int ordinal) => this.GetValue(record, ordinal);

    object? IConverter.ToParameter(object? value) => this.ToParameter((T)value!);

    bool IConverter.CanHandle(Type fieldType) => fieldType == typeof(T);

    IConverter IConverter.SpecializeFor(Type fieldType, IColumnInfos infos) => this.SpecializeFor(infos.Format);
}

}
