using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace XamlDS.Itk.Converters;

/// <summary>
/// Multiplies multiple numeric values together
/// </summary>
public class MultiplyConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count == 0)
            return 0.0;

        try
        {
            double result = 1.0;
            
            foreach (var value in values)
            {
                if (value == null)
                    continue;

                // Convert value to double
                double numericValue = System.Convert.ToDouble(value, culture);
                result *= numericValue;
            }

            return result;
        }
        catch
        {
            return 0.0;
        }
    }
}
