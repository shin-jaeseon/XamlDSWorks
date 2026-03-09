using XamlDS.Itk.Formatters;
using XamlDS.Itk.Operators;
using XamlDS.Itk.Units;

namespace XamlDS.Itk.ViewModels.Gauges;

public enum GaugeStatus
{
    Normal,
    Warning,
    Critical,
    Error,
    Unavailable
}

public class GaugeVm : ViewModelBase
{
    private readonly string _name;
    private readonly NumericUnit _unit;
    private string _label = string.Empty;
    private double _value;
    private IValueFormatter<double> _formatter;
    private IPrecisionOperator _precisionOperator;
    private double _minValue = 0;
    private double _maxValue = 100;
    private double? _thresholdLowWarning = null;
    private double? _thresholdHighWarning = null;
    private double? _thresholdLowCritical = null;
    private double? _thresholdHighCritical = null;
    private bool _isEnabled = true;
    private double? _nominalValue = null;
    private bool _showNominalMarker = true;

    public GaugeVm(string name, NumericUnit unit, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        _name = name;
        _unit = unit;
        _formatter = formatter ?? DefaultFormatter<double>.Instance;
        _precisionOperator = precisionOperator ?? IdentityOperator.Instance;
    }

    public string Name => _name;

    public NumericUnit Unit => _unit;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public string ValueString => _formatter.Format(_value);

    public IPrecisionOperator PrecisionOperator => _precisionOperator;

