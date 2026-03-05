# ITK View & ViewModel Pattern Design

## Executive Summary

**ITK (Integrated Toolkit)** differentiates itself from traditional control libraries by providing **pre-built View & ViewModel pairs** instead of just controls. This approach dramatically reduces boilerplate code and accelerates development by providing ready-to-use, feature-complete components that developers can simply configure through property toggles.

## Core Philosophy

### Traditional Libraries (2010-2025)
```
Philosophy: Provide flexible controls, developers implement everything else
Result: High flexibility, high implementation cost, repetitive code
Framework Lock-in: Tightly coupled to specific UI framework
```

### ITK Approach (2026+)
```
Philosophy: Provide complete solutions, developers configure via toggles
Result: Lower flexibility, minimal implementation cost, zero repetitive code
Metaphor: LEGO blocks - assemble pre-built components
Framework Portability: Business logic in ViewModel, View adapters per framework
```

### Multi-Framework Architecture

ITK's View & ViewModel separation enables **UI Framework portability**:

```
┌─────────────────────────────────────────────────────────┐
│                  Application Business Logic             │
│                  (Framework-Independent)                │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│             TextFieldViewModel (ITK Provides)           │
│  • Configuration properties                             │
│  • Business logic                                       │
│  • Validation                                           │
│  • Data management                                      │
│  ⚡ Framework-agnostic code                             │
└─────────────────────────────────────────────────────────┘
                            ↓
        ┌───────────────────┴───────────────────┐
        ↓                                       ↓
┌──────────────────┐                  ┌──────────────────┐
│ TextFieldView    │                  │ TextFieldView    │
│ (WPF)            │                  │ (AvaloniaUI)     │
│                  │                  │                  │
│ • WPF-specific   │                  │ • Avalonia-      │
│   rendering      │                  │   specific       │
│ • XAML binding   │                  │   rendering      │
└──────────────────┘                  └──────────────────┘

        Future: Uno Platform, MAUI, Blazor, etc.
```

**Currently Supported UI Frameworks:**
- ✅ **WPF** (Windows Presentation Foundation)
- ✅ **AvaloniaUI** (Cross-platform XAML)

**Future Framework Support** (based on industry trends):
- 🔄 Uno Platform
- 🔄 .NET MAUI
- 🔄 Blazor (Web)
- 🔄 Other emerging frameworks

## Problem Statement

### What Developers Currently Face

When using traditional control libraries (DevExpress, Telerik, Syncfusion, etc.):

**Every time a developer uses a chart control, they must implement:**

1. ✗ ViewModel with 20+ properties for chart configuration
2. ✗ Configuration save/load logic
3. ✗ Settings dialog UI and binding logic
4. ✗ Data validation and error handling
5. ✗ State management (enabled/disabled features)
6. ✗ User preference persistence
7. ✗ Export/import functionality
8. ✗ Real-time data streaming logic
9. ✗ Interaction handling (zoom, pan, tooltip)
10. ✗ Integration with application settings system

**Result**: 500-1000 lines of repetitive boilerplate code per control instance

### Example: Implementing a Chart Configuration

**Traditional Approach** (~200+ lines)
```csharp
public class MyChartViewModel : ViewModelBase
{
    private TimeSpan _totalDuration;
    public TimeSpan TotalDuration 
    { 
        get => _totalDuration;
        set => SetProperty(ref _totalDuration, value);
    }
    
    private TimeSpan _realtimeDuration;
    public TimeSpan RealtimeDuration 
    { 
        get => _realtimeDuration;
        set => SetProperty(ref _realtimeDuration, value);
    }
    
    // ... 20+ more properties
    
    public void SaveConfiguration(string path)
    {
        var config = new ChartConfig
        {
            TotalDuration = this.TotalDuration,
            RealtimeDuration = this.RealtimeDuration,
            // ... serialize all properties
        };
        File.WriteAllText(path, JsonSerializer.Serialize(config));
    }
    
    public void LoadConfiguration(string path)
    {
        var config = JsonSerializer.Deserialize<ChartConfig>(
            File.ReadAllText(path)
        );
        this.TotalDuration = config.TotalDuration;
        this.RealtimeDuration = config.RealtimeDuration;
        // ... deserialize all properties
    }
    
    // ... validation logic
    // ... data management logic
    // ... interaction logic
}
```

