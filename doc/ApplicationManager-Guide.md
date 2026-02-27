# ApplicationManager 사용 가이드

`ApplicationManager<TWindowView, TWindowVm>`는 Avalonia 데스크톱 애플리케이션의 초기화 및 생명주기 관리를 단순화하는 ITK 라이브러리의 핵심 클래스입니다.

## 목차
- [기본 사용법](#기본-사용법)
- [커스터마이징](#커스터마이징)
- [기존 프로젝트 마이그레이션](#기존-프로젝트-마이그레이션)
- [ApplicationManager 없이 사용](#applicationmanager-없이-사용)

---

## 기본 사용법

### 1. 가장 간단한 사용 예시

```csharp
using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using XamlDS.ITK.Aui;
using YourApp.ViewModels;
using YourApp.Views;

namespace YourApp;

public partial class App : Application
{
    private ApplicationManager<MainWindowView, MainWindowVm>? _applicationManager;
    
    public static IHost? Host { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _applicationManager = new ApplicationManager<MainWindowView, MainWindowVm>(this, Host);
        base.OnFrameworkInitializationCompleted();
    }
}
```

**이게 전부입니다!** ApplicationManager가 다음을 자동으로 처리합니다:
- ✅ 의존성 주입 컨테이너에서 서비스 가져오기
- ✅ ViewModel 생성 및 Window에 바인딩
- ✅ 언어 변경 메시지 구독
- ✅ RTL/LTR FlowDirection 자동 설정
- ✅ 애플리케이션 종료 시 리소스 정리

### 2. 필요한 DI 등록

`Program.cs`에서 필요한 서비스들을 등록해야 합니다:

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // ITK 필수 서비스
        services.AddSingleton<IMessenger, Messenger>();
        
        // 애플리케이션 ViewModel
        services.AddSingleton<MainWindowVm>();
        
        // 기타 서비스들...
    })
    .Build();

App.Host = host;
```

---

## 커스터마이징

ApplicationManager를 상속하여 동작을 커스터마이징할 수 있습니다.

### 예시 1: Window 설정 커스터마이징

```csharp
public class CustomApplicationManager<TWindowView, TWindowVm> 
    : ApplicationManager<TWindowView, TWindowVm>
    where TWindowView : Window, new()
    where TWindowVm : class
{
    public CustomApplicationManager(Application app, IHost? host) 
        : base(app, host) 
    { }

    protected override void ConfigureWindow(TWindowView window)
    {
        // 기본 Window 설정
        window.Title = "My Custom Application";
        window.Width = 1200;
        window.Height = 800;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}

// App.axaml.cs에서 사용
_applicationManager = new CustomApplicationManager<MainWindowView, MainWindowVm>(this, Host);
```

### 예시 2: ViewModel 생성 로직 커스터마이징

```csharp
protected override MainWindowVm CreateMainViewModel()
{
    // DI 대신 직접 생성
    var messenger = GetRequiredService<IMessenger>();
    var settingsService = GetRequiredService<ISettingsService>();
    
    return new MainWindowVm(messenger, settingsService)
    {
        Title = "Custom Title",
        IsInitialized = true
    };
}
```

### 예시 3: 언어 변경 처리 커스터마이징

```csharp
protected override void OnLanguageChanged(
    IClassicDesktopStyleApplicationLifetime desktop,
    LanguageChangedMessage message)
{
    // Window를 재생성하는 대신 FlowDirection만 변경
    if (desktop.MainWindow != null)
    {
        desktop.MainWindow.FlowDirection = GetCurrentFlowDirection();
    }
    
    // 또는 추가 로직 실행
    Console.WriteLine($"Language changed from {message.OldLanguage?.Name} to {message.NewLanguage.Name}");
    
    // 기본 동작 (Window 재생성)을 원하면 base 호출
    // base.OnLanguageChanged(desktop, message);
}
```

### 예시 4: 추가 메시지 구독

```csharp
protected override void SubscribeToMessages(IClassicDesktopStyleApplicationLifetime desktop)
{
    // 기본 구독 (언어 변경)
    base.SubscribeToMessages(desktop);
    
    // 추가 메시지 구독
    if (Messenger != null)
    {
        Messenger.Subscribe<ThemeChangedMessage>(msg =>
        {
            // 테마 변경 처리
        });
        
        Messenger.Subscribe<UserLogoutMessage>(msg =>
        {
            // 로그아웃 처리
        });
    }
}
```

---

## 기존 프로젝트 마이그레이션

기존 Avalonia 프로젝트에 ITK를 적용하는 단계별 가이드입니다.

### Before (기존 코드 ~80줄)

```csharp
public partial class App : Application
{
    private IMessenger? _messenger;
    private IDisposable? _languageChangedSubscription;
    public static IHost? Host { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (Host == null)
                throw new InvalidOperationException("Host is not initialized.");

            _messenger = Host.Services.GetService<IMessenger>() 
                ?? throw new InvalidOperationException("IMessenger is not registered.");
            
            var viewModel = Host.Services.GetService<MainWindowVm>() 
                ?? throw new InvalidOperationException("MainWindowVm is not registered.");

            desktop.MainWindow = new MainWindowView
            {
                FlowDirection = GetFlowDirection(),
                DataContext = viewModel
            };

            _languageChangedSubscription = _messenger.Subscribe<LanguageChangedMessage>(msg =>
            {
                RecreateMainWindow(desktop);
            });

            desktop.Exit += OnApplicationExit;
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void RecreateMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        var oldWindow = desktop.MainWindow;
        var dataContext = oldWindow?.DataContext;
        desktop.MainWindow = new MainWindowView
        {
            FlowDirection = GetFlowDirection(),
            DataContext = dataContext
        };
        desktop.MainWindow.Show();
        oldWindow?.Close();
    }

    private static FlowDirection GetFlowDirection()
    {
        return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
    }

    private void OnApplicationExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _languageChangedSubscription?.Dispose();
        _languageChangedSubscription = null;
    }
}
```

### After (ApplicationManager 사용 ~20줄)

```csharp
using XamlDS.ITK.Aui;

public partial class App : Application
{
    private ApplicationManager<MainWindowView, MainWindowVm>? _applicationManager;
    public static IHost? Host { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _applicationManager = new ApplicationManager<MainWindowView, MainWindowVm>(this, Host);
        base.OnFrameworkInitializationCompleted();
    }
}
```

**코드 감소: 75% ↓**

---

## ApplicationManager 없이 사용

기존 프로젝트나 특별한 초기화가 필요한 경우, ApplicationManager 없이도 ITK 라이브러리를 사용할 수 있습니다.

### 수동 초기화 예시

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.ITK.Messages;
using XamlDS.Services;

namespace YourApp;

public partial class App : Application
{
    private IMessenger? _messenger;
    private IDisposable? _languageChangedSubscription;
    public static IHost? Host { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Host 확인
            if (Host == null)
                throw new InvalidOperationException("Host is not initialized.");

            // 서비스 가져오기
            _messenger = Host.Services.GetRequiredService<IMessenger>();
            var viewModel = Host.Services.GetRequiredService<MainWindowVm>();

            // Window 생성
            desktop.MainWindow = new MainWindowView
            {
                FlowDirection = GetCurrentFlowDirection(),
                DataContext = viewModel
            };

            // 메시지 구독
            _languageChangedSubscription = _messenger.Subscribe<LanguageChangedMessage>(msg =>
            {
                RecreateMainWindow(desktop);
            });

            // 종료 이벤트 구독
            desktop.Exit += OnApplicationExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RecreateMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        var oldWindow = desktop.MainWindow;
        var dataContext = oldWindow?.DataContext;

        desktop.MainWindow = new MainWindowView
        {
            FlowDirection = GetCurrentFlowDirection(),
            DataContext = dataContext
        };
        desktop.MainWindow.Show();
        oldWindow?.Close();
    }

    private static FlowDirection GetCurrentFlowDirection()
    {
        return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
    }

    private void OnApplicationExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _languageChangedSubscription?.Dispose();
        _languageChangedSubscription = null;
    }
}
```

이 방식을 사용하면:
- ✅ ITK의 모든 기능 사용 가능
- ✅ 완전한 초기화 제어
- ✅ 단계별 마이그레이션 가능
- ❌ 보일러플레이트 코드 증가

---

## 권장 사항

| 상황 | 권장 방법 |
|------|----------|
| 새 프로젝트 시작 | **ApplicationManager 사용** |
| 표준 초기화 로직 | **ApplicationManager 사용** |
| 커스터마이징 필요 | **ApplicationManager 상속** |
| 기존 프로젝트 점진적 적용 | **수동 초기화** → 나중에 **ApplicationManager로 마이그레이션** |
| 매우 특수한 초기화 필요 | **수동 초기화** |

---

## FAQ

### Q: ApplicationManager를 사용하면 성능에 영향이 있나요?
**A:** 아니요. ApplicationManager는 단순히 초기화 로직을 캡슐화한 것으로 런타임 성능에 영향이 없습니다.

### Q: Window 타입이나 ViewModel 타입을 런타임에 결정할 수 있나요?
**A:** 제네릭 타입은 컴파일 타임에 결정됩니다. 런타임에 결정해야 한다면 수동 초기화를 사용하세요.

### Q: 여러 개의 Window를 관리할 수 있나요?
**A:** ApplicationManager는 메인 Window만 관리합니다. 추가 Window는 ViewModel이나 별도 서비스에서 관리하세요.

### Q: 테스트는 어떻게 하나요?
**A:** ApplicationManager를 상속하여 테스트용 Mock 객체를 주입하거나, 수동 초기화 방식으로 테스트하세요.

---

## 관련 문서

- [LanguageSettingsVm 사용 가이드](./LanguageSettings.md)
- [ITK 메시징 시스템](./Messaging.md)
- [의존성 주입 설정](./DependencyInjection.md)
