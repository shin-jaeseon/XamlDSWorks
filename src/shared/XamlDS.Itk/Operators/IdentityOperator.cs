namespace XamlDS.Itk.Operators;

/// <summary>
/// Identity operator that returns the value unchanged (no precision modification).
/// </summary>
public class IdentityOperator : IPrecisionOperator
{
    /// <summary>
    /// Gets the singleton instance of the identity operator.
    /// </summary>
    public static readonly IdentityOperator Instance = new();

    private IdentityOperator() { }

    /// <summary>
    /// Gets the precision level (always 0 for identity).
    /// </summary>
    public int Precision => 0;

    /// <summary>
    /// Returns the value unchanged.
    /// </summary>
    public double Apply(double value) => value;


    public override bool Equals(object? obj)
    {
        return obj is IdentityOperator;
    }

    public override int GetHashCode()
    {
        return typeof(IdentityOperator).GetHashCode();
    }
}
