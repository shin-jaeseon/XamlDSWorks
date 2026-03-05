using System.Numerics;
using XamlDS.Itk.Formatters;

namespace XamlDS.Itk.ViewModels.Fields;

public enum NumericFieldStatus
{
    Normal,
    Warning,
    Error,
    Unavailable,
}

public class NumericFieldVm<T> : FieldVm where T : INumber<T>, IComparable<T>, IEquatable<T>
{
    private string _prefix = string.Empty;
    private string _suffix = string.Empty;
    private T _value = default!;
    private NumericFieldStatus _status = NumericFieldStatus.Normal;
    private IValueFormatter<T> _formatter;

    public NumericFieldVm(string name, IValueFormatter<T>? formatter = null) : base(name)
    {
        _formatter = formatter ?? DefaultNumericFormatter<T>.Instance;
    }

    public string Prefix
    {
        get => _prefix;
        set => SetProperty(ref _prefix, value);
    }

    public string Suffix
    {
        get => _suffix;
        set => SetProperty(ref _suffix, value);
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

    public NumericFieldStatus Status
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
