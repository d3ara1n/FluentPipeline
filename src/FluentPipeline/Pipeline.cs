using System;
using System.Collections.Generic;
using System.Threading;
using DotNext;

namespace FluentPipeline;

public class Pipeline<TError, TP>
    where TError : struct, Enum
{
    private readonly IList<Process<TError, TP>> _processes;
    public int Count => _processes.Count;

    internal Pipeline(IList<Process<TError, TP>> processes)
    {
        _processes = processes;
    }

    public bool Pump(TP input, CancellationToken token = default)
    {
        object? product = null;
        foreach (var process in _processes)
        {
            if (token.IsCancellationRequested)
                return false;
            var type = process.GetType();
            if (type == typeof(ProcessI<,,>))
            {
                if (product != null && product.GetType() == type.GetGenericArguments()[2])
                {
                    var result = Pump<Result<TError>>(process, new object[] { input!, product! });
                    if (result.IsSuccessful)
                    {
                        product = null;
                    }
                    else
                    {
                        // report
                    }
                }
                else
                {
                    // report
                }
            }
            else if (type == typeof(ProcessO<,,>))
            {
                var result = Pump<Result<object, TError>>(process, new object[] { input! });
                if (result.IsSuccessful)
                {
                    product = result.Value;
                }
                else
                {
                    // report
                }
            }
            else if (type == typeof(ProcessIO<,,,>))
            {
                if (product != null && product.GetType() == type.GetGenericArguments()[2])
                {
                    var result = Pump<Result<object, TError>>(
                        process,
                        new object[] { input!, product! }
                    );

                    if (result.IsSuccessful)
                    {
                        product = result.Value;
                    }
                    else
                    {
                        // report
                    }
                }
                else
                {
                    // report
                }
            }
            else
            {
                var result = Pump<Result<TError>>(process, new object[] { input! });
                if (result.IsSuccessful)
                {
                    product = null;
                }
                else
                {
                    // report
                }
            }
            // look for requirement meet list
        }

        throw new NotImplementedException();
    }

    private T? Pump<T>(object subject, object[] arguments)
    {
        var type = subject.GetType();
        var method = type.GetMethod("Pump");
        return (T?)method!.Invoke(subject, arguments);
    }
}
