# Input ViewModels Guide

## Overview

This guide covers the usage of `TextInputVm` (string input) and `NumericInputVm<T>` (numeric input) for handling user input in forms and settings screens.

## Architecture

### Design Philosophy

Input ViewModels are **separated from display ViewModels** to follow the Single Responsibility Principle:

```
XamlDS.Itk.ViewModels
├── Fields (Display only)
│   ├── FieldVm (abstract)
│   ├── TextFieldVm<T>
│   └── NumericFieldVm<T>
│
├── Inputs (Input/Edit)
│   ├── InputVm (abstract)
│   ├── TextInputVm (string input)
│   └── NumericInputVm<T> (numeric input)
│
└── Gauges (Monitoring)
    └── GaugeVm
```

### Why Separate?

| Feature | FieldVm (Display) | InputVm (Input) |
|---------|-------------------|-----------------|
| **Purpose** | Show data | Accept user input |
| **Value Display** | ✅ Formatter | ✅ Formatter |
| **Status** | ✅ Status | ✅ HasError |
| **Unit** | ✅ Unit | ✅ Unit |
| **Validation** | ❌ | ✅ Required |
| **Error Message** | ❌ | ✅ Required |
| **Placeholder** | ❌ | ✅ Needed |
| **Two-way Binding** | ❌ One-way | ✅ Two-way |
| **Undo/Redo** | ❌ | ✅ Needed |
| **Dirty Flag** | ❌ | ✅ Needed |

---

## InputVm Base Class

```csharp
public abstract class InputVm : ViewModelBase
{
    public string Name { get; }           // Unique identifier
    public string Label { get; set; }     // Display label
    public string? ErrorMessage { get; set; }  // Validation error
    public bool HasError { get; set; }    // Has validation error
    public bool IsDirty { get; set; }     // Has unsaved changes
    
    public abstract bool Validate();      // Validate current value
    public abstract void Reset();         // Reset to original value
}
```

---

## TextInputVm<T>

### Features

- ✅ Generic type support
- ✅ Formatter support
- ✅ Placeholder text
- ✅ Custom validation (`Func<T?, bool>`)
- ✅ Custom error message provider
- ✅ Automatic IsDirty tracking
- ✅ Initialize/Commit/Reset pattern

### Basic Usage

```csharp
// Create text input
var nameInput = new TextInputVm<string>("NAME")
{
    Label = "Device Name",
    Placeholder = "e.g., Device-001",
    Validator = value => !string.IsNullOrWhiteSpace(value),
    ErrorMessageProvider = _ => "Device name is required"
};

// Initialize with original value
nameInput.Initialize("Device-001");

// User changes value
nameInput.Value = "Device-002";  // IsDirty = true

// Validate
if (nameInput.Validate())
{
    nameInput.Commit();  // Save and mark as clean
}
else
{
    Console.WriteLine(nameInput.ErrorMessage);
}

// Reset to original
nameInput.Reset();  // Value = "Device-001", IsDirty = false
```

### DateTime Input Example

```csharp
var dateInput = new TextInputVm<DateTime>("START_DATE",
    new DateTimeFormatter("yyyy-MM-dd"))
{
    Label = "Start Date",
    Validator = value => value != null && value > DateTime.Now,
    ErrorMessageProvider = value => $"Start date must be in the future"
};

dateInput.Initialize(DateTime.Now.AddDays(1));
```

### Enum Input Example

```csharp
public enum DeviceType
{
    Sensor,
    Actuator,
    Controller
}

var typeInput = new TextInputVm<DeviceType>("DEVICE_TYPE",
    new EnumFormatter<DeviceType>())
{
    Label = "Device Type",
    Validator = value => value != null
};

typeInput.Initialize(DeviceType.Sensor);
```

---

## NumericInputVm<T>

### Features

- ✅ INumber<T> constraint (numeric types only)
- ✅ Unit support
- ✅ Prefix/Suffix
- ✅ MinValue/MaxValue range validation
- ✅ Custom validation
- ✅ Custom error message provider
- ✅ Automatic IsDirty tracking
- ✅ Initialize/Commit/Reset pattern

### Basic Usage

