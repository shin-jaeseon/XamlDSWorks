# 런타임 언어 변경 가이드

## 개요

이 문서는 WPF와 Avalonia UI에서 애플리케이션 실행 중 사용자가 언어를 변경하면 즉시 UI에 반영되는 다국어 시스템 구현 가이드입니다.

## 아키텍처 개요

### 핵심 컴포넌트

```
📦 XamlDS.ITK (공통 라이브러리)
├─ 📄 LocalizationManager.cs        (전역 다국어 관리 - Singleton)
├─ 📄 LanguageInfo.cs                (언어 정보 모델)
└─ 📁 Resources/
   ├─ CommonStrings.resx
   ├─ CommonStrings.ko-KR.resx
   └─ ...

📦 XamlDS.ITK.WPF
└─ 📄 TranslateExtension.cs         (WPF MarkupExtension)

📦 XamlDS.ITK.Aui
└─ 📄 TranslateExtension.cs         (Avalonia MarkupExtension)

📦 앱 프로젝트
└─ 📄 AppSettingsVm.cs              (설정 ViewModel - Singleton DI)
```

### 동작 흐름

```
1. 앱 시작 → AppSettingsVm.CurrentLanguage 로드 (저장된 설정)
2. LocalizationManager.CurrentCulture 설정
3. UI 바인딩 자동 업데이트 (TranslateExtension)
4. 사용자가 언어 변경
5. AppSettingsVm.CurrentLanguage 변경
6. LocalizationManager.CurrentCulture 업데이트
7. CultureChanged 이벤트 발생
8. 모든 바인딩된 UI 요소 자동 업데이트
```

## 공통 코드 (XamlDS.ITK)

### 1. LanguageInfo.cs (언어 정보 모델)

```csharp
namespace XamlDS.ITK.Resources;

/// <summary>
/// Language information model
/// </summary>
public record LanguageInfo(string DisplayName, string CultureCode, string NativeName)
{
    public static LanguageInfo English { get; } = new("English", "en", "English");
    public static LanguageInfo Korean { get; } = new("Korean", "ko-KR", "한국어");
    public static LanguageInfo ChineseSimplified { get; } = new("Chinese (Simplified)", "zh-CN", "简体中文");
    public static LanguageInfo German { get; } = new("German", "de-DE", "Deutsch");
    public static LanguageInfo Japanese { get; } = new("Japanese", "ja-JP", "日本語");
    public static LanguageInfo Spanish { get; } = new("Spanish", "es", "Español");
    public static LanguageInfo French { get; } = new("French", "fr-FR", "Français");

    /// <summary>
    /// Get all supported languages
    /// </summary>
    public static IReadOnlyList<LanguageInfo> GetAllLanguages() => new[]
    {
        English,
        Korean,
        ChineseSimplified,
        German,
        Japanese,
        Spanish,
        French
    };

    /// <summary>
    /// Find language by culture code
    /// </summary>
    public static LanguageInfo? FindByCultureCode(string cultureCode)
    {
        return GetAllLanguages().FirstOrDefault(l => 
            l.CultureCode.Equals(cultureCode, StringComparison.OrdinalIgnoreCase));
    }
}
```

### 2. LocalizationManager.cs (핵심 다국어 관리)

