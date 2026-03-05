using XamlDS.Itk.Formatters;

namespace XamlDS.Itk.ViewModels.Fields;

public enum TextFieldStatus
{
    Normal,
    Warning,
    Error,
    Idle,          // Inactive or standby state
    Disconnected   // Communication lost or data unavailable
}

public class TextFieldVm<T> : FieldVm
{
    private T _value = default!;
    private TextFieldStatus _status = TextFieldStatus.Disconnected;
    private IValueFormatter<T> _formatter;

    public TextFieldVm(string name, IValueFormatter<T>? formatter = null) : base(name)
    {
        _formatter = formatter ?? DefaultFormatter<T>.Instance;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (SetProperty(ref _value, value))
            {
                OnPropertyChanged(nameof(ValueString));
            }
        }
    }

    public TextFieldStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public IValueFormatter<T> Formatter
    {
        get => _formatter;
        set
        {
            if (SetProperty(ref _formatter, value))
            {
                OnPropertyChanged(nameof(ValueString));
            }
        }
    }

    public string ValueString => _formatter.Format(_value);
}
