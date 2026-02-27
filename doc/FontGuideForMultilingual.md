# 다국어 지원을 위한 폰트 가이드

## 개요

이 문서는 XamlDSWorks 프로젝트에서 7-10개 언어를 지원하기 위한 폰트 설정 및 관리 가이드입니다.

## 다국어 폰트의 과제

### 지원 언어별 문자 세트

| 언어 | 문자 체계 | 특징 | 폰트 요구사항 |
| --- | --- | --- | --- |
| **영어** (en) | Latin | 기본 ASCII | 대부분의 폰트 지원 |
| **한국어** (ko-KR) | Hangul | 한글 11,172자 | 한글 폰트 필수 |
| **중국어** (zh-CN) | Simplified Chinese | 간체자 수천 개 | CJK 폰트 필수 |
| **독일어** (de-DE) | Latin Extended | 움라우트(ä, ö, ü) | 대부분의 서구 폰트 지원 |
| **일본어** (ja-JP) | Kanji, Hiragana, Katakana | 한자 + 가나 | CJK 폰트 필수 |
| **스페인어** (es) | Latin Extended | 억양 부호(á, é, í) | 대부분의 서구 폰트 지원 |
| **프랑스어** (fr-FR) | Latin Extended | 억양 부호(é, è, ê) | 대부분의 서구 폰트 지원 |
| **이탈리아어** (it-IT) | Latin Extended | 억양 부호 | 대부분의 서구 폰트 지원 |
| **포르투갈어** (pt-BR) | Latin Extended | 억양 부호, 틸드(ã) | 대부분의 서구 폰트 지원 |
| **터키어** (tr-TR) | Latin Extended | 특수 문자(ğ, ş, ı) | 터키어 지원 폰트 |

### 핵심 문제

1. **단일 폰트로 모든 언어 지원 불가능**
   - CJK(한중일) 문자는 전용 폰트 필요
   - Latin 계열은 대부분 호환

2. **플랫폼별 기본 폰트 차이**
   - Windows: Segoe UI, Malgun Gothic, Microsoft YaHei
   - Linux: Liberation Sans, Noto Sans CJK
   - macOS: San Francisco, Apple Gothic

3. **폰트 파일 크기**
   - CJK 폰트는 10MB 이상 (한자 포함)
   - 임베디드 시 애플리케이션 크기 증가

## 권장 전략: 폰트 폴백(Font Fallback) 사용

### 폰트 폴백이란?

첫 번째 폰트에 특정 문자가 없으면 자동으로 다음 폰트를 사용하는 메커니즘입니다.

```text
"Segoe UI, Malgun Gothic, Microsoft YaHei, sans-serif"
  ↓
영어 → Segoe UI 사용
한글 → Malgun Gothic 사용 (Segoe UI에 한글 없음)
중국어 → Microsoft YaHei 사용 (Malgun Gothic에 중국어 없음)
```

## WPF 폰트 처리

### 1. 시스템 폰트 활용 (권장)

#### App.xaml에서 전역 폰트 폴백 설정

```xml
<Application x:Class="XamlDS.DemoApp.WPF.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <Style TargetType="Control">
            <Setter Property="FontFamily" Value="Segoe UI, Malgun Gothic, Microsoft YaHei, Meiryo, sans-serif"/>
            <Setter Property="FontSize" Value="14"/>
        </Style>
        
        <Style TargetType="TextBlock">
            <Setter Property="FontFamily" Value="Segoe UI, Malgun Gothic, Microsoft YaHei, Meiryo, sans-serif"/>
            <Setter Property="FontSize" Value="14"/>
        </Style>
    </Application.Resources>
</Application>
```

#### ResourceDictionary로 분리 (더 깔끔한 방법)

