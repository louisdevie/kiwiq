using System;

namespace KiwiQuery.Mapped
{

/// <summary>
/// Indicate that this column is (part of) a non-null, unique index for its table.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class KeyAttribute : Attribute { }

}
