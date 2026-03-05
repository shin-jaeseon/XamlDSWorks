using System.ComponentModel;
using System.Reflection;

namespace XamlDS.Itk.Formatters;

/// <summary>
/// Enum formatter that converts enum values to display text using Description attribute or name.
/// </summary>
/// <typeparam name="TEnum">The enum type to format.</typeparam>
public class EnumFormatter<TEnum> : IValueFormatter<TEnum> where TEnum : struct, Enum
{
    /// <summary>
    /// Gets or sets whether to use DescriptionAttribute if available.
    /// </summary>
    public bool UseDescription { get; set; } = true;

    /// <summary>
    /// Formats the enum value to display text.
    /// </summary>
    public string Format(TEnum value)
    {
        if (UseDescription)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }
        }

        return value.ToString();
    }
}

/// <summary>
/// Nullable Enum formatter that converts nullable enum values to display text.
/// </summary>
/// <typeparam name="TEnum">The enum type to format.</typeparam>
public class NullableEnumFormatter<TEnum> : IValueFormatter<TEnum?> where TEnum : struct, Enum
{
    /// <summary>
    /// Gets or sets whether to use DescriptionAttribute if available.
    /// </summary>
    public bool UseDescription { get; set; } = true;

    /// <summary>
    /// Gets or sets the text to display when value is null.
    /// </summary>
    public string NullText { get; set; } = string.Empty;

    /// <summary>
    /// Formats the nullable enum value to display text.
    /// </summary>
    public string Format(TEnum? value)
    {
        if (!value.HasValue)
            return NullText;

        if (UseDescription)
        {
            var fieldInfo = value.Value.GetType().GetField(value.Value.ToString());
            if (fieldInfo != null)
            {
                var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }
        }

        return value.Value.ToString();
    }
}
