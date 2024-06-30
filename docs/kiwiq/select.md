<div class="alert alert-warning text-center" role="alert">
    🚧 This section is still a work in progress 🚧
</div>

# SELECT

SELECT commands can be created using the `Select` method of a `Schema`. It accepts any number of arguments, either
strings which will interpreted as column names or explicit `Value`s :

```csharp
db.Select("A", "B", "C") // SELECT A, B, C FROM ...

db.Select(db.Table("X").Column("Y"), db.Column("Z") * 100) // SELECT X.Y, Z * 100 FROM ...
    
db.Select() // SELECT * FROM ...
```

If no arguments are given, you can add some columns later using the `And` method (see
[Breaking down the SELECT list](#breaking-down-the-select-list)) or leave it empty to create a SELECT ALL command.

## Choosing the table(s) to read

A first table must always be specified on a SELECT command using the `From` method :

```csharp
db.Select( ... ).From("MY_TABLE") // SELECT ... FROM MY_TABLE
```

You can then use JOIN clause methods (see the [dedicated page](/kiwiq/join)) to read from more tables.

## Breaking down the SELECT list

The `And` method allow you to add more columns to the projection after the first `Select` call. This allows you to break
down long or dynamic projections into multiple parts, or declare both `string` and `Value` columns. 

```csharp
db.Select("A", "B").And(db.Column("C") * 100) // SELECT A, B, C * 100 FROM ...
    
var command = db.Select("A", "B")
if (showDetails) {
    command.And("C", "D");
}
// either SELECT A, B FROM ...
// or SELECT A, B, C, D FROM ...
```

## Removing duplicate rows

To change the SELECT command to a SELECT DISTINCT command, you can use the `Distinct` method on the command :

```csharp
db.Select("A", "B", "C").Distinct() // SELECT DISTINCT A, B, C FROM ...
```

## Filtering the results

...

## Selecting a slice of the results

...

## 