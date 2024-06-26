using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{

internal static class TemporalMapper
{
    public static void RegisterAll(SharedMappers sharedMappers)
    {
        sharedMappers.Register(new DateTime());
    }

    private class DateTime : IFieldMapper
    {
        private readonly string? format;

        public DateTime(string? format = null)
        {
            this.format = format;
        }

        public bool CanHandle(Type fieldType) => fieldType == typeof(System.DateTime);

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info)
        {
            return new DateTime(info.Format);
        }

        public object ReadValue(IDataRecord record, int offset)
        {
            return this.format == null
                ? record.GetDateTime(offset)
                : System.DateTime.ParseExact(record.GetString(offset), this.format, CultureInfo.InvariantCulture);
        }

        public IEnumerable<object?> WriteValue(object? fieldValue)
        {
            object? value;
            if (this.format == null)
            {
                value = fieldValue;
            }
            else
            {
                var dateTimeValue = (System.DateTime)fieldValue!;
                value = dateTimeValue.ToString(this.format, CultureInfo.InvariantCulture);
            }
            return Maybe.Just(value);
        }

        public IEnumerable<string> MetaColumns => Maybe.Nothing<string>();

        public bool CanMapIntegerKey => false;
    }
}

}
