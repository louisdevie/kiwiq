using KiwiQuery.Mapped;
namespace KiwiQuery.Tests.Mapped.Model;

internal static class Fruit
{
    public class Implicit : IEquatable<Implicit>
    {
        [Key]
        [Column(Inserted = false)]
        private readonly int id;

        private readonly string name;

        private readonly string? color;

        [Transient]
        private List<string> somethingElse;

        [PersistenceConstructor]
        private Implicit() { }

        public Implicit(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.somethingElse = new List<string>();
        }

        public int Id => this.id;

        public bool Equals(Implicit? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id == other.id && this.name == other.name && this.color == other.color;
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

            if (obj.GetType() != typeof(Implicit))
            {
                return false;
            }

            return this.Equals((Implicit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.id;
                hashCode = (hashCode * 397) ^ this.name.GetHashCode();
                hashCode = (hashCode * 397) ^ this.color.GetHashCode();
                return hashCode;
            }
        }
    }
    
    [Table("FRUIT")]
    internal class Explicit : IEquatable<Explicit>
    {
        [Key]
        [Column("FRUIT_ID", Inserted = false)]
        private readonly int id;
    
        [Column("NAME")]
        private readonly string name;
    
        [Column("COLOR")]
        private readonly string? color;
    
        [Transient]
        private List<string> somethingElse;
    
        [PersistenceConstructor]
        private Explicit() { }
    
        public Explicit(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.somethingElse = new List<string>();
        }
    
        public int Id => this.id;
    
        public bool Equals(Explicit? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
    
            if (ReferenceEquals(this, other))
            {
                return true;
            }
    
            return this.id == other.id && this.name == other.name && this.color == other.color;
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
    
            if (obj.GetType() != typeof(Explicit))
            {
                return false;
            }
    
            return this.Equals((Explicit)obj);
        }
    
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.id;
                hashCode = (hashCode * 397) ^ this.name.GetHashCode();
                hashCode = (hashCode * 397) ^ this.color.GetHashCode();
                return hashCode;
            }
        }
    }
    
    public class NoEmptyConstructor : IEquatable<NoEmptyConstructor>
    {
        [Key]
        [Column(Inserted = false)]
        private readonly int id;

        private readonly string name;

        private readonly string? color;

        [Transient]
        private List<string> somethingElse;

        [PersistenceConstructor]
        public NoEmptyConstructor(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.somethingElse = new List<string>();
        }

        public int Id => this.id;

        public bool Equals(NoEmptyConstructor? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id == other.id && this.name == other.name && this.color == other.color;
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

            if (obj.GetType() != typeof(NoEmptyConstructor))
            {
                return false;
            }

            return this.Equals((NoEmptyConstructor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.id;
                hashCode = (hashCode * 397) ^ this.name.GetHashCode();
                hashCode = (hashCode * 397) ^ this.color.GetHashCode();
                return hashCode;
            }
        }
    }
    
    public class SingleConstructor : IEquatable<SingleConstructor>
    {
        [Key]
        [Column(Inserted = false)]
        private readonly int id;

        private readonly string name;

        private readonly string? color;

        [Transient]
        private List<string> somethingElse;

        public SingleConstructor(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.somethingElse = new List<string>();
        }

        public int Id => this.id;

        public bool Equals(SingleConstructor? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id == other.id && this.name == other.name && this.color == other.color;
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

            if (obj.GetType() != typeof(SingleConstructor))
            {
                return false;
            }

            return this.Equals((SingleConstructor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.id;
                hashCode = (hashCode * 397) ^ this.name.GetHashCode();
                hashCode = (hashCode * 397) ^ this.color.GetHashCode();
                return hashCode;
            }
        }
    }
}