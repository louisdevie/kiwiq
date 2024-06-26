using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mapped.Model;
using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Mapped;

public class Insert
{
    [Fact]
    public void InsertSingleRowExplicit()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        int id = db.InsertInto<Fruit.Explicit>().Values(new Fruit.Explicit(3, "Apricot", "Orange")).Apply();

        Assert.Equal(InsertQuery.NO_AUTO_ID, id);

        connection.CheckNonQueryExecution(
            1,
            "insert-into $FRUIT ( $NAME , $COLOR ) values ( @p1 , @p2 )",
            ["Apricot", "Orange"]
        );
        connection.CheckScalarExecution(2, "select #last-insert-id");
        connection.ExpectNoMoreThan(2);
    }

    [Fact]
    public void InsertSingleRowImplicit()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        int id = db.InsertInto<Fruit.Implicit>().Values(new Fruit.Implicit(3, "Apricot", "Orange")).Apply();

        Assert.Equal(InsertQuery.NO_AUTO_ID, id);

        connection.CheckNonQueryExecution(
            1,
            "insert-into $Implicit ( $name , $color ) values ( @p1 , @p2 )",
            ["Apricot", "Orange"]
        );
        connection.CheckScalarExecution(2, "select #last-insert-id");
        connection.ExpectNoMoreThan(2);
    }
}
