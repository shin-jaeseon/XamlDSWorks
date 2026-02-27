# TextField Control

## Overview

`TextField` is a flexible display control for showing formatted text values with optional labels, prefixes, and suffixes. It's designed to display read-only data in a consistent, professional format across your application.

### Key Features

- **Label Support**: Display descriptive labels before values
- **Flexible Formatting**: Built-in number formatting or custom converters
- **Prefix/Suffix**: Add decorative text before or after values
- **Type Agnostic**: Works with any object type (numeric, string, custom objects)
- **Customizable Layout**: Control widths of label and value sections
- **Extensible**: Easy to create custom converters for complex formatting

---

## Basic Usage

### Simple Text Display

```xml
<itk:TextField Label="Name" Value="John Doe"/>
```

### With Suffix

```xml
<itk:TextField Label="Status" Value="Active" Suffix="(Online)"/>
```

### With Prefix and Suffix

```xml
<itk:TextField Label="Balance" Value="1250.50" Prefix="$" Suffix="USD"/>
```

### Custom Widths

```xml
<itk:TextField Label="Description" 
               Value="Long text content here" 
               LabelWidth="100" 
               ValueWidth="200"/>
```

---

## Number Formatting

The `NumberFormat` property provides convenient formatting for numeric values without requiring a custom converter.

### Decimal Places

```xml
<!-- 2 decimal places -->
<itk:TextField Label="Weight" Value="10.456" Suffix="Kg" NumberFormat="F2"/>
<!-- Output: 10.46 Kg -->

<!-- 1 decimal place -->
<itk:TextField Label="Height" Value="2.4" Suffix="m" NumberFormat="F1"/>
<!-- Output: 2.4 m -->

<!-- No decimal places -->
<itk:TextField Label="Count" Value="42.8" NumberFormat="F0"/>
<!-- Output: 43 -->
```

### Thousand Separators

```xml
<!-- With thousand separators, 0 decimal places -->
<itk:TextField Label="Population" Value="1234567" NumberFormat="N0"/>
<!-- Output: 1,234,567 -->

<!-- With thousand separators, 2 decimal places -->
<itk:TextField Label="Price" Value="1234567.89" Suffix="원" NumberFormat="N2"/>
<!-- Output: 1,234,567.89 원 -->
```

### Currency Format

```xml
<itk:TextField Label="Price" Value="1250.5" NumberFormat="C"/>
<!-- Output: $1,250.50 (depends on current culture) -->

<itk:TextField Label="Price" Value="1250.5" NumberFormat="C0"/>
<!-- Output: $1,251 -->
```

### Percentage Format

```xml
<itk:TextField Label="Progress" Value="0.856" NumberFormat="P1"/>
<!-- Output: 85.6% -->

<itk:TextField Label="Rate" Value="0.125" NumberFormat="P0"/>
<!-- Output: 13% -->
```

### Custom Number Format

```xml
<!-- Custom pattern: thousands separator with 2 decimals -->
<itk:TextField Label="Amount" Value="1234.5" NumberFormat="#,##0.00"/>
<!-- Output: 1,234.50 -->

<!-- Phone number style -->
<itk:TextField Label="ID" Value="1234567890" NumberFormat="###-####-####"/>
<!-- Output: 123-4567-890 -->
```

### Common Format Strings

| Format | Description | Example (1234.56) |
| --- | --- | --- |
| `F0` | Fixed-point, 0 decimals | 1235 |
| `F1` | Fixed-point, 1 decimal | 1234.6 |
| `F2` | Fixed-point, 2 decimals | 1234.56 |
| `N0` | Number with thousands separator, 0 decimals | 1,235 |
| `N2` | Number with thousands separator, 2 decimals | 1,234.56 |
| `C` | Currency (culture-specific) | $1,234.56 |
| `P0` | Percentage, 0 decimals | 123456% |
| `P1` | Percentage, 1 decimal | 123456.0% |

---

## Custom Converters

For complex formatting scenarios, you can create custom `IValueConverter` implementations.

### Creating a Simple Converter

#### Step 1: Create the Converter Class

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

/// <summary>
/// Converts boolean values to user-friendly status text.
/// </summary>
public class BooleanToStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "활성" : "비활성";
        }
        return "알 수 없음";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

#### Step 2: Register in Resources

```xml
<Window.Resources>
    <converters:BooleanToStatusConverter x:Key="BooleanToStatusConverter"/>
</Window.Resources>
```

#### Step 3: Use in TextField

```xml
<itk:TextField Label="서비스 상태" 
               Value="{Binding IsServiceActive}" 
               Converter="{StaticResource BooleanToStatusConverter}"/>
```

---

## Advanced Converter Examples

### Date/Time Formatting Converter

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

/// <summary>
/// Converts DateTime to formatted string with customizable format.
/// </summary>
public class DateTimeFormatConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            string format = parameter?.ToString() ?? "yyyy-MM-dd HH:mm:ss";
            return dateTime.ToString(format, culture);
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**Usage:**

