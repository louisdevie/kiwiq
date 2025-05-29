using System;
using System.Collections.Concurrent;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Builtin;

namespace KiwiQuery.Mapped.Mappers.Fields
{

internal class FieldMapperCollection : IFieldMapperCollection
{
    private readonly ConcurrentStack<IFieldMapper> ownMappers;
    private readonly IFieldMapperCollection? lower;

    public FieldMapperCollection(IFieldMapperCollection? lower = null)
    {
        this.ownMappers = new ConcurrentStack<IFieldMapper>();
        this.lower = lower;
    }
    
    public void Register(IFieldConverter converter)
    {
        this.Register(new ConverterMapper(converter));
    }

    public void Register(IFieldMapper mapper)
    {
        this.ownMappers.Push(mapper);
    }

    IFieldMapper IFieldMapperCollection.GetMapper(Type fieldType, IColumnInfo info, IFieldMapperCollection topCollection)
    {
        IFieldMapper? found = null;
        
        foreach (IFieldMapper mapper in this.ownMappers)
        {
            if (found == null && mapper.CanHandle(fieldType))
            {
                found = mapper.SpecializeFor(fieldType, info, topCollection);
            }
        }
        
        if (found == null && this.lower != null)
        {
            found = this.lower.GetMapper(fieldType, info, topCollection);
        }

        return found ?? throw new InvalidFieldTypeException(fieldType);
    }
}

}