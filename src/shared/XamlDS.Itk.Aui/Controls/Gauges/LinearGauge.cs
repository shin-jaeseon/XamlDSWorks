using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace XamlDS.Itk.Controls;

/// <summary>
/// A templated control that displays a linear gauge with threshold-based color zones and scale markings.
/// Designed for industrial monitoring systems to visualize metrics with normal, warning, and critical ranges.
/// </summary>
public class LinearGauge : TemplatedControl
{
    #region Styled Properties - Data

    /// <summary>
    /// Defines the MinValue property.
    /// </summary>
    public static readonly StyledProperty<double> MinValueProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(MinValue), 0.0);

    /// <summary>
    /// Defines the MaxValue property.
    /// </summary>
    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(MaxValue), 100.0);

    /// <summary>
    /// Defines the CurrentValue property.
    /// </summary>
    public static readonly StyledProperty<double> CurrentValueProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(CurrentValue), 0.0);

    /// <summary>
    /// Defines the Step property.
    /// </summary>
    public static readonly StyledProperty<double> StepProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(Step), 10.0);

    #endregion

    #region Styled Properties - Thresholds

    /// <summary>
    /// Defines the LowWarningThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> LowWarningThresholdProperty =
        AvaloniaProperty.Register<LinearGauge, double?>(nameof(LowWarningThreshold));

    /// <summary>
    /// Defines the HighWarningThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> HighWarningThresholdProperty =
        AvaloniaProperty.Register<LinearGauge, double?>(nameof(HighWarningThreshold));

    /// <summary>
    /// Defines the LowCriticalThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> LowCriticalThresholdProperty =
        AvaloniaProperty.Register<LinearGauge, double?>(nameof(LowCriticalThreshold));

    /// <summary>
    /// Defines the HighCriticalThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> HighCriticalThresholdProperty =
        AvaloniaProperty.Register<LinearGauge, double?>(nameof(HighCriticalThreshold));

    #endregion

    #region Styled Properties - Appearance

    /// <summary>
    /// Defines the Orientation property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<LinearGauge, Orientation>(nameof(Orientation), Orientation.Horizontal);

    /// <summary>
    /// Defines the GaugeThickness property.
    /// </summary>
    public static readonly StyledProperty<double> GaugeThicknessProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(GaugeThickness), 20.0);

    /// <summary>
    /// Defines the TickLength property.
    /// </summary>
    public static readonly StyledProperty<double> TickLengthProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(TickLength), 10.0);

    #endregion

    #region Styled Properties - Brushes

    /// <summary>
    /// Defines the NormalBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> NormalBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(NormalBrush), Brushes.Green);

    /// <summary>
    /// Defines the WarningBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> WarningBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(WarningBrush), Brushes.Yellow);

    /// <summary>
    /// Defines the CriticalBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CriticalBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(CriticalBrush), Brushes.Red);

    /// <summary>
    /// Defines the ValueBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> ValueBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(ValueBrush), Brushes.DodgerBlue);

    /// <summary>
    /// Defines the TickBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> TickBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(TickBrush), Brushes.Black);

    #endregion

    #region Styled Properties - Formatting

    /// <summary>
    /// Defines the NumberFormat property.
    /// </summary>
    public static readonly StyledProperty<string> NumberFormatProperty =
        AvaloniaProperty.Register<LinearGauge, string>(nameof(NumberFormat), "0.##");

    /// <summary>
    /// Defines the Label property.
    /// </summary>
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<LinearGauge, string?>(nameof(Label));

    /// <summary>
    /// Defines the Unit property.
    /// </summary>
    public static readonly StyledProperty<string?> UnitProperty =
        AvaloniaProperty.Register<LinearGauge, string?>(nameof(Unit));

    #endregion

    #region Properties - Data

    /// <summary>
    /// Gets or sets the minimum value of the gauge range.
    /// </summary>
    public double MinValue
    {
        get => GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum value of the gauge range.
    /// </summary>
    public double MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the current value displayed on the gauge.
    /// </summary>
    public double CurrentValue
    {
        get => GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the step interval for scale markings.
    /// </summary>
    public double Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    #endregion

    #region Properties - Thresholds

    /// <summary>
    /// Gets or sets the low warning threshold. Values below this are shown in warning color.
    /// </summary>
    public double? LowWarningThreshold
    {
        get => GetValue(LowWarningThresholdProperty);
        set => SetValue(LowWarningThresholdProperty, value);
    }

    /// <summary>
    /// Gets or sets the high warning threshold. Values above this are shown in warning color.
    /// </summary>
    public double? HighWarningThreshold
    {
        get => GetValue(HighWarningThresholdProperty);
        set => SetValue(HighWarningThresholdProperty, value);
    }

    /// <summary>
    /// Gets or sets the low critical threshold. Values below this are shown in critical color.
    /// </summary>
    public double? LowCriticalThreshold
    {
        get => GetValue(LowCriticalThresholdProperty);
        set => SetValue(LowCriticalThresholdProperty, value);
    }

    /// <summary>
    /// Gets or sets the high critical threshold. Values above this are shown in critical color.
    /// </summary>
    public double? HighCriticalThreshold
    {
        get => GetValue(HighCriticalThresholdProperty);
        set => SetValue(HighCriticalThresholdProperty, value);
    }

    #endregion

    #region Properties - Appearance

    /// <summary>
    /// Gets or sets the orientation of the gauge (Horizontal or Vertical).
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of the gauge bar.
    /// </summary>
    public double GaugeThickness
    {
        get => GetValue(GaugeThicknessProperty);
        set => SetValue(GaugeThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the length of tick marks.
    /// </summary>
    public double TickLength
    {
        get => GetValue(TickLengthProperty);
        set => SetValue(TickLengthProperty, value);
    }

    #endregion

    #region Properties - Brushes

    /// <summary>
    /// Gets or sets the brush for the normal zone.
    /// </summary>
    public IBrush? NormalBrush
    {
        get => GetValue(NormalBrushProperty);
        set => SetValue(NormalBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the warning zones.
    /// </summary>
    public IBrush? WarningBrush
    {
        get => GetValue(WarningBrushProperty);
        set => SetValue(WarningBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the critical zones.
    /// </summary>
    public IBrush? CriticalBrush
    {
        get => GetValue(CriticalBrushProperty);
        set => SetValue(CriticalBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for the current value indicator.
    /// </summary>
    public IBrush? ValueBrush
    {
        get => GetValue(ValueBrushProperty);
        set => SetValue(ValueBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for tick marks and labels.
    /// </summary>
    public IBrush? TickBrush
    {
        get => GetValue(TickBrushProperty);
        set => SetValue(TickBrushProperty, value);
    }

    #endregion

    #region Properties - Formatting

    /// <summary>
    /// Gets or sets the format string for numeric labels (e.g., "0.##", "F2", "N0").
    /// </summary>
    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the label text displayed with the gauge.
    /// </summary>
    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// Gets or sets the unit text (e.g., "°C", "MPa", "RPM").
    /// </summary>
    public string? Unit
    {
        get => GetValue(UnitProperty);
        set => SetValue(UnitProperty, value);
    }

    #endregion

    #region Internal Properties

    /// <summary>
    /// Gets the calculated width of the value indicator based on CurrentValue.
    /// Used for template binding in XAML.
    /// </summary>
    public double ValueIndicatorWidth
    {
        get
        {
            var range = MaxValue - MinValue;
            if (range <= 0) return 0;

            var ratio = Math.Clamp((CurrentValue - MinValue) / range, 0, 1);

            // This will be calculated based on actual bounds in the template
            // For now, return the ratio (0-1) which can be used with a converter
            return ratio;
        }
    }

    #endregion

    static LinearGauge()
    {
        // Update ValueIndicatorWidth when CurrentValue, MinValue, or MaxValue changes
        CurrentValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, double.NaN, gauge.ValueIndicatorWidth));

        MinValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, double.NaN, gauge.ValueIndicatorWidth));

        MaxValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, double.NaN, gauge.ValueIndicatorWidth));
    }

    /// <summary>
    /// Property for ValueIndicatorWidth to support change notification.
    /// </summary>
    private static readonly DirectProperty<LinearGauge, double> ValueIndicatorWidthProperty =
        AvaloniaProperty.RegisterDirect<LinearGauge, double>(
            nameof(ValueIndicatorWidth),
            o => o.ValueIndicatorWidth);
}
