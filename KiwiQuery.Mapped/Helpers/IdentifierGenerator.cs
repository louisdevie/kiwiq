using System;

namespace KiwiQuery.Mapped.Helpers
{
    internal class IdentifierGenerator
    {
        private ushort uniqueId;

        public IdentifierGenerator()
        {
            this.uniqueId = 0;
        }

        private string RandomHexCode()
        {
            this.uniqueId++;
            return this.uniqueId.ToString();
        }

        public string GetTableAlias()
        {
            return "t" + this.RandomHexCode();
        }
    }
}