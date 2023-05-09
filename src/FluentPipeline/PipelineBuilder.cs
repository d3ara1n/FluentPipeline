using System;
using System.Collections.Generic;
using System.Dynamic;
using DotNext;
using IBuilder;

namespace FluentPipeline;

public class PipelineBuilder<TError, TP> : IBuilder<Pipeline<TError, TP>>
    where TError : struct, Enum
{
    private readonly IList<ProcessBuilder<TError, TP>> builders = new List<ProcessBuilder<TError, TP>>();

    public ProcessBuilder<TError, TP> Create()
    {
        var builder = new ProcessBuilder<TError, TP>(this);
        builders.Add(builder);
        return builder;
    }

    public ProcessBuilder<TError, TP> Then(Func<TP, Result<object?, TError>> process)
    {
        throw new NotImplementedException();
    }

    public ProcessBuilder<TError, TP, TI> Then<TI>(Func<TP, TI, Result<object?, TError>> process)
    {
        throw new NotImplementedException();
    }

    public Pipeline<TError, TP> Build()
    {
        var list = new List<Process<TError, TP>>();
        foreach (var builder in builders)
        {
            var process = builder.Build();
            list.Add(process);
        }

        var pipeline = new Pipeline<TError, TP>(list);
        return pipeline;
    }
}