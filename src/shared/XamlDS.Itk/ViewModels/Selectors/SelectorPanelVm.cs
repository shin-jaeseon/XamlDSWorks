using System.Collections.ObjectModel;
using System.ComponentModel;
using XamlDS.Itk.Themes;

namespace XamlDS.Itk.ViewModels.Selectors;

public enum SelectorPanelLayout
{
    Horizontal,
    Vertical,
    Wrap,
}

/// <summary>
/// Represents the action performed on a selection.
/// </summary>
public enum SelectionAction
{
    /// <summary>
    /// An item was added to the selection.
    /// </summary>
    Added,

    /// <summary>
    /// An item was removed from the selection.
    /// </summary>
    Removed
}

public interface ISelectorPanelVm : INotifyPropertyChanged
{
    bool IsSingleSelector { get; }
    SelectorPanelLayout Layout { get; set; }
    ThemeAccentBrush BorderBrush { get; set; }
}

/// <summary>
/// Special-purpose panel view model for selection controls.
/// Manages SelectableItemVm instances internally and provides a restricted API.
/// </summary>
/// <typeparam name="T">The type of value associated with each selectable item.</typeparam>
public abstract class SelectorPanelVm<T> : PanelVm, ISelectorPanelVm
{
    private readonly ObservableCollection<SelectableItemVm<T>> _items = new();
    private readonly ReadOnlyObservableCollection<SelectableItemVm<T>> _readonlyItems;
    private SelectorPanelLayout _layout = SelectorPanelLayout.Horizontal;
    private ThemeAccentBrush _borderBrush = ThemeAccentBrush.Default;

    public abstract bool IsSingleSelector { get; }

    protected SelectorPanelVm()
    {
        _readonlyItems = new ReadOnlyObservableCollection<SelectableItemVm<T>>(_items);
    }

    public ReadOnlyObservableCollection<SelectableItemVm<T>> Items => _readonlyItems;

    public SelectorPanelLayout Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }

    public ThemeAccentBrush BorderBrush
    {
        get => _borderBrush;
        set => SetProperty(ref _borderBrush, value);
    }

    public SelectorPanelVm<T> Add(string label, T item, ThemeAccentBrush borderBrush = ThemeAccentBrush.Default)
    {
        foreach (var existingItem in _items)
        {
            if (EqualityComparer<T>.Default.Equals(existingItem.Value, item))
            {
                throw new InvalidOperationException($"{item} already exists in the selector panel.");
            }
        }

        var itemVm = new SelectableItemVm<T>(item) { Label = label };
        itemVm.BorderBrush = borderBrush;
        itemVm.Clicked += OnSelectableItemClicked;
        _items.Add(itemVm);
        return this;
    }

    public void Remove(T item)
    {
        var itemVm = _items.FirstOrDefault(x =>
            EqualityComparer<T>.Default.Equals(x.Value, item));

        if (itemVm != null)
        {
            itemVm.Clicked -= OnSelectableItemClicked;
            _items.Remove(itemVm);
        }
    }

    protected abstract void OnSelectableItemClicked(object? sender, SelectableItemClickedEventArgs<T> e);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var item in _items)
            {
                item.Clicked -= OnSelectableItemClicked;
                item.Dispose();
            }
        }
        base.Dispose(disposing);
    }
}

public abstract class SelectorExPanelVm<T> : ContentsPanelVm<SelectableItemVm<T>>, ISelectorPanelVm
{
    private SelectorPanelLayout _layout = SelectorPanelLayout.Horizontal;
    private ThemeAccentBrush _borderBrush = ThemeAccentBrush.Default;
    public abstract bool IsSingleSelector { get; }
    public SelectorPanelLayout Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }
    public ThemeAccentBrush BorderBrush
    {
        get => _borderBrush;
        set => SetProperty(ref _borderBrush, value);
    }
}
