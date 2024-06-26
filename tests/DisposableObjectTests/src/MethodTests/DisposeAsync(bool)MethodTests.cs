using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeAsync__bool__MethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeCalled { get; set; }

    public static bool? DisposeManagedResourcesValue { get; set; }

    protected override ValueTask DisposeAsync(bool disposeManagedResources)
    {
        DisposeCalled = true;

        DisposeManagedResourcesValue = disposeManagedResources;

        return ValueTask.CompletedTask;
    }
}

[TestClass]
public class TheDisposeAsyncMethod
{
    [TestInitialize]
    public void TestInitialize()
    {
        TestDisposable.DisposeCalled = false;
        TestDisposable.DisposeManagedResourcesValue = null;
    }

    [TestMethod]
    public void ShouldBeCalledOnDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeManagedResourcesValue.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledOnDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeManagedResourcesValue.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldBeCalledWithTrueIfCalledFromDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeManagedResourcesValue.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledWithFalseIfCalledFromDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeManagedResourcesValue.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldBeCalledWithFalseIfCalledFromFinalizer()
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
        TestDisposable.DisposeCalled.Should().BeTrue();
        TestDisposable.DisposeManagedResourcesValue.Should().BeFalse();
    }
}
