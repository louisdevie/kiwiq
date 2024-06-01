using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{
    internal class ConverterMapper : IFieldMapper
    {
        private readonly IConverter converter;

        public ConverterMapper(IConverter converter)
        {
            this.converter = converter;
        }

        public bool CanHandle(Type fieldType) => this.converter.CanHandle(fieldType);

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfos infos) => this;

        public object? GetValue(IDataRecord record, int offset) => this.converter.GetValue(record, offset);

        public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();
    }
}