```xml
<converters:DateTimeFormatConverter x:Key="DateTimeConverter"/>

<!-- Long format -->
<itk:TextField Label="Created" 
               Value="{Binding CreatedDate}" 
               Converter="{StaticResource DateTimeConverter}"
               ConverterParameter="yyyy년 MM월 dd일 HH:mm"/>

<!-- Short format -->
<itk:TextField Label="Updated" 
               Value="{Binding UpdatedDate}" 
               Converter="{StaticResource DateTimeConverter}"
               ConverterParameter="MM/dd HH:mm"/>
```

### Geographic Location Converter

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

/// <summary>
/// Represents a geographic location.
/// </summary>
public record GeoLocation(double Latitude, double Longitude);

/// <summary>
/// Converts GeoLocation to formatted coordinate string.
/// </summary>
public class GeoLocationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is GeoLocation location)
        {
            string latDir = location.Latitude >= 0 ? "N" : "S";
            string lonDir = location.Longitude >= 0 ? "E" : "W";
            
            return $"{Math.Abs(location.Latitude):F4}° {latDir}, " +
                   $"{Math.Abs(location.Longitude):F4}° {lonDir}";
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**Usage:**

```xml
<converters:GeoLocationConverter x:Key="GeoConverter"/>

<itk:TextField Label="Location" 
               Value="{Binding CurrentLocation}" 
               Converter="{StaticResource GeoConverter}"/>
<!-- Output: 37.5665° N, 126.9780° E -->
```

### File Size Converter

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

/// <summary>
/// Converts file size in bytes to human-readable format.
/// </summary>
public class FileSizeConverter : IValueConverter
{
    private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long bytes)
        {
            if (bytes == 0) return "0 B";

            int magnitude = (int)Math.Log(bytes, 1024);
            magnitude = Math.Min(magnitude, SizeSuffixes.Length - 1);
            
            double adjustedSize = bytes / Math.Pow(1024, magnitude);
            
            return $"{adjustedSize:F2} {SizeSuffixes[magnitude]}";
        }
        return "0 B";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**Usage:**

```xml
<converters:FileSizeConverter x:Key="FileSizeConverter"/>

<itk:TextField Label="File Size" 
               Value="{Binding FileSizeInBytes}" 
               Converter="{StaticResource FileSizeConverter}"/>
<!-- Input: 1536000 → Output: 1.46 MB -->
```

### Enum to Display Name Converter

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

/// <summary>
/// Converts enum values to localized display names.
/// </summary>
public class OrderStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "대기 중",
                OrderStatus.Processing => "처리 중",
                OrderStatus.Shipped => "배송 중",
                OrderStatus.Delivered => "배송 완료",
                OrderStatus.Cancelled => "취소됨",
                _ => status.ToString()
            };
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**Usage:**

```xml
<converters:OrderStatusConverter x:Key="OrderStatusConverter"/>

<itk:TextField Label="주문 상태" 
               Value="{Binding Status}" 
               Converter="{StaticResource OrderStatusConverter}"/>
```

### Range/Threshold Converter

```csharp
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YourApp.Converters;

/// <summary>
/// Converts numeric values to status text based on thresholds.
/// </summary>
public class ThresholdStatusConverter : IValueConverter
{
    public double LowThreshold { get; set; } = 30;
    public double HighThreshold { get; set; } = 70;
    
    public string LowText { get; set; } = "낮음";
    public string NormalText { get; set; } = "정상";
    public string HighText { get; set; } = "높음";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            if (doubleValue < LowThreshold)
                return $"{doubleValue:F1} ({LowText})";
            if (doubleValue > HighThreshold)
                return $"{doubleValue:F1} ({HighText})";
            return $"{doubleValue:F1} ({NormalText})";
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <converters:ThresholdStatusConverter x:Key="TempConverter" 
                                         LowThreshold="18" 
                                         HighThreshold="26"
                                         LowText="추움"
                                         NormalText="쾌적"
                                         HighText="더움"/>
</Window.Resources>

<itk:TextField Label="온도" 
               Value="{Binding CurrentTemperature}" 
               Converter="{StaticResource TempConverter}"
               Suffix="°C"/>
<!-- Input: 15.5 → Output: 15.5 (추움) °C -->
```

---

## Complete Example

Here's a comprehensive example combining various TextField features:

### ViewModel

```csharp
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace YourApp.ViewModels;

public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    private string _productName = "Premium Widget";
    
    [ObservableProperty]
    private double _price = 1250.50;
    
    [ObservableProperty]
    private double _weight = 2.456;
    
    [ObservableProperty]
    private int _stockCount = 12500;
    
    [ObservableProperty]
    private bool _isAvailable = true;
    
    [ObservableProperty]
    private DateTime _lastUpdated = DateTime.Now;
    
    [ObservableProperty]
    private GeoLocation _warehouseLocation = new(37.5665, 126.9780);
}
```

### View

