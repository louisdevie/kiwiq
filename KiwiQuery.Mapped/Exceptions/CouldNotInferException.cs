using KiwiQuery.Exceptions;
using KiwiQuery.Expressions;

namespace KiwiQuery.Mapped.Exceptions
{

/// <summary>
/// Exception thrown when an optional value could not be guessed from the information available.
/// </summary>
public class CouldNotInferException : KiwiException
{
    private CouldNotInferException(string message) : base(message) { }

    internal static CouldNotInferException ForeignColumn(string foreignTableName, string referencedTableName)
        => new CouldNotInferException(
            $"Could not infer the column from {foreignTableName} referencing {referencedTableName}."
        );

    internal static CouldNotInferException ColumnType(Column column) => new CouldNotInferException(
        $"Could not infer the type of {(column.Table == null ? "" : column.Table + ".")}{column.Name}"
    );
}

}
