namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Specifies the row position of a child element in a grid layout.
/// </summary>
public class GridRowProperty : ParentOwnedProperty<int>
{
    /// <summary>
    /// Singleton instance of GridRowProperty.
    /// </summary>
    public static readonly GridRowProperty Instance = new();

    public override string PropertyName => "GridRow";

    /// <summary>
    /// Sets the grid row position of a child ViewModel within its parent grid.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="row">The zero-based row index.</param>
    public static void Set(ViewModelBase parent, ViewModelBase child, int row)
    {
        if (row < 0) throw new ArgumentException("Row index cannot be negative.", nameof(row));
        Instance.SetValue(parent, child, row);
    }

    /// <summary>
    /// Gets the grid row position of a child ViewModel within its parent grid.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <returns>The row index, or 0 if not set.</returns>
    public static int Get(ViewModelBase parent, ViewModelBase child)
        => Instance.GetValue(parent, child) ?? 0;
}

/// <summary>
/// Specifies the column position of a child element in a grid layout.
/// </summary>
public class GridColumnProperty : ParentOwnedProperty<int>
{
    /// <summary>
    /// Singleton instance of GridColumnProperty.
    /// </summary>
    public static readonly GridColumnProperty Instance = new();

    public override string PropertyName => "GridColumn";

    /// <summary>
    /// Sets the grid column position of a child ViewModel within its parent grid.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <param name="column">The zero-based column index.</param>
    public static void Set(ViewModelBase parent, ViewModelBase child, int column)
    {
        if (column < 0) throw new ArgumentException("Column index cannot be negative.", nameof(column));
        Instance.SetValue(parent, child, column);
    }

    /// <summary>
    /// Gets the grid column position of a child ViewModel within its parent grid.
    /// </summary>
    /// <param name="parent">The parent ViewModel.</param>
    /// <param name="child">The child ViewModel.</param>
    /// <returns>The column index, or 0 if not set.</returns>
    public static int Get(ViewModelBase parent, ViewModelBase child)
        => Instance.GetValue(parent, child) ?? 0;
}
