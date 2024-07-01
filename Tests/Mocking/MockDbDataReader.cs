using System.Collections;
using System.Data.Common;

namespace KiwiQuery.Tests.Mocking
{
    public class MockDbDataReader : DbDataReader
    {
        private readonly List<String> names;
        private readonly List<Row> rows;
        private int i;

        public MockDbDataReader(IEnumerable<string> names, IEnumerable<Row> rows)
        {
            this.names = names.ToList();
            this.rows = rows.ToList();
            this.i = -1;
        }

        public override bool GetBoolean(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<bool>();
        }

        public override byte GetByte(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<byte>();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            long maxLength;
            if (buffer == null)
            {
                maxLength = 0;
            }
            else
            {
                byte[] array = this.rows[this.i].At(ordinal).As<byte[]>();
                maxLength = Math.Min(Math.Min(array.Length - dataOffset, buffer.Length - bufferOffset), length);
                Array.Copy(array, dataOffset, buffer, bufferOffset, maxLength);
            }

            return maxLength;
        }

        public override char GetChar(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<char>();
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            long maxLength;
            if (buffer == null)
            {
                maxLength = 0;
            }
            else
            {
                char[] array = this.rows[this.i].At(ordinal).As<char[]>();
                maxLength = Math.Min(Math.Min(array.Length - dataOffset, buffer.Length - bufferOffset), length);
                Array.Copy(array, dataOffset, buffer, bufferOffset, maxLength);
            }

            return maxLength;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return this.rows[this.i].At(ordinal).GetDataTypeName();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<DateTime>();
        }

        public override decimal GetDecimal(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<decimal>();
        }

        public override double GetDouble(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<double>();
        }

        public override Type GetFieldType(int ordinal)
        {
            return this.rows[this.i].At(ordinal).GetFieldType() ??
                   throw new InvalidOperationException("Cannot get type of null field");
        }

        public override float GetFloat(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<float>();
        }

        public override Guid GetGuid(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<Guid>();
        }

        public override short GetInt16(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<short>();
        }

        public override int GetInt32(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<int>();
        }

        public override long GetInt64(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<long>();
        }

        public override string GetName(int ordinal)
        {
            return this.names[this.i];
        }

        public override int GetOrdinal(string name)
        {
            return this.names.IndexOf(name);
        }

        public override string GetString(int ordinal)
        {
            return this.rows[this.i].At(ordinal).As<string>();
        }

        public override object GetValue(int ordinal)
        {
            return this.rows[this.i].At(ordinal).Value ??
                   throw new InvalidOperationException("Cannot get null value as an object");
        }

        public override int GetValues(object[] values)
        {
            int maxLength = (int)Math.Min(values.Length, this.rows[this.i].Length);
            for (int j = 0; j < maxLength; j++)
            {
                values[this.i] = this.GetValue(this.i);
            }

            return maxLength;
        }

        public override bool IsDBNull(int ordinal)
        {
            return this.rows[this.i].At(ordinal).IsDBNull();
        }

        public override int FieldCount => this.names.Count;

        public override object this[int ordinal] => throw new NotImplementedException();

        public override object this[string name] => throw new NotImplementedException();

        public override int RecordsAffected => 0;

        public override bool HasRows => this.rows.Count > 0;

        public override bool IsClosed => false;

        public override bool NextResult() => false;

        public override bool Read() => ++this.i < this.rows.Count;

        public override int Depth => 0;

        private class Enumerator : IEnumerator
        {
            public MockDbDataReader Parent { get; init; } = null!;

            public bool MoveNext() => this.Parent.Read();

            public void Reset() => throw new NotImplementedException();

            public object Current => this.Parent.rows[this.Parent.i];
        }

        public override IEnumerator GetEnumerator()
        {
            return new Enumerator { Parent = this };
        }
    }
}