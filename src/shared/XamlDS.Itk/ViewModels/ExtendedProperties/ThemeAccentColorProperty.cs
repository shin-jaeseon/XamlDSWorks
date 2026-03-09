namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Specifies theme accent colors for UI elements.
/// </summary>
public enum ThemeAccentColor
{
    /// <summary>
    /// Default theme color (no override).
    /// </summary>
    Default,

    /// <summary>
    /// Blue accent color.
    /// </summary>
    Blue,

    /// <summary>
    /// Orange accent color.
    /// </summary>
    Orange,

    /// <summary>
    /// Green accent color.
    /// </summary>
    Green,

    /// <summary>
    /// Red accent color.
    /// </summary>
    Red,

    /// <summary>
    /// Purple accent color.
    /// </summary>
    Purple,

    /// <summary>
    /// Yellow accent color.
    /// </summary>
    Yellow,

    /// <summary>
    /// Gray accent color.
    /// </summary>
    Gray
}

/// <summary>
/// Wrapper class for ThemeAccentColor value to make it a reference type.
/// </summary>
public class ThemeAccentColorValue
{
    public ThemeAccentColor Color { get; set; }

    public ThemeAccentColorValue(ThemeAccentColor color)
    {
        Color = color;
    }
}

/// <summary>
/// Extended property for specifying the theme accent color of a ViewModel.
/// This is a context-independent property - the theme belongs to the ViewModel itself.
/// </summary>
public class ThemeAccentColorProperty : SelfOwnedProperty<ThemeAccentColorValue>
{
    /// <summary>
    /// Singleton instance of ThemeAccentColorProperty.
    /// </summary>
    public static readonly ThemeAccentColorProperty Instance = new();

    public override string PropertyName => "ThemeAccentColor";

    /// <summary>
    /// Sets the theme accent color for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <param name="color">The accent color.</param>
    public static void Set(ViewModelBase target, ThemeAccentColor color)
        => Instance.SetValue(target, new ThemeAccentColorValue(color));

    /// <summary>
    /// Gets the theme accent color for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <returns>The accent color, or Default if not set.</returns>
    public static ThemeAccentColor Get(ViewModelBase target)
        => Instance.GetValue(target)?.Color ?? ThemeAccentColor.Default;

    /// <summary>
    /// Tries to get the theme accent color for the specified ViewModel.
    /// </summary>
    /// <param name="target">The target ViewModel.</param>
    /// <param name="color">The accent color if found.</param>
    /// <returns>True if the value was found, false otherwise.</returns>
    public static bool TryGet(ViewModelBase target, out ThemeAccentColor color)
    {
        if (Instance.TryGetValue(target, out var value) && value != null)
        {
            color = value.Color;
            return true;
        }
        color = ThemeAccentColor.Default;
        return false;
    }
}
