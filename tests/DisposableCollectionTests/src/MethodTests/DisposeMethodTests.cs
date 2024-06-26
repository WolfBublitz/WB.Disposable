using WB.Disposable;

namespace DisposableCollectionTests.MethodTests.DisposeMethodTests;

[TestClass]
public class TheDisposeMethod
{
    [TestMethod]
    public void ShouldDisposeAllObjectsInTheCollection()
    {
        // Arrange
        DisposableObject disposableObject1 = new();
        DisposableObject disposableObject2 = new();
        DisposableCollection disposableCollection = new()
        {
            disposableObject1,
            disposableObject2
        };

        // Act
        disposableCollection.Dispose();

        // Assert
        disposableObject1.IsDisposed.Should().BeTrue();
        disposableObject2.IsDisposed.Should().BeTrue();
    }
}
