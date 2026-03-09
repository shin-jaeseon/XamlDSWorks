using System.Numerics;
using XamlDS.Itk.Formatters;
using XamlDS.Itk.Units;

namespace XamlDS.Itk.ViewModels.Inputs;

/// <summary>
/// Numeric input view model with validation and error handling.
/// </summary>
/// <typeparam name="T">The numeric type to input.</typeparam>
public class NumericInputVm<T> : InputVm where T : INumber<T>, IComparable<T>, IEquatable<T>
{
    private readonly NumericUnit _unit;
    private T? _value;
    private T? _originalValue;
    private T? _minValue;
    private T? _maxValue;
    private IValueFormatter<T> _formatter;
    private string _prefix = string.Empty;
    private string _suffix = string.Empty;
    private string _placeholder = string.Empty;
    private Func<T?, bool>? _customValidator;
    private Func<T?, string>? _errorMessageProvider;

    public NumericInputVm(string name, NumericUnit unit, IValueFormatter<T>? formatter = null) : base(name)
    {
        _unit = unit;
        _formatter = formatter ?? DefaultNumericFormatter<T>.Instance;
    }

    /// <summary>
    /// Gets the unit of the numeric value.
    /// </summary>
    public NumericUnit Unit => _unit;

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public T? Value
    {
        get => _value;
        set
        {
            if (SetProperty(ref _value, value))
            {
                OnPropertyChanged(nameof(ValueString));
                UpdateDirtyState();
                Validate();
            }
        }
    }

    /// <summary>
    /// Gets the original value before any changes.
    /// </summary>
    public T? OriginalValue => _originalValue;

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    public T? MinValue
    {
        get => _minValue;
        set
        {
            if (SetProperty(ref _minValue, value))
            {
                Validate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    public T? MaxValue
    {
        get => _maxValue;
        set
        {
            if (SetProperty(ref _maxValue, value))
            {
                Validate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the prefix text.
    /// </summary>
    public string Prefix
    {
        get => _prefix;
        set => SetProperty(ref _prefix, value);
    }

    /// <summary>
    /// Gets or sets the suffix text.
    /// </summary>
    public string Suffix
    {
        get => _suffix;
        set => SetProperty(ref _suffix, value);
    }

    /// <summary>
    /// Gets or sets the placeholder text.
    /// </summary>
    public string Placeholder
    {
        get => _placeholder;
        set => SetProperty(ref _placeholder, value);
    }

    /// <summary>
    /// Gets or sets the formatter for displaying the value.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the custom validation function.
    /// </summary>
    public Func<T?, bool>? CustomValidator
    {
        get => _customValidator;
        set
        {
            _customValidator = value;
            Validate();
        }
    }

    /// <summary>
    /// Gets or sets the error message provider function.
    /// </summary>
    public Func<T?, string>? ErrorMessageProvider
    {
        get => _errorMessageProvider;
        set
        {
            _errorMessageProvider = value;
            Validate();
        }
    }

    /// <summary>
    /// Gets the unit string.
    /// </summary>
    public string UnitString => _unit.ToUnitString();

    /// <summary>
    /// Gets the formatted string representation of the value.
    /// </summary>
    public string ValueString => _value != null ? _formatter.Format(_value) : string.Empty;

    /// <summary>
    /// Validates the current value.
    /// </summary>
    /// <returns>True if valid, false otherwise.</returns>
    public override bool Validate()
    {
        if (_value == null)
        {
            HasError = false;
            ErrorMessage = null;
            return true;
        }

        // Range validation
        if (_minValue != null && _value.CompareTo(_minValue) < 0)
        {
            HasError = true;
            ErrorMessage = _errorMessageProvider?.Invoke(_value) 
                ?? $"Value must be greater than or equal to {_minValue}";
            return false;
        }

        if (_maxValue != null && _value.CompareTo(_maxValue) > 0)
        {
            HasError = true;
            ErrorMessage = _errorMessageProvider?.Invoke(_value) 
                ?? $"Value must be less than or equal to {_maxValue}";
            return false;
        }

        // Custom validation
        if (_customValidator != null && !_customValidator(_value))
        {
            HasError = true;
            ErrorMessage = _errorMessageProvider?.Invoke(_value) ?? "Invalid value";
            return false;
        }

        HasError = false;
        ErrorMessage = null;
        return true;
    }

    /// <summary>
    /// Resets the value to the original value.
    /// </summary>
    public override void Reset()
    {
        Value = _originalValue;
        IsDirty = false;
        HasError = false;
        ErrorMessage = null;
    }

    /// <summary>
    /// Commits the current value as the original value.
    /// </summary>
    public void Commit()
    {
        _originalValue = _value;
        IsDirty = false;
    }

    /// <summary>
    /// Initializes the input with a value.
    /// </summary>
    /// <param name="value">The initial value.</param>
    public void Initialize(T? value)
    {
        _originalValue = value;
        _value = value;
        IsDirty = false;
        OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(ValueString));
        OnPropertyChanged(nameof(OriginalValue));
        Validate();
    }

    private void UpdateDirtyState()
    {
        IsDirty = _value != null && _originalValue != null 
            ? !_value.Equals(_originalValue) 
            : _value != _originalValue;
    }
}
