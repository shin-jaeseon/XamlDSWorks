# 다국어 지원 가이드

## 개요

이 문서는 XamlDSWorks 프로젝트의 다국어 지원 전략 및 권장 언어 목록을 제시합니다.

## 프로젝트 특성

- **XamlDS.ITK**: 산업용 터치 애플리케이션을 위한 툴킷
- **XamlDS.DemoApp**: Xaml Design Studio 데모 및 쇼케이스 애플리케이션
- **대상 산업**: 제조업, 산업 자동화, HMI(Human-Machine Interface)

## 권장 다국어 지원 언어

### ✅ 핵심 언어 (필수 4개)

| 언어 | Culture Code | 선정 이유 |
|------|--------------|-----------|
| **영어** | `en` | 글로벌 표준, 기본 언어 |
| **한국어** | `ko-KR` | 개발자 모국어, 한국 시장 |
| **중국어 간체** | `zh-CN` | 세계 최대 제조업 국가 |
| **독일어** | `de-DE` | 산업 자동화 강국 (Siemens, Bosch 등) |

### 🎯 추가 권장 언어 (3개)

#### 우선순위 1: 제조업 강국
| 언어 | Culture Code | 선정 이유 |
|------|--------------|-----------|
| **일본어** | `ja-JP` | 제조업 기술 선진국 (Toyota, Fanuc 등), 높은 자동화 수준 |

#### 우선순위 2: 글로벌 비즈니스 언어
| 언어 | Culture Code | 선정 이유 |
|------|--------------|-----------|
| **스페인어** | `es` | 세계 2위 모국어 사용자, 스페인 + 라틴아메리카 시장 |

#### 우선순위 3: 유럽 시장
| 언어 | Culture Code | 선정 이유 |
|------|--------------|-----------|
| **프랑스어** | `fr-FR` | 유럽 주요 언어, 항공우주/자동차 산업 강국 |

### 🔄 선택적 추가 언어 (여유 시)

| 언어 | Culture Code | 대상 시장 |
|------|--------------|-----------|
| **이탈리아어** | `it-IT` | 이탈리아 제조업 (Ferrari, Fiat 등) |
| **포르투갈어** | `pt-BR` | 브라질 (남미 최대 경제) |
| **러시아어** | `ru-RU` | 러시아 + 동유럽 시장 |
| **터키어** | `tr-TR` | 중동/유럽 게이트웨이, 제조업 성장 |
| **폴란드어** | `pl-PL` | 동유럽 제조업 허브 |

## 최종 권장: 7개 언어 패키지

### 🏆 이상적인 구성

```
1. en          (English)          - 글로벌 기본 언어
2. ko-KR       (한국어)            - 개발자/한국 시장
3. zh-CN       (简体中文)          - 중국 제조업 시장
4. de-DE       (Deutsch)          - 독일/오스트리아/스위스
5. ja-JP       (日本語)            - 일본 산업 시장
6. es          (Español)          - 스페인/라틴아메리카
7. fr-FR       (Français)         - 프랑스/벨기에/아프리카
```

### 🌍 확장형 구성 (10개) - Copilot 활용 시

위 7개 언어에 다음 3개 추가:

```
8. it-IT       (Italiano)         - 이탈리아 시장
9. pt-BR       (Português)        - 브라질 시장
10. tr-TR      (Türkçe)           - 터키/중동 시장
```

## 작업 분량 vs 효과 분석

### Copilot을 활용한 작업 시간 추정

| 언어 수 | 예상 작업 시간 | 시장 커버리지 | 추천도 | 비고 |
|--------|---------------|--------------|-------|------|
| 4개 | 2-3시간 | 기본 | ⭐⭐⭐ | 최소 요구사항 |
| **7개** | **4-5시간** | **주요 시장** | **⭐⭐⭐⭐⭐** | **권장** |
| 10개 | 6-8시간 | 글로벌 | ⭐⭐⭐⭐ | 여유 있을 경우 |
| 15개+ | 10시간+ | 광범위 | ⭐⭐ | 유지보수 부담 증가 |

### 단계별 접근 방식

