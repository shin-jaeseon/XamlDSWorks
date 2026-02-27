# Avalonia UI 커스텀 테마 관리 가이드

이 문서는 XamlDS.ITK.Aui 프로젝트에서 커스텀 테마를 작성하고 관리하는 방법을 정리합니다.

## 📋 목차

1. [개요](#개요)
2. [프로젝트 구조](#프로젝트-구조)
3. [ControlTheme 작성](#controltheme-작성)
4. [Style 작성](#style-작성)
5. [Application 설정](#application-설정)
6. [테마 전환](#테마-전환)
7. [Best Practices](#best-practices)

---

## 개요

Avalonia UI에서 커스텀 테마(ITKDark, ITKLight, ITKBlue 등)를 구현할 때는 다음과 같은 구조를 따릅니다:

- **Themes/** - `ControlTheme` 리소스 정의
- **Styles/** - 테마별 `Style` 정의 (컨트롤의 `Theme` 속성 연결)
- **Application.Resources** - 모든 `ControlTheme` 리소스 병합
- **Application.Styles** - 현재 활성 테마의 스타일 적용

### ⚠️ 중요 사항

- `Default`, `Light`, `Dark`는 시스템 예약 키이며, 자동 테마 전환이 지원됩니다.
- 커스텀 테마(ITKDark, ITKLight 등)는 프로그래밍 방식으로 전환해야 합니다.
- `ResourceDictionary.ThemeDictionaries`는 시스템 테마에만 사용하세요.

---

## 프로젝트 구조

```
XamlDS.ITK.Aui/
├── Themes/                          # ControlTheme 정의
│   ├── ButtonThemes.axaml          # 모든 버튼 테마 (ITKDark, ITKLight, ITKBlue...)
│   ├── TextBoxThemes.axaml         # 모든 텍스트박스 테마
│   ├── CheckBoxThemes.axaml        # 모든 체크박스 테마
│   └── ...                         # 기타 컨트롤 테마들
│
├── Styles/                          # 테마별 Style 정의
│   ├── ITKDarkStyles.axaml        # Dark 테마 스타일
│   ├── ITKLightStyles.axaml       # Light 테마 스타일
│   ├── ITKBlueStyles.axaml        # Blue 테마 스타일
│   └── ...                         # 기타 테마 스타일들
│
└── THEME_GUIDE.md                  # 이 문서
```

### 파일 구성 전략

#### Option 1: 컨트롤별 단일 파일 (권장)
```
Themes/
├── ButtonThemes.axaml              # ITKDarkButtonTheme, ITKLightButtonTheme, ITKBlueButtonTheme
└── TextBoxThemes.axaml             # ITKDarkTextBoxTheme, ITKLightTextBoxTheme, ITKBlueTextBoxTheme
```

#### Option 2: 테마별 + 컨트롤별 분리
```
Themes/
├── Button/
│   ├── ITKDarkButtonTheme.axaml
│   ├── ITKLightButtonTheme.axaml
│   └── ITKBlueButtonTheme.axaml
└── ButtonThemes.axaml              # 공용 리소스 (공유 브러시, 상수 등)
```

---

## ControlTheme 작성

### Themes/ButtonThemes.axaml 예제

```xml
<ResourceDictionary xmlns="https://github.com/avaloniAui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <!-- ITK Dark Button Theme -->
    <ControlTheme x:Key="ITKDarkButtonTheme" TargetType="Button">
        <Setter Property="Background" Value="#2D2D30"/>
        <Setter Property="Foreground" Value="#FFFFFF"/>
        <Setter Property="BorderBrush" Value="#3E3E42"/>
        <Setter Property="BorderThickness" Value="1"/>
        <Setter Property="Padding" Value="16,8"/>
        <Setter Property="CornerRadius" Value="4"/>
        
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        Padding="{TemplateBinding Padding}"
                        CornerRadius="{TemplateBinding CornerRadius}">
                    <ContentPresenter HorizontalContentAlignment="Center" 
                                      VerticalContentAlignment="Center"
                                      Content="{TemplateBinding Content}"/>
                </Border>
            </ControlTemplate>
        </Setter>
        
        <!-- Interaction states -->
        <Style Selector="^:pointerover">
            <Setter Property="Background" Value="#3E3E42"/>
        </Style>
        <Style Selector="^:pressed">
            <Setter Property="Background" Value="#007ACC"/>
        </Style>
        <Style Selector="^:disabled">
            <Setter Property="Opacity" Value="0.4"/>
        </Style>
    </ControlTheme>

    <!-- ITK Light Button Theme -->
    <ControlTheme x:Key="ITKLightButtonTheme" TargetType="Button">
        <Setter Property="Background" Value="#FFFFFF"/>
        <Setter Property="Foreground" Value="#000000"/>
        <Setter Property="BorderBrush" Value="#CCCCCC"/>
        <Setter Property="BorderThickness" Value="1"/>
        <Setter Property="Padding" Value="16,8"/>
        <Setter Property="CornerRadius" Value="4"/>
        
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        Padding="{TemplateBinding Padding}"
                        CornerRadius="{TemplateBinding CornerRadius}">
                    <ContentPresenter HorizontalContentAlignment="Center" 
                                      VerticalContentAlignment="Center"
                                      Content="{TemplateBinding Content}"/>
                </Border>
            </ControlTemplate>
        </Setter>
        
        <!-- Interaction states -->
        <Style Selector="^:pointerover">
            <Setter Property="Background" Value="#E5E5E5"/>
        </Style>
        <Style Selector="^:pressed">
            <Setter Property="Background" Value="#0078D4"/>
            <Setter Property="Foreground" Value="#FFFFFF"/>
        </Style>
        <Style Selector="^:disabled">
            <Setter Property="Opacity" Value="0.4"/>
        </Style>
    </ControlTheme>

    <!-- ITK Blue Button Theme -->
    <ControlTheme x:Key="ITKBlueButtonTheme" TargetType="Button">
        <Setter Property="Background" Value="#0078D4"/>
        <Setter Property="Foreground" Value="#FFFFFF"/>
        <Setter Property="BorderBrush" Value="#005A9E"/>
        <Setter Property="BorderThickness" Value="1"/>
        <Setter Property="Padding" Value="16,8"/>
        <Setter Property="CornerRadius" Value="4"/>
        
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        Padding="{TemplateBinding Padding}"
                        CornerRadius="{TemplateBinding CornerRadius}">
                    <ContentPresenter HorizontalContentAlignment="Center" 
                                      VerticalContentAlignment="Center"
                                      Content="{TemplateBinding Content}"/>
                </Border>
            </ControlTemplate>
        </Setter>
        
        <!-- Interaction states -->
        <Style Selector="^:pointerover">
            <Setter Property="Background" Value="#005A9E"/>
        </Style>
        <Style Selector="^:pressed">
            <Setter Property="Background" Value="#004578"/>
        </Style>
        <Style Selector="^:disabled">
            <Setter Property="Opacity" Value="0.4"/>
        </Style>
    </ControlTheme>

</ResourceDictionary>
```

### 핵심 포인트

1. **x:Key 필수**: 각 `ControlTheme`는 고유한 `x:Key`를 가져야 합니다.
2. **TargetType 지정**: 적용할 컨트롤 타입을 명시합니다.
3. **Template 정의**: 컨트롤의 비주얼 트리를 완전히 재정의합니다.
4. **상태 정의**: `:pointerover`, `:pressed`, `:disabled` 등의 상태별 스타일을 정의합니다.

---

## Style 작성

### Styles/ITKDarkStyles.axaml 예제

```xml
<Styles xmlns="https://github.com/avaloniAui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Apply ITKDark theme to all Buttons by default -->
    <Style Selector="Button">
        <Setter Property="Theme" Value="{StaticResource ITKDarkButtonTheme}"/>
    </Style>
    
    <!-- Apply ITKDark theme to all TextBoxes by default -->
    <Style Selector="TextBox">
        <Setter Property="Theme" Value="{StaticResource ITKDarkTextBoxTheme}"/>
    </Style>
    
    <!-- Apply ITKDark theme to all CheckBoxes by default -->
    <Style Selector="CheckBox">
        <Setter Property="Theme" Value="{StaticResource ITKDarkCheckBoxTheme}"/>
    </Style>
    
    <!-- Apply ITKDark theme to all RadioButtons by default -->
    <Style Selector="RadioButton">
        <Setter Property="Theme" Value="{StaticResource ITKDarkRadioButtonTheme}"/>
    </Style>
    
    <!-- Additional control mappings... -->

</Styles>
```

### Styles/ITKLightStyles.axaml 예제

```xml
<Styles xmlns="https://github.com/avaloniAui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Apply ITKLight theme to all Buttons by default -->
    <Style Selector="Button">
        <Setter Property="Theme" Value="{StaticResource ITKLightButtonTheme}"/>
    </Style>
    
    <!-- Apply ITKLight theme to all TextBoxes by default -->
    <Style Selector="TextBox">
        <Setter Property="Theme" Value="{StaticResource ITKLightTextBoxTheme}"/>
    </Style>
    
    <!-- Apply ITKLight theme to all CheckBoxes by default -->
    <Style Selector="CheckBox">
        <Setter Property="Theme" Value="{StaticResource ITKLightCheckBoxTheme}"/>
    </Style>
    
    <!-- Apply ITKLight theme to all RadioButtons by default -->
    <Style Selector="RadioButton">
        <Setter Property="Theme" Value="{StaticResource ITKLightRadioButtonTheme}"/>
    </Style>
    
    <!-- Additional control mappings... -->

</Styles>
```

### 핵심 포인트

1. **Style Selector**: 기본 컨트롤 타입을 셀렉터로 지정합니다.
2. **Theme 속성 연결**: `Theme` 속성에 `ControlTheme`의 키를 `StaticResource`로 연결합니다.
3. **일관성**: 모든 기본 컨트롤에 대해 테마를 정의해야 합니다.

---

## Application 설정

### App.axaml 예제

```xml
<Application xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="XamlDS.DemoApp.Aui.App">

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Include ALL ControlTheme resources -->
                <ResourceInclude Source="avares://XamlDS.ITK.Aui/Themes/ButtonThemes.axaml"/>
                <ResourceInclude Source="avares://XamlDS.ITK.Aui/Themes/TextBoxThemes.axaml"/>
                <ResourceInclude Source="avares://XamlDS.ITK.Aui/Themes/CheckBoxThemes.axaml"/>
                <ResourceInclude Source="avares://XamlDS.ITK.Aui/Themes/RadioButtonThemes.axaml"/>
                <!-- Add more control themes as needed... -->
                
                <!-- Include other resources (Converters, etc.) -->
                <ResourceInclude Source="avares://XamlDS.ITK.Aui/Converters/Converters.axaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

    <Application.Styles>
        <!-- Include ONLY the active theme's style -->
        <!-- For ITK Dark Theme -->
        <StyleInclude Source="avares://XamlDS.ITK.Aui/Styles/ITKDarkStyles.axaml"/>
        
        <!-- For ITK Light Theme (comment out the one above and uncomment this) -->
        <!-- <StyleInclude Source="avares://XamlDS.ITK.Aui/Styles/ITKLightStyles.axaml"/> -->
        
        <!-- For ITK Blue Theme -->
        <!-- <StyleInclude Source="avares://XamlDS.ITK.Aui/Styles/ITKBlueStyles.axaml"/> -->
    </Application.Styles>
</Application>
```

### 핵심 포인트

1. **Application.Resources**: 모든 `ControlTheme` 리소스를 `MergedDictionaries`에 포함시킵니다.
2. **Application.Styles**: 현재 활성화할 테마의 스타일만 `StyleInclude`로 포함시킵니다.
3. **테마 전환**: 주석을 변경하여 다른 테마를 활성화하거나, 코드에서 동적으로 전환합니다.

---

## 테마 전환

### 런타임 테마 전환 구현

#### 1. ThemeManager 클래스 생성

```csharp
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System;

namespace XamlDS.ITK.Aui.Services
{
    /// <summary>
    /// Manages theme switching at runtime
    /// </summary>
    public class ThemeManager
    {
        private const string BaseUri = "avares://XamlDS.ITK.Aui";
        
        /// <summary>
        /// Available ITK themes
        /// </summary>
        public enum ITKTheme
        {
            Dark,
            Light,
            Blue,
            Green
        }

        /// <summary>
        /// Switch to the specified ITK theme
        /// </summary>
        /// <param name="theme">Target theme to apply</param>
        public static void SwitchTheme(ITKTheme theme)
        {
            var app = Application.Current;
            if (app == null) return;

            var stylePath = theme switch
            {
                ITKTheme.Dark => $"{BaseUri}/Styles/ITKDarkStyles.axaml",
                ITKTheme.Light => $"{BaseUri}/Styles/ITKLightStyles.axaml",
                ITKTheme.Blue => $"{BaseUri}/Styles/ITKBlueStyles.axaml",
                ITKTheme.Green => $"{BaseUri}/Styles/ITKGreenStyles.axaml",
                _ => throw new ArgumentException($"Unknown theme: {theme}")
            };

            // Remove existing theme styles
            RemoveThemeStyles(app);

            // Add new theme style
            var styleInclude = new StyleInclude(new Uri(BaseUri))
            {
                Source = new Uri(stylePath, UriKind.Absolute)
            };
            
            app.Styles.Add(styleInclude);
        }

        /// <summary>
        /// Remove all ITK theme styles from application
        /// </summary>
        private static void RemoveThemeStyles(Application app)
        {
            for (int i = app.Styles.Count - 1; i >= 0; i--)
            {
                if (app.Styles[i] is StyleInclude styleInclude)
                {
                    var source = styleInclude.Source?.ToString() ?? string.Empty;
                    if (source.Contains("/Styles/ITK") && source.EndsWith("Styles.axaml"))
                    {
                        app.Styles.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Get the currently active ITK theme
        /// </summary>
        public static ITKTheme? GetCurrentTheme()
        {
            var app = Application.Current;
            if (app == null) return null;

            foreach (var style in app.Styles)
            {
                if (style is StyleInclude styleInclude)
                {
                    var source = styleInclude.Source?.ToString() ?? string.Empty;
                    
                    if (source.Contains("ITKDarkStyles.axaml")) return ITKTheme.Dark;
                    if (source.Contains("ITKLightStyles.axaml")) return ITKTheme.Light;
                    if (source.Contains("ITKBlueStyles.axaml")) return ITKTheme.Blue;
                    if (source.Contains("ITKGreenStyles.axaml")) return ITKTheme.Green;
                }
            }

            return null;
        }
    }
}
```

#### 2. ViewModel에서 사용 예제

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XamlDS.ITK.Aui.Services;

namespace YourApp.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThemeManager.ITKTheme _selectedTheme = ThemeManager.ITKTheme.Dark;

        [RelayCommand]
        private void ApplyTheme()
        {
            ThemeManager.SwitchTheme(SelectedTheme);
        }

        [RelayCommand]
        private void SwitchToDarkTheme()
        {
            ThemeManager.SwitchTheme(ThemeManager.ITKTheme.Dark);
            SelectedTheme = ThemeManager.ITKTheme.Dark;
        }

        [RelayCommand]
        private void SwitchToLightTheme()
        {
            ThemeManager.SwitchTheme(ThemeManager.ITKTheme.Light);
            SelectedTheme = ThemeManager.ITKTheme.Light;
        }
    }
}
```

#### 3. View에서 테마 전환 UI 예제

```xml
<UserControl xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <StackPanel Spacing="10">
        <TextBlock Text="테마 선택" FontSize="16" FontWeight="Bold"/>
        
        <StackPanel Orientation="Horizontal" Spacing="10">
            <Button Content="Dark" Command="{Binding SwitchToDarkThemeCommand}"/>
            <Button Content="Light" Command="{Binding SwitchToLightThemeCommand}"/>
            <Button Content="Blue" Command="{Binding ApplyThemeCommand}" 
                    CommandParameter="{x:Static services:ThemeManager+ITKTheme.Blue}"/>
        </StackPanel>
    </StackPanel>
</UserControl>
```

---

## Best Practices

### 1. 명명 규칙

#### ControlTheme 명명
```
패턴: {테마이름}{컨트롤이름}Theme
예제: ITKDarkButtonTheme, ITKLightTextBoxTheme
```

#### Style 파일 명명
```
패턴: {테마이름}Styles.axaml
예제: ITKDarkStyles.axaml, ITKLightStyles.axaml
```

#### Theme 리소스 파일 명명
```
패턴: {컨트롤이름}Themes.axaml
예제: ButtonThemes.axaml, TextBoxThemes.axaml
```

### 2. 리소스 구성

#### 공통 리소스 분리
테마 간 공유되는 값들(예: 폰트 크기, 여백 등)은 별도의 리소스 파일로 분리합니다.

```xml
<!-- Themes/CommonResources.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniAui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Common font sizes -->
    <x:Double x:Key="FontSizeSmall">11</x:Double>
    <x:Double x:Key="FontSizeNormal">13</x:Double>
    <x:Double x:Key="FontSizeLarge">15</x:Double>
    <x:Double x:Key="FontSizeXLarge">18</x:Double>
    
    <!-- Common spacing -->
    <Thickness x:Key="PaddingSmall">4</Thickness>
    <Thickness x:Key="PaddingNormal">8</Thickness>
    <Thickness x:Key="PaddingLarge">16</Thickness>
    
    <!-- Common corner radius -->
    <CornerRadius x:Key="CornerRadiusSmall">2</CornerRadius>
    <CornerRadius x:Key="CornerRadiusNormal">4</CornerRadius>
    <CornerRadius x:Key="CornerRadiusLarge">8</CornerRadius>
    
</ResourceDictionary>
```

### 3. 테마별 색상 팔레트

각 테마별로 색상 팔레트를 정의하여 일관성을 유지합니다.

```xml
<!-- Themes/ColorPalettes/ITKDarkColors.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniAui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Background colors -->
    <Color x:Key="BackgroundPrimary">#1E1E1E</Color>
    <Color x:Key="BackgroundSecondary">#2D2D30</Color>
    <Color x:Key="BackgroundTertiary">#3E3E42</Color>
    
    <!-- Foreground colors -->
    <Color x:Key="ForegroundPrimary">#FFFFFF</Color>
    <Color x:Key="ForegroundSecondary">#CCCCCC</Color>
    <Color x:Key="ForegroundDisabled">#808080</Color>
    
    <!-- Accent colors -->
    <Color x:Key="AccentPrimary">#007ACC</Color>
    <Color x:Key="AccentHover">#1C97EA</Color>
    <Color x:Key="AccentPressed">#005A9E</Color>
    
    <!-- Border colors -->
    <Color x:Key="BorderNormal">#3E3E42</Color>
    <Color x:Key="BorderFocused">#007ACC</Color>
    
    <!-- Brushes from colors -->
    <SolidColorBrush x:Key="BackgroundPrimaryBrush" Color="{StaticResource BackgroundPrimary}"/>
    <SolidColorBrush x:Key="BackgroundSecondaryBrush" Color="{StaticResource BackgroundSecondary}"/>
    <!-- ... more brushes ... -->
    
</ResourceDictionary>
```

### 4. 디버깅 팁

#### 테마 적용 확인
```csharp
// Check if theme resources are loaded
var app = Application.Current;
var resources = app.Resources;

// Try to find a specific theme resource
if (resources.TryGetResource("ITKDarkButtonTheme", null, out var theme))
{
    Debug.WriteLine("Theme found!");
}
else
{
    Debug.WriteLine("Theme NOT found!");
}
```

#### 스타일 적용 확인
```csharp
// Check active styles
var app = Application.Current;
foreach (var style in app.Styles)
{
    if (style is StyleInclude styleInclude)
    {
        Debug.WriteLine($"Style: {styleInclude.Source}");
    }
}
```

### 5. 성능 최적화

1. **지연 로딩**: 필요한 테마만 로드합니다.
2. **리소스 캐싱**: 자주 사용하는 브러시는 재사용합니다.
3. **템플릿 최적화**: 불필요한 중첩을 피합니다.

---

## 문제 해결

### Q: ControlTheme을 찾을 수 없다는 오류

**원인**: `Application.Resources`에 해당 테마 리소스 파일이 병합되지 않았습니다.

**해결**:
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceInclude Source="avares://XamlDS.ITK.Aui/Themes/ButtonThemes.axaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Q: 테마가 적용되지 않음

**원인**: `Application.Styles`에 해당 테마의 스타일 파일이 포함되지 않았습니다.

**해결**:
```xml
<Application.Styles>
    <StyleInclude Source="avares://XamlDS.ITK.Aui/Styles/ITKDarkStyles.axaml"/>
</Application.Styles>
```

### Q: 런타임 테마 전환이 작동하지 않음

**원인**: 이전 테마 스타일이 제거되지 않았거나, 리소스가 정적으로 바인딩되었습니다.

**해결**:
1. 이전 스타일을 명시적으로 제거합니다.
2. `StaticResource` 대신 `DynamicResource` 사용을 고려합니다.

---

## 참고 자료

- [Avalonia UI Documentation](https://docs.avaloniAui.net/)
- [Avalonia UI Styling](https://docs.avaloniAui.net/docs/styling/)
- [Control Themes](https://docs.avaloniAui.net/docs/styling/control-themes)
- [ResourceDictionary](https://docs.avaloniAui.net/docs/styling/resources)

---

## 변경 이력

- **2026-02-16: 초안 작성
- **YYYY-MM-DD**: 추가 예제 및 Best Practices 보충

---

**작성일**: 2026
**프로젝트**: XamlDS.ITK.Aui
**유지관리**: XamlDS 개발팀
