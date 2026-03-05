# ITK Dependency Injection Integration

## Overview

This document describes how ITK's **Preset System** integrates with **Dependency Injection (DI)** frameworks using the **Factory Pattern**. This approach allows developers to use ITK in both simple prototyping scenarios (direct Preset usage) and enterprise applications (DI-based Factory pattern).

## Architecture Strategy

```
Preset (Static Helper) → Factory (DI-friendly) → ViewModel Instance with Dependencies
```

## Two-Tier Approach

### Tier 1: Preset System (No DI Required)

**Best for**: Quick prototyping, testing, documentation examples

```csharp
// Direct preset usage - no DI needed
var field = TextFieldPresets.Currency();
field.Label = "Price";
field.Value = product.Price;
```

**Advantages**:
- ✅ Zero boilerplate
- ✅ Perfect for learning and prototyping
- ✅ No DI container setup required

**Limitations**:
- ⚠️ No dependency injection
- ⚠️ Manual service wiring
- ⚠️ Not suitable for enterprise scenarios with logging, configuration services, etc.

### Tier 2: Factory Pattern (DI-Enabled)

**Best for**: Production applications, enterprise systems, testability

```csharp
// Factory-based creation with full DI support
public class OrderViewModel
{
    private readonly ITextFieldViewModelFactory _factory;
    
    public OrderViewModel(ITextFieldViewModelFactory factory)
    {
        _factory = factory;
        
        // Factory handles DI, logging, configuration
        PriceField = _factory.CreateCurrency();
        PriceField.Label = "Price";
    }
    
    public TextFieldViewModel PriceField { get; }
}
```

**Advantages**:
- ✅ Full dependency injection support
- ✅ Automatic logging, configuration, validation services
- ✅ Testable (mock factories)
- ✅ Consistent with enterprise patterns

## Implementation Guide

### Step 1: Define Factory Interface

```csharp
/// <summary>
/// Factory interface for creating TextFieldViewModel instances with dependency injection support
/// </summary>
public interface ITextFieldViewModelFactory
{
    /// <summary>
    /// Create a ViewModel configured for currency input
    /// </summary>
    TextFieldViewModel CreateCurrency();
    
    /// <summary>
    /// Create a ViewModel configured for integer input
    /// </summary>
    TextFieldViewModel CreateInteger();
    
    /// <summary>
    /// Create a ViewModel configured for percentage input
    /// </summary>
    TextFieldViewModel CreatePercentage();
    
    /// <summary>
    /// Create a ViewModel configured for email input
    /// </summary>
    TextFieldViewModel CreateEmail();
    
    /// <summary>
    /// Create a ViewModel configured for phone number input
    /// </summary>
    TextFieldViewModel CreatePhoneNumber();
    
    /// <summary>
    /// Create a ViewModel using a preset type
    /// </summary>
    /// <param name="presetType">The type of preset to use</param>
    TextFieldViewModel Create(TextFieldPresetType presetType);
    
    /// <summary>
    /// Create a default ViewModel without preset configuration
    /// </summary>
    TextFieldViewModel CreateDefault();
}

/// <summary>
/// Enum defining available preset types
/// </summary>
public enum TextFieldPresetType
{
    Currency,
    Integer,
    Percentage,
    Email,
    PhoneNumber,
    Custom
}
```

### Step 2: Implement Factory