**ITK Approach** (~10 lines)
```csharp
var chartViewModel = new CompressedTimelineChartViewModel
{
    EnableZoom = true,
    EnableGrid = true,
    SaveConfigurationOnChange = true,
    DataSource = myDataCollection
};

// Optional: Load saved configuration
chartViewModel.LoadConfiguration("myChart.config");
```

## ITK Solution: View & ViewModel Pairs

### What ITK Provides

For each control, ITK delivers:

```
1. Control (AvaloniaUI TemplatedControl)
   ├── Rendering logic
   ├── Visual states
   └── Default theme

2. View (UserControl)
   ├── Control instance
   ├── Pre-configured layout
   └── Standard UI patterns

3. ViewModel (ViewModelBase)
   ├── All configuration properties
   ├── Built-in save/load system
   ├── Feature modules (zoom, grid, export, etc.)
   ├── Validation logic
   ├── State management
   └── Event system
```

### Architecture Pattern

#### Multi-Framework Package Structure

```
┌──────────────────────────────────────────────────────────────┐
│           ITK Component Package (Multi-Framework)            │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  [ViewModel Layer] ⚡ FRAMEWORK-AGNOSTIC                      │
│  ├── TextFieldViewModel.cs (Shared across all frameworks)    │
│  ├── Feature Modules:                                         │
│  │   ├── ValidationModule.cs                                 │
│  │   ├── FormattingModule.cs                                 │
│  │   └── ConfigurationModule.cs                              │
│  └── Presets:                                                 │
│      ├── CurrencyFieldPreset                                 │
│      ├── DateFieldPreset                                      │
│      └── NumericFieldPreset                                   │
│                                                               │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  [WPF Package] 🖥️ XamlDS.Itk.Wpf                            │
│  ├── Controls/TextField.cs (WPF TemplatedControl)            │
│  ├── Themes/TextFieldThemes.xaml                             │
│  └── Views/TextFieldView.xaml (WPF UserControl)              │
│                                                               │
│  [AvaloniaUI Package] 🌐 XamlDS.Itk.Aui                      │
│  ├── Controls/TextField.cs (Avalonia TemplatedControl)       │
│  ├── Themes/TextFieldThemes.axaml                            │
│  └── Views/TextFieldView.axaml (Avalonia UserControl)        │
│                                                               │
│  [Future: Uno Platform Package] 📱 XamlDS.Itk.Uno            │
│  [Future: MAUI Package] 📱 XamlDS.Itk.Maui                   │
│  [Future: Blazor Package] 🌐 XamlDS.Itk.Blazor               │
│                                                               │
└──────────────────────────────────────────────────────────────┘
```

#### Dependency Flow

```
┌──────────────────────────────────────────────────────────────┐
│          Your Application (e.g., WPF App)                     │
└────────────────────────┬─────────────────────────────────────┘
                         │ References
                         ↓
┌──────────────────────────────────────────────────────────────┐
│           XamlDS.Itk.Wpf (WPF-specific Views)                │
└────────────────────────┬─────────────────────────────────────┘
                         │ References
                         ↓
┌──────────────────────────────────────────────────────────────┐
│     XamlDS.Itk.ViewModels (Framework-agnostic logic)         │
│     ⚡ This code works with ANY UI framework                 │
└──────────────────────────────────────────────────────────────┘

// To migrate to AvaloniaUI:
// 1. Change reference from XamlDS.Itk.Wpf → XamlDS.Itk.Aui
// 2. Update View namespaces in XAML
// 3. All ViewModel code remains unchanged!
```

## Key Differentiators from Traditional Libraries

### Comparison Matrix

| Feature | Traditional Libraries | ITK Approach |
|---------|----------------------|--------------|
| **What's Provided** | Control only | Control + View + ViewModel |
| **Configuration Management** | Developer implements | Built-in save/load system |
| **Settings UI** | Developer creates | Pre-built settings dialogs |
| **Feature Toggles** | Manual implementation | Property-based toggles |
| **Presets** | None | Industry-standard presets |
| **Learning Curve** | Control API + DIY patterns | Unified pattern across all controls |
| **Code Volume** | 500-1000 lines per control | 5-10 lines per control |
| **Maintenance** | Developer responsibility | ITK handles updates |
| **Customization** | Full control | Override-based extension |
| **Best Practices** | Developer decides | Built-in best practices |
| **UI Framework Lock-in** | Tightly coupled to framework | ViewModel is framework-agnostic |
| **Framework Migration** | Rewrite entire application | Minimal effort (View layer only) |
| **Supported Frameworks** | Single framework | WPF, AvaloniaUI, future: Uno, MAUI |

