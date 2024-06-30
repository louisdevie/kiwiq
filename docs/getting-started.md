# Getting started

## Requirements

KiwiQuery is made for .NET 8, but it also supports .NET Standard 2.1. If you are adding it into an existing project, you
can check if your version of .NET is supported [on nuget.org <span class="fas fa-external-link-alt"></span>](https://www.nuget.org/packages/KiwiQuery/0.6.0#supportedframeworks-body-tab){: target="_blank" }. 

## Installation

You can install the library using the .NET CLI :

```sh
dotnet add package KiwiQuery --version 0.6.0
```
or by editing your project file :

```xml
<Project Sdk="Microsoft.NET.Sdk">
    
    <ItemGroup>
        <PackageReference Include="KiwiQuery" Version="0.6.0" />
    </ItemGroup>
    
</Project>
```

alternatively, you can right-click on the dependencies of your project in your IDE, then select *Manage Nuget Packages*
and search for "KiwiQuery" there.

## Creating a “Schema”

In order to do pretty much anything with KiwiQuery, you need a *Schema* that will represent your database connection.
You can create one from any ADO.NET `DbConnection`, for example :

```csharp
var connection = new MySqlConnection("...here goes your connection string");
connection.Open();
var db = new Schema(connection);
```

By default, the schema will be configured for use with MySQL. To change the dialect, you can specify it as the second
argument of the constructor :

```csharp
var db = new Schema(connection, Dialect.Sqlite);
```

!!! note
    Currently, only MySQL and Sqlite are supported out of the box. Check out [Adding a dialect](/extension/dialects) if
    you would like to define another one.

Alternatively, you can set the default dialect for all schemas by calling the following static method :

```csharp
Schema.SetDefaultDialect(Dialect.Sqlite);
```

and all Schema instances created after will use that dialect.

## Writing your first query

You can then use this schema to write commands. Let's say we have a `User` table and a query to find a user by its email
address like this :

```csharp
var command = conn.CreateCommand();
command.CommandText = "SELECT * FROM User WHERE email = @email";
command.Parameters.Add("email", emailAddress);
var results = command.ExecuteReader();
```

Now if we wanted to write the same query using KiwiQuery, we would do :

```csharp
var results = db.Select().From("User").Where(db.Column("email") == emailAddress).Fetch();
```

There is an important concept in the example above : the `db.Column(...) == ...` syntax. Most operators (except the
logical operators `!`, `&&` and `||`) can be applied to a Column, and it will result in a *value* that can be written as
SQL. Values that are known to be boolean are *predicates*. You can use values anywhere you would expect one to appear in
SQL, except in WHERE or ON clauses where a predicate is required in order to catch potential errors at compile-time.

Another important thing to note is that all methods of the `Command` classes (`SelectCommand`, `InsertCommand`, ...)
will return themselves after any operation to let you chain methods and write everything in one line. Also, the command
won't be executed until you call `Fetch()` for SELECT commands or `Apply()` for INSERT, UPDATE and DELETE commands.