```csharp
// Temperature input with range validation
var tempInput = new NumericInputVm<double>(
    "TEMP_MAX", 
    NumericUnit.Celsius,
    new DecimalPlacesFormatter<double> { DecimalPlaces = 1 })
{
    Label = "Maximum Temperature",
    Placeholder = "10~50",
    MinValue = 10.0,
    MaxValue = 50.0,
    Suffix = "°C"
};

tempInput.Initialize(30.0);
tempInput.Value = 55.0;  // Out of range

if (!tempInput.Validate())
{
    Console.WriteLine(tempInput.ErrorMessage);
    // "Value must be less than or equal to 50"
}

tempInput.Value = 35.5;  // Valid
if (tempInput.Validate())
{
    tempInput.Commit();
}
```

### Integer Input Example

```csharp
var countInput = new NumericInputVm<int>("MAX_DEVICES", NumericUnit.Dimensionless)
{
    Label = "Maximum Devices",
    Placeholder = "1~100",
    MinValue = 1,
    MaxValue = 100
};

countInput.Initialize(50);
```

### Custom Validation Example

```csharp
// Even numbers only
var evenInput = new NumericInputVm<int>("EVEN_NUM", NumericUnit.Dimensionless)
{
    Label = "Even Number Input",
    CustomValidator = value => value != null && value % 2 == 0,
    ErrorMessageProvider = value => $"{value} is not an even number"
};

evenInput.Initialize(10);
evenInput.Value = 11;  // Odd number

if (!evenInput.Validate())
{
    Console.WriteLine(evenInput.ErrorMessage);  // "11 is not an even number"
}
```

### Multiple of 5 Example

```csharp
var multipleInput = new NumericInputVm<int>("MULTIPLE", NumericUnit.Dimensionless)
{
    Label = "Multiple of 5",
    MinValue = 0,
    MaxValue = 100,
    CustomValidator = value => value != null && value % 5 == 0,
    ErrorMessageProvider = value => $"{value} must be a multiple of 5"
};
```

---

## Common Patterns

### 1. Initialize → Edit → Validate → Commit/Reset

```csharp
// 1. Initialize: Set original value
input.Initialize(value);

// 2. Edit: User changes value
input.Value = newValue;  // IsDirty = true

// 3. Validate: Check validity
if (input.Validate())
{
    // 4a. Commit: Save successful
    input.Commit();  // IsDirty = false
}
else
{
    // 4b. Reset: Cancel changes
    input.Reset();  // Restore to original
}
```

### 2. Form-Level Validation

```csharp
public bool ValidateForm()
{
    var inputs = new[] { input1, input2, input3 };
    return inputs.All(i => i.Validate());
}

public bool HasUnsavedChanges()
{
    var inputs = new[] { input1, input2, input3 };
    return inputs.Any(i => i.IsDirty);
}
```

### 3. Form ViewModel Pattern

```csharp
public class SettingsViewModel : ViewModelBase
{
    public NumericInputVm<double> MaxTemperature { get; }
    public NumericInputVm<double> MaxPressure { get; }
    public TextInputVm<string> DeviceName { get; }

    public SettingsViewModel()
    {
        MaxTemperature = new NumericInputVm<double>("MAX_TEMP", NumericUnit.Celsius)
        {
            Label = "Maximum Temperature",
            MinValue = 10.0,
            MaxValue = 50.0
        };

        MaxPressure = new NumericInputVm<double>("MAX_PRESS", NumericUnit.Bar)
        {
            Label = "Maximum Pressure",
            MinValue = 0.5,
            MaxValue = 2.0
        };

        DeviceName = new TextInputVm<string>("DEVICE_NAME")
        {
            Label = "Device Name",
            Validator = value => !string.IsNullOrWhiteSpace(value)
        };
    }

    public void LoadSettings(Settings settings)
    {
        MaxTemperature.Initialize(settings.MaxTemp);
        MaxPressure.Initialize(settings.MaxPress);
        DeviceName.Initialize(settings.Name);
    }

    public bool SaveSettings()
    {
        // Validate all inputs
        if (!MaxTemperature.Validate() || 
            !MaxPressure.Validate() || 
            !DeviceName.Validate())
        {
            return false;
        }

        // Save
        var settings = new Settings
        {
            MaxTemp = MaxTemperature.Value!.Value,
            MaxPress = MaxPressure.Value!.Value,
            Name = DeviceName.Value!
        };

        SaveToDatabase(settings);
        
        // Commit after successful save
        MaxTemperature.Commit();
        MaxPressure.Commit();
        DeviceName.Commit();

        return true;
    }

    public void Cancel()
    {
        MaxTemperature.Reset();
        MaxPressure.Reset();
        DeviceName.Reset();
    }

    public bool CanSave()
    {
        return MaxTemperature.IsDirty || 
               MaxPressure.IsDirty || 
               DeviceName.IsDirty;
    }
}
```

