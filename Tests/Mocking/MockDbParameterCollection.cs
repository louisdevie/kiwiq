using System.Collections;
using System.Data.Common;

namespace Tests.Mocking
{
    internal class MockDbParameterCollection : DbParameterCollection, IEnumerable<MockDbParameter>
    {
        private List<MockDbParameter> parameters = new();

        public override int Count => this.parameters.Count;

        public override object SyncRoot => this.parameters;

        public override int Add(object value)
        {
            this.parameters.Add((MockDbParameter)value);
            return this.parameters.Count - 1;
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }

        IEnumerator<MockDbParameter> IEnumerable<MockDbParameter>.GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }
    }
}
