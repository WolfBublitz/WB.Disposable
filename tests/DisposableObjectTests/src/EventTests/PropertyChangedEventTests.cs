using System.ComponentModel;
using FluentAssertions.Events;
using WB.Disposable;

namespace DisposableObjectTests.EventTests.PropertyChangedEventTests;

[TestClass]
public class ThePropertyChangedEvent
{
    [TestMethod]
    public void ShouldBeRaisedWhenIsDisposedChanges()
    {
        // Arrange
        DisposableObject disposableObject = new();
        IMonitor<DisposableObject> monitor = disposableObject.Monitor();

        // Act
        disposableObject.Dispose();

        // Assert
        monitor.Should()
            .RaisePropertyChangeFor(x => x.IsDisposed)
            .WithSender(disposableObject)
            .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(DisposableObject.IsDisposed));
    }
}
