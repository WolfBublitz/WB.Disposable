using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeManagedResourcesAsyncMethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeManagedResourcesAsyncCalled { get; set; }

    protected override ValueTask DisposeManagedResourcesAsync()
    {
        DisposeManagedResourcesAsyncCalled = true;

        return ValueTask.CompletedTask;
    }
}

[TestClass]
public class TheDisposeManagedResourcesAsyncMethod
{
    [TestInitialize]
    public void TestInitialize()
    {
        TestDisposable.DisposeManagedResourcesAsyncCalled = false;
    }

    [TestMethod]
    public void ShouldBeCalledOnDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeManagedResourcesAsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledOnDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeManagedResourcesAsyncCalled.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldNotBeCalledFromFinalizer()
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
        TestDisposable.DisposeManagedResourcesAsyncCalled.Should().BeFalse();
    }
}
