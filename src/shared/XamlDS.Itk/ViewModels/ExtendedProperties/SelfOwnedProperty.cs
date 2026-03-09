using System.Runtime.CompilerServices;

namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Base class for extended properties that are owned by the ViewModel itself (context-independent).
/// These properties belong to the ViewModel and persist regardless of where it's used.
/// Examples: ThemeAccentColor, CustomStyle, Metadata
/// </summary>
/// <typeparam name="T">The type of the property value (must be a reference type).</typeparam>
public abstract class SelfOwnedProperty<T> : ExtendedViewModelProperty where T : class
{
    private readonly ConditionalWeakTable<ViewModelBase, T> _storage = new();

    /// <summary>
    /// Sets the property value for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(ViewModelBase target, T value)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));

        _storage.Remove(target);
        _storage.Add(target, value);

        // Register for automatic disposal
        target.RegisterExtendedProperty(this);
    }

    /// <summary>
    /// Gets the property value for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <returns>The property value, or default if not set.</returns>
    public T? GetValue(ViewModelBase target)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        return _storage.TryGetValue(target, out var value) ? value : default;
    }

    /// <summary>
    /// Tries to get the property value for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <param name="value">The property value if found.</param>
    /// <returns>True if the value was found, false otherwise.</returns>
    public bool TryGetValue(ViewModelBase target, out T? value)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        return _storage.TryGetValue(target, out value);
    }

    /// <summary>
    /// Clears the property value for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    public void ClearValue(ViewModelBase target)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        _storage.Remove(target);
    }

    public override void Dispose()
    {
        if (_disposed) return;
        base.Dispose();
        // ConditionalWeakTable automatically cleans up when targets are garbage collected
    }
}
