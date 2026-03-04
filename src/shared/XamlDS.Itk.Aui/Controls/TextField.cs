using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System.Globalization;

namespace XamlDS.Itk.Controls;

/// <summary>
/// A custom control for displaying text values with labels, prefixes, and suffixes.
/// </summary>
public class TextField : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="Label"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Label));

    /// <summary>
    /// Defines the <see cref="Value"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<TextField, object?>(nameof(Value));

    /// <summary>
    /// Defines the <see cref="Prefix"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> PrefixProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Prefix));

    /// <summary>
    /// Defines the <see cref="Suffix"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> SuffixProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Suffix));

    /// <summary>
    /// Defines the <see cref="LabelWidth"/> property.
    /// </summary>
    public static readonly StyledProperty<double> LabelWidthProperty =
        AvaloniaProperty.Register<TextField, double>(nameof(LabelWidth), defaultValue: 80.0);

    /// <summary>
    /// Defines the <see cref="ValueWidth"/> property.
    /// </summary>
    public static readonly StyledProperty<double> ValueWidthProperty =
        AvaloniaProperty.Register<TextField, double>(nameof(ValueWidth), defaultValue: 100.0);

    public static readonly StyledProperty<double> SuffixWidthProperty =
        AvaloniaProperty.Register<TextField, double>(nameof(SuffixWidth), defaultValue: 50.0);

    /// <summary>
    /// Defines the <see cref="ShowLabel"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowLabelProperty =
        AvaloniaProperty.Register<TextField, bool>(nameof(ShowLabel), defaultValue: true);

    /// <summary>
    /// Defines the <see cref="ValueString"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ValueStringProperty =
        AvaloniaProperty.Register<TextField, string>(nameof(ValueString), defaultValue: string.Empty);

    /// <summary>
    /// Defines the <see cref="Converter"/> property.
    /// </summary>
    public static readonly StyledProperty<IValueConverter?> ConverterProperty =
        AvaloniaProperty.Register<TextField, IValueConverter?>(nameof(Converter));

    /// <summary>
    /// Defines the <see cref="ConverterParameter"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> ConverterParameterProperty =
        AvaloniaProperty.Register<TextField, object?>(nameof(ConverterParameter));

    /// <summary>
    /// Defines the <see cref="NumberFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> NumberFormatProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(NumberFormat));

    /// <summary>
    /// Defines the <see cref="StatusEvaluator"/> property.
    /// </summary>
    public static readonly StyledProperty<IStatusEvaluator?> StatusEvaluatorProperty =
        AvaloniaProperty.Register<TextField, IStatusEvaluator?>(nameof(StatusEvaluator));

    /// <summary>
    /// Defines the <see cref="StatusEvaluatorParameter"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> StatusEvaluatorParameterProperty =
        AvaloniaProperty.Register<TextField, object?>(nameof(StatusEvaluatorParameter));

    /// <summary>
    /// Defines the <see cref="Status"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> StatusProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Status), defaultValue: "Normal");

    /// <summary>
    /// Gets or sets the label text displayed before the value.
    /// </summary>
    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// Gets or sets the value to display.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the prefix text displayed before the value.
    /// </summary>
    public string? Prefix
    {
        get => GetValue(PrefixProperty);
        set => SetValue(PrefixProperty, value);
    }

    /// <summary>
    /// Gets or sets the suffix text displayed after the value.
    /// </summary>
    public string? Suffix
    {
        get => GetValue(SuffixProperty);
        set => SetValue(SuffixProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the label column.
    /// </summary>
    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the value column.
    /// </summary>
    public double ValueWidth
    {
        get => GetValue(ValueWidthProperty);
        set => SetValue(ValueWidthProperty, value);
    }

    public double SuffixWidth
    {
        get => GetValue(SuffixWidthProperty);
        set => SetValue(SuffixWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets whether to show the label.
    /// </summary>
    public bool ShowLabel
    {
        get => GetValue(ShowLabelProperty);
        set => SetValue(ShowLabelProperty, value);
    }

    /// <summary>
    /// Gets the value as a string.
    /// </summary>
    public string ValueString
    {
        get => GetValue(ValueStringProperty);
        private set => SetValue(ValueStringProperty, value);
    }

    /// <summary>
    /// Gets or sets the converter to transform the value to a string.
    /// </summary>
    public IValueConverter? Converter
    {
        get => GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the converter.
    /// </summary>
    public object? ConverterParameter
    {
        get => GetValue(ConverterParameterProperty);
        set => SetValue(ConverterParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the format string for numeric values (e.g., "F2", "N0", "#,##0.00").
    /// This is a convenience property for simple number formatting without needing a converter.
    /// </summary>
    public string? NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the status evaluator to determine the field status based on the value.
    /// </summary>
    public IStatusEvaluator? StatusEvaluator
    {
        get => GetValue(StatusEvaluatorProperty);
        set => SetValue(StatusEvaluatorProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the status evaluator.
    /// </summary>
    public object? StatusEvaluatorParameter
    {
        get => GetValue(StatusEvaluatorParameterProperty);
        set => SetValue(StatusEvaluatorParameterProperty, value);
    }

    /// <summary>
    /// Gets the current status of the field (e.g., "Normal", "Warning", "Critical").
    /// This is automatically updated when the value changes and a StatusEvaluator is provided.
    /// </summary>
    public string? Status
    {
        get => GetValue(StatusProperty);
        private set => SetValue(StatusProperty, value);
    }

    static TextField()
    {
        ValueProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateValueString());
        ConverterProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateValueString());
        ConverterParameterProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateValueString());
        NumberFormatProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateValueString());

        ValueProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateStatus());
        StatusEvaluatorProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateStatus());
        StatusEvaluatorParameterProperty.Changed.AddClassHandler<TextField>((x, e) => x.UpdateStatus());
    }

    private void UpdateValueString()
    {
        if (Value == null)
        {
            ValueString = string.Empty;
            return;
        }

        // If Converter is provided, use it
        if (Converter != null)
        {
            var converted = Converter.Convert(Value, typeof(string), ConverterParameter, CultureInfo.CurrentCulture);
            ValueString = converted?.ToString() ?? string.Empty;
            return;
        }

        // If NumberFormat is provided and Value is formattable, apply formatting
        if (!string.IsNullOrEmpty(NumberFormat) && Value is IFormattable formattable)
        {
            ValueString = formattable.ToString(NumberFormat, CultureInfo.CurrentCulture);
            return;
        }

        // Default ToString()
        ValueString = Value.ToString() ?? string.Empty;
    }

    private void UpdateStatus()
    {
        if (StatusEvaluator != null)
        {
            Status = StatusEvaluator.Evaluate(Value, StatusEvaluatorParameter);
        }
        else
        {
            // Default status logic: NoValue if null, otherwise Normal
            Status = Value == null ? "NoValue" : "Normal";
        }
    }
}
