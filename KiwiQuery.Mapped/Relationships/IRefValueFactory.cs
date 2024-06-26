namespace KiwiQuery.Mapped.Relationships
{

internal interface IRefValueFactory<T>
#if NET8_0_OR_GREATER
where T : notnull
#else
where T : class
#endif
{
    T? MakeValue();
}

}
