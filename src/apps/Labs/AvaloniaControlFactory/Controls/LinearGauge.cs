using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using System;

namespace AvaloniaControlFactory.Controls;

/// <summary>
/// A custom control that renders a linear gauge with scale markings using DrawingContext for optimal performance.
/// </summary>
public class LinearGauge : GaugeBase
{
    #region Styled Properties

    /// <summary>
    /// Defines the Orientation property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<LinearGauge, Orientation>(nameof(Orientation), Orientation.Horizontal);

    /// <summary>
    /// Defines the GaugeThickness property.
    /// </summary>
    public static readonly StyledProperty<double> GaugeThicknessProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(GaugeThickness), 8.0);

    /// <summary>
    /// Defines the TickLength property.
    /// </summary>
    public static readonly StyledProperty<double> TickLengthProperty =
        AvaloniaProperty.Register<LinearGauge, double>(nameof(TickLength), 10.0);

    /// <summary>
    /// Defines the GaugeBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> GaugeBrushProperty =
        AvaloniaProperty.Register<LinearGauge, IBrush?>(nameof(GaugeBrush), Brushes.LightGray);

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

    #region Properties

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

    /// <summary>
    /// Gets or sets the brush for the gauge background.
    /// </summary>
    public IBrush? GaugeBrush
    {
        get => GetValue(GaugeBrushProperty);
        set => SetValue(GaugeBrushProperty, value);
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
    /// Gets or sets the brush for tick marks and text.
    /// </summary>
    public IBrush? TickBrush
    {
        get => GetValue(TickBrushProperty);
        set => SetValue(TickBrushProperty, value);
    }

    #endregion

    static LinearGauge()
    {
        AffectsRender<LinearGauge>(
            OrientationProperty,
            GaugeThicknessProperty,
            TickLengthProperty,
            GaugeBrushProperty,
            ValueBrushProperty,
            TickBrushProperty);

        AffectsMeasure<LinearGauge>(
            OrientationProperty,
            GaugeThicknessProperty,
            TickLengthProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        const double textHeight = 20;
        const double padding = 5;

        if (Orientation == Orientation.Horizontal)
        {
            var height = GaugeThickness + TickLength + textHeight + padding * 2;
            return new Size(availableSize.Width, height);
        }
        else
        {
            var width = GaugeThickness + TickLength + 50 + padding * 2; // 50 for text width
            return new Size(width, availableSize.Height);
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (GaugeBrush == null || ValueBrush == null || TickBrush == null)
            return;

        var bounds = Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        if (Orientation == Orientation.Horizontal)
        {
            RenderHorizontalGauge(context, bounds);
        }
        else
        {
            RenderVerticalGauge(context, bounds);
        }
    }

    private void RenderHorizontalGauge(DrawingContext context, Rect bounds)
    {
        const double padding = 10;
        var gaugeY = padding;
        var gaugeWidth = bounds.Width - padding * 2;
        var gaugeHeight = GaugeThickness;

        // Draw gauge background
        var gaugeRect = new Rect(padding, gaugeY, gaugeWidth, gaugeHeight);
        context.DrawRectangle(GaugeBrush, null, gaugeRect);

        // Draw current value indicator
        var range = MaxValue - MinValue;
        if (range > 0)
        {
            var valueRatio = Math.Clamp((CurrentValue - MinValue) / range, 0, 1);
            var valueWidth = gaugeWidth * valueRatio;
            var valueRect = new Rect(padding, gaugeY, valueWidth, gaugeHeight);
            context.DrawRectangle(ValueBrush, null, valueRect);
        }

        // Draw ticks and labels
        if (Step > 0)
        {
            var tickPen = new Pen(TickBrush, 1);
            var typeface = new Typeface(FontFamily.Default);
            var textBrush = TickBrush;

            for (double value = MinValue; value <= MaxValue; value += Step)
            {
                var ratio = (value - MinValue) / (MaxValue - MinValue);
                var x = padding + gaugeWidth * ratio;

                // Draw tick mark
                var tickStart = new Point(x, gaugeY + gaugeHeight);
                var tickEnd = new Point(x, gaugeY + gaugeHeight + TickLength);
                context.DrawLine(tickPen, tickStart, tickEnd);

                // Draw text label
                var text = value.ToString("0.##");
                var formattedText = new FormattedText(
                    text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    12,
                    textBrush);

                var textX = x - formattedText.Width / 2;
                var textY = tickEnd.Y + 5;
                context.DrawText(formattedText, new Point(textX, textY));
            }
        }
    }

    private void RenderVerticalGauge(DrawingContext context, Rect bounds)
    {
        const double padding = 10;
        var gaugeX = padding;
        var gaugeHeight = bounds.Height - padding * 2;
        var gaugeWidth = GaugeThickness;

        // Draw gauge background
        var gaugeRect = new Rect(gaugeX, padding, gaugeWidth, gaugeHeight);
        context.DrawRectangle(GaugeBrush, null, gaugeRect);

        // Draw current value indicator (from bottom)
        var range = MaxValue - MinValue;
        if (range > 0)
        {
            var valueRatio = Math.Clamp((CurrentValue - MinValue) / range, 0, 1);
            var valueHeight = gaugeHeight * valueRatio;
            var valueY = padding + gaugeHeight - valueHeight;
            var valueRect = new Rect(gaugeX, valueY, gaugeWidth, valueHeight);
            context.DrawRectangle(ValueBrush, null, valueRect);
        }

        // Draw ticks and labels
        if (Step > 0)
        {
            var tickPen = new Pen(TickBrush, 1);
            var typeface = new Typeface(FontFamily.Default);
            var textBrush = TickBrush;

            for (double value = MinValue; value <= MaxValue; value += Step)
            {
                var ratio = (value - MinValue) / (MaxValue - MinValue);
                var y = padding + gaugeHeight - (gaugeHeight * ratio);

                // Draw tick mark
                var tickStart = new Point(gaugeX + gaugeWidth, y);
                var tickEnd = new Point(gaugeX + gaugeWidth + TickLength, y);
                context.DrawLine(tickPen, tickStart, tickEnd);

                // Draw text label
                var text = value.ToString("0.##");
                var formattedText = new FormattedText(
                    text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    12,
                    textBrush);

                var textX = tickEnd.X + 5;
                var textY = y - formattedText.Height / 2;
                context.DrawText(formattedText, new Point(textX, textY));
            }
        }
    }
}