```xml
<!-- Styles/Typography.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- 폰트 패밀리 리소스 정의 -->
    <FontFamily x:Key="GlobalFontFamily">Segoe UI, Malgun Gothic, Microsoft YaHei, Meiryo, sans-serif</FontFamily>
    
    <!-- 기본 텍스트 스타일 -->
    <Style x:Key="BaseTextStyle" TargetType="TextBlock">
        <Setter Property="FontFamily" Value="{StaticResource GlobalFontFamily}"/>
        <Setter Property="FontSize" Value="14"/>
    </Style>
    
    <!-- 제목 스타일 -->
    <Style x:Key="TitleTextStyle" TargetType="TextBlock" BasedOn="{StaticResource BaseTextStyle}">
        <Setter Property="FontSize" Value="20"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
    </Style>
    
    <!-- 버튼 스타일 -->
    <Style x:Key="BaseButtonStyle" TargetType="Button">
        <Setter Property="FontFamily" Value="{StaticResource GlobalFontFamily}"/>
        <Setter Property="FontSize" Value="14"/>
    </Style>
    
</ResourceDictionary>
```

#### App.xaml에서 참조

```xml
<Application x:Class="XamlDS.DemoApp.WPF.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Styles/Typography.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### 2. WPF에서 폰트 폴백 순서

#### Windows 환경 권장 순서

```xml
<FontFamily x:Key="GlobalFontFamily">
    Segoe UI,           <!-- 영어, 서구 언어 (Windows 10/11 기본) -->
    Malgun Gothic,      <!-- 한국어 -->
    Microsoft YaHei,    <!-- 중국어 간체 -->
    Yu Gothic UI,       <!-- 일본어 -->
    sans-serif          <!-- 최종 폴백 -->
</FontFamily>
```

#### 산업용 HMI 애플리케이션 권장 (가독성 우선)

```xml
<FontFamily x:Key="IndustrialFontFamily">
    Segoe UI,           <!-- 깔끔한 서구 언어 -->
    Noto Sans KR,       <!-- 한국어 (Google Noto) -->
    Noto Sans SC,       <!-- 중국어 간체 -->
    Noto Sans JP,       <!-- 일본어 -->
    Roboto,             <!-- 폴백 -->
    sans-serif
</FontFamily>
```

### 3. 임베디드 폰트 사용 (선택적)

#### 프로젝트에 폰트 파일 추가

```text
📁 XamlDS.ITK.WPF/
  📁 Fonts/
    📄 NotoSans-Regular.ttf
    📄 NotoSansKR-Regular.otf
    📄 NotoSansSC-Regular.otf
    📄 NotoSansJP-Regular.otf
```

#### .csproj 파일 설정

```xml
<ItemGroup>
  <Resource Include="Fonts\*.ttf" />
  <Resource Include="Fonts\*.otf" />
</ItemGroup>
```

#### XAML에서 임베디드 폰트 사용

```xml
<FontFamily x:Key="EmbeddedFontFamily">
    /XamlDS.ITK.WPF;component/Fonts/#Noto Sans,
    /XamlDS.ITK.WPF;component/Fonts/#Noto Sans KR,
    /XamlDS.ITK.WPF;component/Fonts/#Noto Sans SC,
    sans-serif
</FontFamily>
```

## Avalonia UI 폰트 처리

### 1. 시스템 폰트 활용 (권장)

#### App.axaml에서 전역 폰트 설정

```xml
<Application xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="XamlDS.DemoApp.Aui.App">
    <Application.Styles>
        <FluentTheme />
        
        <Style Selector="TextBlock">
            <Setter Property="FontFamily" Value="Segoe UI, Malgun Gothic, Microsoft YaHei, Noto Sans CJK KR, sans-serif"/>
        </Style>
        
        <Style Selector="Button">
            <Setter Property="FontFamily" Value="Segoe UI, Malgun Gothic, Microsoft YaHei, Noto Sans CJK KR, sans-serif"/>
        </Style>
    </Application.Styles>
</Application>
```

### 2. Avalonia의 크로스 플랫폼 폰트 폴백

#### Windows + Linux 동시 지원 전략

```xml
<Application.Styles>
    <Style Selector="TextBlock">
        <Setter Property="FontFamily" Value="avares://XamlDS.ITK.Aui/Fonts#Noto Sans, Segoe UI, Liberation Sans, sans-serif"/>
    </Style>
