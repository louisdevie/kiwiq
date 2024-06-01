using System;
using System.Data;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Fields;

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
        public override bool GetValue(IDataRecord record, int offset) => record.GetBoolean(offset);
     }

    private class Byte : FieldMapper<byte> {
        public override byte GetValue(IDataRecord record, int offset) => record.GetByte(offset);
     }

    private class Char : FieldMapper<char> {
        public override char GetValue(IDataRecord record, int offset) => record.GetChar(offset);
     }

    private class Decimal : FieldMapper<decimal> {
        public override decimal GetValue(IDataRecord record, int offset) => record.GetDecimal(offset);
     }

    private class Double : FieldMapper<double> {
        public override double GetValue(IDataRecord record, int offset) => record.GetDouble(offset);
     }

    private class Float : FieldMapper<float> {
        public override float GetValue(IDataRecord record, int offset) => record.GetFloat(offset);
     }

    private class Guid : FieldMapper<System.Guid> {
        public override System.Guid GetValue(IDataRecord record, int offset) => record.GetGuid(offset);
     }

    private class Int16 : FieldMapper<short> {
        public override short GetValue(IDataRecord record, int offset) => record.GetInt16(offset);
     }

    private class Int32 : FieldMapper<int> {
        public override int GetValue(IDataRecord record, int offset) => record.GetInt32(offset);
     }

    private class Int64 : FieldMapper<long> {
        public override long GetValue(IDataRecord record, int offset) => record.GetInt64(offset);
     }

    private class String : FieldMapper<string> {
        public override string GetValue(IDataRecord record, int offset) => record.GetString(offset);
     }
}

}
