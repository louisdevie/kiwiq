using System;
using KiwiQuery.Mapped.Extension;

namespace KiwiQuery.Mapped.Mappers.Fields
{

public interface IFieldMapperCollection
{
    /// <summary>
    /// Register a new converter.
    /// </summary>
    /// <param name="converter">A converter instance.</param>
    void Register(IFieldConverter converter);

    /// <summary>
    /// Register a new field mapper.
    /// </summary>
    /// <param name="mapper">A field mapper instance.</param>
    void Register(IFieldMapper mapper);

    internal IFieldMapper GetMapper(Type fieldType, IColumnInfo info);
}

}
