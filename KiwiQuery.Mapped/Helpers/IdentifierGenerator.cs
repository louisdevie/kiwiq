using System;

namespace KiwiQuery.Mapped.Helpers
{
    internal class IdentifierGenerator
    {
        private static readonly Random Rng = new Random();
        private const int USHORT_SIZE = UInt16.MaxValue + 1;
        
        private ushort uniqueId;

        public IdentifierGenerator()
        {
            this.uniqueId = (ushort)(Rng.Next() % USHORT_SIZE);
        }

        private string RandomHexCode()
        {
            this.uniqueId++;
            return this.uniqueId.ToString("X4");
        }

        public string GetTableAlias()
        {
            return "kiwiT" + this.RandomHexCode();
        }
    }
}