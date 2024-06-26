using System;

namespace KiwiQuery.Mapped.Helpers
{

internal interface ICloneable<out T> : ICloneable
{
    new T Clone();
}

}