```csharp
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace XamlDS.ITK.Resources;

/// <summary>
/// Global localization manager (Singleton)
/// Manages current culture and provides localized strings
/// </summary>
public sealed class LocalizationManager : INotifyPropertyChanged
{
    private static readonly Lazy<LocalizationManager> _instance = new(() => new LocalizationManager());
    private CultureInfo _currentCulture;
    private readonly Dictionary<string, ResourceManager> _resourceManagers;

    private LocalizationManager()
    {
        _currentCulture = CultureInfo.CurrentUICulture;
        _resourceManagers = new Dictionary<string, ResourceManager>
        {
            ["CommonStrings"] = CommonStrings.ResourceManager,
            ["Messages"] = Messages.ResourceManager,
            ["Validations"] = Validations.ResourceManager,
            ["HelpTexts"] = HelpTexts.ResourceManager
        };
    }

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static LocalizationManager Instance => _instance.Value;

    /// <summary>
    /// Current UI culture
    /// When changed, all bound UI elements are automatically updated
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (Equals(_currentCulture, value)) return;

            _currentCulture = value;

            // Update .NET culture
            CultureInfo.CurrentUICulture = value;
            CultureInfo.CurrentCulture = value;
            Thread.CurrentThread.CurrentUICulture = value;
            Thread.CurrentThread.CurrentCulture = value;

            // Notify all subscribers
            OnPropertyChanged();
            OnPropertyChanged(nameof(this[null!])); // Indexer changed
            CultureChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Event raised when culture changes
    /// </summary>
    public event EventHandler? CultureChanged;

    /// <summary>
    /// Indexer for binding in XAML
    /// </summary>
    public string this[string key]
    {
        get => GetString(key);
    }

    /// <summary>
    /// Get localized string by key
    /// Searches all resource managers
    /// </summary>
    public string GetString(string key, string? resourceSet = null)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        try
        {
            // If specific resource set is specified
            if (!string.IsNullOrEmpty(resourceSet) && _resourceManagers.TryGetValue(resourceSet, out var rm))
            {
                return rm.GetString(key, _currentCulture) ?? $"[{key}]";
            }

            // Search all resource managers
            foreach (var manager in _resourceManagers.Values)
            {
                var value = manager.GetString(key, _currentCulture);
                if (value != null)
                    return value;
            }

            return $"[{key}]"; // Key not found
        }
        catch
        {
            return $"[{key}]";
        }
    }

    /// <summary>
    /// Get formatted localized string with parameters
    /// </summary>
    public string GetString(string key, params object[] args)
    {
        var format = GetString(key);
        try
        {
            return string.Format(format, args);
        }
        catch
        {
            return format;
        }
    }

    /// <summary>
    /// Set culture by culture code
    /// </summary>
    public void SetCulture(string cultureCode)
    {
        try
        {
            CurrentCulture = new CultureInfo(cultureCode);
        }
        catch (CultureNotFoundException)
        {
            // Fallback to English
            CurrentCulture = new CultureInfo("en");
        }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
```

### 3. ViewModelBase.cs (MVVM 기본 클래스)

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamlDS.ITK;

/// <summary>
/// Base class for ViewModels
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
```

## WPF 구현

### 1. TranslateExtension.cs (WPF)

```csharp
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using XamlDS.ITK.Resources;

namespace XamlDS.ITK.WPF;

/// <summary>
/// WPF MarkupExtension for dynamic localization
/// Usage: {loc:Translate Key}
/// </summary>
[MarkupExtensionReturnType(typeof(BindingExpression))]
public class TranslateExtension : MarkupExtension
{
    private static readonly DependencyObject _dependencyObject = new();

    /// <summary>
    /// Resource key
    /// </summary>
    [ConstructorArgument("key")]
    public string Key { get; set; }

    /// <summary>
    /// Optional: Resource set name (e.g., "CommonStrings")
    /// </summary>
    public string? ResourceSet { get; set; }

    public TranslateExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Key))
            return string.Empty;

        // Create binding to LocalizationManager
        var binding = new Binding
        {
            Source = LocalizationManager.Instance,
            Path = new PropertyPath($"[{Key}]"),
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        // If design mode, return key for visibility
        if (DesignerProperties.GetIsInDesignMode(_dependencyObject))
            return $"[{Key}]";

        // Evaluate binding
        var expression = binding.ProvideValue(serviceProvider);
        return expression;
    }
}
```

### 2. App.xaml.cs (WPF 앱 초기화)

```csharp
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using XamlDS.ITK.Resources;

