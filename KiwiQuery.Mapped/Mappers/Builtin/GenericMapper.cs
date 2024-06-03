using System;
using System.Collections.Generic;
using System.Data;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{
    internal class GenericMapper : IFieldMapper
    {
        public bool CanHandle(Type fieldType) => false; // never registered

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfos infos) => this;

        public object? ReadValue(IDataRecord record, int offset) => record.GetValue(offset);

        public IEnumerable<object?> WriteValue(object? fieldValue) => Maybe.Just(fieldValue);

        public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();
    }
}