#### Phase 1: 핵심 4개 언어 (먼저 완성)
```
📁 Resources/
  CommonStrings.resx (en)
  CommonStrings.ko-KR.resx
  CommonStrings.zh-CN.resx
  CommonStrings.de-DE.resx
```

**소요 시간**: 2-3시간  
**목표**: 기본적인 다국어 기능 구현 및 테스트

#### Phase 2: 추가 3개 언어 (Copilot 활용)
```
📁 Resources/
  CommonStrings.ja-JP.resx
  CommonStrings.es.resx
  CommonStrings.fr-FR.resx
```

**소요 시간**: 2시간  
**목표**: 주요 시장 커버리지 확대

#### Phase 3: 선택적 확장 (여유 시)
```
📁 Resources/
  CommonStrings.it-IT.resx
  CommonStrings.pt-BR.resx
  CommonStrings.tr-TR.resx
```

**소요 시간**: 2-3시간  
**목표**: 글로벌 임팩트 극대화

## Copilot 활용 전략

### ✅ 번역 품질이 우수한 언어

**학습 데이터가 풍부하여 Copilot이 잘 처리하는 언어:**
- 영어 (en)
- 독일어 (de-DE)
- 프랑스어 (fr-FR)
- 스페인어 (es)
- 일본어 (ja-JP)
- 중국어 간체 (zh-CN)

**특징:**
- 기술 문서 번역 품질 우수
- 산업용 용어 정확도 높음
- 즉시 사용 가능한 수준

### ⚠️ 검증이 필요한 언어

**Copilot 번역 가능하나 추가 검증 권장:**
- 터키어 (tr-TR)
- 폴란드어 (pl-PL)
- 러시아어 (ru-RU)

**권장 사항:**
- 기술 용어 일관성 확인
- 가능한 경우 네이티브 검토
- 커뮤니티 피드백 수집

## Copilot 번역 워크플로우

### 단계별 프로세스

#### 1. 영어 리소스 작성 (정확하게)
```xml
<!-- CommonStrings.resx -->
<data name="Action_Save" xml:space="preserve">
  <value>Save</value>
  <comment>Button text for saving changes</comment>
</data>
```

**중요 사항:**
- 명확하고 간결한 영어 작성
- 맥락을 위한 comment 추가
- 산업용 표준 용어 사용

#### 2. Copilot에게 번역 요청

**프롬프트 예시 (독일어):**
```
CommonStrings.resx의 모든 <value> 태그 내용을 독일어로 번역해서 
CommonStrings.de-DE.resx 파일을 만들어줘.

번역 가이드라인:
- 산업용 터치 HMI 애플리케이션에 사용되는 용어
- 전문적이고 간결한 번역
- 표준 산업 용어 사용
- 버튼 텍스트는 짧고 명확하게
```

**프롬프트 예시 (일본어):**
```
CommonStrings.resx의 모든 <value> 태그 내용을 일본어로 번역해서 
CommonStrings.ja-JP.resx 파일을 만들어줘.

번역 가이드라인:
- 산업 자동화 분야에서 사용되는 표준 용어
- 경어체 사용 (丁寧語)
- HMI 인터페이스에 적합한 간결한 표현
```

#### 3. 번역 품질 검증

**자동 검증 항목:**
- [ ] XML 구조 유효성
- [ ] Key 이름 일치 (번역하지 않음)
- [ ] 파라미터 플레이스홀더 유지 (`{0}`, `{1}` 등)
- [ ] xml:space="preserve" 속성 유지

**수동 검증 항목:**
- [ ] 전문 용어 일관성
- [ ] 문화적 적절성
- [ ] 길이 확인 (UI 레이아웃 고려)

#### 4. 네이티브 검토 (선택적)

**우선순위가 높은 언어:**
- 한국어 (개발자 직접 검토)
- 영어 (기본 언어)
- 중국어, 독일어, 일본어 (가능한 경우)

## 리소스 파일 구조 예시

### 7개 언어 지원 파일 구조

