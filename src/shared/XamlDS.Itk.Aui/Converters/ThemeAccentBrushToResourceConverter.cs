using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using XamlDS.Itk.Themes;

namespace XamlDS.Itk.Converters;

/// <summary>
/// Converts ThemeAccentBrush enum to corresponding Brush resource
/// </summary>
public class ThemeAccentBrushToResourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ThemeAccentBrush accentBrush)
            return GetDefaultBrush();

        string resourceKey = accentBrush switch
        {
            ThemeAccentBrush.AccentA => "AccentABrush",
            ThemeAccentBrush.AccentB => "AccentBBrush",
            ThemeAccentBrush.AccentC => "AccentCBrush",
            ThemeAccentBrush.Default => "Fg0Brush",
            _ => "Fg0Brush"
        };

        // Try to find the resource in Application resources
        if (Application.Current?.TryGetResource(resourceKey, null, out var resource) == true)
        {
            if (resource is IBrush brush)
                return brush;
        }

        return GetDefaultBrush();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static IBrush GetDefaultBrush()
    {
        // Try to get default brush, fallback to white
        if (Application.Current?.TryGetResource("Fg0Brush", null, out var resource) == true)
        {
            if (resource is IBrush brush)
                return brush;
        }

        return Brushes.White;
    }
}
