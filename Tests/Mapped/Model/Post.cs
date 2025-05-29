using System.Collections.Immutable;
using KiwiQuery.Mapped;
using KiwiQuery.Mapped.Relationships;

namespace KiwiQuery.Tests.Mapped.Model;

internal static class Post
{
    public static class NoInverse
    {
        [Table("Post")]
        public class ExplicitRef : IEquatable<ExplicitRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string title;
            private readonly string content;
            [HasMany("postId")]
            private readonly ISet<Comment.NoInverse> comments;

            public ExplicitRef(int id, string title, string content)
            {
                this.id = id;
                this.title = title;
                this.content = content;
                this.comments = new HashSet<Comment.NoInverse>();
            }

            public bool Equals(ExplicitRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id
                       && this.title == other.title
                       && this.content == other.content
                       && Equals(this.content, other.content);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ExplicitRef)obj);
            }
        }
        
        [Table("Post")]
        public class LazyRef : IEquatable<LazyRef>
        {
            [PrimaryKey]
            private readonly int id;
            private readonly string title;
            private readonly string content;
            [HasMany("postId")]
            private readonly Ref<List<Comment.NoInverse>> comments;

            public LazyRef(int id, string title, string content)
            {
                this.id = id;
                this.title = title;
                this.content = content;
                this.comments = new Ref<List<Comment.NoInverse>>([]);
            }

            public bool Equals(LazyRef? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return this.id == other.id
                       && this.title == other.title
                       && this.content == other.content
                       && Equals(this.content, other.content);
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
