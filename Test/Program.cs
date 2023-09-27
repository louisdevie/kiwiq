using KiwiQuery;
using MySql.Data.MySqlClient;

MySqlConnection conn = new();

Schema db = new(conn, Mode.MySql);

//db.InsertInto("Test").Value(1).Value("Ptdr").Apply();

//db.DeleteFrom("Test").Where(db.Column("id") == 6).Apply();

//db.Update("Test").Set("col1", 4).Set("col2", "Ptdr").Where(db.Column("id") == 3).Apply();

var test = db.Table("Test");
db.Select(test.Column("a").As("aaaaaa")).And("b").From(test).Fetch();