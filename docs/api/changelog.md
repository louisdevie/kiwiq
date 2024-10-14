# Changelog

## KiwiQuery 0.6 <small>(latest)</small>

!!! danger "Breaking changes"
    - **Migrated the project from .NET 6 / 7 to .NET 8. .NET Standard 2.1 will continue to be supported.** 
    - Replaced the `Schema.SelectAll` method by a parameterless overload of `Schema.Select`.
    - Renamed all `Query` classes to `Command`. 

- **Added first-class support for Sqlite.**
- **Replaced all clauses methods (`Where`, `Join`, `Limit`, ...) with extension methods so that they are shared by the
  different commands.**
- Added a base class `KiwiException` for exceptions raised by the library.
- Added the `Column.Sibling` method.
- Exposed the default ID used when it couldn't be determined after an INSERT command as the constant
  `InsertCommand.NO_AUTO_ID`.

## KiwiQuery 0.5

SELECT DISTINCT queries and easier table aliasing.

## KiwiQuery 0.4

Added support for table aliases.

## KiwiQuery 0.3

Added support for LIMIT clauses.

## KiwiQuery 0.2.1

The documentation file is now correctly generated.

## KiwiQuery 0.2.0

Added support for logical operators (NOT, AND and OR).

## KiwiQuery 0.1

First alpha version.