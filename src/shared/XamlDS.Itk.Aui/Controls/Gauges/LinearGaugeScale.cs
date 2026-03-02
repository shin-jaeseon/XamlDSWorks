using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;
using System.Globalization;

namespace XamlDS.Itk.Controls.Gauges;

/// <summary>
/// A control that renders scale markings (ticks and labels) for a linear gauge.
/// Displays tick marks and numeric labels based on Min, Max, and Step values.
/// </summary>
public class LinearGaugeScale : Control
{
    private const double _minimumWidth = 128.0;
    private const double _minimumHeight = 128.0;
    private const double _minimumTickSpacing = 16.0;

    #region Styled Properties

    /// <summary>
    /// Defines the MinValue property.
    /// </summary>
    public static readonly StyledProperty<double> MinValueProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(MinValue), 0.0);

    /// <summary>
    /// Defines the MaxValue property.
    /// </summary>
    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(MaxValue), 100.0);

    /// <summary>
    /// Defines the Step property.
    /// </summary>
    public static readonly StyledProperty<double> StepProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(Step), 10.0);

    /// <summary>
    /// Defines the TickLength property.
    /// </summary>
    public static readonly StyledProperty<double> TickLengthProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(TickLength), 10.0);

    /// <summary>
    /// Defines the TickBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> TickBrushProperty =
        AvaloniaProperty.Register<LinearGaugeScale, IBrush?>(nameof(TickBrush), Brushes.Black);

    /// <summary>
    /// Defines the TickThickness property.
    /// </summary>
    public static readonly StyledProperty<double> TickThicknessProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(TickThickness), 1.0);

    /// <summary>
    /// Defines the TextBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> TextBrushProperty =
        AvaloniaProperty.Register<LinearGaugeScale, IBrush?>(nameof(TextBrush), Brushes.Black);

    /// <summary>
    /// Defines the NumberFormat property.
    /// </summary>
    public static readonly StyledProperty<string> NumberFormatProperty =
        AvaloniaProperty.Register<LinearGaugeScale, string>(nameof(NumberFormat), "0.##");

    /// <summary>
    /// Defines the ValueConverter property.
    /// </summary>
    public static readonly StyledProperty<IValueConverter?> ValueConverterProperty =
        AvaloniaProperty.Register<LinearGaugeScale, IValueConverter?>(nameof(ValueConverter));

    /// <summary>
    /// Defines the Orientation property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<LinearGaugeScale, Orientation>(nameof(Orientation), Orientation.Horizontal);

    /// <summary>
    /// Defines the TickOffset property.
    /// </summary>
    public static readonly StyledProperty<double> TickOffsetProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(TickOffset), 0.0);

    /// <summary>
    /// Defines the LabelOffset property.
    /// </summary>
    public static readonly StyledProperty<double> LabelOffsetProperty =
        AvaloniaProperty.Register<LinearGaugeScale, double>(nameof(LabelOffset), 5.0);

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the minimum value of the scale range.
    /// </summary>
    public double MinValue
    {
        get => GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum value of the scale range.
    /// </summary>
    public double MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the step interval for tick marks.
    /// </summary>
    public double Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
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
    /// Gets or sets the brush for tick marks.
    /// </summary>
    public IBrush? TickBrush
    {
        get => GetValue(TickBrushProperty);
        set => SetValue(TickBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of tick marks.
    /// </summary>
    public double TickThickness
    {
        get => GetValue(TickThicknessProperty);
        set => SetValue(TickThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush for text labels.
    /// </summary>
    public IBrush? TextBrush
    {
        get => GetValue(TextBrushProperty);
        set => SetValue(TextBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string for numeric labels (e.g., "0.##", "F2", "N0").
    /// </summary>
    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets a custom value converter for label text.
    /// If provided, overrides NumberFormat.
    /// </summary>
    public IValueConverter? ValueConverter
    {
        get => GetValue(ValueConverterProperty);
        set => SetValue(ValueConverterProperty, value);
    }

    /// <summary>
    /// Gets or sets the orientation of the scale (Horizontal or Vertical).
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the offset from the gauge bar to the start of tick marks.
    /// </summary>
    public double TickOffset
    {
        get => GetValue(TickOffsetProperty);
        set => SetValue(TickOffsetProperty, value);
    }

    /// <summary>
    /// Gets or sets the offset from the end of tick marks to the label text.
    /// </summary>
    public double LabelOffset
    {
        get => GetValue(LabelOffsetProperty);
        set => SetValue(LabelOffsetProperty, value);
    }

    #endregion

    static LinearGaugeScale()
    {
        AffectsRender<LinearGaugeScale>(
            MinValueProperty,
            MaxValueProperty,
            StepProperty,
            TickLengthProperty,
            TickBrushProperty,
            TickThicknessProperty,
            TextBrushProperty,
            NumberFormatProperty,
            ValueConverterProperty,
            OrientationProperty,
            TickOffsetProperty,
            LabelOffsetProperty);

        AffectsMeasure<LinearGaugeScale>(
            OrientationProperty,
            TickLengthProperty,
            TickOffsetProperty,
            LabelOffsetProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        const double estimatedTextHeight = 20;
        const double estimatedTextWidth = 32;

        var range = MaxValue - MinValue;
        if (range <= 0 || Step <= 0)
            return new Size(_minimumWidth, _minimumHeight);

        if (Orientation == Orientation.Horizontal)
        {
            // Calculate required height
            var height = TickOffset + TickLength + LabelOffset + estimatedTextHeight;

            // Calculate required width based on number of ticks and estimated label width
            var tickCount = Math.Floor(range / Step) + 1;
            var minWidth = Math.Max(_minimumWidth, tickCount * _minimumTickSpacing); // Overlap consideration
            var desiredWidth = Math.Min(availableSize.Width, minWidth);

            return new Size(desiredWidth, height);
        }
        else
        {
            // Calculate required width
            var width = TickOffset + TickLength + LabelOffset + estimatedTextWidth;

            // Calculate required height based on number of ticks and spacing
            var tickCount = Math.Floor(range / Step) + 1;
            var minHeight = Math.Max(_minimumHeight, tickCount * _minimumTickSpacing); // Spacing consideration
            var desiredHeight = Math.Min(availableSize.Height, minHeight);

            return new Size(width, desiredHeight);
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (TickBrush == null || TextBrush == null)
            return;

        var bounds = Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        var range = MaxValue - MinValue;
        if (range <= 0 || Step <= 0)
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
        var tickPen = new Pen(TickBrush, TickThickness);
        var typeface = new Typeface(FontFamily.Default);
        var fontSize = 12; // FontSize;

        for (double value = MinValue; value <= MaxValue; value += Step)
        {
            var ratio = (value - MinValue) / range;
            var x = bounds.X + bounds.Width * ratio;

            // Draw tick mark
            var tickStart = new Point(x, bounds.Y + TickOffset);
            var tickEnd = new Point(x, bounds.Y + TickOffset + TickLength);
            context.DrawLine(tickPen, tickStart, tickEnd);

            // Draw text label
            var labelText = FormatValue(value);
            var formattedText = new FormattedText(
                labelText,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                TextBrush);

            var textX = x - formattedText.Width / 2;
            var textY = tickEnd.Y + LabelOffset;
            context.DrawText(formattedText, new Point(textX, textY));
        }
    }

    private void RenderVertical(DrawingContext context, Rect bounds, double range)
    {
        var tickPen = new Pen(TickBrush, TickThickness);
        var typeface = new Typeface(FontFamily.Default);
        var fontSize = 12; // FontSize;

        for (double value = MinValue; value <= MaxValue; value += Step)
        {
            var ratio = (value - MinValue) / range;
            // For vertical orientation, lower values are at the bottom
            var y = bounds.Y + bounds.Height - (bounds.Height * ratio);

            // Draw tick mark
            var tickStart = new Point(bounds.X + TickOffset, y);
            var tickEnd = new Point(bounds.X + TickOffset + TickLength, y);
            context.DrawLine(tickPen, tickStart, tickEnd);

            // Draw text label
            var labelText = FormatValue(value);
            var formattedText = new FormattedText(
                labelText,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                TextBrush);

            var textX = tickEnd.X + LabelOffset;
            var textY = y - formattedText.Height / 2;
            context.DrawText(formattedText, new Point(textX, textY));
        }
    }

    private string FormatValue(double value)
    {
        // Use custom converter if provided
        if (ValueConverter != null)
        {
            var converted = ValueConverter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);
            return converted?.ToString() ?? value.ToString(NumberFormat);
        }

        // Use number format
        return value.ToString(NumberFormat);
    }
}
