namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Specifies the edge of a container to which a child element is docked.
/// </summary>
public enum DockPositon
{
    /// <summary>
    /// Dock to the left edge.
    /// </summary>
    Left,

    /// <summary>
    /// Dock to the top edge.
    /// </summary>
    Top,

    /// <summary>
    /// Dock to the right edge.
    /// </summary>
    Right,

    /// <summary>
    /// Dock to the bottom edge.
    /// </summary>
    Bottom
}

/// <summary>
/// Extended property for specifying the dock position of a child ViewModel within its parent.
/// This is a context-dependent property - the same child can have different dock positions
/// in different parent containers.
/// </summary>
public class DockProperty : ParentOwnedProperty<DockPositon>
{
    /// <summary>
    /// Singleton instance of DockProperty.
    /// </summary>
    public static readonly DockProperty Instance = new();

    public override string PropertyName => "Dock";

    // Static convenience methods

    /// <summary>
    /// Sets the dock position of a child ViewModel within its parent.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="value">The dock position.</param>
    public static void Set(ViewModelBase parent, ViewModelBase child, DockPositon value)
        => Instance.SetValue(parent, child, value);

    /// <summary>
    /// Gets the dock position of a child ViewModel within its parent.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <returns>The dock position, or null if not set.</returns>
    public static DockPositon? Get(ViewModelBase parent, ViewModelBase child)
        => Instance.GetValue(parent, child);

    /// <summary>
    /// Tries to get the dock position of a child ViewModel within its parent.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="value">The dock position if found.</param>
    /// <returns>True if the value was found, false otherwise.</returns>
    public static bool TryGet(ViewModelBase parent, ViewModelBase child, out DockPositon? value)
        => Instance.TryGetValue(parent, child, out value);

    public static void Clear(ViewModelBase parent, ViewModelBase child) => Instance.ClearValue(parent, child);

    public static void ClearAll(ViewModelBase parent) => Instance.ClearAllValues(parent);

}