### DevExpress vs ITK Example

**DevExpress WPF (Traditional)**
```xml
<!-- Developer manually creates everything -->
<dxc:ChartControl ItemsSource="{Binding Data}">
    <dxc:ChartControl.Diagram>
        <dxc:XYDiagram2D>
            <!-- 50+ lines of XAML configuration -->
        </dxc:XYDiagram2D>
    </dxc:ChartControl.Diagram>
</dxc:ChartControl>

<!-- Developer implements: -->
<!-- - ViewModel with all properties -->
<!-- - Settings dialog -->
<!-- - Save/load logic -->
<!-- - Validation -->
```

**ITK Approach**
```xml
<!-- Everything pre-configured, developer just toggles -->
<itk:CompressedTimelineChartView 
    DataContext="{Binding ChartViewModel}"/>
```

```csharp
// ViewModel is pre-built with all features
ChartViewModel = new CompressedTimelineChartViewModel
{
    EnableZoom = true,
    EnableExport = true,
    AutoSaveConfiguration = true
};
```

## Implementation Strategy

### Phase 1: Core Infrastructure

**Build foundational systems used by all ITK ViewModels:**

```csharp
// Base class for all ITK ViewModels
public abstract class ItkViewModelBase : ViewModelBase
{
    // Built-in configuration management
    public void SaveConfiguration(string path) { /* ITK implements */ }
    public void LoadConfiguration(string path) { /* ITK implements */ }
    public void ExportPreset(string name) { /* ITK implements */ }
    
    // Built-in validation system
    protected ValidationModule Validation { get; }
    
    // Built-in logging
    protected ILogger Logger { get; }
    
    // Built-in undo/redo
    protected UndoRedoModule UndoRedo { get; }
}

// Configuration serialization system
public interface IConfigurable
{
    void ApplyConfiguration(Dictionary<string, object> config);
    Dictionary<string, object> GetConfiguration();
}

// Preset system
public interface IPresetProvider<T> where T : ItkViewModelBase
{
    T CreateFromPreset(string presetName);
    IEnumerable<string> GetAvailablePresets();
}
```

### Phase 2: Feature Module System

**Modular features to prevent "God Object" ViewModels:**

```csharp
public class CompressedTimelineChartViewModel : ItkViewModelBase
{
    // Core data (always present)
    public ObservableCollection<TimeSeriesPoint> DataSource { get; set; }
    
    // Feature modules (lazy-loaded when enabled)
    public ZoomModule Zoom { get; }
    public GridModule Grid { get; }
    public ExportModule Export { get; }
    public AlarmModule Alarms { get; }
    public TooltipModule Tooltip { get; }
    
    // Feature toggles
    public bool EnableZoom 
    { 
        get => Zoom.IsEnabled; 
        set => Zoom.IsEnabled = value; 
    }
}

// Example module
public class ZoomModule
{
    public bool IsEnabled { get; set; }
    public double MinZoom { get; set; } = 0.1;
    public double MaxZoom { get; set; } = 10.0;
    public double CurrentZoom { get; set; } = 1.0;
    public ZoomMode Mode { get; set; } = ZoomMode.MouseWheel;
    
    public void ZoomIn() { /* ... */ }
    public void ZoomOut() { /* ... */ }
    public void ResetZoom() { /* ... */ }
}
```

### Phase 3: Preset System

**Industry-standard presets for common scenarios:**

