using System;
using WB.Disposable;

namespace ActionDisposableTests.MethodTests.DisposeMethodTests;

[TestClass]
public class TheDisposeMethod
{
    [TestMethod]
    public void ShouldInvokeTheAction()
    {
        // Arrange
        bool actionInvoked = false;
        void action() => actionInvoked = true;
        ActionDisposable actionDisposable = new(action);

        // Act
        actionDisposable.Dispose();

        // Assert
        actionInvoked.Should().BeTrue();
    }

    [TestMethod]
    public void ShouldInvokeAllActions()
    {
        // Arrange
        bool action1Invoked = false;
        bool action2Invoked = false;
        void action1() => action1Invoked = true;
        void action2() => action2Invoked = true;
        ActionDisposable actionDisposable = new(action1, action2);

        // Act
        actionDisposable.Dispose();

        // Assert
        action1Invoked.Should().BeTrue();
        action2Invoked.Should().BeTrue();
    }
}
