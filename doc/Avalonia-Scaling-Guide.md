# Avalonia UI 화면 스케일링 가이드

## 개요

Avalonia UI에서 화면 스케일링(DPI Scaling)은 다양한 해상도와 DPI 설정을 가진 디스플레이에서 일관된 UI 크기를 유지하기 위한 기능입니다.

## AVALONIA_SCREEN_SCALE_FACTOR 환경 변수

### 제한 사항

`AVALONIA_SCREEN_SCALE_FACTOR` 환경 변수는 **이론적으로는 존재하지만 실제로는 작동하지 않는 경우가 많습니다**.

**작동하지 않는 이유:**

1. **플랫폼 자동 감지 우선순위**
   - `.UsePlatformDetect()`를 사용하면 시스템의 DPI 설정이 환경 변수보다 우선됩니다.
   - Wayland: 컴포지터가 제공하는 스케일 팩터 사용
   - X11: `Xft.dpi`, `GDK_SCALE` 등 시스템 설정 사용

2. **Avalonia 버전별 지원 차이**
   - 버전에 따라 이 환경 변수의 지원이 일관되지 않습니다.
   - 공식 문서에서도 권장하지 않는 방법입니다.

3. **플랫폼별 구현 차이**
   - 각 플랫폼(Windows, Linux, macOS)마다 스케일링 처리 방식이 다릅니다.

### 권장하지 않음

**결론: `AVALONIA_SCREEN_SCALE_FACTOR`를 사용하지 마세요.** 대신 아래의 권장 방법을 사용하세요.

---

## 권장 스케일링 조정 방법

### 방법 1: 시스템 설정 활용 (권장)

가장 안정적이고 권장되는 방법은 **운영체제의 디스플레이 설정**을 사용하는 것입니다.

#### Ubuntu/Linux (GNOME/Wayland)

```bash
# 현재 스케일 팩터 확인
gsettings get org.gnome.desktop.interface scaling-factor

# 스케일 팩터 설정 (1 = 100%, 2 = 200%)
gsettings set org.gnome.desktop.interface scaling-factor 2

# 또는 GUI에서:
# Settings > Displays > Scale
```

#### Ubuntu/Linux (KDE Plasma/Wayland)

```bash
# System Settings > Display and Monitor > Display Configuration > Scale
```

#### Windows

```
Settings > System > Display > Scale and layout
```

**장점:**
- ✅ 모든 애플리케이션에 일관되게 적용
- ✅ Avalonia가 자동으로 감지하여 적용
- ✅ 사용자 경험이 일관됨

**단점:**
- ❌ 애플리케이션별 개별 설정 불가능

---

### 방법 2: Avalonia 프로그램 내에서 조정

특정 애플리케이션만 다른 스케일링을 사용해야 하는 경우:

#### 옵션 A: XAML에서 RenderTransform 사용

```xml
<Window xmlns="https://github.com/avaloniAui"
        x:Class="YourNamespace.MainWindow"
        RenderTransform="scale(1.25)">
    <!-- 모든 컨텐츠가 1.25배 확대됨 -->
</Window>
```

**장점:**
- ✅ 간단한 구현
- ✅ 런타임에서 동적 변경 가능

**단점:**
- ❌ 텍스트가 흐릿해질 수 있음 (비트맵 스케일링)
- ❌ 창 크기와 내용 크기 불일치 가능

#### 옵션 B: 폰트 크기와 레이아웃 조정 (권장)

```xml
<!-- App.axaml에서 전역 스타일 정의 -->
<Application.Styles>
    <Style Selector="TextBlock">
        <Setter Property="FontSize" Value="16"/> <!-- 기본보다 크게 -->
    </Style>
    <Style Selector="Button">
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="Padding" Value="16,8"/>
    </Style>
</Application.Styles>
```

**장점:**
- ✅ 선명한 텍스트 렌더링
- ✅ 레이아웃 정확도 유지
- ✅ 접근성 향상

**단점:**
- ❌ 모든 컨트롤에 대해 개별 설정 필요

#### 옵션 C: AppBuilder 설정 (고급)

Avalonia 11.1+ 버전에서는 플랫폼별 옵션을 설정할 수 있습니다:

```csharp
// Program.cs
public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();
        // NOTE: 추가 스케일링 설정은 Avalonia API 버전에 따라 다름
```

**참고:** Avalonia 11.3 기준으로 명시적인 스케일 팩터 설정 API가 제한적입니다.

---

## 환경별 권장 설정

### 1. Windows (고DPI 모니터)

```json
// launchSettings.json
{
  "profiles": {
    "Windows": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**설명:**
- Windows는 자동 DPI 스케일링이 잘 작동합니다.
- 추가 환경 변수 불필요
- "Settings > Display > Scale"에서 시스템 스케일 설정

---

### 2. WSL + X11 (Windows에서 리눅스 앱 실행)

```json
{
  "profiles": {
    "WSL - Ubuntu": {
      "commandName": "WSL",
      "distributionName": "Ubuntu",
      "environmentVariables": {
        "DISPLAY": ":0",
        "DOTNET_ENVIRONMENT": "Development",
        "GDK_SCALE": "2",
        "GDK_DPI_SCALE": "0.5"
      }
    }
  }
}
```

**설명:**
- `DISPLAY=:0`: Windows X 서버(VcXsrv, Xming 등)에 연결
- `GDK_SCALE=2`: UI 요소를 2배 확대
- `GDK_DPI_SCALE=0.5`: 폰트 크기를 절반으로 조정 (GDK_SCALE의 역효과 보정)

---

### 3. Linux Native (Wayland)

```json
{
  "profiles": {
    "Linux": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "XDG_SESSION_TYPE": "wayland",
        "WAYLAND_DISPLAY": "wayland-0"
      }
    }
  }
}
```

**설명:**
- `XDG_SESSION_TYPE=wayland`: Wayland 세션임을 명시
- `WAYLAND_DISPLAY=wayland-0`: 첫 번째 Wayland 디스플레이 사용
- 스케일링은 **GNOME/KDE 시스템 설정**에서 조정

**Wayland 디스플레이 소켓:**
- `wayland-0`: 첫 번째 Wayland 디스플레이 (기본)
- `wayland-1`: 두 번째 Wayland 디스플레이 (멀티 세션)
- 대부분의 경우 `wayland-0` 사용

**확인 방법:**
```bash
# 현재 Wayland 소켓 확인
echo $WAYLAND_DISPLAY

