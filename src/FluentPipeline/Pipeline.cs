using DotNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace FluentPipeline;

public class Pipeline<TError, TP>
    where TError : struct, Enum
{
    private readonly IList<Process<TError, TP>> _processes;
    public int Count => _processes.Count;

    private object? waste;
    private Exception? exception;
    private TError? error;

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
            var definition = type.GetGenericTypeDefinition();
            if (definition == typeof(ProcessO<,,>))
            {
                try
                {
                    var result = Pump(process, new object[] { input! });
                    if (result.IsSuccessful)
                    {
                        product = result.Value;
                    }
                    else
                    {
                        error = result.Error;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    exception = e;
                    return false;
                }
            }
            else if (definition == typeof(ProcessIO<,,,>))
            {
                if (product != null && product.GetType() == type.GetGenericArguments()[2])
                {
                    try
                    {
                        var result = Pump(process, new object[] { input!, product! });

                        if (result.IsSuccessful)
                        {
                            product = result.Value;
                        }
                        else
                        {
                            error = result.Error;
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        return false;
                    }
                }
                else
                {
                    throw new UnreachableException();
                }
            }
            else
            {
                throw new UnreachableException();
            }
            // look for requirement meet list
        }

        waste = product;
        return true;
    }

    private Result<object, TError> Pump(object subject, object[] arguments)
    {
        var type = subject.GetType();
        var method = type.GetMethod(
            "Pump",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly
        );
        var result = method!.Invoke(subject, arguments);
        var extract = result!.GetType();
        var isSuccessful = extract.GetProperty("IsSuccessful");
        var value = extract.GetProperty("Value");
        var error = extract.GetProperty("Error");
        if (isSuccessful != null && isSuccessful.GetValue(result) is true)
        {
            return new Result<object, TError>(value!.GetValue(result)!);
        }
        else
        {
            return new Result<object, TError>((TError)error!.GetValue(result)!);
        }
    }

    public bool HandleWaste<T>(out T? notWaste)
    {
        if (waste is T present)
        {
            notWaste = present;
            return true;
        }
        else
        {
            notWaste = default;
            return false;
        }
    }

    public bool HandleError(out TError? mustError)
    {
        mustError = error;
        return mustError.HasValue;
    }

    public bool HandleException(out Exception? mustException)
    {
        mustException = exception;
        return mustException != null;
    }
}
