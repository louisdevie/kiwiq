using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using KiwiQuery.Mapped.Extension;
using KiwiQuery.Mapped.Mappers.Builtin;

namespace KiwiQuery.Mapped.Mappers.Fields
{

/// <summary>
/// A thread-safe, singleton implementation of <see cref="IFieldMapperCollection"/>. You can use it to register mappers
/// for the whole application.
/// </summary>
public class SharedMappers : IFieldMapperCollection
{
    private static SharedMappers? current;

    /// <summary>
    /// An instance shared by all connections. Use it to register mappers for the whole application.
    /// </summary>
    public static SharedMappers Current => current ??= new SharedMappers();

    private readonly HashSet<string> loadedAssemblies;
    private readonly ConcurrentStack<IFieldMapper> mappers;
    private readonly ConcurrentDictionary<string, IFieldMapper> resolved;

    private SharedMappers()
    {
        this.loadedAssemblies = new HashSet<string>();
        this.mappers = new ConcurrentStack<IFieldMapper>();
        this.resolved = new ConcurrentDictionary<string, IFieldMapper>();
        
        BasicTypesMapper.RegisterAll(this);
        SpanMapper.RegisterAll(this);
        TemporalMapper.RegisterAll(this);
        UnSignedMapper.RegisterAll(this);
    }

    internal void LoadMappersFrom(Assembly assembly)
    {
        bool shouldLoad = false;
        lock (this.loadedAssemblies)
        {
            shouldLoad = this.loadedAssemblies.Add(assembly.FullName);
        }

        if (shouldLoad)
        {
            foreach (Type type in assembly.GetTypes())
            {
                this.TryRegisterType(type);
            }
        }
    }

    private void TryRegisterType(Type type)
    {
        bool loaded = false;
        foreach (Attribute attr in type.GetCustomAttributes())
        {
            if (!loaded
                && attr is SharedConverterAttribute
                && typeof(IConverter).IsAssignableFrom(type)
                && this.TryInvokeParameterlessConstructor(type, out object? converter))
            {
                this.Register((IConverter)converter);
                loaded = true;
            }
            if (!loaded
                && attr is SharedMapperAttribute
                && this.TryInvokeParameterlessConstructor(type, out object? mapper))
            {
                this.Register((IFieldMapper)mapper);
                loaded = true;
            }
        }
    }

    private bool TryInvokeParameterlessConstructor(Type type, [NotNullWhen(true)] out object? instance)
    {
        bool success = false;
        instance = null;
        if (type.GetConstructor(Array.Empty<Type>()) is { } constructor)
        {
            try
            {
                instance = constructor.Invoke(Array.Empty<object>());
                success = true;
            }
            catch (SystemException) { }
        }
        return success;
    }

    /// <summary>
    /// Register a new converter.
    /// </summary>
    /// <param name="converter">A converter instance.</param>
    public void Register(IConverter converter)
    {
        this.Register(new ConverterMapper(converter));
    }

    /// <summary>
    /// Register a new field mapper.
    /// </summary>
    /// <param name="mapper">A field mapper instance.</param>
    public void Register(IFieldMapper mapper)
    {
        this.mappers.Push(mapper);
    }

    internal IFieldMapper GetMapper(Type fieldType, IColumnInfos infos)
    {
        return this.resolved.GetOrAdd(fieldType.FullName ?? fieldType.Name,
            (key) => this.ResolveMapper(fieldType, infos));
    }

    private IFieldMapper ResolveMapper(Type fieldType, IColumnInfos infos)
    {
        IFieldMapper? found = null;

        foreach (IFieldMapper mapper in this.mappers)
        {
            if (found == null && mapper.CanHandle(fieldType))
            {
                found = mapper.SpecializeFor(fieldType, infos);
            }
        }

        return found ?? new GenericMapper();
    }
}

}
