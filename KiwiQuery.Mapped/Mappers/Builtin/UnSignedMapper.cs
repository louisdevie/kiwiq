using System.Data;
using KiwiQuery.Mapped.Extension;

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
        protected override sbyte ReadValue(IDataRecord record, int offset) => (sbyte)record.GetByte(offset);

        protected override object? WriteValue(sbyte value) => (byte)value;
    }

    private class UInt16 : FieldMapper<ushort>
    {
        protected override ushort ReadValue(IDataRecord record, int offset) => (ushort)record.GetInt16(offset);

        protected override object? WriteValue(ushort value) => (short)value;
    }

    private class UInt32 : FieldMapper<uint>
    {
        protected override uint ReadValue(IDataRecord record, int offset) => (uint)record.GetInt32(offset);

        protected override object? WriteValue(uint value) => (int)value;

        public override bool CanMapIntegerKey => true;

        public override object? MapIntegerKey(int key) => (uint)key;
    }

    private class UInt64 : FieldMapper<ulong>
    {
        protected override ulong ReadValue(IDataRecord record, int offset) => (ulong)record.GetInt64(offset);

        protected override object? WriteValue(ulong value) => (long)value;

        public override bool CanMapIntegerKey => true;

        public override object? MapIntegerKey(int key) => (ulong)key;
    }
}

}
