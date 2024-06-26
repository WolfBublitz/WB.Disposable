using WB.Disposable;

namespace DisposableObjectTests.PropertyTests.IsDisposedPropertyTests;

[TestClass]
public class TheIsDisposedProperty
{
    [TestMethod]
    public void ShouldBeFalseAtDefault()
    {
        // Arrange
        DisposableObject disposableObject = new();

        // Act
        bool isDisposed = disposableObject.IsDisposed;

        // Assert
        isDisposed.Should().BeFalse();
    }

    [TestMethod]
    public void ShouldBeTrueIfDisposed()
    {
        // Arrange
        DisposableObject disposableObject = new();
        disposableObject.Dispose();

        // Act
        bool isDisposed = disposableObject.IsDisposed;

        // Assert
        isDisposed.Should().BeTrue();
    }
}
