namespace XamlDS.Itk.Formatters;

/// <summary>
/// Nullable formatter that displays custom text when value is null.
/// </summary>
/// <typeparam name="T">The nullable type to format.</typeparam>
public class NullableFormatter<T> : IValueFormatter<T?>
{
    /// <summary>
    /// Gets or sets the text to display when value is null.
    /// </summary>
    public string NullText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the inner formatter to use for non-null values.
    /// </summary>
    public IValueFormatter<T>? InnerFormatter { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified null text.
    /// </summary>
    public NullableFormatter(string nullText = "", IValueFormatter<T>? innerFormatter = null)
    {
        NullText = nullText;
        InnerFormatter = innerFormatter;
    }

    /// <summary>
    /// Formats the nullable value or returns null text.
    /// </summary>
    public string Format(T? value)
    {
        if (value == null)
            return NullText;

        if (InnerFormatter != null)
            return InnerFormatter.Format(value);

        return value.ToString() ?? string.Empty;
    }
}
