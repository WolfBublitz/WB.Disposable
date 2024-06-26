using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeMethodTests;

[TestClass]
public class TheDisposeMethod
{
    [TestMethod]
    public void ShouldBeCalledMultipleTimesWithoutError()
    {
        // Arrange
        DisposableObject disposableObject = new();

        // Act
        disposableObject.Dispose();
        disposableObject.Dispose();
        disposableObject.Dispose();

        // Assert
        disposableObject.IsDisposed.Should().BeTrue();
    }
}
