using System.Threading.Tasks;
using WB.Disposable;

namespace DisposableObjectTests.MethodTests.DisposeAsyncMethodTests;

[TestClass]
public class TheDisposeAsyncMethod
{
    [TestMethod]
    public async Task ShouldBeCalledMultipleTimesWithoutError()
    {
        // Arrange
        DisposableObject disposableObject = new();

        // Act
        await disposableObject.DisposeAsync().ConfigureAwait(false);
        await disposableObject.DisposeAsync().ConfigureAwait(false);
        await disposableObject.DisposeAsync().ConfigureAwait(false);

        // Assert
        disposableObject.IsDisposed.Should().BeTrue();
    }
}
