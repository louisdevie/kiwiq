using System.Text.RegularExpressions;
using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped.Joins;

public class MultipleTablesSingleEntity
{
    [Fact]
    public void Select()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["columnFromA", "columnFromB"],
            [[1, "Yellow"], [2, "Purple"], [2, "Green"], [3, "Yellow"]]
        );

        List<AB> results = db.Select<AB>().FetchList();

        string query = connection.GetSingleSelectQuery();
        Match match = Regex.Match(
            query,
            @"select (.+) , (.+) from \$A as (.+) inner join \$B as (.+) on (.+) -> \$bId == \$aId"
        );
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 4, 5);
        AssertThat.GroupsEqualUnordered(["$columnFromA", "$columnFromB"], match, 1, 2);

        Assert.Equal(results, new AB[] { new(1, "Yellow"), new(2, "Purple"), new(2, "Green"), new(3, "Yellow") });
    }

    [Table("A")]
    [Join("B", "bId", "aId")]
    private class AB : IEquatable<AB>
    {
        private readonly int columnFromA;
        private readonly string columnFromB;

        // db constructor
        private AB() { }

        public AB(int columnFromA, string columnFromB)
        {
            this.columnFromA = columnFromA;
            this.columnFromB = columnFromB;
        }

        public bool Equals(AB? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.columnFromA == other.columnFromA && this.columnFromB == other.columnFromB;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((AB)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.columnFromA, this.columnFromB);
        }

        public static bool operator ==(AB? left, AB? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AB? left, AB? right)
        {
            return !Equals(left, right);
        }
    }
}