    public IValueFormatter<double> Formatter
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

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (SetProperty(ref _isEnabled, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public bool IsEnabledLowWarning => ThresholdLowWarning.HasValue;

    public bool IsEnabledHighWarning => ThresholdHighWarning.HasValue;

    public bool IsEnabledLowCritical => ThresholdLowCritical.HasValue;

    public bool IsEnabledHighCritical => ThresholdHighCritical.HasValue;


    public GaugeStatus Status
    {
        get
        {
            if (!_isEnabled)
                return GaugeStatus.Unavailable;

            if (Value < MinValue || Value > MaxValue)
                return GaugeStatus.Error;

            if ((ThresholdLowCritical.HasValue && Value < ThresholdLowCritical.Value) ||
                (ThresholdHighCritical.HasValue && Value > ThresholdHighCritical.Value))
                return GaugeStatus.Critical;

            if ((ThresholdLowWarning.HasValue && Value < ThresholdLowWarning.Value) ||
                (ThresholdHighWarning.HasValue && Value > ThresholdHighWarning.Value))
                return GaugeStatus.Warning;

            return GaugeStatus.Normal;
        }
    }

    public double Value
    {
        get => _value;
        set
        {
            value = _precisionOperator.Apply(value);
            if (SetProperty(ref _value, value))
            {
                OnPropertyChanged(nameof(ValueString));
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public double MinValue
    {
        get => _minValue;
        set
        {
            value = _precisionOperator.Apply(value);

            if (value > MaxValue)
                throw new ArgumentException($"MinValue ({value}) cannot be greater than MaxValue ({MaxValue})");
            if (ThresholdLowCritical.HasValue && value > ThresholdLowCritical.Value)
                throw new ArgumentException($"MinValue ({value}) cannot be greater than ThresholdLowCritical ({ThresholdLowCritical.Value})");
            if (ThresholdLowWarning.HasValue && value > ThresholdLowWarning.Value)
                throw new ArgumentException($"MinValue ({value}) cannot be greater than ThresholdLowWarning ({ThresholdLowWarning.Value})");

            if (SetProperty(ref _minValue, value))
                OnPropertyChanged(nameof(Status));
        }
    }

    public double MaxValue
    {
        get => _maxValue;
        set
        {
            value = _precisionOperator.Apply(value);

            if (value < MinValue)
                throw new ArgumentException($"MaxValue ({value}) cannot be less than MinValue ({MinValue})");
            if (ThresholdHighCritical.HasValue && value < ThresholdHighCritical.Value)
                throw new ArgumentException($"MaxValue ({value}) cannot be less than ThresholdHighCritical ({ThresholdHighCritical.Value})");
            if (ThresholdHighWarning.HasValue && value < ThresholdHighWarning.Value)
                throw new ArgumentException($"MaxValue ({value}) cannot be less than ThresholdHighWarning ({ThresholdHighWarning.Value})");

            if (SetProperty(ref _maxValue, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public double? ThresholdLowWarning
    {
        get => _thresholdLowWarning;
        set
        {
            value = value.HasValue ? _precisionOperator.Apply(value.Value) : (double?)null;
            if (value.HasValue)
            {
                if (value.Value < MinValue || value.Value > MaxValue)
                    throw new ArgumentException($"ThresholdLowWarning ({value.Value}) must be between MinValue ({MinValue}) and MaxValue ({MaxValue})");
                if (ThresholdHighWarning.HasValue && value.Value > ThresholdHighWarning.Value)
                    throw new ArgumentException($"ThresholdLowWarning ({value.Value}) cannot be greater than ThresholdHighWarning ({ThresholdHighWarning.Value})");
                if (ThresholdLowCritical.HasValue && value.Value > ThresholdLowCritical.Value)
                    throw new ArgumentException($"ThresholdLowWarning ({value.Value}) cannot be greater than ThresholdLowCritical ({ThresholdLowCritical.Value})");
            }

            if (SetProperty(ref _thresholdLowWarning, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public double? ThresholdHighWarning
    {
        get => _thresholdHighWarning;
        set
        {
            value = value.HasValue ? _precisionOperator.Apply(value.Value) : (double?)null;
            if (value.HasValue)
            {
                if (value.Value < MinValue || value.Value > MaxValue)
                    throw new ArgumentException($"ThresholdHighWarning ({value.Value}) must be between MinValue ({MinValue}) and MaxValue ({MaxValue})");
                if (ThresholdLowWarning.HasValue && value.Value < ThresholdLowWarning.Value)
                    throw new ArgumentException($"ThresholdHighWarning ({value.Value}) cannot be less than ThresholdLowWarning ({ThresholdLowWarning.Value})");
                if (ThresholdHighCritical.HasValue && value.Value > ThresholdHighCritical.Value)
                    throw new ArgumentException($"ThresholdHighWarning ({value.Value}) cannot be greater than ThresholdHighCritical ({ThresholdHighCritical.Value})");
            }

            if (SetProperty(ref _thresholdHighWarning, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public double? ThresholdLowCritical
    {
        get => _thresholdLowCritical;
        set
        {
            value = value.HasValue ? _precisionOperator.Apply(value.Value) : (double?)null;
            if (value.HasValue)
            {
                if (value.Value < MinValue || value.Value > MaxValue)
                    throw new ArgumentException($"ThresholdLowCritical ({value.Value}) must be between MinValue ({MinValue}) and MaxValue ({MaxValue})");
                if (ThresholdLowWarning.HasValue && value.Value > ThresholdLowWarning.Value)
                    throw new ArgumentException($"ThresholdLowCritical ({value.Value}) cannot be greater than ThresholdLowWarning ({ThresholdLowWarning.Value})");
                if (ThresholdHighCritical.HasValue && value.Value > ThresholdHighCritical.Value)
                    throw new ArgumentException($"ThresholdLowCritical ({value.Value}) cannot be greater than ThresholdHighCritical ({ThresholdHighCritical.Value})");
            }

            if (SetProperty(ref _thresholdLowCritical, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public double? ThresholdHighCritical
    {
        get => _thresholdHighCritical;
        set
        {
            value = value.HasValue ? _precisionOperator.Apply(value.Value) : (double?)null;
            if (value.HasValue)
            {
                if (value.Value < MinValue || value.Value > MaxValue)
                    throw new ArgumentException($"ThresholdHighCritical ({value.Value}) must be between MinValue ({MinValue}) and MaxValue ({MaxValue})");
                if (ThresholdLowCritical.HasValue && value.Value < ThresholdLowCritical.Value)
                    throw new ArgumentException($"ThresholdHighCritical ({value.Value}) cannot be less than ThresholdLowCritical ({ThresholdLowCritical.Value})");
                if (ThresholdHighWarning.HasValue && value.Value < ThresholdHighWarning.Value)
                    throw new ArgumentException($"ThresholdHighCritical ({value.Value}) cannot be less than ThresholdHighWarning ({ThresholdHighWarning.Value})");
            }

            if (SetProperty(ref _thresholdHighCritical, value))
            {
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    /// <summary>
    /// Gets or sets the nominal (rated) value for this gauge.
    /// This represents the center/target value for normal operation.
    /// </summary>
    public double? NominalValue
    {
        get => _nominalValue;
        set
        {
            value = value.HasValue ? _precisionOperator.Apply(value.Value) : (double?)null;
            if (value.HasValue)
            {
                if (value.Value < MinValue || value.Value > MaxValue)
                    throw new ArgumentException($"NominalValue ({value.Value}) must be between MinValue ({MinValue}) and MaxValue ({MaxValue})");
            }

            SetProperty(ref _nominalValue, value);
        }
    }

    /// <summary>
    /// Gets or sets whether to show the nominal value marker on the gauge.
    /// Default is true when NominalValue is set.
    /// </summary>
    public bool ShowNominalMarker
    {
        get => _showNominalMarker;
        set => SetProperty(ref _showNominalMarker, value);
    }
}
