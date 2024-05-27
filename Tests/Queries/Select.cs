using KiwiQuery.Tests.Mocking;

namespace KiwiQuery.Tests.Queries
{
    public class Select
    {
        [Fact]
        public void SimpleSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            db.SelectAll().From("table1").Fetch();
            connection.CheckSelectQueryExecution("select #all from $table1");

            db.Select("col1", "col2", "col3").From("table1").Fetch();
            connection.CheckSelectQueryExecution("select $col1 , $col2 , $col3 from $table1");

            db.Select(db.Table("table1").Column("col1")).From("table1").Fetch();
            connection.CheckSelectQueryExecution("select $table1 -> $col1 from $table1");

            db.Select(db.Table("table1").Column("col1").As("alias")).From("table1").Fetch();
            connection.CheckSelectQueryExecution("select $table1 -> $col1 as $alias from $table1");

            db.Select(db.Column("col1").As("alias"))
                .And("col2", "col3")
                .From("table1")
                .Fetch();
            connection.CheckSelectQueryExecution("select $col1 as $alias , $col2 , $col3 from $table1");

            db.Select("col1")
                .And(db.Column("col2") * 2)
                .From("table1")
                .Fetch();
            Assert.Equal(
                new MockDbParameter[]
                {
                    new MockDbParameter { ParameterName = "@p1", Value = 2 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            connection.CheckSelectQueryExecution("select $col1 , ( $col2 ) * ( @p1 ) from $table1");
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
            connection.CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref");

            db.SelectAll().From(table1).Join(table2, "fk", "ref").Fetch();
            connection.CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref");

            db.SelectAll().From(table1).Join(table2, db.Column("fk"), db.Column("ref")).Fetch();
            connection.CheckSelectQueryExecution("select #all from $table1 inner join $table2 on $fk == $ref");

            db.SelectAll().From(table1).Join(table2.Column("ref"), db.Column("fk")).Fetch();
            connection.CheckSelectQueryExecution(
                "select #all from $table1 inner join $table2 on $table2 -> $ref == $fk");

            db.SelectAll()
                .From(table1)
                .Join(table2.Column("id2"), table1.Column("fk2"))
                .Join(table3.Column("id3"), table1.Column("fk3"))
                .Fetch();
            connection.CheckSelectQueryExecution(
                "select #all from $table1 inner join $table2 on $table2 -> $id2 == $table1 -> $fk2 inner join $table3 on $table3 -> $id3 == $table1 -> $fk3");
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
            connection.CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref");

            db.SelectAll().From("table1").LeftJoin(table2, "fk", "ref").Fetch();
            connection.CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref");

            db.SelectAll().From("table1").LeftJoin(table2, db.Column("fk"), db.Column("ref")).Fetch();
            connection.CheckSelectQueryExecution("select #all from $table1 left join $table2 on $fk == $ref");

            db.SelectAll().From("table1").LeftJoin(table2.Column("ref"), db.Column("fk")).Fetch();
            connection.CheckSelectQueryExecution(
                "select #all from $table1 left join $table2 on $table2 -> $ref == $fk");

            db.SelectAll()
                .From(table1)
                .LeftJoin(table2.Column("id2"), table1.Column("fk2"))
                .LeftJoin(table3.Column("id3"), table1.Column("fk3"))
                .Fetch();
            connection.CheckSelectQueryExecution(
                "select #all from $table1 left join $table2 on $table2 -> $id2 == $table1 -> $fk2 left join $table3 on $table3 -> $id3 == $table1 -> $fk3");
        }

        [Fact]
        public void LimitedSelect()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            db.SelectAll().From("table1").Limit(8).Fetch();
            Assert.Equal(
                new MockDbParameter[]
                {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 0 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            connection.CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2");

            db.SelectAll().From("table1").Limit(8).Offset(14).Fetch();
            Assert.Equal(
                new MockDbParameter[]
                {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 14 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            connection.CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2");

            db.SelectAll().From("table1").Limit(8, 14).Fetch();
            Assert.Equal(
                new MockDbParameter[]
                {
                    new MockDbParameter { ParameterName = "@p1", Value = 8 },
                    new MockDbParameter { ParameterName = "@p2", Value = 14 }
                },
                connection.LastExecutedCommand.MockParameters
            );
            connection.CheckSelectQueryExecution("select #all from $table1 limit @p1 offset @p2");
        }


        [Fact]
        public void SelectDistinct()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            db.Select("col1", "col2", "col3").Distinct().From("table1").Fetch();
            connection.CheckSelectQueryExecution("select distinct $col1 , $col2 , $col3 from $table1");
        }


        [Fact]
        public void SelectAliasedTables()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            // v0.4

            Table table1 = db.Table("table1").As("A");
            Table table1Alias = db.Table("A");
            Table table2 = db.Table("table2").As("B");
            Table table2Alias = db.Table("B");

            db.Select(table1Alias.Column("col1").As("col1A"), table2Alias.Column("col1").As("col1B")).From(table1)
                .Join(table2, table2Alias.Column("ref"), table1Alias.Column("fk")).Fetch();
            connection.CheckSelectQueryExecution(
                "select $A -> $col1 as $col1A , $B -> $col1 as $col1B from $table1 as $A inner join $table2 as $B on $B -> $ref == $A -> $fk");

            // v0.5 and higher

            Table table3 = db.Table("table3").As("A");
            Table table4 = db.Table("table4").As("B");

            db.Select(table3.Column("col1").As("col1A"), table4.Column("col1").As("col1B")).From(table3)
                .Join(table4.Column("ref"), table3.Column("fk")).Fetch();
            connection.CheckSelectQueryExecution(
                "select $A -> $col1 as $col1A , $B -> $col1 as $col1B from $table3 as $A inner join $table4 as $B on $B -> $ref == $A -> $fk");
        }
    }
}