using System;
using KiwiQuery.Mapped.Mappers;
using KiwiQuery.Mapped.Mappers.Fields;

namespace KiwiQuery.Mapped
{
    /// <summary>
    /// Describes how a field should be mapped.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnAttribute: Attribute, IColumnInfos
    {
        /// <summary>
        /// Maps this field to a column with a specific name.
        /// </summary>
        /// <param name="name">The name of the column </param>
        public ColumnAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Maps this field to a column with the same name.
        /// </summary>
        public ColumnAttribute()
        {
            this.Name = null;
        }

        internal string? Name { get; }

        /// <summary>
        /// If set to <see langword="false"/>, this field will never be included in INSERT queries to let the database
        /// provide an auto-incremented ID or a default value.
        /// </summary>
        public bool Inserted { get; set; } = true;

        /// <summary>
        /// Provides a format string to map this field.
        /// </summary>
        public string? Format { get; set; } = null;

        /// <summary>
        /// Provides a column that stores the size of this field, commonly used when dealing with byte arrays.
        /// </summary>
        public string? Size { get; set; } = null;
    }
}