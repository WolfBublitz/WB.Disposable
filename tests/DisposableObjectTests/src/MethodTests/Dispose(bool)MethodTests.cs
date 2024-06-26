using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.Dispose__bool__MethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeCalled { get; set; }

    public static bool? DisposeManagedResourcesValue { get; set; }

    protected override void Dispose(bool disposeManagedResources)
    {
        DisposeCalled = true;

        DisposeManagedResourcesValue = disposeManagedResources;
    }
}

[TestClass]
public class TheDisposeMethod
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
    public async Task ShouldBeCalledWithTrueIfCalledFromDisposeAsync()
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
