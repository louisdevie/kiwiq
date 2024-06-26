using System;
using System.Collections.Generic;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Builtin;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal static class DefaultMapperResolver
{
    public static IFieldMapper ResolveFromList(IEnumerable<IFieldMapper> mappers, Type fieldType, IColumnInfo info)
    {
        IFieldMapper? found = null;

        foreach (IFieldMapper mapper in mappers)
        {
            if (found == null && mapper.CanHandle(fieldType))
            {
                found = mapper.SpecializeFor(fieldType, info);
            }
        }

        return found ?? throw new InvalidFieldTypeException(fieldType);
    }
}

}