```
📁 XamlDS.ITK/
  📁 Resources/
    # CommonStrings - 공통 UI 요소
    📄 CommonStrings.resx                   (en - 기본)
    📄 CommonStrings.ko-KR.resx
    📄 CommonStrings.zh-CN.resx
    📄 CommonStrings.de-DE.resx
    📄 CommonStrings.ja-JP.resx
    📄 CommonStrings.es.resx
    📄 CommonStrings.fr-FR.resx
    📄 CommonStrings.Designer.cs            (자동 생성)
    
    # Messages - 메시지
    📄 Messages.resx                        (en - 기본)
    📄 Messages.ko-KR.resx
    📄 Messages.zh-CN.resx
    📄 Messages.de-DE.resx
    📄 Messages.ja-JP.resx
    📄 Messages.es.resx
    📄 Messages.fr-FR.resx
    📄 Messages.Designer.cs                 (자동 생성)
    
    # Validations - 검증 메시지
    📄 Validations.resx                     (en - 기본)
    📄 Validations.ko-KR.resx
    📄 Validations.zh-CN.resx
    📄 Validations.de-DE.resx
    📄 Validations.ja-JP.resx
    📄 Validations.es.resx
    📄 Validations.fr-FR.resx
    📄 Validations.Designer.cs              (자동 생성)
    
    # HelpTexts - 도움말
    📄 HelpTexts.resx                       (en - 기본)
    📄 HelpTexts.ko-KR.resx
    📄 HelpTexts.zh-CN.resx
    📄 HelpTexts.de-DE.resx
    📄 HelpTexts.ja-JP.resx
    📄 HelpTexts.es.resx
    📄 HelpTexts.fr-FR.resx
    📄 HelpTexts.Designer.cs                (자동 생성)
```

**총 파일 수**: 32개 (4개 리소스 × 7개 언어 + 4개 Designer 파일)

## 런타임 언어 전환

### .NET의 Culture 설정

```csharp
using System.Globalization;
using System.Threading;

// 애플리케이션 시작 시 또는 사용자 설정에 따라
public void SetCulture(string cultureName)
{
    var culture = new CultureInfo(cultureName);
    
    // 현재 스레드의 Culture 설정
    Thread.CurrentThread.CurrentCulture = culture;
    Thread.CurrentThread.CurrentUICulture = culture;
    
    // .NET의 기본 Culture 설정
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

// 사용 예시
SetCulture("ko-KR");  // 한국어
SetCulture("de-DE");  // 독일어
SetCulture("ja-JP");  // 일본어
```

### Culture Code 목록

```csharp
public static class SupportedCultures
{
    public const string English = "en";
    public const string Korean = "ko-KR";
    public const string ChineseSimplified = "zh-CN";
    public const string German = "de-DE";
    public const string Japanese = "ja-JP";
    public const string Spanish = "es";
    public const string French = "fr-FR";
    
    // 확장형
    public const string Italian = "it-IT";
    public const string PortugueseBrazil = "pt-BR";
    public const string Turkish = "tr-TR";
    
    public static readonly string[] All = 
    {
        English,
        Korean,
        ChineseSimplified,
        German,
        Japanese,
        Spanish,
        French
    };
    
    public static readonly Dictionary<string, string> DisplayNames = new()
    {
        { English, "English" },
        { Korean, "한국어" },
        { ChineseSimplified, "简体中文" },
        { German, "Deutsch" },
        { Japanese, "日本語" },
        { Spanish, "Español" },
        { French, "Français" }
    };
}
```

## 데모앱의 언어 선택 UI 예시

### Avalonia UI XAML

```xml
<ComboBox ItemsSource="{Binding AvailableLanguages}"
          SelectedItem="{Binding CurrentLanguage}"
          Width="150">
    <ComboBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding DisplayName}" />
        </DataTemplate>
    </ComboBox.ItemTemplate>
</ComboBox>
```

### ViewModel 예시

