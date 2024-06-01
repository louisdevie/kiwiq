using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped;

public class Explicit
{
    [Table("FRUIT")]
    public class Fruit : IEquatable<Fruit>
    {
        [Column("FRUIT_ID", NotInserted = true)]
        private int id;

        [Column("NAME")]
        private string name;

        [Column("COLOR")]
        private string? color;

        [NotStored]
        private List<string> somethingElse;

        // db constructor
        private Fruit() { }

        public Fruit(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.somethingElse = new List<string>();
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

            if (obj.GetType() != typeof(Fruit))
            {
                return false;
            }

            return Equals((Fruit)obj);
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

    [Fact]
    public void Select()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            new string[] { "FRUIT_ID", "NAME", "COLOR" },
            new Row[]
            {
                new() { 1, "Lemon", "Yellow" },
                new() { 2, "Strawberry", "Red" },
                new() { 3, "Apricot", "Orange" },
            }
        );

        List<Fruit> fruits = db.Select<Fruit>().FetchList();

        string query = connection.GetSingleSelectQuery();
        Match match = Regex.Match(query, @"select (.+) -> (.+) , (.+) -> (.+) , (.+) -> (.+) from \$FRUIT as (.+)");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreEqual(match, 1, 3, 5, 7);
        AssertThat.GroupsEqualUnordered(new[] { "$FRUIT_ID", "$NAME", "$COLOR" }, match, 2, 4, 6);

        Assert.Equal(
            fruits,
            new Fruit[]
            {
                new(1, "Lemon", "Yellow"),
                new(2, "Strawberry", "Red"),
                new(3, "Apricot", "Orange"),
            }
        );
    }

    [Fact]
    public void SelectWhere()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            new string[] { "FRUIT_ID", "NAME", "COLOR" },
            new Row[]
            {
                new() { 3, "Apricot", "Orange" },
            }
        );

        db.Select<Fruit>().Where(db.Column("NAME") == "Apricot").FetchList();

        string query = connection.GetSingleSelectQuery();
        Match match = Regex.Match(query, @"select (.+) from \$FRUIT as (.+) where \$NAME == @p1");
        Assert.True(match.Success, $"Actual: {query}");
    }
}