</Application.Styles>
```

**폰트 폴백 동작:**

- **Windows**: Segoe UI 사용 (시스템 기본)
- **Linux**: Liberation Sans 또는 Noto Sans 사용
- **임베디드 Noto Sans**: 모든 플랫폼에서 우선 사용

### 3. Avalonia에서 임베디드 폰트 (강력 권장)

#### 프로젝트 구조

```text
📁 XamlDS.ITK.Aui/
  📁 Assets/
    📁 Fonts/
      📄 NotoSans-Regular.ttf
      📄 NotoSansKR-Regular.otf
      📄 NotoSansSC-Regular.otf
      📄 NotoSansJP-Regular.otf
```

#### .csproj 파일 설정

```xml
<ItemGroup>
  <AvaloniaResource Include="Assets\Fonts\*.ttf" />
  <AvaloniaResource Include="Assets\Fonts\*.otf" />
</ItemGroup>
```

#### App.axaml에서 사용

```xml
<Application xmlns="https://github.com/avaloniAui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="XamlDS.DemoApp.Aui.App">
    <Application.Resources>
        <!-- 폰트 패밀리 리소스 -->
        <FontFamily x:Key="DefaultFontFamily">
            avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans,
            avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans KR,
            avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans SC,
            Segoe UI,
            sans-serif
        </FontFamily>
    </Application.Resources>
    
    <Application.Styles>
        <FluentTheme />
        
        <Style Selector="TextBlock">
            <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        </Style>
        
        <Style Selector="Button">
            <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        </Style>
    </Application.Styles>
</Application>
```

### 4. C# 코드에서 동적 폰트 설정

```csharp
using Avalonia.Media;

public class FontManager
{
    public static FontFamily GetDefaultFontFamily()
    {
        return new FontFamily("avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans, Segoe UI, sans-serif");
    }
    
    public static FontFamily GetFontFamilyForCulture(string cultureCode)
    {
        return cultureCode switch
        {
            "ko-KR" => new FontFamily("avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans KR, Malgun Gothic, sans-serif"),
            "zh-CN" => new FontFamily("avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans SC, Microsoft YaHei, sans-serif"),
            "ja-JP" => new FontFamily("avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans JP, Meiryo, sans-serif"),
            _ => new FontFamily("avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans, Segoe UI, sans-serif")
        };
    }
}
```

## 권장 폰트 목록

### 1. Google Noto Sans 패밀리 (최우선 권장) ✅

**장점:**

- ✅ **완전한 다국어 지원** (500+ 언어)
- ✅ **오픈소스 무료** (SIL Open Font License)
- ✅ **크로스 플랫폼 일관성**
- ✅ **산업용 UI에 적합** (높은 가독성)
- ✅ **"No Tofu"** - 모든 문자 지원 보장

**다운로드:**

- `https://fonts.google.com/noto`

**권장 웨이트:**

- Regular (400) - 일반 텍스트
- Medium (500) - 버튼, 강조
- Bold (700) - 제목

**필요한 폰트 파일:**

```text
📄 NotoSans-Regular.ttf           (Latin, 2MB)
📄 NotoSansKR-Regular.otf         (한글, 6MB)
📄 NotoSansSC-Regular.otf         (중국어 간체, 8MB)
📄 NotoSansJP-Regular.otf         (일본어, 10MB)
📄 NotoSans-Medium.ttf            (선택적)
📄 NotoSans-Bold.ttf              (선택적)
```

**총 크기 예상:** 26-30MB (모든 언어)

### 2. 시스템 폰트 활용 (경량 옵션)

#### Windows 기본 폰트

```text
Segoe UI          (영어, 서구 언어)
Malgun Gothic     (한국어)
Microsoft YaHei   (중국어)
Yu Gothic UI      (일본어)
```

