using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeManagedResourcesMethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeManagedResourcesCalled { get; set; }

    protected override void DisposeManagedResources()
    {
        DisposeManagedResourcesCalled = true;
    }
}

[TestClass]
public class TheDisposeManagedResourcesMethod
{
    [TestInitialize]
    public void TestInitialize()
    {
        TestDisposable.DisposeManagedResourcesCalled = false;
    }

    [TestMethod]
    public void ShouldBeCalledOnDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeManagedResourcesCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledOnDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeManagedResourcesCalled.Should().BeTrue();
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
        TestDisposable.DisposeManagedResourcesCalled.Should().BeFalse();
    }
}
