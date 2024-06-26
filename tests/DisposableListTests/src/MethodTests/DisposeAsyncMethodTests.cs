using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableListTests.MethodTests.DisposeAsyncMethodTests;

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
public class TheDisposeAsyncMethod
{
    [TestMethod]
    public async Task ShouldDisposeAllObjectsInTheCollection()
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
        await disposableList.DisposeAsync().ConfigureAwait(false);

        // Assert
        disposable.IsDisposed.Should().BeTrue();
        asyncDisposable.IsDisposed.Should().BeTrue();
    }
}
