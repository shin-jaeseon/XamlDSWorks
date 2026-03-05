namespace XamlDS.Itk.Formatters;

/// <summary>
/// Defines a contract for formatting values to string representation.
/// </summary>
/// <typeparam name="T">The type of value to format.</typeparam>
public interface IValueFormatter<in T>
{
    /// <summary>
    /// Formats the specified value to a string representation.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A formatted string representation of the value.</returns>
    string Format(T value);
}