```csharp
/// <summary>
/// Factory implementation that creates TextFieldViewModel instances with injected dependencies
/// </summary>
public class TextFieldViewModelFactory : ITextFieldViewModelFactory
{
    private readonly ILogger<TextFieldViewModel> _logger;
    private readonly IConfigurationService _configService;
    private readonly IValidationService _validationService;
    
    public TextFieldViewModelFactory(
        ILogger<TextFieldViewModel> logger,
        IConfigurationService configService,
        IValidationService validationService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }
    
    public TextFieldViewModel CreateCurrency()
    {
        // Use Preset as template
        var viewModel = TextFieldPresets.Currency();
        
        // Inject dependencies
        InjectDependencies(viewModel);
        
        // Log creation
        _logger.LogDebug("Created Currency TextFieldViewModel");
        
        return viewModel;
    }
    
    public TextFieldViewModel CreateInteger()
    {
        var viewModel = TextFieldPresets.Integer();
        InjectDependencies(viewModel);
        _logger.LogDebug("Created Integer TextFieldViewModel");
        return viewModel;
    }
    
    public TextFieldViewModel CreatePercentage()
    {
        var viewModel = TextFieldPresets.Percentage();
        InjectDependencies(viewModel);
        _logger.LogDebug("Created Percentage TextFieldViewModel");
        return viewModel;
    }
    
    public TextFieldViewModel CreateEmail()
    {
        var viewModel = TextFieldPresets.Email();
        InjectDependencies(viewModel);
        _logger.LogDebug("Created Email TextFieldViewModel");
        return viewModel;
    }
    
    public TextFieldViewModel CreatePhoneNumber()
    {
        var viewModel = TextFieldPresets.PhoneNumber();
        InjectDependencies(viewModel);
        _logger.LogDebug("Created PhoneNumber TextFieldViewModel");
        return viewModel;
    }
    
    public TextFieldViewModel Create(TextFieldPresetType presetType)
    {
        return presetType switch
        {
            TextFieldPresetType.Currency => CreateCurrency(),
            TextFieldPresetType.Integer => CreateInteger(),
            TextFieldPresetType.Percentage => CreatePercentage(),
            TextFieldPresetType.Email => CreateEmail(),
            TextFieldPresetType.PhoneNumber => CreatePhoneNumber(),
            TextFieldPresetType.Custom => CreateDefault(),
            _ => throw new ArgumentException($"Unknown preset type: {presetType}", nameof(presetType))
        };
    }
    
    public TextFieldViewModel CreateDefault()
    {
        var viewModel = new TextFieldViewModel();
        InjectDependencies(viewModel);
        _logger.LogDebug("Created default TextFieldViewModel");
        return viewModel;
    }
    
    private void InjectDependencies(TextFieldViewModel viewModel)
    {
        // Inject logging
        viewModel.Logger = _logger;
        
        // Inject configuration service
        viewModel.ConfigurationService = _configService;
        
        // Inject validation service
        viewModel.ValidationService = _validationService;
        
        // Initialize modules with services
        viewModel.Validation?.Initialize(_validationService);
        viewModel.Configuration?.Initialize(_configService);
    }
}
```

### Step 3: Register Services in DI Container

#### ASP.NET Core / Generic Host

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register ITK core services
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IValidationService, ValidationService>();
        
        // Register ViewModel factories
        services.AddTransient<ITextFieldViewModelFactory, TextFieldViewModelFactory>();
        services.AddTransient<ICompressedTimelineChartViewModelFactory, CompressedTimelineChartViewModelFactory>();
        
        // Register application ViewModels
        services.AddTransient<PatientMonitorViewModel>();
        services.AddTransient<OrderEntryViewModel>();
    }
}
```

#### WPF with Microsoft.Extensions.DependencyInjection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    
    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(builder =>
        {
            builder.AddDebug();
            builder.AddConsole();
        });
        
        // ITK services
        services.AddItkServices();
        
        // Application ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<PatientMonitorViewModel>();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var mainWindow = new MainWindow
        {
            DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
        };
        mainWindow.Show();
    }
}
```

#### AvaloniaUI with Microsoft.Extensions.DependencyInjection

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

public class App : Application
{
    private IServiceProvider? _serviceProvider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }
        
        base.OnFrameworkInitializationCompleted();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddItkServices();
        services.AddTransient<MainWindowViewModel>();
    }
}
```

### Step 4: Extension Methods for Easy Registration

```csharp
/// <summary>
/// Extension methods for registering ITK services in DI container
/// </summary>
public static class ItkServiceCollectionExtensions
{
    /// <summary>
    /// Registers all ITK ViewModels, factories, and core services
    /// </summary>
    public static IServiceCollection AddItkServices(this IServiceCollection services)
    {
        // Core infrastructure
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IValidationService, ValidationService>();
        
        // Logging (if not already registered)
        services.AddLogging();
        
        // ViewModel factories
        services.AddTransient<ITextFieldViewModelFactory, TextFieldViewModelFactory>();
        services.AddTransient<ICompressedTimelineChartViewModelFactory, CompressedTimelineChartViewModelFactory>();
        
        // Generic factory (optional)
        services.AddTransient(typeof(IItkViewModelFactory<>), typeof(ItkViewModelFactory<>));
        
        return services;
    }
    