#### Linux 기본 폰트

```text
Liberation Sans   (영어, 서구 언어)
Noto Sans CJK KR  (한중일 통합, Ubuntu/Debian에 설치 필요)
```

### 3. 대안 폰트

| 폰트 | 라이선스 | 특징 |
| --- | --- | --- |
| **Roboto** | Apache 2.0 | Google의 Android 기본, 현대적 |
| **Source Sans Pro** | SIL OFL | Adobe, 코딩 친화적 |
| **Inter** | SIL OFL | UI 최적화, 작은 크기에서도 명확 |
| **Pretendard** | SIL OFL | 한국어 최적화 (Noto Sans 기반) |

## 플랫폼별 폰트 전략

### Windows (WPF + Avalonia)

**권장 전략:** 시스템 폰트 우선 + 폴백

```xml
<!-- 애플리케이션 크기 최소화 -->
<FontFamily>Segoe UI, Malgun Gothic, Microsoft YaHei, Yu Gothic UI, sans-serif</FontFamily>
```

**장점:**

- ✅ 추가 파일 불필요
- ✅ 빠른 로딩
- ✅ OS 업데이트로 폰트 개선

**단점:**

- ⚠️ 구형 Windows에서 폰트 누락 가능
- ⚠️ 사용자 폰트 설정에 따라 외관 변경

### Linux (Avalonia)

**권장 전략:** 임베디드 Noto Sans 폰트 필수

```xml
<!-- Linux는 일관된 폰트 보장 필요 -->
<FontFamily>
    avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans,
    avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans KR,
    Liberation Sans,
    sans-serif
</FontFamily>
```

**이유:**

- Linux 배포판마다 기본 폰트 다름
- 한글/중국어 폰트 미설치 가능성 높음
- 산업용 HMI는 일관성 필수

## 실전 구현 예시

### XamlDS.ITK 프로젝트 폰트 구조

```text
📁 XamlDS.ITK/
  📁 Resources/
    📄 FontHelper.cs                    (폰트 관리 유틸리티)

📁 XamlDS.ITK.WPF/
  📁 Styles/
    📄 Typography.xaml                  (WPF 폰트 스타일)

📁 XamlDS.ITK.Aui/
  📁 Assets/
    📁 Fonts/                           (임베디드 폰트)
      📄 NotoSans-Regular.ttf
      📄 NotoSansKR-Regular.otf
      📄 NotoSansSC-Regular.otf
  📁 Styles/
    📄 Typography.axaml                 (Avalonia 폰트 스타일)
```

### FontHelper.cs (공통 유틸리티)

```csharp
namespace XamlDS.ITK.Resources;

public static class FontHelper
{
    public const string DefaultFontFallback = "Segoe UI, Malgun Gothic, Microsoft YaHei, Yu Gothic UI, sans-serif";
    
    public static string GetFontFallbackForCulture(string cultureCode)
    {
        return cultureCode switch
        {
            "ko-KR" => "Malgun Gothic, Noto Sans KR, Segoe UI, sans-serif",
            "zh-CN" => "Microsoft YaHei, Noto Sans SC, Segoe UI, sans-serif",
            "ja-JP" => "Yu Gothic UI, Meiryo, Noto Sans JP, Segoe UI, sans-serif",
            "de-DE" or "fr-FR" or "es" or "it-IT" => "Segoe UI, Arial, sans-serif",
            _ => DefaultFontFallback
        };
    }
    
    public static bool IsSystemFontAvailable(string fontName)
    {
        // WPF/Avalonia에서 시스템 폰트 존재 확인 로직
        // (구현은 플랫폼별로 다름)
        return true;
    }
}
```

