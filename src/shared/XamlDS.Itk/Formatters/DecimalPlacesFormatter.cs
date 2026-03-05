using System.Globalization;
using System.Numerics;

namespace XamlDS.Itk.Formatters;

/// <summary>
/// Numeric formatter with configurable decimal places and culture-specific formatting.
/// </summary>
/// <typeparam name="T">The numeric type to format.</typeparam>
public class DecimalPlacesFormatter<T> : IValueFormatter<T> where T : INumber<T>
{
    /// <summary>
    /// Gets or sets the number of decimal places to display.
    /// </summary>
    public int DecimalPlaces { get; set; } = 2;

    /// <summary>
    /// Gets or sets the culture info for formatting. Uses current culture if null.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Formats the value with specified decimal places.
    /// </summary>
    public string Format(T value)
    {
        if (value == null)
            return string.Empty;

        var formatString = $"F{DecimalPlaces}";
        var culture = Culture ?? CultureInfo.CurrentCulture;

        if (value is IFormattable formattable)
        {
            return formattable.ToString(formatString, culture);
        }

        return value.ToString() ?? string.Empty;
    }
}
