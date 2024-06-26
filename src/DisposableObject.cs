using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WB.Disposable;

/// <summary>
/// A base class for disposable objects.
/// </summary>
/// <remarks>
/// This class provides a standard implementation of the <see cref="IDisposable"/> interface.
/// On disposal, the <see cref="Dispose(bool)"/> method is called with <see langword="true"/> to release managed resources.
/// </remarks>
public class DisposableObject : IDisposable, IAsyncDisposable, INotifyPropertyChanged
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private bool isDisposed;

    /// <summary>
    /// Gets a value indicating whether the object has been disposed (<see langword="true"/>) or not (<see langword="false"/>).
    /// </summary>
    /// <remarks>
    /// A change of this property will raise the <see cref="PropertyChanged"/> event.
    /// </remarks>
    public bool IsDisposed
    {
        get => isDisposed;
        protected set
        {
            if (isDisposed == value)
            {
                return;
            }

            isDisposed = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisposed)));
        }
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Events                                                                  │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Finalizes an instance of the <see cref="DisposableObject"/> class.
    /// </summary>
    /// <see cref="DisposeUnmanagedResourcesAsync"/>
    ~DisposableObject()
    {
        Dispose(false);

        DisposeAsync(false).AsTask().GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    /// <seealso cref="Dispose(bool)"/>
    /// <seealso cref="DisposeManagedResourcesAsync"/>
    /// <seealso cref="DisposeUnmanagedResourcesAsync"/>
    public void Dispose()
    {
        try
        {
            Dispose(disposeManagedResources: true);

            DisposeAsync(disposeManagedResources: true).AsTask().GetAwaiter().GetResult();
        }
        finally
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);

            IsDisposed = true;
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        try
        {
            Dispose(disposeManagedResources: true);

            await DisposeAsync(disposeManagedResources: true).ConfigureAwait(false);
        }
        finally
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);

            IsDisposed = true;
        }
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Releases the managed and unmanaged resources used by the <see cref="DisposableObject"/>.
    /// </summary>
    /// <param name="disposeManagedResources">A value indicating whether the method shall dispose managed resources (<see langword="true"/>) or not (<see langword="false"/>).</param>
    protected virtual void Dispose(bool disposeManagedResources)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposeManagedResources)
        {
            DisposeManagedResources();
        }

        DisposeUnmanagedResources();
    }

    /// <summary>
    /// Releases the managed and unmanaged resources used by the <see cref="DisposableObject"/>.
    /// </summary>
    /// <param name="disposeManagedResources">A value indicating whether the method shall dispose managed resources (<see langword="true"/>) or not (<see langword="false"/>).</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected async virtual ValueTask DisposeAsync(bool disposeManagedResources)
    {
        if (IsDisposed)
        {
            return;
        }

        try
        {
            if (disposeManagedResources)
            {
                await DisposeManagedResourcesAsync().ConfigureAwait(false);
            }

            await DisposeUnmanagedResourcesAsync().ConfigureAwait(false);
        }
        finally
        {
            IsDisposed = true;
        }
    }

    /// <summary>
    /// Releases the managed resources used by the <see cref="DisposableObject"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual void DisposeManagedResources()
    {
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="DisposableObject"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual void DisposeUnmanagedResources()
    {
    }

    /// <summary>
    /// Releases the managed resources used by the <see cref="DisposableObject"/> asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual ValueTask DisposeManagedResourcesAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="DisposableObject"/> asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual ValueTask DisposeUnmanagedResourcesAsync()
    {
        return ValueTask.CompletedTask;
    }
}