namespace XamlDS.DemoApp.WPF;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Setup Dependency Injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // Initialize localization
        var appSettings = _serviceProvider.GetRequiredService<AppSettingsVm>();
        LocalizationManager.Instance.SetCulture(appSettings.CurrentLanguage.CultureCode);

        // Show main window
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register Singleton ViewModels
        services.AddSingleton<AppSettingsVm>();

        // Register Windows
        services.AddTransient<MainWindow>();
    }
}
```

### 3. XAML 사용 예시

```xml
<Window x:Class="XamlDS.DemoApp.WPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:XamlDS.ITK.WPF;assembly=XamlDS.ITK.WPF"
        Title="{loc:Translate Title_MainWindow}"
        Height="450" Width="800">
    
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <!-- Language Selection -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="0,0,0,20">
            <TextBlock Text="{loc:Translate Label_Language}" 
                       VerticalAlignment="Center" 
                       Margin="0,0,10,0"/>
            <ComboBox ItemsSource="{Binding AllLanguages}"
                      SelectedItem="{Binding CurrentLanguage}"
                      DisplayMemberPath="NativeName"
                      MinWidth="150"/>
        </StackPanel>

        <!-- Main Content -->
        <StackPanel Grid.Row="1" Spacing="10">
            <!-- Title -->
            <TextBlock Text="{loc:Translate Title_WelcomeMessage}" 
                       FontSize="24" 
                       FontWeight="Bold"/>

            <!-- Buttons with localized content -->
            <StackPanel Orientation="Horizontal" Spacing="10">
                <Button Content="{loc:Translate Action_Save}" 
                        MinWidth="100" 
                        Command="{Binding SaveCommand}"/>
                <Button Content="{loc:Translate Action_Cancel}" 
                        MinWidth="100" 
                        Command="{Binding CancelCommand}"/>
                <Button Content="{loc:Translate Action_Delete}" 
                        MinWidth="100" 
                        Command="{Binding DeleteCommand}"/>
            </StackPanel>

            <!-- Status -->
            <TextBlock Text="{loc:Translate Status_Ready}" 
                       Foreground="Green"/>

            <!-- Information Message -->
            <TextBlock Text="{loc:Translate Info_ChangesSaved}" 
                       TextWrapping="Wrap"/>
        </StackPanel>
    </Grid>
</Window>
```

## Avalonia UI 구현

### 1. TranslateExtension.cs (Avalonia)

```csharp
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using XamlDS.ITK.Resources;

namespace XamlDS.ITK.Aui;

/// <summary>
/// Avalonia MarkupExtension for dynamic localization
/// Usage: {loc:Translate Key}
/// </summary>
public class TranslateExtension : MarkupExtension
{
    /// <summary>
    /// Resource key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Optional: Resource set name
    /// </summary>
    public string? ResourceSet { get; set; }

    public TranslateExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Key))
            return string.Empty;

        // Create binding to LocalizationManager
        var binding = new ReflectionBindingExtension($"[{Key}]")
        {
            Source = LocalizationManager.Instance,
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}
```

### 2. App.axaml.cs (Avalonia 앱 초기화)

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using XamlDS.ITK.Resources;

namespace XamlDS.DemoApp.Aui;

public class App : Application
{
    public static ServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Setup Dependency Injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        // Initialize localization
        var appSettings = Services.GetRequiredService<AppSettingsVm>();
        LocalizationManager.Instance.SetCulture(appSettings.CurrentLanguage.CultureCode);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register Singleton ViewModels
        services.AddSingleton<AppSettingsVm>();

        // Register Windows
        services.AddTransient<MainWindow>();
    }
}
```

### 3. AXAML 사용 예시

```xml
<Window xmlns="https://github.com/avaloniAui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:XamlDS.ITK.Aui;assembly=XamlDS.ITK.Aui"
        x:Class="XamlDS.DemoApp.Aui.MainWindow"
        Title="{loc:Translate Title_MainWindow}"
        Width="800" Height="450">

    <Grid Margin="20" RowDefinitions="Auto,*">

        <!-- Language Selection -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Spacing="10" Margin="0,0,0,20">
            <TextBlock Text="{loc:Translate Label_Language}" 
                       VerticalAlignment="Center"/>
            <ComboBox ItemsSource="{Binding AllLanguages}"
                      SelectedItem="{Binding CurrentLanguage}"
                      MinWidth="150">
                <ComboBox.ItemTemplate>
                    <DataTemplate>
                        <TextBlock Text="{Binding NativeName}"/>
                    </DataTemplate>
                </ComboBox.ItemTemplate>
            </ComboBox>
        </StackPanel>

        <!-- Main Content -->
        <StackPanel Grid.Row="1" Spacing="10">
            <!-- Title -->
            <TextBlock Text="{loc:Translate Title_WelcomeMessage}" 
                       FontSize="24" 
                       FontWeight="Bold"/>

            <!-- Buttons with localized content -->
            <StackPanel Orientation="Horizontal" Spacing="10">
                <Button Content="{loc:Translate Action_Save}" 
                        MinWidth="100" 
                        Command="{Binding SaveCommand}"/>
                <Button Content="{loc:Translate Action_Cancel}" 
                        MinWidth="100" 
                        Command="{Binding CancelCommand}"/>
                <Button Content="{loc:Translate Action_Delete}" 
                        MinWidth="100" 
                        Command="{Binding DeleteCommand}"/>
            </StackPanel>

            <!-- Status -->
            <TextBlock Text="{loc:Translate Status_Ready}" 
                       Foreground="Green"/>

            <!-- Information Message -->
            <TextBlock Text="{loc:Translate Info_ChangesSaved}" 
                       TextWrapping="Wrap"/>
        </StackPanel>
    </Grid>
</Window>
```

