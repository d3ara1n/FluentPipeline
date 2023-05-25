using System;
using System.Collections.Generic;
using DotNext;
using IBuilder;

namespace FluentPipeline;

public class PipelineBuilder<TError, TP> : IBuilder<Pipeline<TError, TP>>
    where TError : struct, Enum
{
    private readonly IList<ProcessBuilder<TError, TP>> builders =
        new List<ProcessBuilder<TError, TP>>();

    private PipelineBuilder() { }

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

    public static ProcessBuilder<TError, TP> Create(Func<TP, Result<TError>> process)
    {
        var parent = new PipelineBuilder<TError, TP>();
        var builder = new ProcessBuilder<TError, TP>(parent, process);
        parent.builders.Add(builder);
        return builder;
    }

    public static ProcessBuilderO<TError, TP, TO> Create<TO>(Func<TP, Result<TO, TError>> process)
    {
        var parent = new PipelineBuilder<TError, TP>();
        var builder = new ProcessBuilderO<TError, TP, TO>(parent, process);
        parent.builders.Add(builder);
        return builder;
    }

    public ProcessBuilderIO<TError, TP, TI, TO> Then<TI, TO>(
        PipelineBuilder<TError, TP> parent,
        Func<TP, TI, Result<TO, TError>> process
    )
    {
        var builder = new ProcessBuilderIO<TError, TP, TI, TO>(parent, process);
        return builder;
    }

    public ProcessBuilderI<TError, TP, TI> Then<TI>(
        PipelineBuilder<TError, TP> parent,
        Func<TP, TI, Result<TError>> process
    )
    {
        var builder = new ProcessBuilderI<TError, TP, TI>(parent, process);
        return builder;
    }
}