```csharp
public static class TextFieldPresets
{
    public static TextFieldViewModel Currency()
    {
        return new TextFieldViewModel
        {
            Validation = { IsEnabled = true },
            Formatting = 
            { 
                Type = FormatType.Currency,
                DecimalPlaces = 2,
                ThousandsSeparator = true
            },
            Prefix = "$",
            Alignment = TextAlignment.Right
        };
    }
    
    public static TextFieldViewModel PhoneNumber()
    {
        return new TextFieldViewModel
        {
            Validation = 
            { 
                IsEnabled = true,
                Pattern = @"^\d{3}-\d{3}-\d{4}$"
            },
            Formatting = 
            { 
                Mask = "###-###-####"
            }
        };
    }
    
    public static TextFieldViewModel Email() { /* ... */ }
    public static TextFieldViewModel Integer() { /* ... */ }
    public static TextFieldViewModel Percentage() { /* ... */ }
}

// Usage
var currencyField = TextFieldPresets.Currency();
currencyField.Label = "Price";
currencyField.DataSource = product.Price;
```

## Advantages

### 1. Development Speed ⚡

**80-90% reduction in code volume**

```csharp
// Traditional: ~50 lines for basic setup
// ITK: 5-10 lines

var field = new TextFieldViewModel
{
    EnableValidation = true,
    EnableFormatting = true,
    SaveOnChange = true
};
```

### 2. Consistency 🎯

**Same pattern across all controls**
- Learn once, apply everywhere
- Predictable API surface
- Consistent user experience

### 3. Maintainability 🔧

**ITK handles complexity**
- Bug fixes propagate automatically
- New features available via updates
- Best practices enforced
- Less developer-written code to maintain

### 4. Settings Management 💾

**Built-in configuration system**
- Automatic save/load
- User preference persistence
- Import/export presets
- Version migration

### 5. Best Practices 📚

**Embedded domain knowledge**
- Medical field presets (ECG, SpO2 ranges)
- Financial presets (currency, stock formats)
- Industrial presets (sensor calibrations)

### 6. Reduced Learning Curve 📖

**Standardized approach**
- Single documentation pattern
- Consistent property naming
- Unified module system
- Common troubleshooting

### 7. UI Framework Portability 🔄

**Switch UI frameworks with minimal effort**

Since business logic resides in ViewModels (framework-agnostic), developers can migrate between UI frameworks by only replacing the View layer.

**Example: Migrating from WPF to AvaloniaUI**

```csharp
// ViewModel code (UNCHANGED)
public class PatientMonitorViewModel
{
    public TextFieldViewModel HeartRateField { get; }
    public TextFieldViewModel BloodPressureField { get; }

    public PatientMonitorViewModel()
    {
        HeartRateField = TextFieldPresets.Integer();
        HeartRateField.Label = "Heart Rate";
        HeartRateField.Suffix = "bpm";

        // All business logic stays the same
    }
}
```

```xml
<!-- BEFORE: WPF View -->
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:itk="clr-namespace:XamlDS.Itk.Wpf.Views">
    <itk:TextFieldView DataContext="{Binding HeartRateField}"/>
</Window>

<!-- AFTER: AvaloniaUI View -->
<Window xmlns="https://github.com/avaloniaui"
        xmlns:itk="using:XamlDS.Itk.Aui.Views">
    <itk:TextFieldView DataContext="{Binding HeartRateField}"/>
</Window>
```

**Migration Effort**:
- ✅ ViewModel code: 0% change (same code)
- ✅ Business logic: 0% change (same code)
- ⚠️ View XAML: Namespace changes only
- ⚠️ UI-specific styling: Adapt to new framework's theme system

**Result**: 90%+ of codebase remains unchanged when switching frameworks

**Supported Migration Paths**:
```
WPF ←→ AvaloniaUI
  ↓         ↓
 Future: Uno Platform, MAUI, Blazor
```

**Why This Matters**:
- **Technology Evolution**: Adopt new frameworks as they emerge
- **Cross-Platform Needs**: Start on Windows (WPF), expand to macOS/Linux (AvaloniaUI)
- **Risk Mitigation**: Not locked into a single framework vendor
- **Future-Proofing**: Investment in ViewModel code is portable

## Challenges & Mitigation

### Challenge 1: Loss of Flexibility

**Problem**: Pre-built ViewModels may not cover all use cases

**Mitigation**: Three-tier extensibility

```csharp
// Tier 1: Property toggles (80% of cases)
viewModel.EnableZoom = true;
viewModel.Grid.LineStyle = DashStyle.Dot;

// Tier 2: Event hooks (15% of cases)
viewModel.BeforeRender += (s, e) => 
{
    // Custom logic before rendering
};

// Tier 3: Inheritance (5% of cases)
public class MyCustomViewModel : TextFieldViewModel
{
    protected override bool ValidateValue(string value)
    {
        // Custom validation logic
        return base.ValidateValue(value) && MyCustomRule(value);
    }
}
```

