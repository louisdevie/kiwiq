using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mapped.Model;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped.Relationships;

public class JustHasMany
{
    [Fact]
    public void SelectLazy()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);
        
        // 1. FETCHING POSTS

        connection.MockResults(["id", "name", "kiwi_r0_lazy"], [[1, "alice", 1], [2, "bob", 2]]);

        var posts = db.Select<Post.NoInverse.LazyRef>().FetchList();

        connection.CheckSelectCommandExecution(
            "select $kiwi -> $id , $kiwi -> $name , $kiwi -> $id as $kiwi_r0_lazy from $User as $kiwi"
        );

        Assert.Equal(new Post.NoInverse.LazyRef[] { new(1, "f", "foo"), new(2, "b", "bar") }, posts);
        
        // 2. FETCHING COMMENTS FOR A POST
        
        // connection.MockResults(["id", "number"], [[8, "305-269-4990"]]);
        //
        // Phone.NoInverse alicesPhone = users[0]..Value;
        //
        // connection.CheckSelectCommandExecution(
        //     "select $kiwi_r0 -> $id , $kiwi_r0 -> $number from $Phone as $kiwi_r0 where $kiwi_r0 -> $userId == @p1",
        //     1
        // );
        //
        // Assert.Equal(new Phone.NoInverse(8, "305-269-4990"), alicesPhone);
        //
        // connection.MockResults(["id", "number"], [[9, "931-442-2530"]]);
        //
        // Phone.NoInverse bobsPhone = users[1].Phone.Value;
        //
        // connection.CheckSelectCommandExecution(
        //     "select $kiwi_r0 -> $id , $kiwi_r0 -> $number from $Phone as $kiwi_r0 where $kiwi_r0 -> $userId == @p1",
        //     2
        // );
        //
        // Assert.Equal(new Phone.NoInverse(9, "931-442-2530"), bobsPhone);
    }
}