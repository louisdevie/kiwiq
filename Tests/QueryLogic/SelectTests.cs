using KiwiQuery;
using KiwiQuery.Expressions;
using Tests.Mocking;
using Xunit;

namespace Tests.QueryLogic
{
    public class SelectTests
    {
        private static void CheckSelectQueryExecution(string expected, MockDbConnection connection)
        {
            Assert.Equal(1, connection.ExecutedCommandCount);

            Assert.Equal(expected, connection.LastExecutedCommand.CommandText);
            Assert.Equal(ExecutionMethod.Reader, connection.LastExecutionMethod);

            connection.ClearExecutionHistory();
        }

        [Fact]
        public void SimpleSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

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
            Assert.Equal(
                new MockDbParameter[1] {
                    new MockDbParameter { ParameterName = "@p1", Value = 2 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            CheckSelectQueryExecution("select $col1 , ( $col2 ) * ( @p1 ) from $table1", connection);
        }

        [Fact]
        public void JoinedSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            Table table1 = db.Table("table1");
            Table table2 = db.Table("table2");
            Table table3 = db.Table("table3");

            db.SelectAll().From("table1").Join("table2", "fk", "ref").Fetch();
            CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").Join(table2, "fk", "ref").Fetch();
            CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").Join(table2, db.Column("fk"), db.Column("ref")).Fetch();
            CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").Join(table2.Column("ref"), db.Column("fk")).Fetch();
            CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $table2 -> $ref == $fk", connection);

            db.SelectAll()
              .From(table1)
              .Join(table2.Column("id2"), table1.Column("fk2"))
              .Join(table3.Column("id3"), table1.Column("fk3"))
              .Fetch();
            CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $table2 -> $id2 == $table1 -> $fk2 inner join $table3 on $table3 -> $id3 == $table1 -> $fk3", connection);
        }

        [Fact]
        public void LeftJoinedSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            Table table1 = db.Table("table1");
            Table table2 = db.Table("table2");
            Table table3 = db.Table("table3");

            db.SelectAll().From("table1").LeftJoin("table2", "fk", "ref").Fetch();
            CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").LeftJoin(table2, "fk", "ref").Fetch();
            CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").LeftJoin(table2, db.Column("fk"), db.Column("ref")).Fetch();
            CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref", connection);

            db.SelectAll().From("table1").LeftJoin(table2.Column("ref"), db.Column("fk")).Fetch();
            CheckSelectQueryExecution("select #all from $table1 left join $table2 on $table2 -> $ref == $fk", connection);

            db.SelectAll()
              .From(table1)
              .LeftJoin(table2.Column("id2"), table1.Column("fk2"))
              .LeftJoin(table3.Column("id3"), table1.Column("fk3"))
              .Fetch();
            CheckSelectQueryExecution("select #all from $table1 left join $table2 on $table2 -> $id2 == $table1 -> $fk2 left join $table3 on $table3 -> $id3 == $table1 -> $fk3", connection);
        }

        [Fact]
        public void LimitedSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            db.SelectAll().From("table1").Limit(8).Fetch();
            Assert.Equal(
                new MockDbParameter[2] {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 0 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2", connection);

            db.SelectAll().From("table1").Limit(8).Offset(14).Fetch();
            Assert.Equal(
                new MockDbParameter[2] {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 14 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2", connection);

            db.SelectAll().From("table1").Limit(8, 14).Fetch();
            Assert.Equal(
                new MockDbParameter[2] {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 14 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2", connection);
        }
    }
}