### Challenge 2: ViewModel Bloat

**Problem**: Feature-rich ViewModels can become too large

**Mitigation**: Module architecture

```csharp
// Features organized into modules
viewModel.Validation.IsEnabled = true;
viewModel.Formatting.DecimalPlaces = 2;
viewModel.Export.Format = ExportFormat.CSV;

// Modules are lazy-loaded
// Unused features don't consume memory
```

### Challenge 3: Learning ITK Patterns

**Problem**: Developers need to learn ITK-specific patterns

**Mitigation**: Progressive disclosure

```csharp
// Level 1: Beginners use presets
var field = TextFieldPresets.Currency();

// Level 2: Intermediate users toggle features
field.EnableValidation = true;
field.Validation.MinValue = 0;

// Level 3: Advanced users extend
public class MyViewModel : TextFieldViewModel { ... }
```

### Challenge 4: Breaking Changes

**Problem**: Updates to ITK ViewModels could break existing code

**Mitigation**: Versioning strategy

```csharp
// Semantic versioning
// Major: Breaking changes (rare)
// Minor: New features (backward compatible)
// Patch: Bug fixes (always safe)

// Deprecation warnings
[Obsolete("Use NewProperty instead. Will be removed in v3.0")]
public string OldProperty { get; set; }
```

## First Implementation: TextField

### Why TextField First?

✅ **Low Complexity**: Simple data model, minimal state  
✅ **High Usage**: Common control used in most apps  
✅ **Clear Scope**: Well-defined features and behaviors  
✅ **Easy Validation**: Pattern can be easily evaluated  
✅ **Quick Feedback**: Rapid prototyping possible  

### TextField Implementation Plan

#### Phase 1: TextFieldViewModel
```csharp
public class TextFieldViewModel : ItkViewModelBase
{
    // Core properties
    public string Label { get; set; }
    public string Value { get; set; }
    public string Prefix { get; set; }
    public string Suffix { get; set; }
    
    // Feature modules
    public ValidationModule Validation { get; }
    public FormattingModule Formatting { get; }
    public ConfigurationModule Configuration { get; }
    
    // Feature toggles
    public bool EnableValidation { get; set; }
    public bool EnableFormatting { get; set; }
    public bool EnableAutoSave { get; set; }
    
    // Events
    public event EventHandler<ValueChangedEventArgs> ValueChanged;
    public event EventHandler<ValidationEventArgs> ValidationFailed;
}
```

#### Phase 2: TextFieldView
```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:itk="http://xamldesignstudio.com"
             x:Class="XamlDS.Itk.Aui.Views.TextFieldView">
    
    <itk:TextField 
        Label="{Binding Label}"
        Value="{Binding Value}"
        Prefix="{Binding Prefix}"
        Suffix="{Binding Suffix}"
        Status="{Binding ValidationStatus}"/>
    
</UserControl>
```

#### Phase 3: Presets
```csharp
public static class TextFieldPresets
{
    public static TextFieldViewModel Currency() { ... }
    public static TextFieldViewModel Integer() { ... }
    public static TextFieldViewModel Percentage() { ... }
    public static TextFieldViewModel Email() { ... }
    public static TextFieldViewModel PhoneNumber() { ... }
}
```

### Success Criteria

✅ **Developer Feedback**: 5+ ITK users validate the pattern  
✅ **Code Reduction**: 80%+ less code vs traditional approach  
✅ **Performance**: No measurable overhead vs manual implementation  
✅ **Extensibility**: Common customization scenarios work smoothly  
✅ **Documentation**: Complete guide with 10+ examples  

### Evaluation Questions

After TextField implementation, evaluate:

1. **Usability**: Is the pattern intuitive for developers?
2. **Completeness**: Does it cover 80%+ of real-world use cases?
3. **Performance**: Any noticeable overhead?
4. **Extensibility**: Can developers customize when needed?
5. **Scalability**: Will this pattern work for complex controls?

## Rollout Strategy

### Phase 1: Pilot (TextField)
- Implement TextField View + ViewModel
- Create 5+ presets
- Write comprehensive documentation
- Gather feedback from early adopters

