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
    
internal class EnumMapper : IFieldMapper
{
    public bool CanHandle(Type fieldType) => fieldType.IsEnum;

    public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection)
    {
        IFieldMapper underlyingMapper;
        try
        {
            underlyingMapper = collection.GetMapper(fieldType.GetEnumUnderlyingType(), new NoColumnInfo());
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

    public bool CanMapIntegerKey => true;

    private class Specialized : IFieldMapper
    {
        private readonly Type enumType;
        private readonly IFieldMapper underlyingMapper;

        public Specialized(Type enumType, IFieldMapper underlyingMapper)
        {
            this.enumType = enumType;
            this.underlyingMapper = underlyingMapper;
        }

        public bool CanHandle(Type fieldType) => fieldType == this.enumType;

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection) => this;

        public object? ReadValue(IDataRecord record, int offset)
        {
            return this.underlyingMapper.ReadValue(record, offset);
        }

        public IEnumerable<object?> WriteValue(object? fieldValue)
        { 
            return this.underlyingMapper.WriteValue(fieldValue);
        }

        public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();

        public bool CanMapIntegerKey => true;
    }
}
    
}