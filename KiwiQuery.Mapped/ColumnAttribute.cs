using System;

namespace KiwiQuery.Mapped
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnAttribute: Attribute
    {
        public ColumnAttribute(string name)
        {
            
        }

        public bool AutoIncremented
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Default
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}