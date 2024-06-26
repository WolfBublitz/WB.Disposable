using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WB.Disposable;

/// <summary>
/// A disposable collection of disposable objects.
/// </summary>
/// <remarks>
/// On disposal, all items in the collection are disposed.
/// </remarks>
/// <seealso cref="DisposableObject"/>
public sealed class DisposableCollection : DisposableObject, ICollection<IDisposable>, ICollection<IAsyncDisposable>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    private readonly ICollection<object> disposables = [];

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            lock (disposables)
            {
                return disposables.Count;
            }
        }
    }

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    /// <seealso cref="Add(IAsyncDisposable)"/>
    /// <seealso cref="Add(DisposableObject)"/>
    public void Add(IDisposable item)
    {
        lock (disposables)
        {
            disposables.Add(item);
        }
    }

    /// <inheritdoc/>
    /// <seealso cref="Add(IDisposable)"/>
    /// <seealso cref="Add(DisposableObject)"/>
    public void Add(IAsyncDisposable item)
    {
        lock (disposables)
        {
            disposables.Add(item);
        }
    }

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <exception cref="ArgumentException">Raised if <paramref name="item"/> neither of type <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.</exception>
    /// <seealso cref="Add(IDisposable)"/>
    /// <seealso cref="Add(IAsyncDisposable)"/>
    public void Add(DisposableObject item)
    {
        lock (disposables)
        {
            disposables.Add(item);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        lock (disposables)
        {
            disposables.Clear();
        }
    }

    /// <inheritdoc/>
    public bool Contains(IDisposable item)
    {
        lock (disposables)
        {
            return disposables.Contains(item);
        }
    }

    /// <inheritdoc/>
    public bool Contains(IAsyncDisposable item)
    {
        lock (disposables)
        {
            return disposables.Contains(item);
        }
    }

    /// <inheritdoc/>
    public void CopyTo(IDisposable[] array, int arrayIndex)
    {
        lock (disposables)
        {
            IDisposable[] collection = disposables.OfType<IDisposable>().ToArray();

            collection.CopyTo(array, arrayIndex);
        }
    }

    /// <inheritdoc/>
    public void CopyTo(IAsyncDisposable[] array, int arrayIndex)
    {
        lock (disposables)
        {
            IAsyncDisposable[] collection = disposables.OfType<IAsyncDisposable>().ToArray();

            collection.CopyTo(array, arrayIndex);
        }
    }

    /// <inheritdoc/>
    public bool Remove(IDisposable item)
    {
        lock (disposables)
        {
            return disposables.Remove(item);
        }
    }

    /// <inheritdoc/>
    public bool Remove(IAsyncDisposable item)
    {
        lock (disposables)
        {
            return disposables.Remove(item);
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => disposables.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator<IDisposable> IEnumerable<IDisposable>.GetEnumerator()
        => disposables.OfType<IDisposable>().GetEnumerator();

    /// <inheritdoc/>
    IEnumerator<IAsyncDisposable> IEnumerable<IAsyncDisposable>.GetEnumerator()
        => disposables.OfType<IAsyncDisposable>().GetEnumerator();

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();

        lock (disposables)
        {
            foreach (IDisposable disposable in disposables.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeManagedResourcesAsync()
    {
        await base.DisposeManagedResourcesAsync().ConfigureAwait(false);

        Task[] tasks;

        lock (disposables)
        {
            IAsyncDisposable[] collection = disposables.OfType<IAsyncDisposable>().ToArray();

            tasks = new Task[collection.Length];

            for (int i = 0; i < collection.Length; i++)
            {
                tasks[i] = collection[i].DisposeAsync().AsTask();
            }
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
