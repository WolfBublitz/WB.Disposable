using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WB.Disposable;

/// <summary>
/// A disposable list of disposable objects.
/// </summary>
/// <remarks>
/// On disposal, all items in the list are disposed.
/// </remarks>
public sealed class DisposableList : DisposableObject, IList<IDisposable>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    private readonly IList<IDisposable> collection = [];

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            lock (collection)
            {
                return collection.Count;
            }
        }
    }

    /// <inheritdoc/>
    public bool IsReadOnly => collection.IsReadOnly;


    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Indexer                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public IDisposable this[int index]
    {
        get
        {
            lock (collection)
            {
                return collection[index];
            }
        }
        set
        {
            lock (collection)
            {
                collection[index] = value;
            }
        }
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public void Add(IDisposable item)
    {
        lock (collection)
        {
            collection.Add(item);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        lock (collection)
        {
            collection.Clear();
        }
    }

    /// <inheritdoc/>
    public bool Contains(IDisposable item)
    {
        lock (collection)
        {
            return collection.Contains(item);
        }
    }

    /// <inheritdoc/>
    public void CopyTo(IDisposable[] array, int arrayIndex)
    {
        lock (collection)
        {
            collection.CopyTo(array, arrayIndex);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<IDisposable> GetEnumerator() => collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(IDisposable item)
    {
        lock (collection)
        {
            return collection.Remove(item);
        }
    }

    /// <inheritdoc/>
    public int IndexOf(IDisposable item)
    {
        lock (collection)
        {
            return collection.IndexOf(item);
        }
    }

    /// <inheritdoc/>
    public void Insert(int index, IDisposable item)
    {
        lock (collection)
        {
            collection.Insert(index, item);
        }
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        lock (collection)
        {
            collection.RemoveAt(index);
        }
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    protected async override ValueTask DisposeManagedResourcesAsync()
    {
        await base.DisposeManagedResourcesAsync().ConfigureAwait(false);

        lock (collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Dispose();
            }

            collection.Clear();
        }
    }
}
