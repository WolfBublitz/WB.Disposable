using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeUnmanagedResourcesAsyncMethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeUnmanagedResourcesAsyncCalled { get; set; }

    protected override ValueTask DisposeUnmanagedResourcesAsync()
    {
        DisposeUnmanagedResourcesAsyncCalled = true;

        return ValueTask.CompletedTask;
    }
}

[TestClass]
public class TheDisposeUnmanagedResourcesAsyncMethod
{
    [TestInitialize]
    public void TestInitialize()
    {
        TestDisposable.DisposeUnmanagedResourcesAsyncCalled = false;
    }

    [TestMethod]
    public void ShouldBeCalledOnDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeUnmanagedResourcesAsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledOnDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeUnmanagedResourcesAsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldBeCalledFromFinalizer()
    {
        // Arrange
        static void action()
        {
            TestDisposable testDisposable = new();
        }

        // Act
        action();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // Assert
        TestDisposable.DisposeUnmanagedResourcesAsyncCalled.Should().BeTrue();
    }
}