```xml
<UserControl xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:itk="http://xamldesignstudio.com"
             xmlns:converters="using:YourApp.Converters"
             x:Class="YourApp.Views.ProductView">
    
    <UserControl.Resources>
        <converters:BooleanToStatusConverter x:Key="BooleanConverter"/>
        <converters:DateTimeFormatConverter x:Key="DateTimeConverter"/>
        <converters:GeoLocationConverter x:Key="GeoConverter"/>
    </UserControl.Resources>
    
    <StackPanel Spacing="5">
        <!-- Simple text -->
        <itk:TextField Label="상품명" Value="{Binding ProductName}"/>
        
        <!-- Number with currency format -->
        <itk:TextField Label="가격" 
                       Value="{Binding Price}" 
                       Suffix="원"
                       NumberFormat="N0"/>
        
        <!-- Decimal with 2 places -->
        <itk:TextField Label="무게" 
                       Value="{Binding Weight}" 
                       Suffix="Kg"
                       NumberFormat="F2"/>
        
        <!-- Integer with thousand separators -->
        <itk:TextField Label="재고" 
                       Value="{Binding StockCount}" 
                       Suffix="개"
                       NumberFormat="N0"/>
        
        <!-- Boolean with converter -->
        <itk:TextField Label="상태" 
                       Value="{Binding IsAvailable}" 
                       Converter="{StaticResource BooleanConverter}"/>
        
        <!-- DateTime with converter -->
        <itk:TextField Label="최종 업데이트" 
                       Value="{Binding LastUpdated}" 
                       Converter="{StaticResource DateTimeConverter}"
                       ConverterParameter="yyyy-MM-dd HH:mm:ss"/>
        
        <!-- Custom object with converter -->
        <itk:TextField Label="창고 위치" 
                       Value="{Binding WarehouseLocation}" 
                       Converter="{StaticResource GeoConverter}"/>
    </StackPanel>
</UserControl>
```

---

## Properties Reference

### Core Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Label` | `string?` | `null` | Label text displayed before the value |
| `Value` | `object?` | `null` | The value to display |
| `Prefix` | `string?` | `null` | Text displayed before the value |
| `Suffix` | `string?` | `null` | Text displayed after the value (e.g., units) |

### Formatting Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `NumberFormat` | `string?` | `null` | Format string for numeric values (e.g., "F2", "N0") |
| `Converter` | `IValueConverter?` | `null` | Custom converter for value transformation |
| `ConverterParameter` | `object?` | `null` | Parameter passed to the converter |

### Layout Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `LabelWidth` | `double` | `80.0` | Width of the label column |
| `ValueWidth` | `double` | `100.0` | Width of the value column |
| `ShowLabel` | `bool` | `true` | Whether to display the label |

### Read-Only Properties

| Property | Type | Description |
| --- | --- | --- |
| `ValueString` | `string` | The formatted value string (computed) |

---

## Best Practices

### 1. Choose the Right Formatting Approach

- **Simple numbers**: Use `NumberFormat` property
- **Complex logic**: Create a custom converter
- **Reusable formatting**: Create and register converters as resources

### 2. Performance Considerations

- Register converters as static resources to avoid creating multiple instances
- Avoid complex calculations in converters for frequently updated values
- Use `NumberFormat` when possible (it's more efficient than converters)

### 3. Localization

- Use `CultureInfo` in converters for locale-specific formatting
- Consider creating culture-aware converters for multi-language apps
- Test number formats with different cultures

### 4. Error Handling

Always handle null and invalid types in your converters:

```csharp
public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
{
    if (value == null)
        return string.Empty;
    
    if (value is not ExpectedType typedValue)
        return value.ToString();
    
    // Your conversion logic here
}
```

### 5. Unit Display

Use `Suffix` for units to maintain consistency:

```xml
<!-- Good -->
<itk:TextField Label="Weight" Value="10.5" Suffix="Kg" NumberFormat="F1"/>

<!-- Avoid concatenating units in the value -->
<itk:TextField Label="Weight" Value="10.5 Kg"/>
```

---

## Troubleshooting

### Value Not Displaying

**Problem**: Value appears empty even though data is bound.

**Solutions**:

- Check that `Value` property is properly bound
- Verify the binding path in your ViewModel
- Ensure the converter returns a valid string (not null)

### Number Format Not Applied

**Problem**: `NumberFormat` doesn't affect the output.

**Solutions**:

- Ensure the `Value` is a numeric type (`int`, `double`, `decimal`, etc.)
- Check that `NumberFormat` string is valid (.NET format string)
- Verify no `Converter` is set (Converter takes precedence over `NumberFormat`)

### Converter Not Called

**Problem**: Custom converter isn't being invoked.

**Solutions**:

- Verify the converter is registered in resources
- Check the `x:Key` matches the `StaticResource` reference
- Ensure proper namespace import for the converter

## See Also

- [Avalonia Data Binding](https://docs.avaloniAui.net/docs/basics/data-binding)
- [Avalonia Value Converters](https://docs.avaloniAui.net/docs/basics/data-binding/data-binding-converters)
- [.NET Standard Numeric Format Strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings)
- [.NET Custom Numeric Format Strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings)