    /// <summary>
    /// Registers ITK services with custom configuration
    /// </summary>
    public static IServiceCollection AddItkServices(
        this IServiceCollection services, 
        Action<ItkServiceOptions> configure)
    {
        var options = new ItkServiceOptions();
        configure(options);
        
        services.AddSingleton(options);
        services.AddItkServices();
        
        return services;
    }
}

/// <summary>
/// Configuration options for ITK services
/// </summary>
public class ItkServiceOptions
{
    public bool EnableAutoConfiguration { get; set; } = true;
    public bool EnableValidation { get; set; } = true;
    public bool EnableLogging { get; set; } = true;
    public string ConfigurationBasePath { get; set; } = "./config";
}
```

## Usage Examples

### Example 1: Simple Application (No DI)

```csharp
// Program.cs or ViewModel constructor
public class SimpleViewModel
{
    public TextFieldViewModel PriceField { get; }
    public TextFieldViewModel QuantityField { get; }
    
    public SimpleViewModel()
    {
        // Quick and simple - use presets directly
        PriceField = TextFieldPresets.Currency();
        PriceField.Label = "Price";
        
        QuantityField = TextFieldPresets.Integer();
        QuantityField.Label = "Quantity";
    }
}
```

### Example 2: Enterprise Application (DI + Factory)

```csharp
public class PatientMonitorViewModel
{
    private readonly ITextFieldViewModelFactory _fieldFactory;
    private readonly ICompressedTimelineChartViewModelFactory _chartFactory;
    private readonly ILogger<PatientMonitorViewModel> _logger;
    
    public TextFieldViewModel HeartRateField { get; }
    public TextFieldViewModel BloodPressureField { get; }
    public TextFieldViewModel TemperatureField { get; }
    public CompressedTimelineChartViewModel EcgChart { get; }
    
