# Field Status System

## Overview

The Field Status System provides a flexible mechanism for evaluating and styling controls based on their current value state. It enables dynamic visual feedback for monitoring applications, dashboards, and industrial HMI systems where value thresholds and status conditions are critical.

### Key Concepts

- **Status**: A string representing the current state (e.g., "Normal", "Warning", "Critical")
- **StatusEvaluator**: Custom logic to determine status based on value
- **StatusEvaluatorParameter**: Additional context for evaluation logic
- **Status-Based Styling**: Automatic visual changes using Avalonia selectors

---

## Status Property

### Purpose

The `Status` property is a **read-only string** that represents the current state of a control. It's automatically updated when:
- The control's value changes
- The StatusEvaluator changes
- The StatusEvaluatorParameter changes

### Common Status Values

While you can use any string value, here are recommended standard statuses:

| Status | Description | Typical Use Case |
|--------|-------------|------------------|
| `"Normal"` | Value is within acceptable range | Default state |
| `"Warning"` | Value is approaching limits | Pre-alert condition |
| `"Critical"` | Value has exceeded safe limits | Alert/alarm condition |
| `"TooLow"` | Value is below minimum threshold | Low-side warning |
| `"TooHigh"` | Value is above maximum threshold | High-side warning |
| `"NoValue"` | No data available (null) | Disconnected/unavailable |
| `"InvalidValue"` | Value type or format is invalid | Data error |
| `"Idle"` | System is idle/standby | Inactive state |
| `"Disconnected"` | Communication lost | Connection error |

**Note**: These are conventions, not enforced. You can define custom status strings for your specific domain.

---

## StatusEvaluator Property

### Purpose

The `StatusEvaluator` property accepts an implementation of `IStatusEvaluator` that contains the logic for determining the status based on the control's value.

### IStatusEvaluator Interface

```csharp
namespace XamlDS.ITK.Controls;

public interface IStatusEvaluator
{
    string? Evaluate(object? value, object? parameter);
}
```

### Default Behavior (No Evaluator)

When `StatusEvaluator` is **not set**, the default logic is:
- Value is `null` → `"NoValue"`
- Value is not `null` → `"Normal"`

---

## StatusEvaluatorParameter Property

### Purpose

The `StatusEvaluatorParameter` provides additional context or configuration to the evaluator. This allows a single evaluator class to be reused with different thresholds or settings.

### Example Use Cases

- Passing threshold values (min/max)
- Providing unit conversion factors
- Specifying evaluation mode (strict/lenient)
- Injecting localization resources

---

## Creating Custom StatusEvaluators

### Example 1: Numeric Range Evaluator

```csharp
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Evaluates numeric values against configurable thresholds.
/// </summary>
public class NumericRangeStatusEvaluator : IStatusEvaluator
{
    /// <summary>
    /// Critical low threshold. Values below this are "Critical".
    /// </summary>
    public double? LowCritical { get; set; }

    /// <summary>
    /// Warning low threshold. Values below this are "TooLow".
    /// </summary>
    public double? LowWarning { get; set; }

    /// <summary>
    /// Warning high threshold. Values above this are "TooHigh".
    /// </summary>
    public double? HighWarning { get; set; }

    /// <summary>
    /// Critical high threshold. Values above this are "Critical".
    /// </summary>
    public double? HighCritical { get; set; }

    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return "NoValue";

        if (value is not IConvertible)
            return "InvalidValue";

        try
        {
            double numValue = Convert.ToDouble(value);

            // Check critical conditions first
            if (LowCritical.HasValue && numValue < LowCritical.Value)
                return "Critical";
            if (HighCritical.HasValue && numValue > HighCritical.Value)
                return "Critical";

            // Check warning conditions
            if (LowWarning.HasValue && numValue < LowWarning.Value)
                return "TooLow";
            if (HighWarning.HasValue && numValue > HighWarning.Value)
                return "TooHigh";

            return "Normal";
        }
        catch
        {
            return "InvalidValue";
        }
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <evaluators:NumericRangeStatusEvaluator x:Key="TempEvaluator"
                                            LowCritical="10"
                                            LowWarning="18"
                                            HighWarning="26"
                                            HighCritical="35"/>
</Window.Resources>

<itk:TextField Label="온도" 
               Value="{Binding Temperature}" 
               Suffix="°C"
               NumberFormat="F1"
               StatusEvaluator="{StaticResource TempEvaluator}"/>
```

---

