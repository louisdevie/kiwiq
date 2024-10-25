using System.Text.RegularExpressions;
using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mapped.Model;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped;

public class Select
{
    [Fact]
    public void SelectExplicit()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["FRUIT_ID", "NAME", "COLOR"],
            [
                [1, "Lemon", "Yellow"], [2, "Strawberry", "Red"], [3, "Apricot", "Orange"],
            ]
        );

        List<Fruit.Explicit> explicitFruits = db.Select<Fruit.Explicit>().FetchList();

        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(query, @"select (.+) -> (.+) , (.+) -> (.+) , (.+) -> (.+) from \$FRUIT as (.+)");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 1, 3, 5, 7);
        AssertThat.GroupsEqualUnordered(["$FRUIT_ID", "$NAME", "$COLOR"], match, 2, 4, 6);

        Assert.Equal(
            explicitFruits,
            new Fruit.Explicit[]
            {
                new(1, "Lemon", "Yellow"), new(2, "Strawberry", "Red"), new(3, "Apricot", "Orange"),
            }
        );
    }

    [Fact]
    public void SelectImplicit()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["id", "name", "color"],
            [
                [1, "Lemon", "Yellow"], [2, "Strawberry", "Red"], [3, "Apricot", "Orange"],
            ]
        );

        List<Fruit.Implicit> explicitFruits = db.Select<Fruit.Implicit>().FetchList();

        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(query, @"select (.+) -> (.+) , (.+) -> (.+) , (.+) -> (.+) from \$Implicit as (.+)");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 1, 3, 5, 7);
        AssertThat.GroupsEqualUnordered(["$id", "$name", "$color"], match, 2, 4, 6);

        Assert.Equal(
            explicitFruits,
            new Fruit.Implicit[]
            {
                new(1, "Lemon", "Yellow"), new(2, "Strawberry", "Red"), new(3, "Apricot", "Orange"),
            }
        );
    }

    [Fact]
    public void SelectWhere()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(["FRUIT_ID", "NAME", "COLOR"], [[3, "Apricot", "Orange"]]);

        db.Select<Fruit.Explicit>().Where(fruit => fruit.Attribute("name") == "Apricot").FetchList();

        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(query, @"select (.+) from \$FRUIT as (.+) where (.+) -> \$NAME == @p1");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 2, 3);
    }
    
    [Fact]
    public void SelectNoEmptyConstructor()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["FRUIT_ID", "NAME", "COLOR"],
            [
                [1, "Lemon", "Yellow"], [2, "Strawberry", "Red"], [3, "Apricot", "Orange"],
            ]
        );

        List<Fruit.NoEmptyConstructor> fruits = db.Select<Fruit.NoEmptyConstructor>().FetchList();
        
        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(query, @"select (.+) -> (.+) , (.+) -> (.+) , (.+) -> (.+) from \$NoEmptyConstructor as (.+)");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 1, 3, 5, 7);
        AssertThat.GroupsEqualUnordered(["$id", "$name", "$color"], match, 2, 4, 6);

        Assert.Equal(
            fruits,
            new Fruit.NoEmptyConstructor[]
            {
                new(1, "Lemon", "Yellow"), new(2, "Strawberry", "Red"), new(3, "Apricot", "Orange"),
            }
        );
    }
    
    [Fact]
    public void SelectSingleConstructor()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["FRUIT_ID", "NAME", "COLOR"],
            [
                [1, "Lemon", "Yellow"], [2, "Strawberry", "Red"], [3, "Apricot", "Orange"],
            ]
        );

        List<Fruit.Explicit> explicitFruits = db.Select<Fruit.Explicit>().FetchList();

        string query = connection.GetSingleSelectCommand();
        Match match = Regex.Match(query, @"select (.+) -> (.+) , (.+) -> (.+) , (.+) -> (.+) from \$FRUIT as (.+)");
        Assert.True(match.Success, $"Actual: {query}");
        AssertThat.GroupsAreTheSame(match, 1, 3, 5, 7);
        AssertThat.GroupsEqualUnordered(["$FRUIT_ID", "$NAME", "$COLOR"], match, 2, 4, 6);

        Assert.Equal(
            explicitFruits,
            new Fruit.Explicit[]
            {
                new(1, "Lemon", "Yellow"), new(2, "Strawberry", "Red"), new(3, "Apricot", "Orange"),
            }
        );
    }
}