# 사용 가능한 소켓 목록
ls -la /run/user/$UID/wayland-*
```

---

### 4. Linux Native (X11)

```json
{
  "profiles": {
    "Linux X11": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "XDG_SESSION_TYPE": "x11",
        "DISPLAY": ":0",
        "GDK_SCALE": "2",
        "GDK_DPI_SCALE": "0.5"
      }
    }
  }
}
```

**설명:**
- `DISPLAY=:0`: 첫 번째 X11 디스플레이
- `GDK_SCALE`: GTK 애플리케이션 스케일링 (Avalonia도 참조)

---

## 디버깅 및 확인

### 현재 스케일 팩터 확인

```csharp
// Avalonia 애플리케이션 내부에서
var screen = Screens.Primary;
Console.WriteLine($"Scaling: {screen?.Scaling}");
Console.WriteLine($"Bounds: {screen?.Bounds}");
```

### 환경 변수 확인

```bash
# Linux/WSL
echo $WAYLAND_DISPLAY
echo $DISPLAY
echo $GDK_SCALE
gsettings get org.gnome.desktop.interface scaling-factor

# Windows (PowerShell)
$env:AVALONIA_SCREEN_SCALE_FACTOR
```

---

## 문제 해결

### 문제 1: UI가 너무 작게 표시됨

**해결책:**
1. 시스템 디스플레이 설정에서 스케일 증가 (권장)
2. XAML에서 폰트 크기 증가
3. X11 환경이라면 `GDK_SCALE=2` 설정

### 문제 2: UI가 흐릿하게 표시됨

**해결책:**
1. Wayland 사용 (X11보다 스케일링 품질 우수)
2. 정수 배율 사용 (1.0, 2.0) - 분수 배율(1.25, 1.5) 피하기
3. `RenderTransform` 대신 폰트 크기 조정

### 문제 3: WSL에서 X 서버 연결 실패

**해결책:**
```bash
# Windows의 IP 주소 확인
export DISPLAY=$(grep nameserver /etc/resolv.conf | awk '{print $2}'):0

# 또는 Windows X 서버에서 "Disable access control" 옵션 활성화
```

---

## 모범 사례

### ✅ DO (권장)

1. **시스템 스케일링 사용**
   - OS의 디스플레이 설정에서 조정
   - Avalonia의 `.UsePlatformDetect()` 사용

2. **폰트 크기 조정**
   - 터치 UI: 최소 16pt 이상
   - 데스크톱 UI: 12-14pt 기본

3. **Wayland 우선 사용 (Linux)**
   - X11보다 스케일링 품질 우수
   - HiDPI 지원 우수

4. **테스트**
   - 다양한 DPI 설정에서 테스트
   - 100%, 125%, 150%, 200%

### ❌ DON'T (비권장)

1. **`AVALONIA_SCREEN_SCALE_FACTOR` 사용하지 마세요**
   - 작동이 불안정함
   - 공식 지원 불확실

2. **분수 스케일 피하기**
   - 가능하면 1.0, 2.0 같은 정수 배율 사용
   - 1.25, 1.5는 흐릿함 발생 가능

3. **하드코딩된 픽셀 크기 피하기**
   - 절대 크기 대신 상대 크기 사용
   - Grid의 `*` 또는 `Auto` 활용

---

## 참고 자료

- [Avalonia UI Documentation - Platform-specific](https://docs.avaloniAui.net/docs/guides/platforms/)
- [Avalonia GitHub - HiDPI Support](https://github.com/AvaloniAui/Avalonia)
- [GNOME HiDPI - ArchWiki](https://wiki.archlinux.org/title/HiDPI)
- [Wayland Display Configuration](https://wayland.freedesktop.org/)

---

## 결론

### 핵심 요약

1. **`AVALONIA_SCREEN_SCALE_FACTOR`는 사용하지 마세요** - 작동하지 않습니다.
2. **시스템 디스플레이 설정을 사용하세요** - 가장 안정적입니다.
3. **Wayland 환경에서는 추가 설정 불필요** - 자동 감지가 잘 작동합니다.
4. **X11 환경에서는 `GDK_SCALE` 사용** - WSL 등에서 유용합니다.

### launchSettings.json 최종 권장 구성

```json
{
  "profiles": {
    "Windows": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development"
      }
    },
    "WSL - Ubuntu": {
      "commandName": "WSL",
      "distributionName": "Ubuntu",
      "environmentVariables": {
        "DISPLAY": ":0",
        "DOTNET_ENVIRONMENT": "Development",
        "GDK_SCALE": "2",
        "GDK_DPI_SCALE": "0.5"
      }
    },
    "Linux": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "XDG_SESSION_TYPE": "wayland",
        "WAYLAND_DISPLAY": "wayland-0"
      }
    }
  }
}
```