### Example 2: Boolean Status Evaluator

```csharp
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Evaluates boolean values to specific status strings.
/// </summary>
public class BooleanStatusEvaluator : IStatusEvaluator
{
    public string TrueStatus { get; set; } = "Normal";
    public string FalseStatus { get; set; } = "Warning";
    public string NullStatus { get; set; } = "NoValue";

    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return NullStatus;

        if (value is bool boolValue)
            return boolValue ? TrueStatus : FalseStatus;

        return "InvalidValue";
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <evaluators:BooleanStatusEvaluator x:Key="ConnectionEvaluator"
                                       TrueStatus="Normal"
                                       FalseStatus="Disconnected"/>
</Window.Resources>

<itk:TextField Label="연결 상태" 
               Value="{Binding IsConnected}"
               StatusEvaluator="{StaticResource ConnectionEvaluator}"/>
```

---

### Example 3: String Pattern Evaluator

```csharp
using System.Text.RegularExpressions;
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Evaluates string values based on pattern matching.
/// </summary>
public class StringPatternStatusEvaluator : IStatusEvaluator
{
    public string? ErrorPattern { get; set; }
    public string? WarningPattern { get; set; }

    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return "NoValue";

        string text = value.ToString() ?? string.Empty;

        if (!string.IsNullOrEmpty(ErrorPattern) && Regex.IsMatch(text, ErrorPattern))
            return "Critical";

        if (!string.IsNullOrEmpty(WarningPattern) && Regex.IsMatch(text, WarningPattern))
            return "Warning";

        return "Normal";
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <evaluators:StringPatternStatusEvaluator x:Key="LogEvaluator"
                                             ErrorPattern="(?i)error|exception|fail"
                                             WarningPattern="(?i)warning|deprecated"/>
</Window.Resources>

<itk:TextField Label="최근 로그" 
               Value="{Binding LastLogMessage}"
               StatusEvaluator="{StaticResource LogEvaluator}"/>
```

---

### Example 4: Threshold List Evaluator

```csharp
using System.Collections.Generic;
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Evaluates values against a list of threshold definitions.
/// </summary>
public class ThresholdListStatusEvaluator : IStatusEvaluator
{
    public List<ThresholdDefinition> Thresholds { get; set; } = new();

    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return "NoValue";

        if (value is not IConvertible)
            return "InvalidValue";

        try
        {
            double numValue = Convert.ToDouble(value);

            // Find first matching threshold (order matters!)
            foreach (var threshold in Thresholds)
            {
                if (threshold.Matches(numValue))
                    return threshold.Status;
            }

            return "Normal";
        }
        catch
        {
            return "InvalidValue";
        }
    }
}

public class ThresholdDefinition
{
    public double? Min { get; set; }
    public double? Max { get; set; }
    public string Status { get; set; } = "Normal";

    public bool Matches(double value)
    {
        if (Min.HasValue && value < Min.Value)
            return false;
        if (Max.HasValue && value > Max.Value)
            return false;
        return true;
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <evaluators:ThresholdListStatusEvaluator x:Key="PressureEvaluator">
        <evaluators:ThresholdListStatusEvaluator.Thresholds>
            <!-- Order matters: check critical first -->
            <evaluators:ThresholdDefinition Max="10" Status="Critical"/>
            <evaluators:ThresholdDefinition Max="20" Status="TooLow"/>
            <evaluators:ThresholdDefinition Min="80" Max="90" Status="TooHigh"/>
            <evaluators:ThresholdDefinition Min="90" Status="Critical"/>
            <!-- Default range: 20-80 = Normal (implicit) -->
        </evaluators:ThresholdListStatusEvaluator.Thresholds>
    </evaluators:ThresholdListStatusEvaluator>
</Window.Resources>

<itk:TextField Label="압력" 
               Value="{Binding Pressure}" 
               Suffix="PSI"
               StatusEvaluator="{StaticResource PressureEvaluator}"/>
```

---

### Example 5: Time-Based Status Evaluator

```csharp
using System;
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Evaluates DateTime values based on age (time elapsed).
/// </summary>
public class TimeAgeStatusEvaluator : IStatusEvaluator
{
    /// <summary>
    /// Maximum age in seconds before showing warning.
    /// </summary>
    public double WarningAgeSeconds { get; set; } = 60;

    /// <summary>
    /// Maximum age in seconds before showing critical.
    /// </summary>
    public double CriticalAgeSeconds { get; set; } = 300;

    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return "NoValue";

        if (value is not DateTime dateTime)
            return "InvalidValue";

        var age = DateTime.Now - dateTime;

        if (age.TotalSeconds > CriticalAgeSeconds)
            return "Critical";

        if (age.TotalSeconds > WarningAgeSeconds)
            return "Warning";

        return "Normal";
    }
}
```

