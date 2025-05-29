using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using KiwiQuery.Mapped.Mappers.Builtin;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped.Extension
{

/// <summary>
/// A singleton implementation of <see cref="IFieldMapperCollection"/>. You can use it to register
/// mappers for the whole application.
/// </summary>
public class SharedMappers : IFieldMapperCollection
{
    private static SharedMappers? current;

    /// <summary>
    /// An instance shared by all connections. Use it to register mappers for the whole application.
    /// </summary>
    public static SharedMappers Current => current ??= new SharedMappers();

    private readonly HashSet<string> loadedAssemblies;
    private readonly FieldMapperCollection mappers;

    private SharedMappers()
    {
        this.loadedAssemblies = new HashSet<string>();
        this.mappers = new FieldMapperCollection();

        BasicTypesMapper.RegisterAll(this);
        SpanMapper.RegisterAll(this);
        TemporalMapper.RegisterAll(this);
        UnSignedMapper.RegisterAll(this);
        this.Register(new EnumMapper());
        this.Register(new NullableMapper());

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            this.LoadMappersFrom(assembly);
        }
        AppDomain.CurrentDomain.AssemblyLoad += this.CurrentDomainOnAssemblyLoad;
    }

    private void CurrentDomainOnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
    {
        this.LoadMappersFrom(args.LoadedAssembly);
    }

    private void LoadMappersFrom(Assembly assembly)
    {
        bool shouldLoad;
        lock (this.loadedAssemblies)
        {
            shouldLoad = this.loadedAssemblies.Add(assembly.FullName ?? assembly.Location);
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
                && typeof(IFieldConverter).IsAssignableFrom(type)
                && this.TryInvokeParameterlessConstructor(type, out object? converter))
            {
                this.mappers.Register((IFieldConverter)converter);
                loaded = true;
            }
            if (!loaded
                && attr is SharedMapperAttribute
                && typeof(IFieldMapper).IsAssignableFrom(type)
                && this.TryInvokeParameterlessConstructor(type, out object? mapper))
            {
                this.mappers.Register((IFieldMapper)mapper);
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

    /// <inheritdoc />
    public void Register(IFieldConverter converter)
    {
        this.mappers.Register(converter);
    }

    /// <inheritdoc />
    public void Register(IFieldMapper mapper)
    {
        this.mappers.Register(mapper);
    }

    IFieldMapper IFieldMapperCollection.GetMapper(Type fieldType, IColumnInfo info, IFieldMapperCollection topCollection)
    {
        return ((IFieldMapperCollection)this.mappers).GetMapper(fieldType, info, topCollection);
    }
}

}
