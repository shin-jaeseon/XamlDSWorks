using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using System;

namespace AvaloniaControlFactory.Controls;

/// <summary>
/// A control that renders threshold-based multi-color zones for a linear gauge.
/// Displays Normal, Warning, and Critical zones based on configured thresholds.
/// </summary>
public class LinearGaugeBar : Control
{
    #region Styled Properties

    /// <summary>
    /// Defines the MinValue property.
    /// </summary>
    public static readonly StyledProperty<double> MinValueProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double>(nameof(MinValue), 0.0);

    /// <summary>
    /// Defines the MaxValue property.
    /// </summary>
    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double>(nameof(MaxValue), 100.0);

    /// <summary>
    /// Defines the LowWarningThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> LowWarningThresholdProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double?>(nameof(LowWarningThreshold));

    /// <summary>
    /// Defines the HighWarningThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> HighWarningThresholdProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double?>(nameof(HighWarningThreshold));

    /// <summary>
    /// Defines the LowCriticalThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> LowCriticalThresholdProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double?>(nameof(LowCriticalThreshold));

    /// <summary>
    /// Defines the HighCriticalThreshold property.
    /// </summary>
    public static readonly StyledProperty<double?> HighCriticalThresholdProperty =
        AvaloniaProperty.Register<LinearGaugeBar, double?>(nameof(HighCriticalThreshold));

    /// <summary>
    /// Defines the NormalBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> NormalBrushProperty =
        AvaloniaProperty.Register<LinearGaugeBar, IBrush?>(nameof(NormalBrush), Brushes.Green);

    /// <summary>
    /// Defines the WarningBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> WarningBrushProperty =
        AvaloniaProperty.Register<LinearGaugeBar, IBrush?>(nameof(WarningBrush), Brushes.Yellow);

    /// <summary>
    /// Defines the CriticalBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CriticalBrushProperty =
        AvaloniaProperty.Register<LinearGaugeBar, IBrush?>(nameof(CriticalBrush), Brushes.Red);

    /// <summary>
    /// Defines the Orientation property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<LinearGaugeBar, Orientation>(nameof(Orientation), Orientation.Horizontal);

    #endregion

    #region Properties

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
    /// Gets or sets the orientation of the gauge bar (Horizontal or Vertical).
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    #endregion

    static LinearGaugeBar()
    {
        AffectsRender<LinearGaugeBar>(
            MinValueProperty,
            MaxValueProperty,
            LowWarningThresholdProperty,
            HighWarningThresholdProperty,
            LowCriticalThresholdProperty,
            HighCriticalThresholdProperty,
            NormalBrushProperty,
            WarningBrushProperty,
            CriticalBrushProperty,
            OrientationProperty);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        var range = MaxValue - MinValue;
        if (range <= 0)
            return;

        if (Orientation == Orientation.Horizontal)
        {
            RenderHorizontal(context, bounds, range);
        }
        else
        {
            RenderVertical(context, bounds, range);
        }
    }

    private void RenderHorizontal(DrawingContext context, Rect bounds, double range)
    {
        // Draw base (normal zone) first
        context.DrawRectangle(NormalBrush, null, bounds);

        // Draw low critical zone
        if (LowCriticalThreshold.HasValue)
        {
            var width = ((LowCriticalThreshold.Value - MinValue) / range) * bounds.Width;
            if (width > 0)
            {
                var rect = new Rect(bounds.X, bounds.Y, width, bounds.Height);
                context.DrawRectangle(CriticalBrush, null, rect);
            }
        }

        // Draw low warning zone
        if (LowWarningThreshold.HasValue)
        {
            var start = LowCriticalThreshold ?? MinValue;
            var startX = ((start - MinValue) / range) * bounds.Width;
            var width = ((LowWarningThreshold.Value - start) / range) * bounds.Width;
            if (width > 0)
            {
                var rect = new Rect(bounds.X + startX, bounds.Y, width, bounds.Height);
                context.DrawRectangle(WarningBrush, null, rect);
            }
        }

        // Draw high warning zone
        if (HighWarningThreshold.HasValue)
        {
            var startX = ((HighWarningThreshold.Value - MinValue) / range) * bounds.Width;
            var end = HighCriticalThreshold ?? MaxValue;
            var width = ((end - HighWarningThreshold.Value) / range) * bounds.Width;
            if (width > 0)
            {
                var rect = new Rect(bounds.X + startX, bounds.Y, width, bounds.Height);
                context.DrawRectangle(WarningBrush, null, rect);
            }
        }

        // Draw high critical zone
        if (HighCriticalThreshold.HasValue)
        {
            var startX = ((HighCriticalThreshold.Value - MinValue) / range) * bounds.Width;
            var width = bounds.Width - startX;
            if (width > 0)
            {
                var rect = new Rect(bounds.X + startX, bounds.Y, width, bounds.Height);
                context.DrawRectangle(CriticalBrush, null, rect);
            }
        }
    }

    private void RenderVertical(DrawingContext context, Rect bounds, double range)
    {
        // Draw base (normal zone) first
        context.DrawRectangle(NormalBrush, null, bounds);

        // For vertical orientation, lower values are at the bottom
        // Draw low critical zone (bottom)
        if (LowCriticalThreshold.HasValue)
        {
            var height = ((LowCriticalThreshold.Value - MinValue) / range) * bounds.Height;
            if (height > 0)
            {
                var y = bounds.Y + bounds.Height - height;
                var rect = new Rect(bounds.X, y, bounds.Width, height);
                context.DrawRectangle(CriticalBrush, null, rect);
            }
        }

        // Draw low warning zone
        if (LowWarningThreshold.HasValue)
        {
            var start = LowCriticalThreshold ?? MinValue;
            var startRatio = (start - MinValue) / range;
            var endRatio = (LowWarningThreshold.Value - MinValue) / range;
            var height = (endRatio - startRatio) * bounds.Height;
            if (height > 0)
            {
                var y = bounds.Y + bounds.Height - endRatio * bounds.Height;
                var rect = new Rect(bounds.X, y, bounds.Width, height);
                context.DrawRectangle(WarningBrush, null, rect);
            }
        }

        // Draw high warning zone
        if (HighWarningThreshold.HasValue)
        {
            var end = HighCriticalThreshold ?? MaxValue;
            var startRatio = (HighWarningThreshold.Value - MinValue) / range;
            var endRatio = (end - MinValue) / range;
            var height = (endRatio - startRatio) * bounds.Height;
            if (height > 0)
            {
                var y = bounds.Y + bounds.Height - endRatio * bounds.Height;
                var rect = new Rect(bounds.X, y, bounds.Width, height);
                context.DrawRectangle(WarningBrush, null, rect);
            }
        }

        // Draw high critical zone (top)
        if (HighCriticalThreshold.HasValue)
        {
            var startRatio = (HighCriticalThreshold.Value - MinValue) / range;
            var height = (1.0 - startRatio) * bounds.Height;
            if (height > 0)
            {
                var rect = new Rect(bounds.X, bounds.Y, bounds.Width, height);
                context.DrawRectangle(CriticalBrush, null, rect);
            }
        }
    }
}
