using System;
using System.Collections.Generic;
using DotNext;

namespace FluentPipeline;

public class Process<TError, TP>
    where TError : struct, Enum
{
    private readonly Func<TP, Result<TError>> _process;

    internal Process(
        Func<TP, Result<TError>> process
    )
    {
        _process = process;
    }

    internal Result<TError> Pump(TP product)
    {
        return _process.Invoke(product);
    }
}

public class ProcessO<TError, TP, TO> : Process<TError, TP>
    where TError : struct, Enum
{
    private readonly Func<TP, Result<TO, TError>> _process;

    internal ProcessO(Func<TP, Result<TO, TError>> process) : base(null!)
    {
        _process = process;
    }

    internal Result<TO, TError> Pump(TP product)
    {
        return _process.Invoke(product);
    }
}

public class ProcessIO<TError, TP, TI, TO> : Process<TError, TP>
    where TError : struct, Enum
{
    private readonly Func<TP, TI, Result<TO, TError>> _process;

    internal ProcessIO(Func<TP, TI, Result<TO, TError>> process) : base(null!)
    {
        _process = process;
    }

    internal Result<TO, TError> Pump(TP product, TI intermediate)
    {
        return _process.Invoke(product, intermediate);
    }
}

public class ProcessI<TError, TP, TI> : Process<TError, TP>
    where TError : struct, Enum
{
    private readonly Func<TP, TI, Result<TError>> _process;
    
    internal ProcessI(Func<TP, TI, Result<TError>> process) : base(null!)
    {
        _process = process;
    }

    internal Result<TError> Pump(TP product, TI intermediate)
    {
        return _process.Invoke(product, intermediate);
    }
}