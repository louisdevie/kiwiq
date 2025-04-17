using System;

namespace KiwiQuery.Mapped
{
    /// <summary>
    /// Indicate that this column is (part of) a non-null, unique index for its table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// Setting this property to <c>true</c> will exclude this field from INSERT commands to let the database
        /// generate an ID, as if <see cref="ColumnAttribute.Inserted"/> was set to <c>false</c>.
        /// </summary>
        public bool AutoIncrement { get; set; } = false;
    }
}