**Usage:**

```xml
<Window.Resources>
    <evaluators:TimeAgeStatusEvaluator x:Key="LastUpdateEvaluator"
                                       WarningAgeSeconds="30"
                                       CriticalAgeSeconds="120"/>
</Window.Resources>

<itk:TextField Label="마지막 업데이트" 
               Value="{Binding LastUpdateTime}"
               Converter="{StaticResource DateTimeConverter}"
               ConverterParameter="HH:mm:ss"
               StatusEvaluator="{StaticResource LastUpdateEvaluator}"/>
```

---

### Example 6: Parameterized Evaluator

```csharp
using XamlDS.ITK.Controls;

namespace YourApp.Evaluators;

/// <summary>
/// Uses the parameter to determine evaluation mode.
/// </summary>
public class ParameterizedRangeEvaluator : IStatusEvaluator
{
    public string? Evaluate(object? value, object? parameter)
    {
        if (value == null)
            return "NoValue";

        if (value is not IConvertible)
            return "InvalidValue";

        // Parameter contains threshold configuration
        if (parameter is not ThresholdConfig config)
            return "Normal";

        try
        {
            double numValue = Convert.ToDouble(value);

            if (numValue < config.Min)
                return config.BelowMinStatus ?? "TooLow";
            if (numValue > config.Max)
                return config.AboveMaxStatus ?? "TooHigh";

            return "Normal";
        }
        catch
        {
            return "InvalidValue";
        }
    }
}

public class ThresholdConfig
{
    public double Min { get; set; }
    public double Max { get; set; }
    public string? BelowMinStatus { get; set; }
    public string? AboveMaxStatus { get; set; }
}
```

**Usage (Code-Behind):**

```csharp
var tempConfig = new ThresholdConfig 
{ 
    Min = 18, 
    Max = 26,
    BelowMinStatus = "TooLow",
    AboveMaxStatus = "TooHigh"
};

myTextField.StatusEvaluator = new ParameterizedRangeEvaluator();
myTextField.StatusEvaluatorParameter = tempConfig;
```

---

## Status-Based Styling

### Basic Status Styling

```xml
<Style Selector="itk|TextField[Status=Normal]">
    <Setter Property="Foreground" Value="#00C853"/>
</Style>

<Style Selector="itk|TextField[Status=Warning]">
    <Setter Property="Foreground" Value="#FF6F00"/>
    <Setter Property="FontWeight" Value="Bold"/>
</Style>

<Style Selector="itk|TextField[Status=TooLow], itk|TextField[Status=TooHigh]">
    <Setter Property="Foreground" Value="#FFB300"/>
</Style>

<Style Selector="itk|TextField[Status=Critical]">
    <Setter Property="Foreground" Value="#D50000"/>
    <Setter Property="FontWeight" Value="Bold"/>
</Style>

<Style Selector="itk|TextField[Status=NoValue], itk|TextField[Status=Disconnected]">
    <Setter Property="Foreground" Value="#9E9E9E"/>
    <Setter Property="FontStyle" Value="Italic"/>
</Style>

<Style Selector="itk|TextField[Status=InvalidValue]">
    <Setter Property="Foreground" Value="#F50057"/>
    <Setter Property="TextDecorations" Value="Underline"/>
</Style>
```

---

### Animated Status Styles

```xml
<!-- Blinking animation for Critical status -->
<Style Selector="itk|TextField[Status=Critical]">
    <Setter Property="Foreground" Value="#D50000"/>
    <Setter Property="FontWeight" Value="Bold"/>
    <Style.Animations>
        <Animation Duration="0:0:1" IterationCount="Infinite">
            <KeyFrame Cue="0%">
                <Setter Property="Opacity" Value="1.0"/>
            </KeyFrame>
            <KeyFrame Cue="50%">
                <Setter Property="Opacity" Value="0.3"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Opacity" Value="1.0"/>
            </KeyFrame>
        </Animation>
    </Style.Animations>
</Style>

<!-- Pulsing background for Warning -->
<Style Selector="itk|TextField[Status=Warning]">
    <Setter Property="Background" Value="#FFF3E0"/>
    <Style.Animations>
        <Animation Duration="0:0:2" IterationCount="Infinite">
            <KeyFrame Cue="0%">
                <Setter Property="Background" Value="#FFF3E0"/>
            </KeyFrame>
            <KeyFrame Cue="50%">
                <Setter Property="Background" Value="#FFE0B2"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Background" Value="#FFF3E0"/>
            </KeyFrame>
        </Animation>
    </Style.Animations>
</Style>
```

