Title: Home

# ![the kiwi query logo](images/logo.png) KiwiQuery


!!! warning
    This library is still in early development. Lots of features are missing and *some parts of the API may change
    completely* in the next updates.

KiwiQuery is a SQL query builder for .NET. It is compatible with any ADO.NET connector, and has first-class support for
**MySQL** and **SQLite** dialects.

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
