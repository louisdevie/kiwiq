using System.Data;
using System.Data.Common;

namespace KiwiQuery.Tests.Mocking
{
    internal class MockDbParameter : DbParameter
    {
        public override DbType DbType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ParameterDirection Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsNullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ParameterName { get; set; } = "";
        public override int Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string SourceColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool SourceColumnNullMapping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override object? Value { get; set; }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return obj is MockDbParameter other
                && (this.Value?.Equals(other.Value) ?? other.Value == null);
        }
    }
}