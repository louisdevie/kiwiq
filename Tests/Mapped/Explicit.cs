using KiwiQuery;
using KiwiQuery.Mapped;
using Tests.Queries;

namespace Tests.Mapped
{
    public class Explicit
    {
        [Table("FRUIT")]
        public class Fruit
        {
            [Column("FRUIT_ID", AutoIncremented = true)]
            private int id;
            [Column("NAME")]
            private string name;
            [Column("COLOR", Default = true)]
            private string? color;
            [NotStored]
            private List<string> somethingElse; 

            [DbConstructor]
            public Fruit(int id, string name, string color)
            {
                this.id = id;
                this.name = name;
                this.color = color;
                this.somethingElse = new List<string>();
            }
        }

        [Fact]
        public void Select()
        {
            var connection = new MockDbConnection();
            Schema db = new(connection, MockQueryBuilder.MockDialect);

            db.Select<Fruit>().FetchList();
        }
    }
} 