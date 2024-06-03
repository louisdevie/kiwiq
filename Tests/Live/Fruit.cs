using KiwiQuery.Tests.Mapped;

namespace KiwiQuery.Tests.Live
{
    internal class Fruit : IEquatable<Fruit>
    {
        private readonly int id;
        private readonly string name;

        private Fruit() : this(-1, "") {}
        
        public Fruit(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public bool Equals(Fruit? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id == other.id && this.name == other.name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Fruit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.id * 397) ^ this.name.GetHashCode();
            }
        }
    }
}