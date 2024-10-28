using System.Data;
using KiwiQuery.Mapped.Extension;

namespace KiwiQuery.Mapped.Mappers.Builtin
{

internal static class BasicTypesMapper
{
    public static void RegisterAll(SharedMappers sharedMappers)
    {
        sharedMappers.Register(new Boolean());
        sharedMappers.Register(new Byte());
        sharedMappers.Register(new Char());
        sharedMappers.Register(new Decimal());
        sharedMappers.Register(new Double());
        sharedMappers.Register(new Float());
        sharedMappers.Register(new Guid());
        sharedMappers.Register(new Int16());
        sharedMappers.Register(new Int32());
        sharedMappers.Register(new Int64());
        sharedMappers.Register(new String());
    }

    private class Boolean : FieldMapper<bool> {
        protected override bool ReadValue(IDataRecord record, int offset) => record.GetBoolean(offset);

        protected override object WriteValue(bool value) => value;
    }

    private class Byte : FieldMapper<byte> {
        protected override byte ReadValue(IDataRecord record, int offset) => record.GetByte(offset);

        protected override object WriteValue(byte value) => value;
     }

    private class Char : FieldMapper<char> {
        protected override char ReadValue(IDataRecord record, int offset) => record.GetChar(offset);

        protected override object WriteValue(char value) => value;
     }

    private class Decimal : FieldMapper<decimal> {
        protected override decimal ReadValue(IDataRecord record, int offset) => record.GetDecimal(offset);

        protected override object WriteValue(decimal value) => value;
     }

    private class Double : FieldMapper<double> {
        protected override double ReadValue(IDataRecord record, int offset) => record.GetDouble(offset);

        protected override object WriteValue(double value) => value;
     }

    private class Float : FieldMapper<float> {
        protected override float ReadValue(IDataRecord record, int offset) => record.GetFloat(offset);

        protected override object WriteValue(float value) => value;
     }

    private class Guid : FieldMapper<System.Guid> {
        protected override System.Guid ReadValue(IDataRecord record, int offset) => record.GetGuid(offset);

        protected override object WriteValue(System.Guid value) => value;
     }

    private class Int16 : FieldMapper<short> {
        protected override short ReadValue(IDataRecord record, int offset) => record.GetInt16(offset);

        protected override object WriteValue(short value) => value;
    }

    private class Int32 : FieldMapper<int> {
        protected override int ReadValue(IDataRecord record, int offset) => record.GetInt32(offset);

        protected override object WriteValue(int value) => value;

        public override bool CanMapIntegerKey => true;

        public override object MapIntegerKey(int key) => key;
     }

    private class Int64 : FieldMapper<long> {
        protected override long ReadValue(IDataRecord record, int offset) => record.GetInt64(offset);

        protected override object WriteValue(long value) => value;

        public override bool CanMapIntegerKey => true;

        public override object MapIntegerKey(int key) => (long)key;
     }

    private class String : FieldMapper<string?> {
        protected override string? ReadValue(IDataRecord record, int offset) => record.IsDBNull(offset) ? null : record.GetString(offset);

        protected override object? WriteValue(string? value) => value;
     }
}

}
