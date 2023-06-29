using DotNext;
using IBuilder;
using System;

namespace FluentPipeline;

public class ProcessBuilder<TError, TP> : IBuilder<Process<TError, TP>>
    where TError : struct, Enum
{
    protected readonly PipelineBuilder<TError, TP> _parent;
    private readonly Func<TP, Result<TError>> _process;

    internal ProcessBuilder(PipelineBuilder<TError, TP> parent, Func<TP, Result<TError>> process)
    {
        _parent = parent;
        _process = process;
    }

    public PipelineBuilder<TError, TP> Setup() => _parent;

    public virtual Process<TError, TP> Build()
    {
        var process = new Process<TError, TP>(_process);
        return process;
    }
}

public class ProcessBuilderO<TError, TP, TO> : ProcessBuilder<TError, TP>
    where TError : struct, Enum
{
    private readonly Func<TP, Result<TO, TError>> _process;

    internal ProcessBuilderO(
        PipelineBuilder<TError, TP> parent,
        Func<TP, Result<TO, TError>> process
    )
        : base(parent, null!)
    {
        _process = process;
    }

    public override Process<TError, TP> Build()
    {
        var process = new ProcessO<TError, TP, TO>(_process);
        return process;
    }

    public ProcessBuilderIO<TError, TP, TO, TO2> Then<TO2>(
        Func<TP, TO, Result<TO2, TError>> process
    )
    {
        return _parent.Then(_parent, process);
    }
}

public class ProcessBuilderIO<TError, TP, TI, TO> : ProcessBuilderO<TError, TP, TO>
    where TError : struct, Enum
{
    private readonly Func<TP, TI, Result<TO, TError>> _process;

    internal ProcessBuilderIO(
        PipelineBuilder<TError, TP> parent,
        Func<TP, TI, Result<TO, TError>> process
    )
        : base(parent, null!)
    {
        _process = process;
    }

    public override Process<TError, TP> Build()
    {
        var process = new ProcessIO<TError, TP, TI, TO>(_process);
        return process;
    }
}
