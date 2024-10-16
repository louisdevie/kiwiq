using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Helpers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Mappers.Builtin
{

internal static class SpanMapper
{
    public static void RegisterAll(SharedMappers sharedMappers)
    {
        sharedMappers.Register(new Bytes());
        sharedMappers.Register(new Chars());
    }

    private const int INITIAL_BUFFER_SIZE = 32;
    private const int BUFFER_EXPANSION_THRESHOLD = 4;

    private interface IReadingStream<T>
    {
        void Push(T[] buffer, int count);

        T[] ToArray();
    }

    private abstract class ArrayOf<T> : IFieldMapper
    {
        private readonly string? sizeColumn;

        protected ArrayOf(string? sizeColumn)
        {
            this.sizeColumn = sizeColumn;
        }

        protected abstract ArrayOf<T> Clone(string? sizeColumn);

        public bool CanHandle(Type fieldType) => fieldType == typeof(T[]);

        public IFieldMapper SpecializeFor(Type fieldType, IColumnInfo info, IFieldMapperCollection collection) => this.Clone(info.Size);

        public object ReadValue(IDataRecord record, int offset)
        {
            return this.sizeColumn == null ? this.GetValueDynamic(record, offset) : this.GetValueSized(record, offset);
        }

        public IEnumerable<string> MetaColumns => Maybe.FromNullable(this.sizeColumn);

        public bool CanMapIntegerKey => false;

        private T[] GetValueSized(IDataRecord record, int offset)
        {
            int size = record.GetInt32(offset + 1);
            var array = new T[size];
            this.ReadIntoBuffer(record, offset, 0, array, size);
            return array;
        }
        
        private T[] GetValueDynamic(IDataRecord record, int offset)
        {
            var stream = this.GetReadingStream();
            int bufferSize = INITIAL_BUFFER_SIZE;
            int read;
            long totalRead = 0;
            var buffer = new T[bufferSize];

            do
            {
                read = this.ReadIntoBuffer(record, offset, totalRead, buffer, bufferSize);
                stream.Push(buffer, read);
                totalRead += read;
                if (totalRead >= bufferSize * BUFFER_EXPANSION_THRESHOLD)
                {
                    bufferSize *= 2;
                    buffer = new T[bufferSize];
                }
            } while (read == bufferSize);

            return stream.ToArray();
        }

        protected abstract IReadingStream<T> GetReadingStream();

        protected abstract int ReadIntoBuffer(
            IDataRecord record, int ordinal, long readOffset, T[] buffer, int bufferSize
        );

        IEnumerable<object?> IFieldMapper.WriteValue(object? fieldValue)
        {
            var values = new List<object?>{ fieldValue };
            if (this.sizeColumn == null)
            {
                var arrayValue = (T[])fieldValue!;
                values.Add(arrayValue.Length);
            }
            return values;
        }
    }

    private class Bytes : ArrayOf<byte>
    {
        public Bytes(string? sizeColumn = null) : base(sizeColumn) { }

        protected override ArrayOf<byte> Clone(string? sizeColumn) => new Bytes(sizeColumn);

        protected override IReadingStream<byte> GetReadingStream() => new BytesReadingStream();

        protected override int ReadIntoBuffer(
            IDataRecord record, int ordinal, long readOffset, byte[] buffer, int bufferSize
        )
        {
            return (int)record.GetBytes(ordinal, readOffset, buffer, 0, bufferSize);
        }
    }

    private class BytesReadingStream : IReadingStream<byte>
    {
        private readonly MemoryStream stream = new MemoryStream();

        public void Push(byte[] buffer, int count) => this.stream.Write(buffer, 0, count);

        public byte[] ToArray() => this.stream.ToArray();
    }

    private class Chars : ArrayOf<char>
    {
        public Chars(string? sizeColumn = null) : base(sizeColumn) { }

        protected override ArrayOf<char> Clone(string? sizeColumn) => new Chars(sizeColumn);

        protected override IReadingStream<char> GetReadingStream() => new CharsReadingStream();

        protected override int ReadIntoBuffer(
            IDataRecord record, int ordinal, long readOffset, char[] buffer, int bufferSize
        )
        {
            return (int)record.GetChars(ordinal, readOffset, buffer, 0, bufferSize);
        }
    }

    private class CharsReadingStream : IReadingStream<char>
    {
        private readonly List<char> list = new List<char>();

        public void Push(char[] buffer, int count)
        {
            for (var i = 0; i < count; i++)
            {
                this.list.Add(buffer[i]);
            }
        }

        public char[] ToArray() => this.list.ToArray();
    }
}

}
