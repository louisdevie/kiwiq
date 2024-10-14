using System;

namespace KiwiQuery.Mapped
{
    /// <summary>
    /// Indicates this field should NOT be persisted in the database.    
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TransientAttribute: Attribute
    {
        
    }
}