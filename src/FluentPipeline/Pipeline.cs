using System;
using System.Collections.Generic;

namespace FluentPipeline;

public class Pipeline<TError, TP>
where TError: struct, Enum
{
    private readonly IEnumerable<Process<TError, TP>> _processes;
    private readonly int _count;
    internal Pipeline(IList<Process<TError, TP>> processes)
    {
        _processes = processes;
        _count = processes.Count;
    }

    public int Count => _count;

    public void Pump()
    {
        
    }
}