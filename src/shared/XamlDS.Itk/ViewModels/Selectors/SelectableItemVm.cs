using System.ComponentModel;
using XamlDS.Itk.Themes;

namespace XamlDS.Itk.ViewModels.Selectors;

public interface ISelectableItem
{
    event PropertyChangedEventHandler? PropertyChanged;

    void OnClicked();

    bool Selected { get; set; }

    ThemeAccentColor BorderBrush { get; set; }
}

public class SelectableItemClickedEventArgs<T> : EventArgs
{
    public SelectableItemClickedEventArgs(T value)
    {
        Value = value;
    }

    public T Value { get; }
}

public class SelectableItemVm<T> : ViewModelBase, ISelectableItem
{
    private string _label = string.Empty;
    private readonly T _value;
    private bool _selected;
    private ThemeAccentColor _borderBrush = ThemeAccentColor.Default;

    public event EventHandler<SelectableItemClickedEventArgs<T>>? Clicked;

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

    public void OnClicked()
    {
        Clicked?.Invoke(this, new SelectableItemClickedEventArgs<T>(_value));
    }
}