### WPF Typography.xaml

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- 기본 폰트 패밀리 -->
    <FontFamily x:Key="DefaultFontFamily">Segoe UI, Malgun Gothic, Microsoft YaHei, Yu Gothic UI, sans-serif</FontFamily>
    
    <!-- 산업용 HMI에 최적화된 폰트 크기 -->
    <system:Double x:Key="FontSizeSmall">12</system:Double>
    <system:Double x:Key="FontSizeNormal">14</system:Double>
    <system:Double x:Key="FontSizeLarge">16</system:Double>
    <system:Double x:Key="FontSizeTitle">20</system:Double>
    <system:Double x:Key="FontSizeHeader">24</system:Double>
    
    <!-- 기본 텍스트 스타일 -->
    <Style x:Key="BaseTextStyle" TargetType="TextBlock">
        <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        <Setter Property="FontSize" Value="{StaticResource FontSizeNormal}"/>
        <Setter Property="TextWrapping" Value="Wrap"/>
    </Style>
    
    <!-- 제목 스타일 -->
    <Style x:Key="TitleTextStyle" TargetType="TextBlock" BasedOn="{StaticResource BaseTextStyle}">
        <Setter Property="FontSize" Value="{StaticResource FontSizeTitle}"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
    </Style>
    
    <!-- 터치 친화적 버튼 스타일 (큰 폰트) -->
    <Style x:Key="TouchButtonStyle" TargetType="Button">
        <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        <Setter Property="FontSize" Value="{StaticResource FontSizeLarge}"/>
        <Setter Property="MinHeight" Value="44"/>
        <Setter Property="MinWidth" Value="100"/>
    </Style>
    
</ResourceDictionary>
```

### Avalonia Typography.axaml

```xml
<Styles xmlns="https://github.com/avaloniAui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <Style>
        <Style.Resources>
            <!-- 임베디드 폰트 사용 -->
            <FontFamily x:Key="DefaultFontFamily">
                avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans,
                avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans KR,
                avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans SC,
                Segoe UI,
                Liberation Sans,
                sans-serif
            </FontFamily>
        </Style.Resources>
    </Style>
    
    <!-- 전역 TextBlock 스타일 -->
    <Style Selector="TextBlock">
        <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        <Setter Property="FontSize" Value="14"/>
    </Style>
    
    <!-- 제목 스타일 -->
    <Style Selector="TextBlock.title">
        <Setter Property="FontSize" Value="20"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
    </Style>
    
    <!-- 터치 버튼 스타일 -->
    <Style Selector="Button.touch">
        <Setter Property="FontFamily" Value="{StaticResource DefaultFontFamily}"/>
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="MinHeight" Value="44"/>
        <Setter Property="MinWidth" Value="100"/>
    </Style>
    
</Styles>
```

## 폰트 테스트 체크리스트

### 1. 문자 렌더링 테스트

각 언어별 샘플 텍스트로 확인:

```csharp
public static class FontTestStrings
{
    public const string English = "The quick brown fox jumps over the lazy dog. 0123456789";
    public const string Korean = "다람쥐 헌 쳇바퀴에 타고파. 가나다라마바사아자차카타파하 0123456789";
    public const string ChineseSimplified = "我能吞下玻璃而不伤身体。一二三四五六七八九十 0123456789";
    public const string German = "Zwölf Boxkämpfer jagen Viktor quer über den großen Sylter Deich. 0123456789";
    public const string Japanese = "いろはにほへと ちりぬるを わかよたれそ つねならむ 0123456789";
    public const string Spanish = "El veloz murciélago hindú comía feliz cardillo y kiwi. 0123456789";
    public const string French = "Portez ce vieux whisky au juge blond qui fume. 0123456789";
}
```

### 2. 플랫폼별 테스트

- [ ] Windows 10/11에서 모든 언어 렌더링 확인
- [ ] Ubuntu/Debian Linux에서 한글/중국어 확인
- [ ] 폰트 폴백이 정상 동작하는지 확인
- [ ] 특수 문자(독일어 움라우트 등) 확인

### 3. 크기별 가독성 테스트

- [ ] 12pt (작은 텍스트)
- [ ] 14pt (기본 텍스트)
- [ ] 16pt (큰 버튼)
- [ ] 20pt 이상 (제목)

### 4. 성능 테스트

- [ ] 애플리케이션 로딩 시간
- [ ] 첫 문자 렌더링 시간
- [ ] 메모리 사용량 (임베디드 폰트 사용 시)

## 권장 설정 요약

### 최소 요구사항 (시스템 폰트)

```xml
<!-- WPF -->
<FontFamily>Segoe UI, Malgun Gothic, Microsoft YaHei, sans-serif</FontFamily>

