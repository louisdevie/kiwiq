using System.Data;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{

internal static class UnSignedMapper
{
    public static void RegisterAll(SharedMappers sharedMappers)
    {
        sharedMappers.Register(new SByte());
        sharedMappers.Register(new UInt16());
        sharedMappers.Register(new UInt32());
        sharedMappers.Register(new UInt64());
    }

    private class SByte : FieldMapper<sbyte>
    {
        public override sbyte GetValue(IDataRecord record, int offset) => (sbyte)record.GetByte(offset);
    }

    private class UInt16 : FieldMapper<ushort>
    {
        public override ushort GetValue(IDataRecord record, int offset) => (ushort)record.GetInt16(offset);
    }

    private class UInt32 : FieldMapper<uint>
    {
        public override uint GetValue(IDataRecord record, int offset) => (uint)record.GetInt32(offset);
    }

    private class UInt64 : FieldMapper<ulong>
    {
        public override ulong GetValue(IDataRecord record, int offset) => (ulong)record.GetInt64(offset);
    }
}

}
