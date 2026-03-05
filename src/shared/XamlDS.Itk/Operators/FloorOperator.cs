namespace XamlDS.Itk.Operators;

/// <summary>
/// Floor operator that rounds values down to specified precision.
/// </summary>
public class FloorOperator : IPrecisionOperator
{
    /// <summary>
    /// Gets the precision level (e.g., 3 for 1000s, -2 for 0.01s).
    /// </summary>
    public int Precision { get; }

    /// <summary>
    /// Initializes a new instance with the specified precision.
    /// </summary>
    /// <param name="precision">
    /// The precision level. Positive values round to powers of 10 (e.g., 3 = 1000s),
    /// negative values round to decimal places (e.g., -2 = 0.01s).
    /// </param>
    public FloorOperator(int precision = 0)
    {
        Precision = precision;
    }

    /// <summary>
    /// Applies floor operation to the value.
    /// </summary>
    public double Apply(double value)
    {
        if (Precision == 0)
            return Math.Floor(value);

        double factor = Math.Pow(10, -Precision);
        return Math.Floor(value / factor) * factor;
    }

    public override bool Equals(object? obj)
    {
        return Precision == (obj as FloorOperator)?.Precision;
    }

    public override int GetHashCode()
    {
        return Precision.GetHashCode();
    }
}
