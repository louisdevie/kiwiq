using KiwiQuery.Mapped;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Tests.Mapped.Model;

internal static class User
{
    public static class NoInverse
    {
        public class ImplicitRef : IEquatable<ImplicitRef>
        {
            private readonly int id;
            private readonly string name;
            [HasOne]
            private readonly Phone.NoInverse phone;

            [PersistenceConstructor]
            private ImplicitRef() { }

            public ImplicitRef(int id, string name, Phone.NoInverse phone)
            {
                this.id = id;
                this.name = name;
                this.phone = phone;
            }

            public bool Equals(ImplicitRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name && Equals(this.phone, other.phone);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ImplicitRef)obj);
            }
        }

        [Table("User")]
        public class ExplicitRef : IEquatable<ExplicitRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string name;
            [HasOne("userId")]
            private readonly Phone.NoInverse phone;

            [PersistenceConstructor]
            private ExplicitRef() { }

            public ExplicitRef(int id, string name, Phone.NoInverse phone)
            {
                this.id = id;
                this.name = name;
                this.phone = phone;
            }

            public bool Equals(ExplicitRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name && Equals(this.phone, other.phone);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ExplicitRef)obj);
            }
        }

        [Table("User")]
        public class LazyRef : IEquatable<LazyRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string name;
            [HasOne("userId")]
            private readonly Ref<Phone.NoInverse> phone;

            [PersistenceConstructor]
            private LazyRef() { }

            public LazyRef(int id, string name)
            {
                this.id = id;
                this.name = name;
                this.phone = new Ref<Phone.NoInverse>(null);
            }

            public Ref<Phone.NoInverse> Phone => this.phone;

            public bool Equals(LazyRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((LazyRef)obj);
            }
        }
    }
    
    public static class WithInverse
    {
        public class ImplicitRef : IEquatable<ImplicitRef>
        {
            private readonly int id;
            private readonly string name;
            [HasOne]
            private readonly Phone.WithInverse.ImplicitRef? phone;

            [PersistenceConstructor]
            private ImplicitRef() { }

            public ImplicitRef(int id, string name)
            {
                this.id = id;
                this.name = name;
                this.phone = null;
            }

            public bool Equals(ImplicitRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name && Equals(this.phone, other.phone);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ImplicitRef)obj);
            }
        }

        [Table("User")]
        public class ExplicitRef : IEquatable<ExplicitRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string name;
            [HasOne("userId")]
            private readonly Phone.WithInverse.ExplicitRef phone;

            [PersistenceConstructor]
            private ExplicitRef() { }

            public ExplicitRef(int id, string name, Phone.WithInverse.ExplicitRef phone)
            {
                this.id = id;
                this.name = name;
                phone.User = this;
                this.phone = phone;
            }

            public bool Equals(ExplicitRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name && Equals(this.phone, other.phone);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ExplicitRef)obj);
            }
        }

        [Table("User")]
        public class LazyRef : IEquatable<LazyRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string name;
            [HasOne("userId")]
            private readonly Ref<Phone.WithInverse.LazyRef?> phone;

            [PersistenceConstructor]
            private LazyRef() { }

            public LazyRef(int id, string name)
            {
                this.id = id;
                this.name = name;
                this.phone = new Ref<Phone.WithInverse.LazyRef?>(null);
            }

            public Ref<Phone.WithInverse.LazyRef?> Phone => this.phone;

            public bool Equals(LazyRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id && this.name == other.name;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((LazyRef)obj);
            }
        }
    }
}
