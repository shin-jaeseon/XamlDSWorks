using System.Collections.ObjectModel;

namespace XamlDS.Itk.ViewModels.Selectors;


/// <summary>
/// Event arguments for multi-selection changes.
/// </summary>
public class MultiSelectionChangedEventArgs<T> : EventArgs
{
    public MultiSelectionChangedEventArgs(T item, SelectionAction action, IEnumerable<T> currentSelection)
    {
        Item = item;
        Action = action;
        CurrentSelection = currentSelection;
    }

    /// <summary>
    /// The item that was added or removed.
    /// </summary>
    public T Item { get; }

    /// <summary>
    /// The action performed (Added or Removed).
    /// </summary>
    public SelectionAction Action { get; }

    /// <summary>
    /// The current collection of selected items after the change.
    /// </summary>
    public IEnumerable<T> CurrentSelection { get; }
}

public class MultiSelectorPanelVm<T> : SelectorPanelVm<T>
{
    private ObservableCollection<T> _selectedItems = new ObservableCollection<T>();

    public override bool IsSingleSelector => false;

    /// <summary>
    /// Event raised when the selected items collection changes.
    /// </summary>
    public event EventHandler<MultiSelectionChangedEventArgs<T>>? SelectionChanged;

    public ObservableCollection<T> SelectedItems => _selectedItems;

    /// <summary>
    /// Toggles the selection state of the specified item in the selector panel.
    /// </summary>
    /// <param name="item">The item to toggle.</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when item does not exist in the selector panel.</exception>
    public void ToggleItem(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Selected item cannot be null.");
        }

        bool found = false;
        bool wasSelected = false;

        foreach (var child in Children)
        {
            if (EqualityComparer<T>.Default.Equals(child.Value, item))
            {
                found = true;
                wasSelected = child.Selected;

                if (child.Selected)
                {
                    child.Selected = false;
                    _selectedItems.Remove(item);
                    OnSelectionChanged(item, SelectionAction.Removed);
                }
                else
                {
                    child.Selected = true;
                    _selectedItems.Add(item);
                    OnSelectionChanged(item, SelectionAction.Added);
                }
            }
        }

        if (!found)
        {
            throw new InvalidOperationException($"{item} does not exist in the selector panel.");
        }
    }

    /// <summary>
    /// Raises the SelectionChanged event.
    /// </summary>
    protected virtual void OnSelectionChanged(T item, SelectionAction action)
    {
        SelectionChanged?.Invoke(this, new MultiSelectionChangedEventArgs<T>(item, action, _selectedItems));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var items in SelectedItems)
            {
                if (items is IDisposable disposableChild)
                {
                    disposableChild.Dispose();
                }
            }
            SelectedItems.Clear();
        }
        base.Dispose(disposing);
    }

    protected override void OnSelectableItemClicked(object? sender, SelectableItemClickedEventArgs<T> e)
    {
        ToggleItem(e.Value);
    }
}
