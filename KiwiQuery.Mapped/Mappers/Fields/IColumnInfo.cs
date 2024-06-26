namespace KiwiQuery.Mapped.Mappers.Fields
{
    public interface IColumnInfo
    {
        /// <summary>
        /// A format string for this field.
        /// </summary>
        string? Format { get; }

        /// <summary>
        /// A column that stores the size of this field.
        /// </summary>
        string? Size { get; }
    }
}