namespace KiwiQuery.Sql
{
    /// <summary>
    /// Describes how something can be referred to at this point in the query.
    /// </summary>
    public enum NameContext
    {
        /// <summary>
        /// The original name of the object must be used.
        /// </summary>
        Canonical,

        /// <summary>
        /// The object is being declared.
        /// </summary>
        Declaration,

        /// <summary>
        /// The aliases of the object can be used.
        /// </summary>
        Aliased
    }
}