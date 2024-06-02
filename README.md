<h1>KiwiQuery <img src="kiwiq-128.png" height="80px"/> </h1>

*Note : this library is still in early development. Lots of features are missing and some parts of the API may change completly in the next updates.*

KiwiQuery is a SQL query builder for MySQL. It is compatible with both [`MySql.Data`](https://www.nuget.org/packages/MySql.Data) and [`MySqlConnector`](https://www.nuget.org/packages/MySqlConnector) and can technically work with any ADO.NET connector, though it doesn't support other SQL dialects.

The queries are built using a DSL that tries to mimic the way SQL is written, for example :

```cs
var results = db.Select("id", "name").From("books")
                .Where(db.Column("publishingDate") > new DateTime(2020, 01, 01))
                .Fetch();
```

```cs
db.InsertInto("books")
  .Value("name", "Illuminae, Tome 1 : Dossier Alexander")
  .Value("publishingDate", new DateTime(2016, 9, 14))
  .Value("authors", "Amie Kaufman;Jay Kristoff")
  .Apply();
```
