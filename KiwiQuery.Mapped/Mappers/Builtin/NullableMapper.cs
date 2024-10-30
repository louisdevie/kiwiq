using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Mapped.Exceptions.Internal;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{
    
internal class NullableMapper : IFieldMapper
{
    public bool CanHandle(Type fieldType) => fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>);

    public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection)
    {
        IFieldMapper underlyingMapper;
        try
        {
            underlyingMapper = collection.GetMapper(Nullable.GetUnderlyingType(fieldType)!, new NoColumnInfo());
        }
        catch (InvalidFieldTypeException e)
        {
            throw new InvalidFieldTypeException(fieldType, $", because the underlying type {e.ProblematicType.FullName ?? e.ProblematicType.Name} cannot be mapped");
        }
        return new Specialized(fieldType, underlyingMapper);
    }

    public object? ReadValue(IDataRecord record, int offset) => throw new MapperMustBeSpecializedException();

    public IEnumerable<object?> WriteValue(object? fieldValue) => throw new MapperMustBeSpecializedException();

    public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();

    public bool CanMapIntegerKey => false;

    private class Specialized : IFieldMapper
    {
        private readonly Type nullableType;
        private readonly IFieldMapper underlyingMapper;

        public Specialized(Type nullableType, IFieldMapper underlyingMapper)
        {
            this.nullableType = nullableType;
            this.underlyingMapper = underlyingMapper;
        }

        public bool CanHandle(Type fieldType) => fieldType == this.nullableType;

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection) => this;

        public object? ReadValue(IDataRecord record, int offset)
        {
            return record.IsDBNull(offset) ? null : this.underlyingMapper.ReadValue(record, offset);
        }

        public IEnumerable<object?> WriteValue(object? fieldValue)
        { 
            return fieldValue == null ? Maybe.Just<object?>(null) : this.underlyingMapper.WriteValue(fieldValue);
        }

        public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();

        public bool CanMapIntegerKey => this.underlyingMapper.CanMapIntegerKey;
    }
}
    
}