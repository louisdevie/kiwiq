using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using KiwiQuery.Mapped.Exceptions.Internal;

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

internal readonly struct Maybe<T> : IEnumerable<T>
{
    private readonly T value;
    private readonly bool isSomething;

    public Maybe(T value)
    {
        this.value = value;
        this.isSomething = true;
    }

    private class Enumerator : IEnumerator<T>
    {
        private readonly Maybe<T> parent;
        private bool alreadyRead;

        public Enumerator(Maybe<T> parent)
        {
            this.parent = parent;
            this.alreadyRead = false;
        }

        public bool MoveNext()
        {
            bool canRead = this.parent.isSomething && !this.alreadyRead;
            this.alreadyRead = true;
            return canRead;
        }

        public void Reset()
        {
            this.alreadyRead = false;
        }

        public T Current => this.parent.Value;

        object? IEnumerator.Current => this.parent.Value;

        public void Dispose() { }
    }

    [Pure]
    public bool IsSomething => this.isSomething;
    
    [Pure]
    public T Value => this.isSomething ? this.value : throw new MaybeHadNoValueException();

    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
}

}
