using XamlDS.Itk.Themes;

namespace XamlDS.Itk.ViewModels.Selectors;

public enum SelectorLayout
{
    Horizontal,
    Vertical,
    Wrapping,
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

public abstract class SelectorPanelVm<T> : PanelVm<SelectableItemVm<T>>
{
    private SelectorLayout _layout = SelectorLayout.Horizontal;
    private ThemeAccentColor _borderBrush = ThemeAccentColor.Default;

    public SelectorLayout Layout
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
        AddChild(itemVm);
        return this;
    }
}
