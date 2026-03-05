using System.Numerics;

namespace XamlDS.Itk.Formatters;

/// <summary>
/// Default numeric value formatter that uses the value's ToString() method.
/// </summary>
/// <typeparam name="T">The numeric type to format.</typeparam>
public class DefaultNumericFormatter<T> : IValueFormatter<T> where T : INumber<T>
{
    /// <summary>
    /// Gets the singleton instance of the default formatter.
    /// </summary>
    public static readonly DefaultNumericFormatter<T> Instance = new();

    private DefaultNumericFormatter() { }

    /// <summary>
    /// Formats the value using its default ToString() method.
    /// </summary>
    public string Format(T value)
    {
        return value?.ToString() ?? string.Empty;
    }
}