### Phase 2: Expansion (Simple Controls)
- Apply pattern to: Button, CheckBox, Slider
- Refine base infrastructure based on learnings
- Establish documentation templates

### Phase 3: Complex Controls
- CompressedTimelineChart (View + ViewModel)
- DataGrid (View + ViewModel)
- TreeView (View + ViewModel)

### Phase 4: Ecosystem
- Settings dialog generator
- Preset marketplace
- Code generators for custom ViewModels

## Documentation Requirements

For each ITK component, provide:

### 1. Quick Start Guide
```markdown
# TextField Quick Start

## Step 1: Add ViewModel
```csharp
var field = TextFieldPresets.Currency();
```

## Step 2: Bind to View
```xml
<itk:TextFieldView DataContext="{Binding MyField}"/>
```

## Step 3: Connect Data
```csharp
field.Value = product.Price;
```
```

### 2. Feature Toggle Reference
```markdown
| Property | Default | Description |
|----------|---------|-------------|
| EnableValidation | false | Enable input validation |
| EnableFormatting | false | Apply value formatting |
| EnableAutoSave | false | Auto-save on change |
```

### 3. Preset Catalog
```markdown
## Available Presets

- `Currency()`: Money values with $ prefix
- `Integer()`: Whole numbers only
- `Percentage()`: 0-100 with % suffix
- `PhoneNumber()`: XXX-XXX-XXXX format
```

### 4. Extension Guide
```markdown
## Custom Validation

```csharp
public class MyViewModel : TextFieldViewModel
{
    protected override bool ValidateValue(string value)
    {
        return value.Length > 5; // Custom rule
    }
}
```
```

### 5. Migration Guide
```markdown
## Migrating from TextField Control to TextFieldView

### Before (Control Only)
```csharp
// ~50 lines of ViewModel code
public class MyViewModel
{
    public string Label { get; set; }
    public string Value { get; set; }
    // ... validation logic
    // ... formatting logic
}
```

### After (ITK View + ViewModel)
```csharp
// 5 lines
var field = TextFieldPresets.Integer();
field.Label = "Age";
field.Value = person.Age;
```
```

## Target Audience Considerations

### For Library Developers
- Clear extension points
- Module contribution guidelines
- Preset contribution process

### For Application Developers
- Minimal learning curve
- Quick wins with presets
- Clear path to customization

### For End Users
- Consistent UI patterns
- Persistent preferences
- Familiar settings dialogs

## Success Metrics

### Quantitative
- **Development Time**: 80%+ reduction vs traditional approach
- **Code Volume**: 90%+ reduction in ViewModel boilerplate
- **Adoption Rate**: 60%+ of ITK users adopt View+ViewModel pattern
- **Bug Reports**: 50%+ fewer configuration-related bugs

### Qualitative
- **Developer Satisfaction**: 4.5+ stars in surveys
- **Documentation Quality**: Comprehensive coverage, high clarity
- **Community Engagement**: Active preset contributions

## Risk Mitigation

### Risk: Over-engineering
**Mitigation**: Start simple with TextField, iterate based on real feedback

### Risk: Performance Overhead
**Mitigation**: Benchmark early, optimize module loading, profile memory usage

### Risk: Developer Pushback
**Mitigation**: Make adoption optional, provide clear migration paths, document benefits

### Risk: Maintenance Burden
**Mitigation**: Automated testing, clear module boundaries, community contributions

## Conclusion

The ITK View & ViewModel pattern represents a paradigm shift in control library design:

- **Traditional Libraries**: "Here's a control, you do the rest"
- **ITK**: "Here's a complete solution, toggle what you need"

By providing pre-built, feature-rich ViewModels alongside Views and Controls, ITK eliminates thousands of lines of repetitive code and accelerates application development while maintaining extensibility for advanced scenarios.

**Next Steps**:
1. Implement TextFieldViewModel (pilot)
2. Create 5+ TextField presets
3. Write comprehensive documentation
4. Gather developer feedback
5. Iterate and expand to other controls

---

**Document Version**: 1.0  
**Created**: 2024  
**Last Updated**: 2024  
**Status**: Design Phase  
**Next Implementation**: TextFieldViewModel (Simple control pilot)  
**Future Discussion**: CompressedTimelineChartViewModel (Complex control application)
