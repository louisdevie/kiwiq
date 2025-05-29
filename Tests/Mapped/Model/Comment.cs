using KiwiQuery.Mapped;

namespace KiwiQuery.Tests.Mapped.Model;

public class Comment
{
    [Table("Comment")]
    public class NoInverse : IEquatable<NoInverse>
    {
        [PrimaryKey]
        private readonly int id;
        private readonly string text;

        public NoInverse(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public bool Equals(NoInverse? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.id == other.id && this.text == other.text;
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