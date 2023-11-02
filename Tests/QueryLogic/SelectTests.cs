using KiwiQuery;
using KiwiQuery.Sql;
using Tests.Mocking;
using Xunit;

namespace Tests.QueryLogic
{
    public class SelectTests
    {
        private static void CheckSelectQueryExecution(string expected, MockDbConnection connection)
        {
            Assert.Equal(1, connection.ExecutedCommandCount);

            Assert.Equal(expected, connection.LastExecutedCommand);
            Assert.Equal(ExecutionMethod.Reader, connection.LastExecutionMethod);

            connection.ClearExecutionHistory();
        }

        [Fact]
        public void SimpleSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockMode);

            db.SelectAll().From("table1").Fetch();
            CheckSelectQueryExecution("select #all from $table1", connection);

            db.Select("col1", "col2", "col3").From("table1").Fetch();
            CheckSelectQueryExecution("select $col1 , $col2 , $col3 from $table1", connection);

            db.Select(db.Table("table1").Column("col1")).From("table1").Fetch();
            CheckSelectQueryExecution("select $table1 -> $col1 from $table1", connection);

            db.Select(db.Table("table1").Column("col1").As("alias")).From("table1").Fetch();
            CheckSelectQueryExecution("select $table1 -> $col1 as $alias from $table1", connection);

            db.Select(db.Column("col1").As("alias"))
              .And("col2", "col3")
              .From("table1")
              .Fetch();
            CheckSelectQueryExecution("select $col1 as $alias , $col2 , $col3 from $table1", connection);

            db.Select("col1")
              .And(db.Column("col2") * 2)
              .From("table1")
              .Fetch();
            CheckSelectQueryExecution("select $col1 , $col2 * 2 from $table1", connection);
        }
    }
}
