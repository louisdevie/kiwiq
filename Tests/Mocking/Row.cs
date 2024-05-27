using System.Collections;

namespace KiwiQuery.Tests.Mocking
{
    public class Row : IEnumerable<RowValue>
    {
        private readonly List<RowValue> values;
        
        public Row()
        {
            this.values = new List<RowValue>();
        }

        public long Length => this.values.Count();

        public void Add(object? value)
        {
            this.values.Add(new RowValue(value));
        }

        public IEnumerator<RowValue> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        public RowValue At(int ordinal)
        {
            return this.values[ordinal];
        }
    }
}