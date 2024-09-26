# FluentPipeline



```csharp
// 依赖 RUST 中的类型导出

await Pipeline.Create()
    .Procedure<GlobFilesProcedure>()
    .Procedure<DeleteFilesProcedure>()
    .ExecuteAsync();

class GlobFilesProcedure(IFileSystem fileSystem): Procedure<string[]>
{
    // omitted...
}

class DeleteFilesProcedure(IResultOf<GlobFilesProcedure> files, IFileSystem filesystem): Procedure
{
    // omitted..
}

interface IResultOf<T>
    where T: Procedure
{
    T::Result Value { get; }
}

```

或者

```csharp
// 通过 C# 的 Then 函数重载实现，每个 Then<> 类型都包含可以到达的多种 Then() 函数重载

await Pipeline.CreateSlim() // Then<Unit, Unit>.Then(Func<T>)
    .Then(() => [0,1,2]) // Then<Unit, T: string[]>.Then(Action<string[]>)
    .Then(x: string[] => x.ForEach(Console.WriteLine)) // Then<T: string[], Unit>
    .ExecuteAsync();
```
