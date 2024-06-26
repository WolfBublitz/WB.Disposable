# WB.Disposable

[![NuGet](https://img.shields.io/nuget/v/WB.Disposable.svg)](https://www.nuget.org/packages/WB.Disposable)

Provides

## Install

```bash
dotnet add package WB.Disposable
```

## Disposables

### DisposableObject

The `DisposableObject` class is a base class implementing the [IDisposable pattern](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#implement-the-dispose-pattern) for synchronous and asynchronous disposal. That means a `DisposableObject` can either be disposed by calling the `Dispose` method or by calling the `DisposeAsync` method. Note that in case of synchronous disposal, the `Dispose` method will call the `DisposeAsync` method and waits for it to complete blocking. This type provides four virtual methods that can be overridden to dispose managed and unmanaged resources:

| Method                           | Description                                  |
| -------------------------------- | -------------------------------------------- |
| `DisposeManagedResources`        | Disposes managed resources.                  |
| `DisposeUnmanagedResources`      | Disposes unmanaged resources.                |
| `DisposeManagedResourcesAsync`   | Disposes managed resources asynchronously.   |
| `DisposeUnmanagedResourcesAsync` | Disposes unmanaged resources asynchronously. |

The following example demonstrates how to create a disposable object:

```csharp
public class MyDisposable : DisposableObject
{
    protected override void DisposeManagedResources()
    {
        // Dispose managed resources
    }

    protected override void DisposeUnmanagedResources()
    {
        // Dispose unmanaged resources
    }

    protected override ValueTask DisposeManagedResourcesAsync()
    {
        // Dispose managed resources asynchronously

        return ValueTask.CompletedTask;
    }

    protected override ValueTask DisposeUnmanagedResourcesAsync()
    {
        // Dispose unmanaged resources asynchronously

        return ValueTask.CompletedTask;
    }
}
```

### ActionDisposable

The `ActionDisposable` class is a disposable that executes on or more actions when disposed.

#### Single Action

```csharp
using (ActionDisposable disposable = new(() => Console.WriteLine("Disposed")))
{
    // Do something
}
// Output: Disposed
```

#### Multiple Actions

```csharp
Action action1 = () => Console.WriteLine("Disposed 1");
Action action2 = () => Console.WriteLine("Disposed 2");

using (ActionDisposable disposable = new(action1, action2))
{
    // Do something
}
// Output: Disposed 1
// Output: Disposed 2
```
