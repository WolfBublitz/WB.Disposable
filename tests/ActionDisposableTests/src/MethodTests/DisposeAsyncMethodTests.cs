using System;
using System.Threading.Tasks;
using WB.Disposable;

namespace ActionDisposableTests.MethodTests.DisposeAsyncMethodTests;

[TestClass]
public class TheDisposeAsyncMethod
{
    [TestMethod]
    public async Task ShouldInvokeTheAction()
    {
        // Arrange
        bool actionInvoked = false;
        void action() => actionInvoked = true;
        ActionDisposable actionDisposable = new(action);

        // Act
        await actionDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        actionInvoked.Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldInvokeAllActions()
    {
        // Arrange
        bool action1Invoked = false;
        bool action2Invoked = false;
        void action1() => action1Invoked = true;
        void action2() => action2Invoked = true;
        ActionDisposable actionDisposable = new(action1, action2);

        // Act
        await actionDisposable.DisposeAsync().ConfigureAwait(false);

        // Assert
        action1Invoked.Should().BeTrue();
        action2Invoked.Should().BeTrue();
    }
}
