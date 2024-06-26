using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableCollectionTests.MethodTests.DisposeAsyncMethodTests;

[TestClass]
public class TheDisposeAsyncMethod
{
    [TestMethod]
    public async Task ShouldDisposeAllObjectsInTheCollection()
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
        await disposableCollection.DisposeAsync().ConfigureAwait(false);

        // Assert
        disposableObject1.IsDisposed.Should().BeTrue();
        disposableObject2.IsDisposed.Should().BeTrue();
    }
}
