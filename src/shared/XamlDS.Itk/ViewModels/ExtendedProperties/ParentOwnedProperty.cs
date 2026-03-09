using System.Runtime.CompilerServices;

namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Base class for extended properties that are owned by a parent ViewModel (context-dependent).
/// These properties describe the relationship between parent and child ViewModels.
/// The same child ViewModel can have different property values in different parent contexts.
/// Examples: Dock, GridRow, GridColumn, LayoutInfo
/// </summary>
/// <typeparam name="T">The type of the property value (must be a reference type or nullable value type).</typeparam>
public abstract class ParentOwnedProperty<T> : ExtendedViewModelProperty where T : struct
{
    // Storage: parent -> (child -> value)
    private readonly ConditionalWeakTable<ViewModelBase, Dictionary<ViewModelBase, T>> _storage = new();

    /// <summary>
    /// Sets the property value for a child ViewModel within its parent context.
    /// </summary>
    /// <param name="parent">The parent ViewModel that owns this property value.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(ViewModelBase parent, ViewModelBase child, T value)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (child == null) throw new ArgumentNullException(nameof(child));

        var childDict = _storage.GetOrCreateValue(parent);
        var isNewOrChanged = !childDict.ContainsKey(child) || !EqualityComparer<T>.Default.Equals(childDict[child], value);

        childDict[child] = value;

        // Register on parent for automatic disposal and notify if new
        if (isNewOrChanged)
        {
            parent.RegisterExtendedProperty(this);
        }
    }

    /// <summary>
    /// Gets the property value for a child ViewModel within its parent context.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <returns>The property value, or default if not set.</returns>
    public T? GetValue(ViewModelBase parent, ViewModelBase child)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (child == null) throw new ArgumentNullException(nameof(child));

        if (_storage.TryGetValue(parent, out var childDict) && 
            childDict.TryGetValue(child, out var value))
        {
            return value;
        }
        return default;
    }

    /// <summary>
    /// Tries to get the property value for a child ViewModel within its parent context.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="value">The property value if found.</param>
    /// <returns>True if the value was found, false otherwise.</returns>
    public bool TryGetValue(ViewModelBase parent, ViewModelBase child, out T? value)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (child == null) throw new ArgumentNullException(nameof(child));

        if (_storage.TryGetValue(parent, out var childDict) && childDict.TryGetValue(child, out var v))
        {
            value = v;
            return true;
        }
        value = default;
        return false;
    }

    /// <summary>
    /// Clears the property value for a specific child within its parent context.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    public void ClearValue(ViewModelBase parent, ViewModelBase child)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (child == null) throw new ArgumentNullException(nameof(child));

        if (_storage.TryGetValue(parent, out var childDict))
        {
            childDict.Remove(child);
        }
    }

    /// <summary>
    /// Clears all property values for all children of the specified parent.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    public void ClearAllValues(ViewModelBase parent)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        _storage.Remove(parent);
    }

    public override void Dispose()
    {
        if (_disposed) return;
        base.Dispose();
        // ConditionalWeakTable automatically cleans up when targets are garbage collected
    }
}
