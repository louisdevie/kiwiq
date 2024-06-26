using System;
using System.Data;

namespace KiwiQuery.Mapped.Helpers
{

internal class OffsetRecord : IDataRecord
{
    private readonly IDataRecord original;
    private readonly int offset;

    public OffsetRecord(IDataRecord original, int offset)
    {
        this.original = original;
        this.offset = offset;
    }

    public bool GetBoolean(int i) => this.original.GetBoolean(this.offset + i);

    public byte GetByte(int i) => this.original.GetByte(this.offset + i);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        => this.original.GetBytes(this.offset + i, fieldOffset, buffer, bufferoffset, length);

    public char GetChar(int i) => this.original.GetChar(this.offset + i);

    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length)
        => this.original.GetChars(this.offset + i, fieldoffset, buffer, bufferoffset, length);

    public IDataReader? GetData(int i) => this.original.GetData(this.offset + i);

    public string GetDataTypeName(int i) => this.original.GetDataTypeName(this.offset + i);

    public DateTime GetDateTime(int i) => this.original.GetDateTime(this.offset + i);

    public decimal GetDecimal(int i) => this.original.GetDecimal(this.offset + i);

    public double GetDouble(int i) => this.original.GetDouble(this.offset + i);

    public Type GetFieldType(int i) => this.original.GetFieldType(this.offset + i);

    public float GetFloat(int i) => this.original.GetFloat(this.offset + i);

    public Guid GetGuid(int i) => this.original.GetGuid(this.offset + i);

    public short GetInt16(int i) => this.original.GetInt16(this.offset + i);

    public int GetInt32(int i) => this.original.GetInt32(this.offset + i);

    public long GetInt64(int i) => this.original.GetInt64(this.offset + i);

    public string GetName(int i) => this.original.GetName(this.offset + i);

    public int GetOrdinal(string name) => this.original.GetOrdinal(name);

    public string GetString(int i) => this.original.GetString(this.offset + i);

    public object GetValue(int i) => this.original.GetValue(this.offset + i);

    public int GetValues(object[] values) => this.original.GetValues(values);

    public bool IsDBNull(int i) => this.original.IsDBNull(this.offset + i);

    public int FieldCount => this.original.FieldCount;

    public object this[int i] => this.original[this.offset + i];

    public object this[string name] => this.original[name];
}

}
