using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeUnmanagedResourcesMethodTests;

internal class TestDisposable : DisposableObject
{
    public static bool DisposeUnmanagedResourcesCalled { get; set; }

    protected override void DisposeUnmanagedResources()
    {
        DisposeUnmanagedResourcesCalled = true;
    }
}

[TestClass]
public class TheDisposeUnmanagedResourcesMethod
{
    [TestInitialize]
    public void TestInitialize()
    {
        TestDisposable.DisposeUnmanagedResourcesCalled = false;
    }

    [TestMethod]
    public void ShouldBeCalledOnDispose()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        testDisposable.Dispose();

        // Assert
        TestDisposable.DisposeUnmanagedResourcesCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldBeCalledOnDisposeAsync()
    {
        // Arrange
        TestDisposable testDisposable = new();

        // Act
        await testDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        TestDisposable.DisposeUnmanagedResourcesCalled.Should().BeTrue();
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
        TestDisposable.DisposeUnmanagedResourcesCalled.Should().BeTrue();
    }
}
