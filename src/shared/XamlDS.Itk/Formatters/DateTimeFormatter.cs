using System.Globalization;

namespace XamlDS.Itk.Formatters;

/// <summary>
/// DateTime formatter with configurable format string and culture.
/// </summary>
public class DateTimeFormatter : IValueFormatter<DateTime>
{
    /// <summary>
    /// Gets or sets the format string (e.g., "yyyy-MM-dd", "HH:mm:ss").
    /// </summary>
    public string FormatString { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Gets or sets the culture info for formatting. Uses current culture if null.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified format string.
    /// </summary>
    public DateTimeFormatter(string formatString = "yyyy-MM-dd HH:mm:ss")
    {
        FormatString = formatString;
    }

    /// <summary>
    /// Formats the DateTime value.
    /// </summary>
    public string Format(DateTime value)
    {
        var culture = Culture ?? CultureInfo.CurrentCulture;
        return value.ToString(FormatString, culture);
    }
}

/// <summary>
/// Nullable DateTime formatter with configurable format string and null placeholder.
/// </summary>
public class NullableDateTimeFormatter : IValueFormatter<DateTime?>
{
    /// <summary>
    /// Gets or sets the format string (e.g., "yyyy-MM-dd", "HH:mm:ss").
    /// </summary>
    public string FormatString { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Gets or sets the text to display when value is null.
    /// </summary>
    public string NullText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the culture info for formatting. Uses current culture if null.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified format string and null text.
    /// </summary>
    public NullableDateTimeFormatter(string formatString = "yyyy-MM-dd HH:mm:ss", string nullText = "")
    {
        FormatString = formatString;
        NullText = nullText;
    }

    /// <summary>
    /// Formats the DateTime value or returns null text.
    /// </summary>
    public string Format(DateTime? value)
    {
        if (!value.HasValue)
            return NullText;

        var culture = Culture ?? CultureInfo.CurrentCulture;
        return value.Value.ToString(FormatString, culture);
    }
}
