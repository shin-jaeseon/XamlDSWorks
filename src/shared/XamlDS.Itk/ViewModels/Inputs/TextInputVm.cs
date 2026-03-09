using XamlDS.Itk.Formatters;

namespace XamlDS.Itk.ViewModels.Inputs;

/// <summary>
/// Text input view model with validation and error handling for string values.
/// </summary>
public class TextInputVm : InputVm
{
    private string? _value;
    private string? _originalValue;
    private IValueFormatter<string> _formatter;
    private string _placeholder = string.Empty;
    private Func<string?, bool>? _validator;
    private Func<string?, string>? _errorMessageProvider;

    public TextInputVm(string name, IValueFormatter<string>? formatter = null) : base(name)
    {
        _formatter = formatter ?? DefaultFormatter<string>.Instance;
    }

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public string? Value
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
    public string? OriginalValue => _originalValue;

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
    public IValueFormatter<string> Formatter
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
    /// Gets or sets the validation function.
    /// </summary>
    public Func<string?, bool>? Validator
    {
        get => _validator;
        set
        {
            _validator = value;
            Validate();
        }
    }

    /// <summary>
    /// Gets or sets the error message provider function.
    /// </summary>
    public Func<string?, string>? ErrorMessageProvider
    {
        get => _errorMessageProvider;
        set
        {
            _errorMessageProvider = value;
            Validate();
        }
    }

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
        if (_validator == null)
        {
            HasError = false;
            ErrorMessage = null;
            return true;
        }

        bool isValid = _validator(_value);
        HasError = !isValid;

        if (!isValid && _errorMessageProvider != null)
        {
            ErrorMessage = _errorMessageProvider(_value);
        }
        else if (!isValid)
        {
            ErrorMessage = "Invalid value";
        }
        else
        {
            ErrorMessage = null;
        }

        return isValid;
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
    public void Initialize(string? value)
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
        IsDirty = !EqualityComparer<string?>.Default.Equals(_value, _originalValue);
    }
}
