using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WB.Disposable;

/// <summary>
/// A disposable list of elements of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>
/// On disposal, all items of type <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> are disposed.
/// </remarks>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public sealed class DisposableList<T> : DisposableObject, IList<T>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly IList<T> list = [];

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            lock (list)
            {
                return list.Count;
            }
        }
    }

    /// <inheritdoc/>
    public bool IsReadOnly
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            lock (list)
            {
                return list.IsReadOnly;
            }
        }
    }


    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Indexer                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public T this[int index]
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            lock (list)
            {
                return list[index];
            }
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            lock (list)
            {
                list[index] = value;
            }
        }
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public void Add(T item)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            list.Add(item);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            list.Clear();
        }
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            return list.Contains(item);
        }
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            list.CopyTo(array, arrayIndex);
        }
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            return list.Remove(item);
        }
    }

    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            return list.IndexOf(item);
        }
    }

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            list.Insert(index, item);
        }
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        lock (list)
        {
            list.RemoveAt(index);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        return list.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        return GetEnumerator();
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    protected override void DisposeManagedResources()
    {
        base.DisposeManagedResources();

        lock (list)
        {
            foreach (IDisposable disposable in list.OfType<IDisposable>())
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

        lock (list)
        {
            IAsyncDisposable[] disposables = list.OfType<IAsyncDisposable>().ToArray();

            tasks = new Task[disposables.Length];

            for (int i = 0; i < disposables.Length; i++)
            {
                tasks[i] = disposables[i].DisposeAsync().AsTask();
            }
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
