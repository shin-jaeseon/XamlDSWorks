namespace XamlDS.Itk.Formatters;

/// <summary>
/// Default value formatter that uses the value's ToString() method.
/// </summary>
/// <typeparam name="T">The type to format.</typeparam>
public class DefaultFormatter<T> : IValueFormatter<T>
{
    /// <summary>
    /// Gets the singleton instance of the default formatter.
    /// </summary>
    public static readonly DefaultFormatter<T> Instance = new();

    private DefaultFormatter() { }

    /// <summary>
    /// Formats the value using its default ToString() method.
    /// </summary>
    public string Format(T value)
    {
        return value?.ToString() ?? string.Empty;
    }
}
