using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace KiwiQuery.Mapped.Queries
{


internal class UnbufferedReader : IEnumerator
{
    private readonly DbDataReader reader;

    public UnbufferedReader(DbDataReader reader)
    {
        this.reader = reader;
    }

    public bool MoveNext()
    {
        return this.reader.Read();
    }

    public void Reset()
    {
        throw new InvalidOperationException("Cannot reset a data reader as it is a forward-only stream.");
    }
    
    object IEnumerator.Current => throw new NotImplementedException();

    public void Dispose()
    {
        this.reader.Dispose();
    }
}

internal class UnbufferedReader<T> : IEnumerator<T>, IAsyncEnumerator<T>
where T : notnull
{
    private readonly DbDataReader reader;

    public UnbufferedReader(DbDataReader reader)
    {
        this.reader = reader;
    }

    public bool MoveNext()
    {
        return this.reader.Read();
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        return await this.reader.ReadAsync();
    }

    public void Reset()
    {
        throw new InvalidOperationException("Cannot reset a data reader as it is a forwad-only stream.");
    }

    public T Current => throw new NotImplementedException();

    object IEnumerator.Current => this.Current;

    public void Dispose()
    {
        this.reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await this.reader.DisposeAsync();
    }
}

}
