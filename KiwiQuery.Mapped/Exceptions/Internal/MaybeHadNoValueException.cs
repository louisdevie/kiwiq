using System;

namespace KiwiQuery.Mapped.Exceptions.Internal
{

internal class MaybeHadNoValueException : InvalidOperationException
{
    public MaybeHadNoValueException(): base("The maybe was not a value.") { }
}

}