## AppSettingsVm 구현

```csharp
using System.Collections.ObjectModel;
using XamlDS.ITK;
using XamlDS.ITK.Resources;

namespace XamlDS.DemoApp;

/// <summary>
/// Application settings ViewModel (Singleton via DI)
/// </summary>
public class AppSettingsVm : ViewModelBase
{
    private LanguageInfo _currentLanguage;

    public AppSettingsVm()
    {
        // Load saved language or use system default
        var savedCultureCode = LoadSavedLanguage() ?? "en";
        _currentLanguage = LanguageInfo.FindByCultureCode(savedCultureCode) ?? LanguageInfo.English;

        AllLanguages = new ObservableCollection<LanguageInfo>(LanguageInfo.GetAllLanguages());
    }

    /// <summary>
    /// All supported languages
    /// </summary>
    public ObservableCollection<LanguageInfo> AllLanguages { get; }

    /// <summary>
    /// Currently selected language
    /// </summary>
    public LanguageInfo CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (SetProperty(ref _currentLanguage, value))
            {
                // Update LocalizationManager
                LocalizationManager.Instance.SetCulture(value.CultureCode);

                // Save to persistent storage
                SaveLanguage(value.CultureCode);

                // Update font if needed
                UpdateFontForLanguage(value.CultureCode);
            }
        }
    }

    /// <summary>
    /// Load saved language from settings
    /// </summary>
    private string? LoadSavedLanguage()
    {
        try
        {
            // Load from user settings (file, registry, database, etc.)
            // Example: return Properties.Settings.Default.Language;
            return null; // Default: system language
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Save language to persistent storage
    /// </summary>
    private void SaveLanguage(string cultureCode)
    {
        try
        {
            // Save to user settings
            // Example: Properties.Settings.Default.Language = cultureCode;
            // Example: Properties.Settings.Default.Save();
        }
        catch
        {
            // Handle error
        }
    }

    /// <summary>
    /// Update font family based on language (optional)
    /// </summary>
    private void UpdateFontForLanguage(string cultureCode)
    {
        // Example: Update global font based on language
        // var fontFamily = FontHelper.GetFontFallbackForCulture(cultureCode);
        // Application.Current.Resources["DefaultFontFamily"] = fontFamily;
    }
}
```

## 리소스 파일 예시

### CommonStrings.resx (영어)

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="Title_MainWindow" xml:space="preserve">
    <value>XamlDS Demo Application</value>
  </data>
  
  <data name="Title_WelcomeMessage" xml:space="preserve">
    <value>Welcome to XamlDS Toolkit</value>
  </data>
  
  <data name="Label_Language" xml:space="preserve">
    <value>Language:</value>
  </data>
  
  <data name="Action_Save" xml:space="preserve">
    <value>Save</value>
  </data>
  
  <data name="Action_Cancel" xml:space="preserve">
    <value>Cancel</value>
  </data>
  
  <data name="Action_Delete" xml:space="preserve">
    <value>Delete</value>
  </data>
  
  <data name="Status_Ready" xml:space="preserve">
    <value>Ready</value>
  </data>
  
  <data name="Info_ChangesSaved" xml:space="preserve">
    <value>Your changes have been saved successfully.</value>
  </data>