---

### Custom Status Styling

```xml
<!-- Domain-specific custom statuses -->
<Style Selector="itk|TextField[Status=OutOfStock]">
    <Setter Property="Foreground" Value="Red"/>
    <Setter Property="Suffix" Value="(재고 없음)"/>
</Style>

<Style Selector="itk|TextField[Status=LowStock]">
    <Setter Property="Foreground" Value="Orange"/>
    <Setter Property="Suffix" Value="(재고 부족)"/>
</Style>

<Style Selector="itk|TextField[Status=InStock]">
    <Setter Property="Foreground" Value="Green"/>
    <Setter Property="Suffix" Value="(재고 충분)"/>
</Style>
```

---

## Complete Example: Temperature Monitoring

### ViewModel

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace YourApp.ViewModels;

public class TemperatureMonitorViewModel : INotifyPropertyChanged
{
    private double _currentTemperature = 22.0;
    private Timer _timer;

    public TemperatureMonitorViewModel()
    {
        // Simulate temperature changes
        _timer = new Timer(1000);
        _timer.Elapsed += (s, e) =>
        {
            CurrentTemperature += Random.Shared.Next(-2, 3) * 0.5;
        };
        _timer.Start();
    }

    public double CurrentTemperature
    {
        get => _currentTemperature;
        set
        {
            _currentTemperature = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### View

```xml
<UserControl xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:itk="http://xamldesignstudio.com"
             xmlns:evaluators="using:YourApp.Evaluators"
             x:Class="YourApp.Views.TemperatureMonitorView">

    <UserControl.Resources>
        <evaluators:NumericRangeStatusEvaluator x:Key="TempEvaluator"
                                                LowCritical="10"
                                                LowWarning="18"
                                                HighWarning="26"
                                                HighCritical="35"/>
    </UserControl.Resources>

    <UserControl.Styles>
        <Style Selector="itk|TextField[Status=Normal]">
            <Setter Property="Foreground" Value="#00C853"/>
        </Style>
        <Style Selector="itk|TextField[Status=TooLow]">
            <Setter Property="Foreground" Value="#2196F3"/>
            <Setter Property="FontWeight" Value="Bold"/>
        </Style>
        <Style Selector="itk|TextField[Status=TooHigh]">
            <Setter Property="Foreground" Value="#FF6F00"/>
            <Setter Property="FontWeight" Value="Bold"/>
        </Style>
        <Style Selector="itk|TextField[Status=Critical]">
            <Setter Property="Foreground" Value="#D50000"/>
            <Setter Property="FontWeight" Value="Bold"/>
            <Style.Animations>
                <Animation Duration="0:0:0.8" IterationCount="Infinite">
                    <KeyFrame Cue="0%"><Setter Property="Opacity" Value="1.0"/></KeyFrame>
                    <KeyFrame Cue="50%"><Setter Property="Opacity" Value="0.3"/></KeyFrame>
                    <KeyFrame Cue="100%"><Setter Property="Opacity" Value="1.0"/></KeyFrame>
                </Animation>
            </Style.Animations>
        </Style>
    </UserControl.Styles>

    <StackPanel Spacing="10">
        <TextBlock Text="실시간 온도 모니터링" FontSize="18" FontWeight="Bold"/>
        
        <itk:TextField Label="현재 온도" 
                       Value="{Binding CurrentTemperature}" 
                       Suffix="°C"
                       NumberFormat="F1"
                       StatusEvaluator="{StaticResource TempEvaluator}"
                       LabelWidth="100"
                       ValueWidth="80"/>
        
        <Border Padding="10" Background="#F5F5F5" CornerRadius="4">
            <StackPanel Spacing="5">
                <TextBlock Text="상태 범위:" FontWeight="Bold"/>
                <TextBlock Text="• Critical: &lt; 10°C 또는 &gt; 35°C" Foreground="#D50000"/>
                <TextBlock Text="• TooLow: 10°C ~ 18°C" Foreground="#2196F3"/>
                <TextBlock Text="• Normal: 18°C ~ 26°C" Foreground="#00C853"/>
                <TextBlock Text="• TooHigh: 26°C ~ 35°C" Foreground="#FF6F00"/>
            </StackPanel>
        </Border>
    </StackPanel>
</UserControl>
```

---

## Future Application to Other Controls

The Field Status System is designed to be **control-agnostic**. In the future, it can be applied to:

### Potential Control Types

1. **CircularGauge** - Visual gauge with color zones
```xml
<itk:CircularGauge Value="{Binding Speed}"
                   StatusEvaluator="{StaticResource SpeedEvaluator}"/>
```

2. **LinearGauge** - Bar gauge with threshold markers
```xml
<itk:LinearGauge Value="{Binding Pressure}"
                 StatusEvaluator="{StaticResource PressureEvaluator}"/>
```

3. **Chart Series** - Dynamic series coloring
```xml
<itk:LineSeries Values="{Binding DataPoints}"
                StatusEvaluator="{StaticResource TrendEvaluator}"/>
```

4. **LED Indicator** - Multi-color status light
```xml
<itk:LedIndicator Value="{Binding SystemState}"
                  StatusEvaluator="{StaticResource StateEvaluator}"/>
```

### Implementation Pattern

For new controls, add the same three properties:

```csharp
public class YourControl : TemplatedControl
{
    public static readonly StyledProperty<IStatusEvaluator?> StatusEvaluatorProperty = ...;
    public static readonly StyledProperty<object?> StatusEvaluatorParameterProperty = ...;
    public static readonly StyledProperty<string?> StatusProperty = ...;
    
    // Property change handlers
    static YourControl()
    {
        ValueProperty.Changed.AddClassHandler<YourControl>((x, e) => x.UpdateStatus());
        StatusEvaluatorProperty.Changed.AddClassHandler<YourControl>((x, e) => x.UpdateStatus());
        StatusEvaluatorParameterProperty.Changed.AddClassHandler<YourControl>((x, e) => x.UpdateStatus());
    }
    
    private void UpdateStatus()
    {
        if (StatusEvaluator != null)
            Status = StatusEvaluator.Evaluate(Value, StatusEvaluatorParameter);
        else
            Status = Value == null ? "NoValue" : "Normal";
    }
}
```

---

## Best Practices

### 1. Consistent Status Names

Use standard status names across your application for consistent styling:
```csharp
public static class AppStatus
{
    public const string Normal = "Normal";
    public const string Warning = "Warning";
    public const string Critical = "Critical";
    // ... etc
}
```

### 2. Reusable Evaluators

Register evaluators as resources for reuse:
```xml
<Application.Resources>
    <evaluators:NumericRangeStatusEvaluator x:Key="StandardTempEvaluator"
                                            LowWarning="18" HighWarning="26"/>
</Application.Resources>
```

### 3. Performance

- StatusEvaluator logic should be lightweight (called on every value change)
- Avoid heavy computations or I/O operations in Evaluate()
- Cache complex calculations in the evaluator instance

### 4. Null Safety

Always handle null values in your evaluators:
```csharp
public string? Evaluate(object? value, object? parameter)
{
    if (value == null) return "NoValue";
    // ... rest of logic
}
```

### 5. Type Safety

Check value types before conversion:
```csharp
if (value is not double doubleValue)
    return "InvalidValue";
```

---

## Troubleshooting

### Status Not Updating

**Problem**: Status property doesn't change when value changes.

**Solutions**:
- Verify `StatusEvaluator` is set correctly
- Check that value property implements `INotifyPropertyChanged`
- Ensure evaluator's `Evaluate()` method is returning different values

### Style Not Applied

**Problem**: Status selector doesn't apply the style.

**Solutions**:
- Verify the selector syntax: `[Status=Critical]` (no quotes around property name)
- Check status string matches exactly (case-sensitive)
- Ensure style is defined in the correct scope (Window/UserControl resources)

### Performance Issues

**Problem**: UI lags when values change rapidly.

**Solutions**:
- Simplify evaluator logic
- Consider throttling value updates in the ViewModel
- Use static evaluator instances (don't create new ones on each evaluation)

---

## See Also

- [TextField Control Documentation](TextField.md)
- [Avalonia Style Selectors](https://docs.avaloniAui.net/docs/basics/user-interface/styling)
- [Avalonia Animations](https://docs.avaloniAui.net/docs/animations/keyframe-animations)
