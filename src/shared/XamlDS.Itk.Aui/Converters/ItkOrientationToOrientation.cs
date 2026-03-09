using Avalonia.Data.Converters;
using Avalonia.Layout;
using System.Globalization;

namespace XamlDS.Itk.Converters;

public class ItkOrientationToOrientation : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
            return Orientation.Horizontal;
        if (value is not Enums.ItkOrientation || parameter is not string)
            return Orientation.Horizontal;
        var itkOrientation = (Enums.ItkOrientation)value;
        return itkOrientation switch
        {
            Enums.ItkOrientation.Vertical => Orientation.Vertical,
            Enums.ItkOrientation.Horizontal => Orientation.Horizontal,
            _ => Orientation.Horizontal
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null || parameter is null)
            return Enums.ItkOrientation.Horizontal;
        if (value is not Orientation || parameter is not string)
            return Orientation.Horizontal;

        var orientation = (Orientation)value;
        return orientation switch
        {
            Orientation.Vertical => Enums.ItkOrientation.Vertical,
            Orientation.Horizontal => Enums.ItkOrientation.Horizontal,
            _ => Enums.ItkOrientation.Horizontal
        };
    }
}
