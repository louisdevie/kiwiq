using System.Text.RegularExpressions;
using KiwiQuery.Mapped;
using KiwiQuery.Mapped.Relationships;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped.Relationships;

public class OneToOne
{
    [Fact]
    public void Select()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(["columnFromA", "columnFromB"], []);

        List<User> fruits = db.Select<User>().FetchList();

        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(
            query,
            @"select (.+) , (.+), (.+) from \$User as (.+) inner join \$B as (.+) on (.+) -> \$bId == \$aId"
        );
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 4, 5);
        AssertThat.GroupsEqualUnordered(["$columnFromA", "$columnFromB"], match, 1, 2);

        Assert.Equal(
            fruits,
            new User[] { new("Apple", new Phone(3, "Pepins")), new("Peach", new Phone(2, "Large Noyau")) }
        );
    }

    private class User : IEquatable<User>
    {
        private readonly string name;
        [HasOne]
        private readonly Phone phone;

        // db constructor
        private User() { }

        public User(string name, Phone phone)
        {
            this.name = name;
            this.phone = phone;
        }

        public bool Equals(User? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.name == other.name && Equals(this.phone, other.phone);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((User)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.name, this.phone);
        }

        public static bool operator ==(User? left, User? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User? left, User? right)
        {
            return !Equals(left, right);
        }
    }

    private class Phone : IEquatable<Phone>
    {
        private readonly int id;
        private readonly string number;

        // db constructor
        private Phone() { }

        public Phone(int id, string number)
        {
            this.id = id;
            this.number = number;
        }

        public bool Equals(Phone? other)
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
            return this.Equals((Phone)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.id, this.number);
        }

        public static bool operator ==(Phone? left, Phone? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Phone? left, Phone? right)
        {
            return !Equals(left, right);
        }
    }
}
