using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Queries;

namespace KiwiQuery.Mapped.Relationships
{

internal class HasOneFactory<T> : IRefValueFactory<T>
#if NET8_0_OR_GREATER
where T : notnull
#else
where T : class
#endif
{
    private readonly IMapper mapper;
    private readonly Predicate predicate;
    private readonly Schema schema;

    private HasOneFactory(IMapper mapper, Predicate predicate, Schema schema)
    {
        this.mapper = mapper;
        this.predicate = predicate;
        this.schema = schema;
    }

    public T? MakeValue()
    {
        MappedSelectQuery query = new MappedSelectQuery(this.schema.Select(), this.mapper).Where(this.predicate);
        
        T? value = default;
        foreach (object? result in query.FetchList()) value = (T?)result;
        
        return value;
    }
}

}
