using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Queries;

public class Insert
{
    [Fact]
    public void InsertSingleRow()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        int id = db.InsertInto("table1").Value("col1", 1).Value("col2", 2).Value("col3", 3).Apply();

        Assert.Equal(InsertCommand.NO_AUTO_ID, id);

        connection.CheckNonQueryExecution(
            1,
            "insert-into $table1 ( $col1 , $col2 , $col3 ) values ( @p1 , @p2 , @p3 )",
            new object[] { 1, 2, 3 }
        );
        connection.CheckScalarExecution(2, "select #last-insert-id");
        connection.ExpectNoMoreThan(2);
    }
    
    [Fact]
    public void InsertSingleRowWithId()
    {
        var connection = new MockDbConnection();
        Schema db = new(connection, MockQueryBuilder.MockDialect);

        connection.MockScalarResult(123);

        int id = db.InsertInto("table1").Value("col1", 1).Value("col2", 2).Value("col3", 3).Apply();

        Assert.Equal(123, id);

        connection.CheckNonQueryExecution(
            1,
            "insert-into $table1 ( $col1 , $col2 , $col3 ) values ( @p1 , @p2 , @p3 )",
            new object[] { 1, 2, 3 }
        );
        connection.CheckScalarExecution(2, "select #last-insert-id");
        connection.ExpectNoMoreThan(2);
    }
}
