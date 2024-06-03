using System.Data.Common;
using System.Data.SQLite;
using KiwiQuery.Mapped;
using KiwiQuery.Tests.Mapped;

namespace KiwiQuery.Tests.Live
{
    public class Select
    {
        private static void SetUpDatabase(DbConnection connection)
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE TABLE Fruit ( id INTEGER PRIMARY KEY, name TEXT )";
            cmd.ExecuteNonQuery();
            
            cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Fruit VALUES ( 1, 'Apple' ), ( 2, 'Orange' ), ( 3, 'Kiwi' ), ( 4, 'Apricot' ), ( 5, 'Cherry' )";
            cmd.ExecuteNonQuery();
        }

        [Fact]
        public void SelectAll()
        {
            DbConnection conn = new SQLiteConnection("Data Source=:memory:");
            SetUpDatabase(conn);
            Schema db = new Schema(conn);

            using var results = db.SelectAll().From("Fruit").Fetch();
            int id = results.GetOrdinal("id");
            int name = results.GetOrdinal("name");
            
            Assert.True(results.Read());
            Assert.Equal(1, results.GetInt32(id));
            Assert.Equal("Apple", results.GetString(name));
            Assert.True(results.Read());
            Assert.Equal(2, results.GetInt32(id));
            Assert.Equal("Orange", results.GetString(name));
            Assert.True(results.Read());
            Assert.Equal(3, results.GetInt32(id));
            Assert.Equal("Kiwi", results.GetString(name));
            Assert.True(results.Read());
            Assert.Equal(4, results.GetInt32(id));
            Assert.Equal("Apricot", results.GetString(name));
            Assert.True(results.Read());
            Assert.Equal(5, results.GetInt32(id));
            Assert.Equal("Cherry", results.GetString(name));
            Assert.False(results.Read());
        }

        [Fact]
        public void SelectAllMapped()
        {
            DbConnection conn = new SQLiteConnection("Data Source=:memory:");
            SetUpDatabase(conn);
            Schema db = new Schema(conn);

            List<Fruit> fruits = db.Select<Fruit>().FetchList();
            
            Assert.Equal(5, fruits.Count);
            Assert.Equal(new Fruit(1, "Apple"), fruits[0]);
            Assert.Equal(new Fruit(2, "Orange"), fruits[1]);
            Assert.Equal(new Fruit(3, "Kiwi"), fruits[2]);
            Assert.Equal(new Fruit(4, "Apricot"), fruits[3]);
            Assert.Equal(new Fruit(5, "Cherry"), fruits[4]);
        }

        [Fact]
        public void SelectOne()
        {
            DbConnection conn = new SQLiteConnection("Data Source=:memory:");
            SetUpDatabase(conn);
            Schema db = new Schema(conn);

            using (var results = db.Select("name").From("Fruit").Where(db.Column("id") == 3).Fetch())
            {
                Assert.True(results.Read());
                Assert.Equal("Kiwi", results.GetString(0));
                Assert.False(results.Read());
            }

            using (var results = db.SelectAll().From("Fruit").Where(db.Column("name") == "Cherry").Fetch()){
                int id = results.GetOrdinal("id");
                int name = results.GetOrdinal("name");

                Assert.True(results.Read());
                Assert.Equal(5, results.GetInt32(id));
                Assert.Equal("Cherry", results.GetString(name));
                Assert.False(results.Read());
            }
        }

        [Fact]
        public void SelectOneMapped()
        {
            DbConnection conn = new SQLiteConnection("Data Source=:memory:");
            SetUpDatabase(conn);
            Schema db = new Schema(conn);

            List<Fruit> fruits = db.Select<Fruit>().FetchList();
            
            Assert.Equal(5, fruits.Count);
            Assert.Equal(new Fruit(1, "Apple"), fruits[0]);
            Assert.Equal(new Fruit(2, "Orange"), fruits[1]);
            Assert.Equal(new Fruit(3, "Kiwi"), fruits[2]);
            Assert.Equal(new Fruit(4, "Apricot"), fruits[3]);
            Assert.Equal(new Fruit(5, "Cherry"), fruits[4]);
        }
    }
}