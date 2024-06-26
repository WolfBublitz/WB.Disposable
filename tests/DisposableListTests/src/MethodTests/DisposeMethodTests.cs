using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableListTests.MethodTests.DisposeMethodTests;

internal sealed class Disposable : IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

internal sealed class AsyncDisposable : IAsyncDisposable
{
    public bool IsDisposed { get; private set; }

    public ValueTask DisposeAsync()
    {
        IsDisposed = true;

        return ValueTask.CompletedTask;
    }
}

[TestClass]
public class TheDisposeMethod
{
    [TestMethod]
    public void ShouldDisposeAllObjectsInTheCollection()
    {
        // Arrange
        Disposable disposable = new();
        AsyncDisposable asyncDisposable = new();
        DisposableList<object> disposableList =
        [
            disposable,
            asyncDisposable,
            new object(),
        ];

        // Act
        disposableList.Dispose();

        // Assert
        disposable.IsDisposed.Should().BeTrue();
        asyncDisposable.IsDisposed.Should().BeTrue();
    }
}