</root>
```

### CommonStrings.ko-KR.resx (한국어)

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="Title_MainWindow" xml:space="preserve">
    <value>XamlDS 데모 애플리케이션</value>
  </data>
  
  <data name="Title_WelcomeMessage" xml:space="preserve">
    <value>XamlDS 툴킷에 오신 것을 환영합니다</value>
  </data>
  
  <data name="Label_Language" xml:space="preserve">
    <value>언어:</value>
  </data>
  
  <data name="Action_Save" xml:space="preserve">
    <value>저장</value>
  </data>
  
  <data name="Action_Cancel" xml:space="preserve">
    <value>취소</value>
  </data>
  
  <data name="Action_Delete" xml:space="preserve">
    <value>삭제</value>
  </data>
  
  <data name="Status_Ready" xml:space="preserve">
    <value>준비됨</value>
  </data>
  
  <data name="Info_ChangesSaved" xml:space="preserve">
    <value>변경 사항이 성공적으로 저장되었습니다.</value>
  </data>
</root>
```

## C# 코드에서 사용

### ViewModel에서 동적 문자열 사용

```csharp
using XamlDS.ITK;
using XamlDS.ITK.Resources;

public class MainViewModel : ViewModelBase
{
    private string _statusMessage;

    public MainViewModel()
    {
        // Subscribe to culture change
        LocalizationManager.Instance.CultureChanged += OnCultureChanged;
        UpdateStatusMessage();
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    private void OnCultureChanged(object? sender, EventArgs e)
    {
        // Update all localized properties
        UpdateStatusMessage();
        OnPropertyChanged(nameof(Title));
    }

    private void UpdateStatusMessage()
    {
        StatusMessage = LocalizationManager.Instance.GetString("Status_Ready");
    }

    public string Title => LocalizationManager.Instance.GetString("Title_MainWindow");
}
```

### Command에서 메시지 표시

```csharp
using System.Windows;
using System.Windows.Input;
using XamlDS.ITK.Resources;

public class SaveCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        // Perform save operation
        DoSave();

        // Show localized success message
        var message = LocalizationManager.Instance.GetString("Info_ChangesSaved");
        var title = LocalizationManager.Instance.GetString("Title_Success");
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void DoSave()
    {
        // Save logic
    }
}
```

## 전체 동작 흐름

### 1. 앱 시작

```
App.OnStartup()
├─ ConfigureServices() - DI 설정
├─ AppSettingsVm 생성 (Singleton)
│  └─ LoadSavedLanguage() - 저장된 언어 로드 (예: "ko-KR")
├─ LocalizationManager.SetCulture("ko-KR")
│  └─ CurrentCulture 변경
│     ├─ CultureInfo.CurrentUICulture 업데이트
│     └─ CultureChanged 이벤트 발생
└─ MainWindow 생성 및 표시
   └─ XAML 바인딩 평가
      └─ {loc:Translate Key} → 한국어 문자열 표시
```

### 2. 언어 변경

```
사용자가 ComboBox에서 "English" 선택
├─ AppSettingsVm.CurrentLanguage 변경
│  └─ SetProperty() 호출
│     ├─ LocalizationManager.SetCulture("en")
│     │  └─ CurrentCulture 변경
│     │     ├─ CultureInfo.CurrentUICulture = "en"
│     │     ├─ OnPropertyChanged() 호출
│     │     └─ CultureChanged 이벤트 발생
│     ├─ SaveLanguage("en") - 설정 저장
│     └─ UpdateFontForLanguage("en") - 폰트 업데이트 (선택적)
└─ UI 자동 업데이트
   └─ 모든 {loc:Translate Key} 바인딩 재평가
      └─ 영어 문자열로 표시 변경
```

### 3. 바인딩 업데이트 메커니즘

```
LocalizationManager.CurrentCulture 변경
├─ OnPropertyChanged("CurrentCulture")
├─ OnPropertyChanged("Item[]") - Indexer 변경 알림
└─ CultureChanged 이벤트 발생

↓

WPF/Avalonia Binding System
├─ Binding {Source=LocalizationManager.Instance, Path=[Key]} 감지
├─ PropertyChanged 이벤트 수신
└─ UI 요소 업데이트
   └─ TextBlock.Text = "New translated value"
```

## 고급 기능

### 1. 언어 변경 확인 대화상자

