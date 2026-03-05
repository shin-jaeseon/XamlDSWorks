using System.Numerics;

namespace XamlDS.Itk.Formatters;

/// <summary>
/// Numeric formatter with custom format string support.
/// </summary>
/// <typeparam name="T">The numeric type to format.</typeparam>
public class CustomNumericFormatter<T> : IValueFormatter<T> where T : INumber<T>
{
    /// <summary>
    /// Gets or sets the custom format string (e.g., "N2", "E4", "P1").
    /// </summary>
    public string FormatString { get; set; } = "G";

    /// <summary>
    /// Initializes a new instance with the specified format string.
    /// </summary>
    public CustomNumericFormatter(string formatString = "G")
    {
        FormatString = formatString;
    }

    /// <summary>
    /// Formats the value using the custom format string.
    /// </summary>
    public string Format(T value)
    {
        if (value == null)
            return string.Empty;

        if (value is IFormattable formattable)
        {
            return formattable.ToString(FormatString, null);
        }

        return value.ToString() ?? string.Empty;
    }
}
