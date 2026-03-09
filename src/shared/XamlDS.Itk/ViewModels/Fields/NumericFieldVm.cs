using System.Numerics;
using XamlDS.Itk.Formatters;
using XamlDS.Itk.Units;

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
    private readonly NumericUnit _unit;
    private string _prefix = string.Empty;
    private string _suffix = string.Empty;
    private T _value = default!;
    private NumericFieldStatus _status = NumericFieldStatus.Normal;
    private IValueFormatter<T> _formatter;

    public NumericFieldVm(string name, NumericUnit? unit = null, IValueFormatter<T>? formatter = null) : base(name)
    {
        _unit = unit ?? NumericUnit.None;
        _formatter = formatter ?? DefaultNumericFormatter<T>.Instance;
    }

    public NumericUnit Unit => _unit;

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

    public string UnitString => _unit.ToUnitString();

    public string ValueString => _formatter.Format(_value);
}