    // Constructor injection
    public PatientMonitorViewModel(
        ITextFieldViewModelFactory fieldFactory,
        ICompressedTimelineChartViewModelFactory chartFactory,
        ILogger<PatientMonitorViewModel> logger)
    {
        _fieldFactory = fieldFactory ?? throw new ArgumentNullException(nameof(fieldFactory));
        _chartFactory = chartFactory ?? throw new ArgumentNullException(nameof(chartFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Create fields using factory
        HeartRateField = _fieldFactory.CreateInteger();
        HeartRateField.Label = "Heart Rate";
        HeartRateField.Suffix = "bpm";
        HeartRateField.Validation.MinValue = 40;
        HeartRateField.Validation.MaxValue = 200;
        
        BloodPressureField = _fieldFactory.CreateCustom();
        BloodPressureField.Label = "Blood Pressure";
        BloodPressureField.Suffix = "mmHg";
        
        TemperatureField = _fieldFactory.CreatePercentage();
        TemperatureField.Label = "Temperature";
        TemperatureField.Suffix = "°C";
        
        // Create chart using factory
        EcgChart = _chartFactory.CreateMedicalECG();
        
        _logger.LogInformation("PatientMonitorViewModel initialized");
    }
}
```

### Example 3: Unit Testing with Mock Factory

```csharp
using Moq;
using Xunit;

public class PatientMonitorViewModelTests
{
    [Fact]
    public void Constructor_CreatesAllFields()
    {
        // Arrange
        var mockFieldFactory = new Mock<ITextFieldViewModelFactory>();
        var mockChartFactory = new Mock<ICompressedTimelineChartViewModelFactory>();
        var mockLogger = new Mock<ILogger<PatientMonitorViewModel>>();
        
        mockFieldFactory.Setup(f => f.CreateInteger())
            .Returns(new TextFieldViewModel());
        mockFieldFactory.Setup(f => f.CreateCustom())
            .Returns(new TextFieldViewModel());
        mockFieldFactory.Setup(f => f.CreatePercentage())
            .Returns(new TextFieldViewModel());
        mockChartFactory.Setup(f => f.CreateMedicalECG())
            .Returns(new CompressedTimelineChartViewModel());
        
        // Act
        var viewModel = new PatientMonitorViewModel(
            mockFieldFactory.Object,
            mockChartFactory.Object,
            mockLogger.Object);
        
        // Assert
        Assert.NotNull(viewModel.HeartRateField);
        Assert.NotNull(viewModel.BloodPressureField);
        Assert.NotNull(viewModel.TemperatureField);
        Assert.NotNull(viewModel.EcgChart);
        
        mockFieldFactory.Verify(f => f.CreateInteger(), Times.Once);
        mockFieldFactory.Verify(f => f.CreateCustom(), Times.Once);
        mockFieldFactory.Verify(f => f.CreatePercentage(), Times.Once);
    }
}
```

## Advanced Pattern: Generic Factory

### Interface Definition

```csharp
/// <summary>
/// Generic factory interface for creating any ITK ViewModel type
/// </summary>
/// <typeparam name="TViewModel">ViewModel type to create</typeparam>
public interface IItkViewModelFactory<TViewModel> where TViewModel : ItkViewModelBase
{
    /// <summary>
    /// Create a ViewModel instance with optional preset
    /// </summary>
    /// <param name="presetName">Optional preset name to apply</param>
    TViewModel Create(string? presetName = null);
    
    /// <summary>
    /// Create multiple ViewModel instances
    /// </summary>
    /// <param name="count">Number of instances to create</param>
    IEnumerable<TViewModel> CreateMany(int count);
}
```

### Generic Implementation

```csharp
/// <summary>
/// Generic factory implementation supporting any ITK ViewModel type
/// </summary>
public class ItkViewModelFactory<TViewModel> : IItkViewModelFactory<TViewModel> 
    where TViewModel : ItkViewModelBase, new()
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TViewModel> _logger;
    
    public ItkViewModelFactory(
        IServiceProvider serviceProvider,
        ILogger<TViewModel> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public TViewModel Create(string? presetName = null)
    {
        TViewModel viewModel;
        
        if (!string.IsNullOrEmpty(presetName))
        {
            // Use reflection to call preset method
            viewModel = CreateFromPreset(presetName);
        }
        else
        {
            // Create default instance
            viewModel = new TViewModel();
        }
        
        // Inject dependencies
        InjectServices(viewModel);
        
        _logger.LogDebug("Created {ViewModelType} with preset: {PresetName}", 
            typeof(TViewModel).Name, 
            presetName ?? "none");
        
        return viewModel;
    }
    
    public IEnumerable<TViewModel> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }
    
    private TViewModel CreateFromPreset(string presetName)
    {
        // Find preset provider class (e.g., TextFieldPresets)
        var presetProviderType = typeof(TViewModel).Assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == $"{typeof(TViewModel).Name.Replace("ViewModel", "")}Presets");
            
        if (presetProviderType != null)
        {
            var method = presetProviderType.GetMethod(presetName, BindingFlags.Public | BindingFlags.Static);
            if (method != null && method.ReturnType == typeof(TViewModel))
            {
                var result = method.Invoke(null, null);
                if (result is TViewModel viewModel)
                {
                    return viewModel;
                }
            }
        }
        
        _logger.LogWarning("Preset {PresetName} not found for {ViewModelType}, using default", 
            presetName, 
            typeof(TViewModel).Name);
        
        return new TViewModel();
    }
    
    private void InjectServices(TViewModel viewModel)
    {
        // Inject logger
        viewModel.Logger = _logger;
        
        // Inject configuration service
        var configService = _serviceProvider.GetService<IConfigurationService>();
        if (configService != null)
        {
            viewModel.ConfigurationService = configService;
        }
        
        // Inject validation service
        var validationService = _serviceProvider.GetService<IValidationService>();
        if (validationService != null)
        {
            viewModel.ValidationService = validationService;
        }
    }
}
```

### Usage with Generic Factory

```csharp
public class DashboardViewModel
{
    public DashboardViewModel(
        IItkViewModelFactory<TextFieldViewModel> textFieldFactory,
        IItkViewModelFactory<CompressedTimelineChartViewModel> chartFactory)
    {
        // Create using generic factory
        var nameField = textFieldFactory.Create();
        var priceField = textFieldFactory.Create("Currency");
        
        var chart = chartFactory.Create("MedicalECG");
        
        // Create multiple instances
        var fields = textFieldFactory.CreateMany(5).ToList();
    }
}
```

## Best Practices

### 1. Choose the Right Approach

| Scenario | Recommended Approach |
|----------|---------------------|
| Prototyping, demos, learning | Direct Preset usage |
| Unit tests without external dependencies | Direct Preset usage |
| Small desktop applications | Direct Preset usage |
| Enterprise applications | Factory + DI |
| Applications requiring logging | Factory + DI |
| Applications with configuration management | Factory + DI |
| Unit tests with mocking | Factory + DI (with Moq) |

### 2. Hybrid Approach

```csharp
public class HybridViewModel
{
    // Simple fields: use presets directly
    public TextFieldViewModel SimpleField { get; } = TextFieldPresets.Integer();
    