---

## Advanced Scenarios

### Dependent Validation

```csharp
public class RangeInputViewModel : ViewModelBase
{
    public NumericInputVm<double> MinValue { get; }
    public NumericInputVm<double> MaxValue { get; }

    public RangeInputViewModel()
    {
        MinValue = new NumericInputVm<double>("MIN", NumericUnit.Celsius)
        {
            Label = "Minimum",
            CustomValidator = value => value == null || MaxValue.Value == null || value < MaxValue.Value,
            ErrorMessageProvider = _ => "Min must be less than Max"
        };

        MaxValue = new NumericInputVm<double>("MAX", NumericUnit.Celsius)
        {
            Label = "Maximum",
            CustomValidator = value => value == null || MinValue.Value == null || value > MinValue.Value,
            ErrorMessageProvider = _ => "Max must be greater than Min"
        };

        // Re-validate on changes
        MinValue.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(MinValue.Value))
                MaxValue.Validate();
        };

        MaxValue.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(MaxValue.Value))
                MinValue.Validate();
        };
    }
}
```

### Async Validation

```csharp
public class AsyncTextInputVm<T> : TextInputVm<T>
{
    private Func<T?, Task<bool>>? _asyncValidator;

    public Func<T?, Task<bool>>? AsyncValidator
    {
        get => _asyncValidator;
        set => _asyncValidator = value;
    }

    public async Task<bool> ValidateAsync()
    {
        // First, run synchronous validation
        if (!Validate())
            return false;

        // Then run async validation
        if (_asyncValidator != null)
        {
            bool isValid = await _asyncValidator(_value);
            HasError = !isValid;
            if (!isValid)
            {
                ErrorMessage = ErrorMessageProvider?.Invoke(_value) ?? "Validation failed";
            }
            return isValid;
        }

        return true;
    }
}

// Usage: Check if username is available
var usernameInput = new AsyncTextInputVm<string>("USERNAME")
{
    Label = "Username",
    Validator = value => !string.IsNullOrWhiteSpace(value),
    AsyncValidator = async value => await CheckUsernameAvailableAsync(value),
    ErrorMessageProvider = value => $"Username '{value}' is already taken"
};
```

---

## Best Practices

### 1. Always Initialize

```csharp
// ✅ Good
var input = new NumericInputVm<int>("COUNT", NumericUnit.Dimensionless);
input.Initialize(0);  // Set original value

// ❌ Bad
var input = new NumericInputVm<int>("COUNT", NumericUnit.Dimensionless);
input.Value = 0;  // IsDirty will be true!
```

### 2. Validate Before Save

```csharp
// ✅ Good
if (input.Validate())
{
    Save(input.Value);
    input.Commit();
}

// ❌ Bad
Save(input.Value);  // May save invalid data!
```

### 3. Handle Unsaved Changes

```csharp
public void OnNavigatingAway()
{
    if (HasUnsavedChanges())
    {
        var result = ShowConfirmDialog("You have unsaved changes. Discard?");
        if (result == DialogResult.Yes)
        {
            ResetAll();
        }
        else
        {
            CancelNavigation();
        }
    }
}
```

### 4. Provide Clear Error Messages

```csharp
// ✅ Good
var input = new NumericInputVm<int>("AGE", NumericUnit.Dimensionless)
{
    MinValue = 18,
    MaxValue = 100,
    ErrorMessageProvider = value => 
    {
        if (value < 18) return "Must be at least 18 years old";
        if (value > 100) return "Age must be less than 100";
        return "Invalid age";
    }
};

// ❌ Bad
var input = new NumericInputVm<int>("AGE", NumericUnit.Dimensionless)
{
    MinValue = 18,
    MaxValue = 100
    // Generic error message only
};
```

