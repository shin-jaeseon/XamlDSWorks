# Messenger Pattern Guide for MVVM Applications

## 📋 목차

1. [개요](#개요)
2. [Messenger 패턴이란?](#messenger-패턴이란)
3. [MVVM에서의 활용](#mvvm에서의-활용)
4. [구현 예제](#구현-예제)
5. [실제 사용 시나리오](#실제-사용-시나리오)
6. [Best Practices](#best-practices)
7. [장단점](#장단점)

---

## 개요

Messenger 패턴(Event Aggregator 또는 Message Bus라고도 함)은 애플리케이션의 다양한 컴포넌트 간에 **느슨한 결합(Loose Coupling)**을 유지하면서 통신할 수 있게 해주는 디자인 패턴입니다.

### 왜 Messenger 패턴이 필요한가?

**문제 상황:**
```csharp
// ❌ 잘못된 예: ViewModel이 View를 직접 참조
public class LanguageSettingsVm
{
    private MainWindow _mainWindow;
    
    public void ChangeLanguage()
    {
        // MVVM 원칙 위반!
        _mainWindow.Recreate();
    }
}
```

**Messenger 패턴을 사용한 해결:**
```csharp
// ✅ 올바른 예: 메시지를 발행하고 구독자가 처리
public class LanguageSettingsVm
{
    private readonly IMessenger _messenger;
    
    public void ChangeLanguage()
    {
        // ViewModel은 View에 대해 알 필요 없음
        _messenger.Send(new LanguageChangedMessage());
    }
}
```

---

## Messenger 패턴이란?

### 핵심 개념

```
┌─────────────┐      Message      ┌──────────────┐      Message      ┌─────────────┐
│  Publisher  │ ───────────────> │   Messenger  │ ───────────────> │ Subscriber  │
│ (발행자)     │                   │  (중재자)     │                   │ (구독자)     │
└─────────────┘                   └──────────────┘                   └─────────────┘
```

- **Publisher (발행자)**: 메시지를 보내는 쪽
- **Messenger (중재자)**: 메시지를 중계하는 중앙 허브
- **Subscriber (구독자)**: 메시지를 받는 쪽

### 주요 특징

1. **발행자와 구독자가 서로를 알 필요가 없음**
2. **1:N 통신 가능** (하나의 메시지를 여러 구독자가 받을 수 있음)
3. **타입 안전성** (강타입 메시지 사용)

---

## MVVM에서의 활용

### MVVM 관심사 분리

```
┌────────────────────────────────────────────────────────────┐
│                        View Layer                           │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐              │
│  │ MainView │   │ Settings │   │ Dialog   │              │
│  └────┬─────┘   └────┬─────┘   └────┬─────┘              │
│       │              │              │                      │
└───────┼──────────────┼──────────────┼──────────────────────┘
        │              │              │
        │   Binding    │   Binding    │   Binding
        │              │              │
┌───────┼──────────────┼──────────────┼──────────────────────┐
│       ▼              ▼              ▼                       │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐              │
│  │ MainVM   │   │ Settings │   │ DialogVM │              │
│  │          │◄──┤    VM    │──►│          │              │
│  └──────────┘   └──────────┘   └──────────┘              │
│       ▲              │              ▲                       │
│       │    Messenger │              │                       │
│       └──────────────┴──────────────┘                       │
│                  ViewModel Layer                            │
└────────────────────────────────────────────────────────────┘
```

### 사용 시나리오

1. **ViewModel → View 통신** (예: Window 재생성)
2. **ViewModel → ViewModel 통신** (예: 설정 변경 알림)
3. **Cross-cutting concerns** (예: 로딩 상태, 에러 처리)

---

## 구현 예제

### 1단계: IMessenger 인터페이스 정의

```csharp
// Services/IMessenger.cs
namespace XamlDS.Services;

/// <summary>
/// Provides a messaging service for loosely coupled communication between components.
/// </summary>
public interface IMessenger
{
    /// <summary>
    /// Sends a message to all registered subscribers.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to send.</typeparam>
    /// <param name="message">The message instance to send.</param>
    void Send<TMessage>(TMessage message);

    /// <summary>
    /// Subscribes to messages of a specific type.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to subscribe to.</typeparam>
    /// <param name="handler">The handler to invoke when a message is received.</param>
    /// <returns>A subscription token that can be used to unsubscribe.</returns>
    IDisposable Subscribe<TMessage>(Action<TMessage> handler);
}
```

### 2단계: Messenger 구현

```csharp
// Services/Messenger.cs
namespace XamlDS.Services;

public class Messenger : IMessenger
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly object _lock = new();

    public void Send<TMessage>(TMessage message)
    {
        List<Delegate>? handlers;
        
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.TryGetValue(messageType, out handlers))
                return;
            
            // Copy to avoid modification during iteration
            handlers = handlers.ToList();
        }

        foreach (var handler in handlers.Cast<Action<TMessage>>())
        {
            try
            {
                handler(message);
            }
            catch (Exception ex)
            {
                // Log error but continue processing other handlers
                Console.Error.WriteLine($"Error in message handler: {ex.Message}");
            }
        }
    }

    public IDisposable Subscribe<TMessage>(Action<TMessage> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<Delegate>();
            }
            _subscribers[messageType].Add(handler);
        }

        return new Subscription<TMessage>(this, handler);
    }

    private void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (_subscribers.TryGetValue(messageType, out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    _subscribers.Remove(messageType);
                }
            }
        }
    }

    private class Subscription<TMessage> : IDisposable
    {
        private readonly Messenger _messenger;
        private readonly Action<TMessage> _handler;
        private bool _disposed;

        public Subscription(Messenger messenger, Action<TMessage> handler)
        {
            _messenger = messenger;
            _handler = handler;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _messenger.Unsubscribe(_handler);
                _disposed = true;
            }
        }
    }
}
```

### 3단계: 메시지 정의

```csharp
// Messages/WindowMessages.cs
namespace XamlDS.Messages;

/// <summary>
/// Reason for requesting window recreation.
/// </summary>
public enum WindowRecreateReason
{
    LanguageChanged,
    ThemeChanged,
    ScalingChanged,
    SettingsChanged
}

/// <summary>
/// Message requesting the main window to be recreated.
/// </summary>
/// <param name="Reason">The reason for recreation.</param>
public record RecreateWindowMessage(WindowRecreateReason Reason);

/// <summary>
/// Message indicating language has changed.
/// </summary>
/// <param name="CultureName">The new culture name (e.g., "en-US").</param>
public record LanguageChangedMessage(string CultureName);

/// <summary>
/// Message requesting to show a notification.
/// </summary>
/// <param name="Title">Notification title.</param>
/// <param name="Message">Notification message.</param>
/// <param name="Type">Notification type (Info, Warning, Error).</param>
public record ShowNotificationMessage(string Title, string Message, NotificationType Type);

public enum NotificationType
{
    Info,
    Warning,
    Error,
    Success
}
```

---

## 실제 사용 시나리오

### 시나리오 1: 언어 변경 시 Window 재생성

#### ViewModel에서 메시지 발행

```csharp
// ViewModels/LanguageSettingsVm.cs
using XamlDS.Messages;
using XamlDS.Services;

namespace XamlDS.ITK.ViewModels;

public sealed class LanguageSettingsVm : ViewModelBase
{
    private readonly IMessenger _messenger;
    private CultureInfo? _currentLanguage;

    public LanguageSettingsVm(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public CultureInfo? CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (SetProperty(ref _currentLanguage, value))
            {
                ApplyLanguage();
            }
        }
    }

    private void ApplyLanguage()
    {
        if (CurrentLanguage == null)
            return;

        // Apply culture settings
        Thread.CurrentThread.CurrentCulture = CurrentLanguage;
        Thread.CurrentThread.CurrentUICulture = CurrentLanguage;
        CultureInfo.DefaultThreadCurrentCulture = CurrentLanguage;
        CultureInfo.DefaultThreadCurrentUICulture = CurrentLanguage;

        // Notify that language has changed
        _messenger.Send(new LanguageChangedMessage(CurrentLanguage.Name));
        
        // Request window recreation
        _messenger.Send(new RecreateWindowMessage(WindowRecreateReason.LanguageChanged));
    }
}
```

#### App.axaml.cs에서 메시지 구독

```csharp
// App.axaml.cs
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using XamlDS.Messages;
using XamlDS.Services;

namespace XamlDS.DemoApp.Aui;

public partial class App : Application
{
    public static IHost? Host { get; set; }
    private IDisposable? _windowRecreateSubscription;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (Host == null)
                throw new InvalidOperationException("Host is not set.");

            // Subscribe to window recreation messages
            var messenger = Host.Services.GetRequiredService<IMessenger>();
            _windowRecreateSubscription = messenger.Subscribe<RecreateWindowMessage>(msg =>
            {
                RecreateMainWindow(desktop, msg.Reason);
            });

            var mainWindow = new AppWindowView
            {
                DataContext = Host.Services.GetRequiredService<AppWindowVm>()
            };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RecreateMainWindow(IClassicDesktopStyleApplicationLifetime desktop, 
        WindowRecreateReason reason)
    {
        if (Host == null || desktop.MainWindow == null)
            return;

        var oldWindow = desktop.MainWindow;
        var dataContext = oldWindow.DataContext;

        // Create new window with updated resources
        desktop.MainWindow = Host.Services.GetRequiredService<AppWindowView>();
        desktop.MainWindow.DataContext = dataContext;
        desktop.MainWindow.Show();
        
        oldWindow.Close();

        Console.WriteLine($"Window recreated: {reason}");
    }

    // Clean up subscription on app exit
    public override void OnShutdown()
    {
        _windowRecreateSubscription?.Dispose();
        base.OnShutdown();
    }
}
```

### 시나리오 2: ViewModel 간 통신

```csharp
// ViewModels/UserProfileVm.cs
public class UserProfileVm : ViewModelBase
{
    private readonly IMessenger _messenger;

    public UserProfileVm(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public void SaveProfile()
    {
        // Save logic...
        
        // Notify other ViewModels
        _messenger.Send(new UserProfileUpdatedMessage(CurrentUser));
    }
}

// ViewModels/DashboardVm.cs
public class DashboardVm : ViewModelBase
{
    private readonly IDisposable _subscription;

    public DashboardVm(IMessenger messenger)
    {
        // Subscribe to profile updates
        _subscription = messenger.Subscribe<UserProfileUpdatedMessage>(msg =>
        {
            // Update dashboard with new user info
            UpdateUserInfo(msg.User);
        });
    }

    private void UpdateUserInfo(User user)
    {
        // Update UI properties
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(UserAvatar));
    }

    // Don't forget to dispose subscription
    public void Dispose()
    {
        _subscription?.Dispose();
    }
}
```

### 시나리오 3: 전역 알림 시스템

```csharp
// ViewModels/NotificationVm.cs
public class NotificationVm : ViewModelBase
{
    private readonly IMessenger _messenger;
    private string? _message;
    private bool _isVisible;

    public NotificationVm(IMessenger messenger)
    {
        _messenger = messenger;
        
        messenger.Subscribe<ShowNotificationMessage>(msg =>
        {
            Message = $"{msg.Title}: {msg.Message}";
            IsVisible = true;
            
            // Auto-hide after 3 seconds
            Task.Delay(3000).ContinueWith(_ => IsVisible = false);
        });
    }

    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }
}

// 다른 ViewModel에서 알림 표시
public class SomeViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public async Task SaveData()
    {
        try
        {
            // Save logic...
            
            _messenger.Send(new ShowNotificationMessage(
                "Success",
                "Data saved successfully",
                NotificationType.Success
            ));
        }
        catch (Exception ex)
        {
            _messenger.Send(new ShowNotificationMessage(
                "Error",
                ex.Message,
                NotificationType.Error
            ));
        }
    }
}
```

---

## Best Practices

### 1. DI 컨테이너에 Singleton으로 등록

```csharp
// Program.cs
services.AddSingleton<IMessenger, Messenger>();
```

### 2. 메시지는 Immutable Record 사용

```csharp
// ✅ Good: Immutable record
public record LanguageChangedMessage(string CultureName);

// ❌ Bad: Mutable class
public class LanguageChangedMessage
{
    public string CultureName { get; set; }
}
```

### 3. 구독 해제 관리

```csharp
// ✅ Good: Dispose subscription
public class MyViewModel : ViewModelBase, IDisposable
{
    private readonly IDisposable _subscription;

    public MyViewModel(IMessenger messenger)
    {
        _subscription = messenger.Subscribe<SomeMessage>(HandleMessage);
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }
}

// 또는 using을 활용
public void SomeMethod()
{
    using var subscription = _messenger.Subscribe<SomeMessage>(msg => 
    {
        // Handle message
    });
    
    // subscription은 스코프를 벗어날 때 자동 해제
}
```

### 4. 메시지 네이밍 규칙

```csharp
// Event를 나타낼 때: [Subject][Action]Message
public record UserLoggedInMessage(string UserId);
public record DataSavedMessage(int RecordId);

// Command를 나타낼 때: [Action][Subject]Message
public record RecreateWindowMessage(WindowRecreateReason Reason);
public record ShowDialogMessage(string DialogType);
```

### 5. 에러 처리

```csharp
// Messenger 구현에 try-catch 추가
public void Send<TMessage>(TMessage message)
{
    foreach (var handler in handlers)
    {
        try
        {
            handler(message);
        }
        catch (Exception ex)
        {
            // Log but don't stop processing other handlers
            LogError(ex);
        }
    }
}
```

### 6. 메시지 우선순위가 필요한 경우

```csharp
public interface IMessenger
{
    void Send<TMessage>(TMessage message, int priority = 0);
    IDisposable Subscribe<TMessage>(Action<TMessage> handler, int priority = 0);
}
```

---

## 장단점

### ✅ 장점

1. **느슨한 결합 (Loose Coupling)**
   - 발행자와 구독자가 서로를 알 필요 없음
   - MVVM 원칙을 깨지 않음

2. **확장성 (Scalability)**
   - 새로운 구독자 추가가 쉬움
   - 기존 코드 수정 불필요

3. **테스트 용이성**
   - ViewModel을 독립적으로 테스트 가능
   - Mock Messenger 사용 가능

4. **1:N 통신**
   - 하나의 메시지를 여러 구독자가 받을 수 있음

### ⚠️ 단점

1. **디버깅 어려움**
   - 메시지 흐름 추적이 어려울 수 있음
   - 해결책: 로깅, 디버그 모드에서 메시지 추적

2. **메모리 누수 위험**
   - 구독 해제를 잊으면 메모리 누수 발생
   - 해결책: IDisposable 패턴 사용

3. **과도한 사용 시 복잡도 증가**
   - 모든 통신을 메시지로 하면 오히려 복잡해짐
   - 해결책: 적절한 경우에만 사용

4. **컴파일 타임 체크 부족**
   - 메시지 발행자가 있는지 컴파일 타임에 확인 불가
   - 해결책: 명확한 네이밍과 문서화

### 🎯 사용 권장 시나리오

**사용하면 좋은 경우:**
- ViewModel → View 통신 (예: Window 재생성)
- Cross-cutting concerns (예: 로딩, 알림)
- 여러 ViewModel 간 데이터 동기화

**사용하지 않아도 되는 경우:**
- 부모-자식 관계의 직접 통신
- 단순한 이벤트 처리
- 이미 잘 정의된 인터페이스가 있는 경우

---

## 추가 리소스

### CommunityToolkit.Mvvm의 WeakReferenceMessenger

더 고급 기능이 필요하다면 검증된 라이브러리 사용을 고려:

```bash
dotnet add package CommunityToolkit.Mvvm
```

```csharp
using CommunityToolkit.Mvvm.Messaging;

// Send message
WeakReferenceMessenger.Default.Send(new LanguageChangedMessage("ko-KR"));

// Subscribe
WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
{
    // Handle message
});
```

**특징:**
- Weak reference 사용으로 메모리 누수 방지
- 채널(Channel) 개념으로 메시지 그룹화 가능
- Request/Response 패턴 지원

---

## 결론

Messenger 패턴은 MVVM 애플리케이션에서 **느슨한 결합을 유지하면서 효과적으로 통신**할 수 있는 강력한 도구입니다. 

특히:
- 언어/테마 변경 시 UI 업데이트
- 여러 ViewModel 간 데이터 동기화
- 전역 알림 시스템

등의 시나리오에서 매우 유용합니다.

적절히 사용하면 유지보수가 쉽고 확장 가능한 애플리케이션을 만들 수 있습니다! 🚀
