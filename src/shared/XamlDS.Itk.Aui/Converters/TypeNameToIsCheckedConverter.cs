using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace XamlDS.Itk.Converters;

/// <summary>
/// Converts an object instance to a boolean value by comparing its type name with a parameter string.
/// Used for RadioButton IsChecked binding to determine if the bound object matches the specified type.
/// </summary>
public class TypeNameToIsCheckedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is null || parameter is null)
            return false;

        var typeName = value.GetType().Name;
        var parameterString = parameter.ToString();

        return string.Equals(typeName, parameterString, StringComparison.Ordinal);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        // ConvertBack is not supported for this converter
        throw new NotSupportedException("TypeNameToIsCheckedConverter does not support ConvertBack operation.");
    }
}
