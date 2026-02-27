using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XamlDS.Itk.Converters;

public class StringToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
            return false;
        if (value is not string || parameter is not string)
            return false;
        return string.Equals(value as String, parameter as String, StringComparison.Ordinal);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter;
    }
}
