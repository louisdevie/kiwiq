using System;
using System.Collections;
using System.Collections.Generic;

namespace KiwiQuery.Mapped.Helpers
{

internal static class Maybe
{
    public static Maybe<T> Just<T>(T value)
    {
        return new Maybe<T>(value);
    }

#if NET6_0_OR_GREATER
    public static Maybe<T> FromNullable<T>(T? value)
    {
        return value != null ? new Maybe<T>(value) : new Maybe<T>();
    }
#else 
    public static Maybe<T> FromNullable<T>(T? value)
    where T : class
    {
        return value != null ? new Maybe<T>(value) : new Maybe<T>();
    }
    
    public static Maybe<T> FromNullable<T>(T? value)
    where T : struct
    {
        return value.HasValue ? new Maybe<T>(value.Value) : new Maybe<T>();
    }
#endif
    
    public static Maybe<T> Nothing<T>()
    {
        return new Maybe<T>();
    }
}

internal class Maybe<T> : IEnumerable<T>
{
    private interface IVariant
    {
        IEnumerator<T> GetEnumerator();
    }

#if NET6_0_OR_GREATER
    private record NothingVariant : IVariant
    {
        public IEnumerator<T> GetEnumerator() => new NothingEnumerator();
    }
#else
    private class NothingVariant : IVariant
    {
        public IEnumerator<T> GetEnumerator() => new NothingEnumerator();
    }
#endif
    
    private class NothingEnumerator : IEnumerator<T>
    {
        public bool MoveNext() => false;

        public void Reset() {}

        public T Current => throw new InvalidOperationException("Attempt to read nothing.");

        object? IEnumerator.Current => this.Current;

        public void Dispose() { }
    }

#if NET6_0_OR_GREATER
    private record JustVariant(T Value) : IVariant
    {
        public IEnumerator<T> GetEnumerator() => new JustEnumerator(this);
    }
#else
    private class JustVariant : IVariant
    {
        public JustVariant(T value)
        {
            this.Value = value;
        }

        public T Value { get; }
        
        public IEnumerator<T> GetEnumerator() => new JustEnumerator(this);
    }
#endif

    private class JustEnumerator : IEnumerator<T>
    {
        private readonly JustVariant wrapped;
        private bool read;

        public JustEnumerator(JustVariant wrapped)
        {
            this.wrapped = wrapped;
            this.read = false;
        }

        public bool MoveNext()
        {
            bool notReadYet = !this.read;
            this.read = true;
            return notReadYet;
        }

        public void Reset()
        {
            this.read = false;
        }

        public T Current => this.wrapped.Value;

        object? IEnumerator.Current => this.wrapped.Value;

        public void Dispose() { }
    }

    private readonly IVariant variant;

    public Maybe()
    {
        this.variant = new NothingVariant();
    }

    public Maybe(T value)
    {
        this.variant = new JustVariant(value);
    }

    public IEnumerator<T> GetEnumerator() => this.variant.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.variant.GetEnumerator();
}

}
