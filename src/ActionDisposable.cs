using System;
using System.Threading.Tasks;

namespace WB.Disposable;

/// <summary>
/// A disposable object that executes an <see cref="Action"/> on disposal.
/// </summary>
public sealed class ActionDisposable : DisposableObject
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly Action[] actions;

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                            │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionDisposable"/> class.
    /// </summary>
    /// <param name="action">The <see cref="Action"/> that shall be executed on disposal.</param>
    /// <exception cref="ArgumentNullException">Raised when <paramref name="action"/> is <see langword="null"/>.</exception>
    public ActionDisposable(Action action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        actions = [action];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionDisposable"/> class.
    /// </summary>
    /// <param name="actions">The <see cref="Action"/>s that shall be executed on disposal.</param>
    /// <exception cref="ArgumentNullException">Raised when <paramref name="actions"/> is <see langword="null"/>.</exception>
    public ActionDisposable(params Action[] actions)
    {
        ArgumentNullException.ThrowIfNull(actions, nameof(actions));

        this.actions = actions;
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    protected override async ValueTask DisposeManagedResourcesAsync()
    {
        await base.DisposeManagedResourcesAsync().ConfigureAwait(false);

        for (int i = 0; i < actions.Length; i++)
        {
            actions[i]();
        }
    }
}