```csharp
using System.Collections.ObjectModel;
using System.Globalization;
using XamlDS.ITK.Resources;

public class LanguageViewModel : ViewModelBase
{
    private LanguageInfo _currentLanguage;
    
    public ObservableCollection<LanguageInfo> AvailableLanguages { get; }
    
    public LanguageInfo CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (SetProperty(ref _currentLanguage, value))
            {
                ApplyLanguage(value.CultureCode);
            }
        }
    }
    
    public LanguageViewModel()
    {
        AvailableLanguages = new ObservableCollection<LanguageInfo>
        {
            new("English", "en"),
            new("한국어", "ko-KR"),
            new("简体中文", "zh-CN"),
            new("Deutsch", "de-DE"),
            new("日本語", "ja-JP"),
            new("Español", "es"),
            new("Français", "fr-FR")
        };
        
        _currentLanguage = AvailableLanguages[0];
    }
    
    private void ApplyLanguage(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        
        // UI 갱신 이벤트 발생
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler? LanguageChanged;
}

public record LanguageInfo(string DisplayName, string CultureCode);
```

## 번역 관리 팁

### 1. 일관성 유지

**용어집(Glossary) 작성:**
```markdown
| 영어 | 한국어 | 중국어 | 독일어 | 일본어 |
|------|--------|--------|--------|--------|
| Save | 저장 | 保存 | Speichern | 保存 |
| Load | 불러오기 | 加载 | Laden | 読み込み |
| Settings | 설정 | 设置 | Einstellungen | 設定 |
| Theme | 테마 | 主题 | Thema | テーマ |
```

### 2. 길이 제약 고려

**버튼 텍스트 길이 비교:**
- 영어: "Save" (4자)
- 독일어: "Speichern" (10자)
- 프랑스어: "Enregistrer" (12자)

**권장 사항:**
- UI 디자인 시 여유 공간 확보
- 텍스트 줄바꿈 옵션 고려
- 약어 사용 가능성 검토

### 3. 문화적 고려사항

**날짜/시간 형식:**
- 미국: MM/DD/YYYY
- 한국: YYYY-MM-DD
- 독일: DD.MM.YYYY

**숫자 형식:**
- 미국: 1,234.56
- 독일: 1.234,56
- 프랑스: 1 234,56

## 유지보수 전략

### 신규 리소스 추가 시

1. **영어 리소스 먼저 추가**
   ```xml
   <!-- CommonStrings.resx -->
   <data name="Action_Export" xml:space="preserve">
     <value>Export</value>
   </data>
   ```

2. **Copilot으로 일괄 번역**
   ```
   "CommonStrings.resx에 새로 추가된 Action_Export 키를 
   모든 지원 언어로 번역해서 각 언어별 .resx 파일에 추가해줘"
   ```

3. **빌드 검증**
   - 누락된 번역 확인
   - 컴파일 오류 확인

### 번역 누락 검사 스크립트

```powershell
# PowerShell 스크립트 예시
$baseFile = "CommonStrings.resx"
$languages = @("ko-KR", "zh-CN", "de-DE", "ja-JP", "es", "fr-FR")

$baseXml = [xml](Get-Content $baseFile)
$baseKeys = $baseXml.root.data | Select-Object -ExpandProperty name

foreach ($lang in $languages) {
    $langFile = "CommonStrings.$lang.resx"
    if (Test-Path $langFile) {
        $langXml = [xml](Get-Content $langFile)
        $langKeys = $langXml.root.data | Select-Object -ExpandProperty name
        
        $missingKeys = $baseKeys | Where-Object { $_ -notin $langKeys }
        
        if ($missingKeys) {
            Write-Host "Missing keys in $langFile:" -ForegroundColor Yellow
            $missingKeys | ForEach-Object { Write-Host "  - $_" }
        }
    }
}
```

## 결론

### 권장 사항 요약

✅ **7개 언어 패키지 채택**
- 영어, 한국어, 중국어, 독일어, 일본어, 스페인어, 프랑스어

✅ **단계별 구현**
- Phase 1: 핵심 4개 (2-3시간)
- Phase 2: 추가 3개 (2시간)

✅ **Copilot 활용**
- 빠른 번역 생성
- 일관된 용어 사용
- 네이티브 검토로 품질 보장

✅ **유지보수 고려**
- 체계적인 파일 구조
- 자동화된 검증
- 명확한 워크플로우

### 예상 결과

- **개발 시간**: 4-5시간 (Copilot 활용)
- **시장 커버리지**: 주요 산업 시장 90% 이상
- **데모 효과**: 글로벌 지원 기능 시연
- **유지보수성**: 우수 (표준 .NET 리소스 시스템)
