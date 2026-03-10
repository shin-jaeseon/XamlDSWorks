using XamlDS.Itk.Themes;

namespace XamlDS.Itk.ViewModels.Selectors;

public class SelectableItemVm<T> : ViewModelBase
{
    private string _label = string.Empty;
    private readonly T _value;
    private bool _selected;
    private ThemeAccentColor _borderBrush = ThemeAccentColor.Default;

    public SelectableItemVm(T value)
    {
        _value = value;
    }

    public T Value => _value;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public bool Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ThemeAccentColor BorderBrush
    {
        get => _borderBrush;
        set => SetProperty(ref _borderBrush, value);
    }
}
