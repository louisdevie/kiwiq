using System;

namespace KiwiQuery.Mapped
{
    /// <summary>
    /// Specifies the name of the table in the database, if it is different from the class name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute: Attribute
    {
        private string name;
        
        /// <summary>
        /// Specifies the name of the table in the database, if it is different from the class name.
        /// </summary>
        /// <param name="name">The name of the table in the database.</param>
        public TableAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// The name of the table.
        /// </summary>
        public string Name => this.name;
    }
}