```csharp
public class AppSettingsVm : ViewModelBase
{
    public LanguageInfo CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage == value) return;

            // Confirm language change
            var message = LocalizationManager.Instance.GetString("Prompt_LanguageChange");
            var result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                SetProperty(ref _currentLanguage, value);
                LocalizationManager.Instance.SetCulture(value.CultureCode);
                SaveLanguage(value.CultureCode);
            }
            else
            {
                // Revert selection
                OnPropertyChanged(nameof(CurrentLanguage));
            }
        }
    }
}
```

### 2. 언어별 날짜/시간 형식

```csharp
public class DateTimeHelper
{
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("G", LocalizationManager.Instance.CurrentCulture);
    }

    public static string FormatDate(DateTime date)
    {
        return date.ToString("d", LocalizationManager.Instance.CurrentCulture);
    }

    public static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C", LocalizationManager.Instance.CurrentCulture);
    }
}
```

### 3. 언어별 정렬

```csharp
public class LocalizedStringComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        return string.Compare(x, y, LocalizationManager.Instance.CurrentCulture, CompareOptions.None);
    }
}

// 사용 예시
var sortedList = items.OrderBy(x => x.Name, new LocalizedStringComparer()).ToList();
```

### 4. 애니메이션 효과와 함께 언어 변경

```csharp
// Avalonia UI 예시
public async Task ChangeLanguageWithAnimation(LanguageInfo newLanguage)
{
    // Fade out
    await Dispatcher.UIThread.InvokeAsync(async () =>
    {
        var fadeOut = new DoubleTransition
        {
            Duration = TimeSpan.FromMilliseconds(200),
            Property = OpacityProperty,
            Easing = new CubicEaseOut()
        };
        
        MainContent.Opacity = 0;
        await Task.Delay(200);

        // Change language
        LocalizationManager.Instance.SetCulture(newLanguage.CultureCode);

        // Fade in
        MainContent.Opacity = 1;
    });
}
```

### 5. 언어별 설정 페이지

```xml
<UserControl xmlns="https://github.com/avaloniAui"
             xmlns:loc="clr-namespace:XamlDS.ITK.Aui;assembly=XamlDS.ITK.Aui"
             x:Class="XamlDS.DemoApp.Aui.Views.SettingsView">
    
    <StackPanel Spacing="20" Margin="20">
        <!-- Language Settings Section -->
        <TextBlock Text="{loc:Translate Settings_Language_Title}" 
                   FontSize="18" 
                   FontWeight="Bold"/>
        
        <StackPanel Spacing="10">
            <TextBlock Text="{loc:Translate Settings_Language_Description}" 
                       TextWrapping="Wrap"
                       Foreground="Gray"/>
            
            <ComboBox ItemsSource="{Binding AllLanguages}"
                      SelectedItem="{Binding CurrentLanguage}"
                      MinWidth="200">
                <ComboBox.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Horizontal" Spacing="10">
                            <TextBlock Text="{Binding NativeName}" 
                                       FontWeight="Bold"/>
                            <TextBlock Text="{Binding DisplayName}" 
                                       Foreground="Gray"/>
                        </StackPanel>
                    </DataTemplate>
                </ComboBox.ItemTemplate>
            </ComboBox>
        </StackPanel>

        <!-- Font Settings -->
        <TextBlock Text="{loc:Translate Settings_Font_Title}" 
                   FontSize="18" 
                   FontWeight="Bold"/>
        
        <CheckBox Content="{loc:Translate Settings_UseSystemFont}"
                  IsChecked="{Binding UseSystemFont}"/>

        <!-- Restart Notice -->
        <Border Background="#FFF4E5" 
                Padding="10" 
                CornerRadius="4"
                IsVisible="{Binding LanguageChanged}">
            <StackPanel Orientation="Horizontal" Spacing="10">
                <PathIcon Data="{StaticResource InfoIcon}" 
                          Foreground="Orange"/>
                <TextBlock Text="{loc:Translate Settings_RestartRequired}" 
                           TextWrapping="Wrap"/>
            </StackPanel>
        </Border>
    </StackPanel>
</UserControl>
```

## 성능 최적화

### 1. 리소스 캐싱

