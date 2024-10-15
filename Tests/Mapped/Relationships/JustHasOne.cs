using System.Text.RegularExpressions;
using KiwiQuery.Mapped;
using KiwiQuery.Mapped.Exceptions;
using KiwiQuery.Tests.Mapped.Model;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped.Relationships;

public class JustHasOne
{
    [Fact]
    public void Select()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockResults(
            ["id", "name", "id", "number"],
            [[1, "alice", 8, "305-269-4990"], [2, "bob", 9, "931-442-2530"]]
        );

        var users = db.Select<User.NoInverse.ExplicitRef>().FetchList();

        connection.CheckSelectCommandExecution(
            "select $kiwi -> $id , $kiwi -> $name , $kiwi_r0 -> $id , $kiwi_r0 -> $number "
            + "from $User as $kiwi left join $Phone as $kiwi_r0 on $kiwi_r0 -> $userId == $kiwi -> $id"
        );

        Assert.Equal(
            new User.NoInverse.ExplicitRef[]
            {
                new(1, "alice", new Phone.NoInverse(8, "305-269-4990")),
                new(2, "bob", new Phone.NoInverse(9, "931-442-2530"))
            },
            users
        );
    }

    [Fact]
    public void TryToUseImplicitRef()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        Assert.Throws<CouldNotInferException>(() => db.Select<User.NoInverse.ImplicitRef>().FetchList());
    }

    [Fact]
    public void SelectLazy()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);
        
        // 1. FETCHING USERS

        connection.MockResults(["id", "name", "kiwi_r0_lazy"], [[1, "alice", 1], [2, "bob", 2]]);

        var users = db.Select<User.NoInverse.LazyRef>().FetchList();

        connection.CheckSelectCommandExecution(
            "select $kiwi -> $id , $kiwi -> $name , $kiwi -> $id as $kiwi_r0_lazy from $User as $kiwi"
        );

        Assert.Equal(new User.NoInverse.LazyRef[] { new(1, "alice"), new(2, "bob") }, users);
        
        // 2. FETCHING PHONES OF USERS
        
        connection.MockResults(["id", "number"], [[8, "305-269-4990"]]);
        
        Phone.NoInverse alicesPhone = users[0].Phone.Value;

        connection.CheckSelectCommandExecution(
            "select $kiwi_r0 -> $id , $kiwi_r0 -> $number from $Phone as $kiwi_r0 where $kiwi_r0 -> $userId == @p1",
            1
        );

        Assert.Equal(new Phone.NoInverse(8, "305-269-4990"), alicesPhone);
        
        connection.MockResults(["id", "number"], [[9, "931-442-2530"]]);

        Phone.NoInverse bobsPhone = users[1].Phone.Value;

        connection.CheckSelectCommandExecution(
            "select $kiwi_r0 -> $id , $kiwi_r0 -> $number from $Phone as $kiwi_r0 where $kiwi_r0 -> $userId == @p1",
            2
        );

        Assert.Equal(new Phone.NoInverse(9, "931-442-2530"), bobsPhone);
    }
}
