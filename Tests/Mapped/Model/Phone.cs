using KiwiQuery.Mapped;

namespace KiwiQuery.Tests.Mapped.Model;

internal static class Phone
{
    [Table("Phone")]
    public class NoInverse : IEquatable<NoInverse>
    {
        [Key]
        private readonly int id;
        private readonly string number;

        // db constructor
        private NoInverse() { }

        public NoInverse(int id, string number)
        {
            this.id = id;
            this.number = number;
        }

        public bool Equals(NoInverse? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.id == other.id && this.number == other.number;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((NoInverse)obj);
        }
    }
}
