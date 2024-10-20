using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Exceptions.Internal;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{

internal class ConverterMapper : IFieldMapper
{
    private readonly IFieldConverter converter;

    public ConverterMapper(IFieldConverter converter)
    {
        this.converter = converter;
    }

    public bool CanHandle(Type fieldType) => this.converter.CanHandle(fieldType);

    public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection)
    {
        return new Specialized(
            this.converter.SpecializeFor(fieldType, info),
            collection.GetMapper(this.converter.StorageType, new NoColumnInfo())
        );
    }
    
    public object? ReadValue(IDataRecord record, int offset) => throw new MapperMustBeSpecializedException();

    public IEnumerable<object?> WriteValue(object? fieldValue) => throw new MapperMustBeSpecializedException();

    public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();

    public bool CanMapIntegerKey => false;
    
    private class Specialized : IFieldMapper
    {
        private readonly IFieldConverter converter;
        private readonly IFieldMapper underlyingMapper;

        public Specialized(IFieldConverter converter, IFieldMapper underlyingMapper)
        {
            this.converter = converter;
            this.underlyingMapper = underlyingMapper;
        }

        public bool CanHandle(Type fieldType) => this.converter.CanHandle(fieldType);

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection) => this;

        public object? ReadValue(IDataRecord record, int offset)
        {
            return this.converter.FromStoredValue(this.underlyingMapper.ReadValue(record, offset));
        }

        public IEnumerable<object?> WriteValue(object? fieldValue)
        {
            return this.underlyingMapper.WriteValue(this.converter.ToStoredValue(fieldValue));
        }

        public IEnumerable<string> MetaColumns => this.underlyingMapper.MetaColumns;

        public bool CanMapIntegerKey => this.underlyingMapper.CanMapIntegerKey;

        public object? MapIntegerKey(int key) => this.converter.FromStoredValue(this.underlyingMapper.MapIntegerKey(key));
    }
}

}