<!-- Avalonia (Windows) -->
<FontFamily>Segoe UI, Malgun Gothic, Microsoft YaHei, sans-serif</FontFamily>
```

**장점:** 추가 파일 불필요, 빠른 로딩  
**단점:** Linux에서 한글/중국어 미지원 가능

### 권장 설정 (임베디드 Noto Sans)

```xml
<!-- Avalonia (크로스 플랫폼) -->
<FontFamily>
    avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans,
    avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans KR,
    avares://XamlDS.ITK.Aui/Assets/Fonts#Noto Sans SC,
    Segoe UI,
    sans-serif
</FontFamily>
```

**장점:** 모든 플랫폼 일관성, 완벽한 다국어 지원  
**단점:** 애플리케이션 크기 증가 (약 20-30MB)

### 산업용 HMI 최적 설정

```xml
<!-- 명확한 가독성 + 터치 친화적 -->
<FontFamily>Noto Sans, Noto Sans KR, Noto Sans SC, Roboto, sans-serif</FontFamily>
<FontSize>16</FontSize>  <!-- 터치 UI는 큰 폰트 -->
<FontWeight>Medium</FontWeight>  <!-- 약간 두껍게 -->
```

## Linux에서 폰트 설치 (시스템 폰트 사용 시)

### Ubuntu/Debian

```bash
# Noto Sans CJK 설치
sudo apt-get update
sudo apt-get install fonts-noto-cjk

# 폰트 캐시 업데이트
fc-cache -fv

# 설치 확인
fc-list | grep -i "noto"
```

### 애플리케이션 배포 시 주의사항

**README.md에 폰트 요구사항 명시:**

```markdown
## Linux에서 실행 시 필요 사항

한글, 중국어, 일본어를 올바르게 표시하려면 다음 폰트 패키지를 설치하세요:

```bash
sudo apt-get install fonts-noto-cjk
```

## 결론

### XamlDSWorks 프로젝트 권장 사항

✅ **WPF (Windows 전용)**

- 시스템 폰트 폴백 사용
- `Segoe UI, Malgun Gothic, Microsoft YaHei, Yu Gothic UI`
- 추가 파일 불필요

✅ **Avalonia UI (크로스 플랫폼)**

- **Noto Sans 패밀리 임베디드** (강력 권장)
- Regular + Medium 웨이트 포함
- 약 20-30MB 크기 증가 허용

✅ **공통**

- 폰트 폴백 메커니즘 활용
- 터치 UI를 위한 큰 폰트 크기 (16pt 이상)
- 일관된 타이포그래피 스타일 정의

✅ **산업용 HMI 특성**

- 높은 가독성 우선
- 명확한 문자 구분 (O vs 0, I vs l)
- 적절한 폰트 웨이트 (Regular 또는 Medium)

### 다음 단계

1. **Noto Sans 폰트 다운로드**
   - `https://fonts.google.com/noto`

2. **XamlDS.ITK.Aui 프로젝트에 폰트 추가**
   - Assets/Fonts 폴더 생성
   - Regular 웨이트 파일 복사

3. **Typography 스타일 정의**
   - WPF: Styles/Typography.xaml
   - Avalonia: Styles/Typography.axaml

4. **폰트 테스트 페이지 작성**
   - 모든 지원 언어 샘플 표시
   - 다양한 크기와 웨이트 확인

5. **문서화**
   - 개발자 가이드에 폰트 설정 추가
   - Linux 사용자를 위한 설치 가이드
