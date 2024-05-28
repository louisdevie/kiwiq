using System;

namespace KiwiQuery.Mapped
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute: Attribute
    {
        private string name;
        
        public TableAttribute(string name)
        {
            this.name = name;
        }

        public string Name => this.name;
    }
}