using System;
using DotNext;

namespace FluentPipeline;

public class Process<TError, TP>
    where TError : struct, Enum
{
    internal virtual Result<object?, TError> Pump()
    {
        throw new NotImplementedException();
    }
}

public class Process<TError, TP, TI> : Process<TError, TP>
    where TError : struct, Enum
{
    internal override Result<object?, TError> Pump()
    {
        throw new NotImplementedException();
    }
}