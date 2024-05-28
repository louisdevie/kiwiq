using System;

namespace KiwiQuery.Mapped
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnAttribute: Attribute
    {
        private readonly string name;

        public ColumnAttribute(string name)
        {
            this.name = name;
        }

        public bool AutoIncremented { get; set; }

        public bool Default { get; set; }

        public string Name => this.name;
    }
}