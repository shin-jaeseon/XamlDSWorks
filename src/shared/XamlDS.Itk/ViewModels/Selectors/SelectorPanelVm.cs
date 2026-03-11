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
    ThemeAccentColor BorderBrush { get; set; }
}


public abstract class SelectorPanelVm<T> : PanelVm<SelectableItemVm<T>>, ISelectorPanelVm
{
    private SelectorPanelLayout _layout = SelectorPanelLayout.Horizontal;
    private ThemeAccentColor _borderBrush = ThemeAccentColor.Default;
    public abstract bool IsSingleSelector { get; }

    public SelectorPanelLayout Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }

    public ThemeAccentColor BorderBrush
    {
        get => _borderBrush;
        set => SetProperty(ref _borderBrush, value);
    }

    public SelectorPanelVm<T> Add(string label, T item)
    {
        foreach (var child in Children)
        {
            if (EqualityComparer<T>.Default.Equals(child.Value, item))
            {
                throw new InvalidOperationException($"{item} already exists in the selector panel.");
            }
        }

        var itemVm = new SelectableItemVm<T>(item) { Label = label };
        itemVm.Clicked += OnSelectableItemClicked;
        AddChild(itemVm);
        return this;
    }

    protected abstract void OnSelectableItemClicked(object? sender, SelectableItemClickedEventArgs<T> e);
}
