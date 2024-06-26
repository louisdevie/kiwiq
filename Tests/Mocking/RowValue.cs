namespace KiwiQuery.Tests.Mocking
{
    public class RowValue
    {
        private readonly object? value;

        public RowValue(object? value)
        {
            this.value = value;
        }

        public static implicit operator RowValue(int @int) => new (@int);
        public static implicit operator RowValue(string @string) => new (@string);

        public object? Value => this.value;

        public string GetDataTypeName()
        {
            Type? type = this.GetFieldType();
            return type?.FullName ?? type?.Name ?? "null";
        }

        public T As<T>()
        {
            if (this.value is null)
            {
                throw new InvalidCastException($"Cannot cast null to {typeof(T).FullName}");
            }

            Type type = this.GetFieldType()!;
            if (!type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"Cannot cast {type.FullName} to {typeof(T).FullName}");
            }
            
            return (T)this.value;
        }

        public Type? GetFieldType()
        {
            return this.value?.GetType();
        }

        public bool IsDBNull()
        {
            return this.value is null;
        }
    }
}