    // Complex fields requiring services: use factory
    private readonly ITextFieldViewModelFactory _factory;
    
    public TextFieldViewModel ComplexField { get; }
    
    public HybridViewModel(ITextFieldViewModelFactory factory)
    {
        _factory = factory;
        ComplexField = _factory.CreateCurrency();
        
        // Configure both
        SimpleField.Label = "Simple";
        ComplexField.Label = "Complex";
    }
}
```

### 3. Factory Naming Convention

```
Control Name + "ViewModel" + "Factory"

Examples:
- TextFieldViewModelFactory
- CompressedTimelineChartViewModelFactory
- DataGridViewModelFactory
```

### 4. Service Registration Strategy

```csharp
// Register factories as Transient (new instance per request)
services.AddTransient<ITextFieldViewModelFactory, TextFieldViewModelFactory>();

// Register core services as Singleton (single instance)
services.AddSingleton<IConfigurationService, ConfigurationService>();
services.AddSingleton<IValidationService, ValidationService>();

// Register ViewModels as Transient (new instance per request)
services.AddTransient<PatientMonitorViewModel>();
```

## Framework-Specific Integration

### .NET MAUI

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        
        // Register ITK services
        builder.Services.AddItkServices();
        
        // Register pages and ViewModels
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
        
        return builder.Build();
    }
}
```

### Blazor

```csharp
// Program.cs
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        
        // Register ITK services
        builder.Services.AddItkServices();
        
        // Register ViewModels
        builder.Services.AddTransient<DashboardViewModel>();
        
        await builder.Build().RunAsync();
    }
}
```

## Troubleshooting

### Issue: Services Not Injected

**Problem**: ViewModel created but services are null

**Solution**: Ensure factory is registered in DI container

```csharp
// Missing registration
services.AddTransient<ITextFieldViewModelFactory, TextFieldViewModelFactory>();

// Also ensure core services are registered
services.AddSingleton<IConfigurationService, ConfigurationService>();
```

### Issue: Circular Dependency

**Problem**: DI container throws circular dependency error

**Solution**: Use lazy initialization or redesign dependency graph

```csharp
// Use Lazy<T> to break circular dependency
public class ViewModel
{
    private readonly Lazy<IOtherService> _otherService;
    
    public ViewModel(Lazy<IOtherService> otherService)
    {
        _otherService = otherService;
    }
}
```

### Issue: Factory Method Not Found

**Problem**: Generic factory can't find preset method

**Solution**: Ensure preset class naming follows convention

```
ViewModel: TextFieldViewModel
Preset Class: TextFieldPresets (remove "ViewModel", add "Presets")
Method: Currency() - matches preset name exactly
```

## Performance Considerations

### Factory Overhead

- **Transient Lifetime**: New instance per request (~1-5 µs overhead)
- **Scoped Lifetime**: One instance per scope (minimal overhead)
- **Singleton Lifetime**: Single instance (no overhead after first creation)

### Recommendation

```csharp
// ViewModels: Transient (typical)
services.AddTransient<ITextFieldViewModelFactory, TextFieldViewModelFactory>();

// Factories: Transient (lightweight)
services.AddTransient<IItkViewModelFactory<TextFieldViewModel>, ItkViewModelFactory<TextFieldViewModel>>();

// Core Services: Singleton (heavy initialization)
services.AddSingleton<IConfigurationService, ConfigurationService>();
```

## Conclusion

ITK's dual approach to ViewModel creation provides flexibility for all developer experience levels:

- **Beginners & Prototyping**: Direct Preset usage (zero DI complexity)
- **Enterprise & Production**: Factory Pattern with full DI support

This design philosophy ensures ITK remains accessible to newcomers while providing the robustness required for enterprise applications.

---

**Document Version**: 1.0  
**Created**: 2024  
**Last Updated**: 2024  
**Status**: Design Phase  
**Related Documents**: 
- [ITK View & ViewModel Pattern Design](./ITK-ViewModel-View-Pattern.md)
- [CompressedTimelineChart Design](./CompressedTimelineChart-Design.md)