```csharp
public sealed class LocalizationManager : INotifyPropertyChanged
{
    private readonly Dictionary<string, string> _cache = new();

    public string GetString(string key, string? resourceSet = null)
    {
        var cacheKey = $"{_currentCulture.Name}_{resourceSet}_{key}";

        if (_cache.TryGetValue(cacheKey, out var cached))
            return cached;

        var value = GetStringInternal(key, resourceSet);
        _cache[cacheKey] = value;
        return value;
    }

    private string GetStringInternal(string key, string? resourceSet)
    {
        // Actual resource loading logic
    }

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (Equals(_currentCulture, value)) return;

            _currentCulture = value;
            _cache.Clear(); // Clear cache on culture change

            // ... rest of the code
        }
    }
}
```

### 2. WeakReference를 사용한 메모리 관리

```csharp
public class TranslateExtension : MarkupExtension
{
    private static readonly ConditionalWeakTable<object, List<WeakReference>> _targetCache = new();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
        if (target?.TargetObject != null)
        {
            // Track target for cleanup
            TrackTarget(target.TargetObject);
        }

        // ... rest of the code
    }

    private void TrackTarget(object target)
    {
        if (!_targetCache.TryGetValue(target, out var list))
        {
            list = new List<WeakReference>();
            _targetCache.Add(target, list);
        }
        list.Add(new WeakReference(target));
    }
}
```

## 디버깅 및 테스트

### 1. 누락된 번역 탐지

```csharp
public sealed class LocalizationManager : INotifyPropertyChanged
{
    private readonly HashSet<string> _missingKeys = new();

    public string GetString(string key, string? resourceSet = null)
    {
        // ... existing code

        if (value == null)
        {
            if (_missingKeys.Add(key))
            {
                // Log missing translation
                Debug.WriteLine($"Missing translation: {key} for culture {_currentCulture.Name}");
            }
            return $"[{key}]";
        }

        return value;
    }

    public IReadOnlySet<string> GetMissingKeys() => _missingKeys;
}
```

### 2. 언어 전환 테스트 도구

```csharp
public class LanguageTester
{
    public static void TestAllLanguages()
    {
        var languages = LanguageInfo.GetAllLanguages();
        var keys = new[] { "Action_Save", "Action_Cancel", "Title_MainWindow" };

        foreach (var language in languages)
        {
            LocalizationManager.Instance.SetCulture(language.CultureCode);

            Debug.WriteLine($"\nTesting: {language.NativeName} ({language.CultureCode})");

            foreach (var key in keys)
            {
                var value = LocalizationManager.Instance.GetString(key);
                Debug.WriteLine($"  {key}: {value}");
            }
        }
    }
}
```

## 결론

### 핵심 구현 포인트

✅ **LocalizationManager (Singleton)**
- 전역 Culture 관리
- INotifyPropertyChanged 구현
- CultureChanged 이벤트 제공

✅ **TranslateExtension (MarkupExtension)**
- XAML에서 간단한 구문: `{loc:Translate Key}`
- 자동 업데이트 지원
- WPF/Avalonia 모두 동일한 사용법

✅ **AppSettingsVm 연동**
- Singleton DI 패턴
- CurrentLanguage 속성 변경 시 자동 적용
- 설정 저장/로드

✅ **즉시 UI 업데이트**
- Binding 메커니즘 활용
- 언어 변경 시 전체 UI 자동 갱신
- ViewModel도 CultureChanged 이벤트로 업데이트

### 사용 흐름 요약

```csharp
// 1. DI 설정
services.AddSingleton<AppSettingsVm>();

// 2. 앱 시작 시 초기화
var appSettings = serviceProvider.GetRequiredService<AppSettingsVm>();
LocalizationManager.Instance.SetCulture(appSettings.CurrentLanguage.CultureCode);

// 3. XAML에서 사용
// <TextBlock Text="{loc:Translate Action_Save}" />

// 4. 언어 변경 (자동으로 UI 업데이트됨)
appSettings.CurrentLanguage = LanguageInfo.Korean;
```

### 추가 권장사항

- ✅ 리소스 파일은 빌드 시 EmbeddedResource로 포함
- ✅ 모든 사용자 표시 문자열은 리소스 파일로 관리
- ✅ 하드코딩된 문자열 금지
- ✅ 번역 누락 검사 도구 활용
- ✅ 언어별 UI 테스트 수행

이 구조를 사용하면 런타임에 언어를 자유롭게 변경할 수 있으며, 모든 UI 요소가 즉시 업데이트됩니다! 🌍
