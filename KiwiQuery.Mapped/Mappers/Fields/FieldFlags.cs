using System;

namespace KiwiQuery.Mapped.Mappers.Fields
{

[Flags]
internal enum FieldFlags
{
    None = 0,
    PrimaryKey = 1,
    NotInserted = 2,
}

}
