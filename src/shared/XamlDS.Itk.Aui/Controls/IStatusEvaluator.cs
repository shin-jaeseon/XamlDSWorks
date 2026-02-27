namespace XamlDS.Itk.Controls;

/// <summary>
/// Defines a contract for evaluating the status of a field based on its value.
/// </summary>
public interface IStatusEvaluator
{
    /// <summary>
    /// Evaluates the status based on the provided value and parameter.
    /// </summary>
    /// <param name="value">The value to evaluate.</param>
    /// <param name="parameter">Optional parameter for evaluation logic.</param>
    /// <returns>A string representing the status (e.g., "Normal", "Warning", "Critical").</returns>
    string? Evaluate(object? value, object? parameter);
}
