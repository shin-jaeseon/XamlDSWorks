namespace XamlDS.Itk.Formatters;

/// <summary>
/// Boolean formatter that converts true/false to custom text.
/// </summary>
public class BooleanFormatter : IValueFormatter<bool>
{
    /// <summary>
    /// Gets or sets the text to display when value is true.
    /// </summary>
    public string TrueText { get; set; } = "True";

    /// <summary>
    /// Gets or sets the text to display when value is false.
    /// </summary>
    public string FalseText { get; set; } = "False";

    /// <summary>
    /// Initializes a new instance with custom true/false text.
    /// </summary>
    public BooleanFormatter(string trueText = "True", string falseText = "False")
    {
        TrueText = trueText;
        FalseText = falseText;
    }

    /// <summary>
    /// Formats the boolean value to custom text.
    /// </summary>
    public string Format(bool value)
    {
        return value ? TrueText : FalseText;
    }
}

/// <summary>
/// Nullable Boolean formatter that converts true/false/null to custom text.
/// </summary>
public class NullableBooleanFormatter : IValueFormatter<bool?>
{
    /// <summary>
    /// Gets or sets the text to display when value is true.
    /// </summary>
    public string TrueText { get; set; } = "True";

    /// <summary>
    /// Gets or sets the text to display when value is false.
    /// </summary>
    public string FalseText { get; set; } = "False";

    /// <summary>
    /// Gets or sets the text to display when value is null.
    /// </summary>
    public string NullText { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance with custom true/false/null text.
    /// </summary>
    public NullableBooleanFormatter(string trueText = "True", string falseText = "False", string nullText = "")
    {
        TrueText = trueText;
        FalseText = falseText;
        NullText = nullText;
    }

    /// <summary>
    /// Formats the nullable boolean value to custom text.
    /// </summary>
    public string Format(bool? value)
    {
        if (!value.HasValue)
            return NullText;

        return value.Value ? TrueText : FalseText;
    }
}
