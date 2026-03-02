using Avalonia;
using Avalonia.Controls;

namespace AvaloniaControlFactory.Controls;

public abstract class GaugeBase : Control
{
    #region Styled Properties

    /// <summary>
    /// Defines the MinValue property.
    /// </summary>
    public static readonly StyledProperty<double> MinValueProperty =
        AvaloniaProperty.Register<GaugeBase, double>(nameof(MinValue), 0.0);

    /// <summary>
    /// Defines the MaxValue property.
    /// </summary>
    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<GaugeBase, double>(nameof(MaxValue), 100.0);

    /// <summary>
    /// Defines the CurrentValue property.
    /// </summary>
    public static readonly StyledProperty<double> CurrentValueProperty =
        AvaloniaProperty.Register<GaugeBase, double>(nameof(CurrentValue), 0.0);

    /// <summary>
    /// Defines the Step property.
    /// </summary>
    public static readonly StyledProperty<double> StepProperty =
        AvaloniaProperty.Register<GaugeBase, double>(nameof(Step), 10.0);

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the minimum value of the gauge.
    /// </summary>
    public double MinValue
    {
        get => GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum value of the gauge.
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

    static GaugeBase()
    {
        AffectsRender<LinearGauge>(
            MinValueProperty,
            MaxValueProperty,
            CurrentValueProperty,
            StepProperty);
    }
}
