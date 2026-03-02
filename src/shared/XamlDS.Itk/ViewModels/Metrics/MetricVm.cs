using XamlDS.ViewModels;

namespace XamlDS.Itk.ViewModels.Metrics;

public enum MetricStatus
{
    Normal,
    Warning,
    Critical,
    NoData
}

public enum AlarmBehavior
{
    Silent,    // View: 색상만 변경
    Visual,    // View: 색상 + 깜빡임 or 아이콘
    Audible    // View: 색상 + 애니메이션 + 사운드 (플랫폼별 구현)
}

public class MetricVm : ViewModelBase
{
    private string _label = string.Empty;
    private string _unit = string.Empty;
    private double _min;
    private double _max;
    private double _value;
    private double _step;
    private MetricStatus _status = MetricStatus.NoData;
    private DateTime _timestamp = DateTime.MinValue;
    private double? _lowWarningThreshold = null;
    private double? _highWarningThreshold = null;
    private double? _lowCriticalThreshold = null;
    private double? _highCriticalThreshold = null;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public MetricStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set => SetProperty(ref _timestamp, value);
    }

    public string Unit
    {
        get => _unit;
        set => SetProperty(ref _unit, value);
    }

    public double Min
    {
        get => _min;
        set => SetProperty(ref _min, value);
    }

    public double Max
    {
        get => _max;
        set => SetProperty(ref _max, value);
    }

    public double Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public double Step
    {
        get => _step;
        set => SetProperty(ref _step, value);
    }

    public double? LowWarningThreshold
    {
        get => _lowWarningThreshold;
        set => SetProperty(ref _lowWarningThreshold, value);
    }

    public double? HighWarningThreshold
    {
        get => _highWarningThreshold;
        set => SetProperty(ref _highWarningThreshold, value);
    }

    public double? LowCriticalThreshold
    {
        get => _lowCriticalThreshold;
        set => SetProperty(ref _lowCriticalThreshold, value);
    }

    public double? HighCriticalThreshold
    {
        get => _highCriticalThreshold;
        set => SetProperty(ref _highCriticalThreshold, value);
    }

    public Dictionary<string, string> Tags { get; } = new Dictionary<string, string>();
}
