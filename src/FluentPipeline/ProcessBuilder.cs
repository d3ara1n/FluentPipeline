using System;
using DotNext;
using IBuilder;

namespace FluentPipeline;

public class ProcessBuilder<TError, TP> : IBuilder<Process<TError, TP>>
    where TError : struct, Enum
{
    private readonly PipelineBuilder<TError, TP> _parent;

    public ProcessBuilder(PipelineBuilder<TError, TP> parent)
    {
        _parent = parent;
    }

    public ProcessBuilder<TError, TP> Then(Func<TP, Result<object?, TError>> process) =>
        _parent.Then(process);

    public ProcessBuilder<TError, TP, TI> Then<TI>(Func<TP, TI, Result<object?, TError>> process) =>
        _parent.Then<TI>(process);

    public virtual Process<TError, TP> Build()
    {
        throw new NotImplementedException();
    }

    public ProcessBuilder<TError, TP, TI> Produces<TI>(Func<object, TI> cast)
    {
        throw new NotImplementedException();
    }

    public ProcessBuilder<TError, TP> Requires<TEngine, TFuel>(Func<object, TFuel> cast)
    {
        throw new NotImplementedException();
    }
}

public class ProcessBuilder<TError, TP, TI> : ProcessBuilder<TError, TP>, IBuilder<Process<TError, TP, TI>>
    where TError : struct, Enum
{
    public ProcessBuilder(PipelineBuilder<TError, TP> parent) : base(parent)
    {
    }

    public override Process<TError, TP, TI> Build()
    {
        throw new NotImplementedException();
    }

    public ProcessBuilder<TError, TP, TII> Produces<TII>(Func<object, TII> cast)
    {
        throw new NotImplementedException();
    }

    public ProcessBuilder<TError, TP, TI> Requires<TEngine, TFuel>(Func<object, TFuel> cast)
    {
        throw new NotImplementedException();
    }
}