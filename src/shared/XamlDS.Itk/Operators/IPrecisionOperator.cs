namespace XamlDS.Itk.Operators;

/// <summary>
/// Defines precision operation for numeric values.
/// </summary>
public interface IPrecisionOperator
{
    /// <summary>
    /// Applies precision operation to the value.
    /// </summary>
    /// <param name="value">The value to process.</param>
    /// <returns>The value with precision applied.</returns>
    double Apply(double value);

    /// <summary>
    /// Gets the precision level (e.g., 3 for 1000s, -2 for 0.01s).
    /// </summary>
    int Precision { get; }
}
