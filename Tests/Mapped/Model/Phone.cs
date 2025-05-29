using KiwiQuery.Mapped;

namespace KiwiQuery.Tests.Mapped.Model;

internal static class Phone
{
    [Table("Phone")]
    public class NoInverse : IEquatable<NoInverse>
    {
        [PrimaryKey]
        private readonly int id;
        private readonly string number;

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

    public static class WithInverse
    {
        [Table("Phone")]
        public class ImplicitRef : IEquatable<ImplicitRef>
        {
            [PrimaryKey]
            private readonly int id;

            private readonly string number;

            [BelongsTo]
            private readonly User.WithInverse.ImplicitRef? user;

            public ImplicitRef(int id, string number, User.WithInverse.ImplicitRef user)
            {
                this.id = id;
                this.number = number;
                this.user = user;
            }

            public bool Equals(ImplicitRef? other)
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
                return this.Equals((ImplicitRef)obj);
            }
        }
        
        [Table("Phone")]
        public class ExplicitRef : IEquatable<ExplicitRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string number;
            [BelongsTo]
            private User.WithInverse.ExplicitRef? user;

            public ExplicitRef(int id, string number)
            {
                this.id = id;
                this.number = number;
                this.user = null;
            }

            public User.WithInverse.ExplicitRef? User
            {
                get => this.user;
                set => this.user = value;
            }

            public bool Equals(ExplicitRef? other)
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
                return this.Equals((ExplicitRef)obj);
            }
        }
        
        [Table("Phone")]
        public class LazyRef : IEquatable<LazyRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string number;
            [BelongsTo]
            private readonly User.WithInverse.LazyRef user;

            public LazyRef(int id, string number, User.WithInverse.LazyRef user)
            {
                this.id = id;
                this.number = number;
                this.user = user;
            }

            public bool Equals(LazyRef? other)
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
                return this.Equals((LazyRef)obj);
            }
        }
    }
}
