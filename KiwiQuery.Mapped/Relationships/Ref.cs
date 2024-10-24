using System;
using System.Reflection;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Mapped.Mappers;

namespace KiwiQuery.Mapped.Relationships
{

/// <summary>
/// This type lets you declare a lazy relationship to ONE other entity. To reference multiple entities
/// </summary>
/// <typeparam name="T"></typeparam>
public class Ref<T>
#if NET8_0_OR_GREATER
where T : notnull
#else
where T : class
#endif
{
    private readonly IRefValueFactory<T>? factory;
    private bool initialized;
    private T? value;

    /// <summary>
    /// Creates a ref containing a static value.
    /// </summary>
    /// <param name="value">The value to wrap in the ref.</param>
    public Ref(T value)
    {
        this.factory = null;
        this.initialized = true;
        this.value = value;
    }

    // ReSharper disable once UnusedMember.Local
    // used through reflection
    private Ref(IRefValueFactory<T> factory)
    {
        this.factory = factory;
        this.initialized = false;
        this.value = default!;
    }

    /// <summary>
    /// Gets the referenced value, fetching it if necessary.
    /// </summary>
    public T Value
    {
        get
        {
            if (!this.initialized)
            {
                this.value = this.factory != null ? this.factory.MakeValue() : default;
                this.initialized = true;
            }
            return this.value!;
        }
    }
}

internal class RefActivator
{
    private readonly ConstructorInfo refCtor;
    private readonly ConstructorInfo factoryCtor;

    private const BindingFlags CONSTRUCTOR_BINDING_FLAGS
        = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    public RefActivator(Type refType, Type factoryType)
    {
#if NET8_0_OR_GREATER
        ConstructorInfo? refCtorInfo = refType.GetConstructor(CONSTRUCTOR_BINDING_FLAGS, new[] { factoryType });
        ConstructorInfo? factoryCtorInfo = factoryType.GetConstructor(
            CONSTRUCTOR_BINDING_FLAGS,
            new[] { typeof(IMapper), typeof(Predicate), typeof(Schema) }
        );
#else
        ConstructorInfo? refCtorInfo = refType.GetConstructor(
            CONSTRUCTOR_BINDING_FLAGS,
            null,
            new[] { factoryType },
            null
        );
        ConstructorInfo? factoryCtorInfo = factoryType.GetConstructor(
            CONSTRUCTOR_BINDING_FLAGS,
            null,
            new[] { typeof(IMapper), typeof(Predicate), typeof(Schema) },
            null
        );
#endif
        this.refCtor = refCtorInfo ?? throw new NullReferenceException($"no ctor found to activate {refType.FullName}");
        this.factoryCtor = factoryCtorInfo
                           ?? throw new NullReferenceException($"no ctor found to activate {factoryType.FullName}");
    }

    public object Activate(IMapper mapper, object? referencedValue, Schema schema)
    {
        return this.refCtor.Invoke(new[] { this.factoryCtor.Invoke(new[] { mapper, referencedValue, schema }) });
    }
}

}
