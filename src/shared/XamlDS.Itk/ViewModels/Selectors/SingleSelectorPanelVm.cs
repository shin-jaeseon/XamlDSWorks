namespace XamlDS.Itk.ViewModels.Selectors;

/// <summary>
/// Event arguments for single selection changes.
/// </summary>
public class SelectionChangedEventArgs<T> : EventArgs
{
    public SelectionChangedEventArgs(T? previousItem, T? newItem)
    {
        PreviousItem = previousItem;
        NewItem = newItem;
    }

    /// <summary>
    /// The previously selected item.
    /// </summary>
    public T? PreviousItem { get; }

    /// <summary>
    /// The newly selected item.
    /// </summary>
    public T? NewItem { get; }
}

public class SingleSelectorPanelVm<T> : SelectorPanelVm<T>
{
    private T? _selectedItem;

    public override bool IsSingleSelector => true;

    /// <summary>
    /// Event raised when the selected item changes.
    /// </summary>
    public event EventHandler<SelectionChangedEventArgs<T>>? SelectionChanged;

    public T? SelectedItem
    {
        get => _selectedItem;
        protected set => SetProperty(ref _selectedItem, value);
    }

    /// <summary>
    /// Selects the specified item in the selector panel.
    /// </summary>
    /// <param name="item">The item to select.</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when item does not exist in the selector panel.</exception>
    public void SelectItem(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Selected item cannot be null.");
        }

        bool found = false;
        T? previousItem = _selectedItem;

        foreach (var child in Children)
        {
            child.Selected = EqualityComparer<T>.Default.Equals(child.Value, item);
            if (child.Selected)
            {
                found = true;
            }
        }

        if (!found)
        {
            throw new InvalidOperationException($"{item} does not exist in the selector panel.");
        }

        SelectedItem = item;

        // Raise SelectionChanged event
        OnSelectionChanged(previousItem, item);
    }

    /// <summary>
    /// Raises the SelectionChanged event.
    /// </summary>
    protected virtual void OnSelectionChanged(T? previousItem, T? newItem)
    {
        SelectionChanged?.Invoke(this, new SelectionChangedEventArgs<T>(previousItem, newItem));
    }

    protected override void OnSelectableItemClicked(object? sender, SelectableItemClickedEventArgs<T> e)
    {
        SelectItem(e.Value);
    }
}