---

## Testing Examples

### Unit Test: TextInputVm

```csharp
[Test]
public void TextInputVm_ValidValue_PassesValidation()
{
    // Arrange
    var input = new TextInputVm("TEST")
    {
        Validator = value => !string.IsNullOrWhiteSpace(value)
    };
    input.Initialize("Original");

    // Act
    input.Value = "Valid";
    bool isValid = input.Validate();

    // Assert
    Assert.IsTrue(isValid);
    Assert.IsFalse(input.HasError);
    Assert.IsNull(input.ErrorMessage);
}

[Test]
public void TextInputVm_InvalidValue_FailsValidation()
{
    // Arrange
    var input = new TextInputVm("TEST")
    {
        Validator = value => !string.IsNullOrWhiteSpace(value),
        ErrorMessageProvider = _ => "Value is required"
    };
    input.Initialize("Original");

    // Act
    input.Value = "";
    bool isValid = input.Validate();

    // Assert
    Assert.IsFalse(isValid);
    Assert.IsTrue(input.HasError);
    Assert.AreEqual("Value is required", input.ErrorMessage);
}

[Test]
public void TextInputVm_Reset_RestoresOriginalValue()
{
    // Arrange
    var input = new TextInputVm("TEST");
    input.Initialize("Original");
    input.Value = "Changed";

    // Act
    input.Reset();

    // Assert
    Assert.AreEqual("Original", input.Value);
    Assert.IsFalse(input.IsDirty);
}
```

### Unit Test: NumericInputVm

```csharp
[Test]
public void NumericInputVm_ValueInRange_PassesValidation()
{
    // Arrange
    var input = new NumericInputVm<double>("TEST", NumericUnit.Dimensionless)
    {
        MinValue = 0.0,
        MaxValue = 100.0
    };
    input.Initialize(50.0);

    // Act
    input.Value = 75.0;
    bool isValid = input.Validate();

    // Assert
    Assert.IsTrue(isValid);
    Assert.IsFalse(input.HasError);
}

[Test]
public void NumericInputVm_ValueOutOfRange_FailsValidation()
{
    // Arrange
    var input = new NumericInputVm<double>("TEST", NumericUnit.Dimensionless)
    {
        MinValue = 0.0,
        MaxValue = 100.0
    };
    input.Initialize(50.0);

    // Act
    input.Value = 150.0;
    bool isValid = input.Validate();

    // Assert
    Assert.IsFalse(isValid);
    Assert.IsTrue(input.HasError);
    Assert.IsNotNull(input.ErrorMessage);
}

[Test]
public void NumericInputVm_Commit_UpdatesOriginalValue()
{
    // Arrange
    var input = new NumericInputVm<int>("TEST", NumericUnit.Dimensionless);
    input.Initialize(10);
    input.Value = 20;

    // Act
    input.Commit();

    // Assert
    Assert.AreEqual(20, input.OriginalValue);
    Assert.IsFalse(input.IsDirty);
}
```

---

## Summary

### When to Use

| Scenario | ViewModel |
|----------|-----------|
| Display sensor data | `NumericFieldVm<T>` |
| Display status text | `TextFieldVm<T>` |
| Monitor with thresholds | `GaugeVm` |
| **Settings form** | **`NumericInputVm<T>`** |
| **User input** | **`TextInputVm<T>`** |
| **Data entry** | **`NumericInputVm<T>` / `TextInputVm<T>`** |

### Key Features

- ✅ **Validation**: Built-in and custom
- ✅ **Error Handling**: Clear error messages
- ✅ **Dirty Tracking**: Detect unsaved changes
- ✅ **Undo Support**: Reset to original
- ✅ **Type Safety**: Generic with constraints
- ✅ **Testable**: Easy to unit test

### Remember

1. Always `Initialize()` before use
2. `Validate()` before saving
3. `Commit()` after successful save
4. `Reset()` to cancel changes
5. Check `IsDirty` for unsaved changes
