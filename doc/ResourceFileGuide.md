# 리소스 파일(.resx) 가이드

## 개요

이 문서는 XamlDSWorks 프로젝트에서 다국어 지원을 위한 리소스 파일 관리 가이드입니다.

## 리소스 파일 형식 비교

### .resx (Resource XML) 파일
- **.NET Framework 및 .NET (Core) 표준 리소스 파일**
- XML 형식으로 저장되며, 문자열, 이미지, 아이콘 등 다양한 리소스 저장 가능
- Visual Studio에서 Designer 지원
- **강력한 타입의 리소스 클래스 자동 생성** (`.Designer.cs`)
- 다국어 지원: `ResourceName.ko-KR.resx`, `ResourceName.ja-JP.resx` 형식
- **WPF, Avalonia UI, Console 등 모든 .NET 플랫폼에서 사용 가능**

**장점:**
- .NET 표준, 크로스 플랫폼 호환성 우수
- 컴파일 타임 타입 안정성
- Linux, macOS에서도 정상 작동

### .resw (Resource for Windows) 파일
- **UWP(Universal Windows Platform) 및 WinUI 전용**
- Uno Platform에서 UWP 호환성을 위해 사용
- XML 형식이지만 .resx와는 다른 스키마
- **Linux 환경에서는 사용 불가**
- **XamlDSWorks 프로젝트에는 부적합**

## XamlDSWorks 프로젝트 권장 사항

### 선택: .resx 파일 사용

**이유:**
1. **완벽한 크로스 플랫폼 호환성** - Windows, Linux, macOS 모두 지원
2. **.NET 표준 리소스 시스템** - Avalonia, WPF 모두 지원
3. **강력한 타입 안정성** - 컴파일 타임 체크
4. **IDE 지원** - Visual Studio, Rider 모두 우수한 편집 환경 제공

### 공통 리소스 위치: XamlDS.ITK 프로젝트

```
XamlDS.ITK (공통 라이브러리)
    ↓ 참조
├─ XamlDS.ITK.Aui (Avalonia UI)
└─ XamlDS.ITK.WPF (WPF)
```

**장점:**
- ✅ WPF에서 사용 가능
- ✅ Avalonia UI에서 사용 가능
- ✅ Linux, macOS, Windows 모두 지원
- ✅ 단일 관리 지점 (DRY 원칙)
- ✅ 양쪽 UI에서 동일한 문구 보장
- ✅ 유지보수 비용 절감

## 권장 리소스 파일 구조

### XamlDS.ITK 프로젝트 리소스 구조

```
📁 XamlDS.ITK/
  📁 Resources/
    # 공통 리소스
    📄 CommonStrings.resx              # 버튼, 레이블 등 공통 UI 요소
    📄 CommonStrings.ko-KR.resx
    📄 CommonStrings.ja-JP.resx
    
    # 메시지 유형별
    📄 Messages.resx                   # 정보/경고/에러 메시지
    📄 Messages.ko-KR.resx
    📄 Messages.ja-JP.resx
    
    # 검증 관련
    📄 Validations.resx                # 입력 검증 메시지
    📄 Validations.ko-KR.resx
    📄 Validations.ja-JP.resx
    
    # 도움말
    📄 HelpTexts.resx                  # 툴팁, 가이드 텍스트
    📄 HelpTexts.ko-KR.resx
    📄 HelpTexts.ja-JP.resx
```

### 구조화 원칙

#### ❌ 권장하지 않는 방식: UI 요소별 분리
```
ButtonStrings.resx      ❌
LabelStrings.resx       ❌
TextBoxStrings.resx     ❌
```

**이유:**
- 같은 화면의 리소스가 여러 파일에 분산
- 번역 시 여러 파일을 왔다갔다 해야 함
- Key 중복 가능성 (Button_Save, Label_Save 등)

#### ✅ 권장 방식: 기능/목적별 분리
- 기능별로 응집도 높음
- 번역 작업 시 맥락 파악 용이
- 모듈 단위로 관리 가능

## 리소스 Key 네이밍 규칙

### Prefix 패턴

- `Action_` - 버튼, 메뉴 동작
- `Label_` - 레이블, 헤더
- `Status_` - 상태 메시지
- `Info_` - 정보 메시지
- `Warning_` - 경고 메시지
- `Error_` - 에러 메시지
- `Critical_` - 치명적 에러
- `Validation_` - 검증 메시지
- `Help_` - 도움말
- `Tooltip_` - 툴팁
- `Title_` - 창/다이얼로그 제목
- `Prompt_` - 사용자 입력 요청

### 계층 구조 표현
```
Settings_General_Title
Settings_General_Description
Settings_Appearance_Theme
Settings_Appearance_Language
```

## 리소스 파일 예시

### CommonStrings.resx
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 동작 관련 버튼 -->
  <data name="Action_Save" xml:space="preserve">
    <value>Save</value>
  </data>
  
  <data name="Action_Cancel" xml:space="preserve">
    <value>Cancel</value>
  </data>
  
  <data name="Action_Delete" xml:space="preserve">
    <value>Delete</value>
  </data>
  
  <data name="Action_Apply" xml:space="preserve">
    <value>Apply</value>
  </data>

  <!-- 공통 레이블 -->
  <data name="Label_Name" xml:space="preserve">
    <value>Name</value>
  </data>
  
  <data name="Label_Description" xml:space="preserve">
    <value>Description</value>
  </data>

  <!-- 상태 -->
  <data name="Status_Loading" xml:space="preserve">
    <value>Loading...</value>
  </data>
  
  <data name="Status_Ready" xml:space="preserve">
    <value>Ready</value>
  </data>
</root>
```

### Messages.resx
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 정보 메시지 -->
  <data name="Info_ChangesSaved" xml:space="preserve">
    <value>Changes have been saved successfully.</value>
  </data>

  <!-- 경고 메시지 -->
  <data name="Warning_UnsavedChanges" xml:space="preserve">
    <value>You have unsaved changes. Do you want to continue?</value>
  </data>

  <!-- 에러 메시지 -->
  <data name="Error_FileNotFound" xml:space="preserve">
    <value>The specified file was not found.</value>
  </data>

  <!-- 치명적 에러 -->
  <data name="Critical_SystemFailure" xml:space="preserve">
    <value>A critical system error has occurred.</value>
  </data>
</root>
```

### Validations.resx
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="Validation_Required" xml:space="preserve">
    <value>This field is required.</value>
  </data>

  <data name="Validation_EmailInvalid" xml:space="preserve">
    <value>Please enter a valid email address.</value>
  </data>

  <data name="Validation_RangeError" xml:space="preserve">
    <value>Value must be between {0} and {1}.</value>
    <comment>Parameters: {0} = min value, {1} = max value</comment>
  </data>
</root>
```

### HelpTexts.resx
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="Help_ThemeSelection" xml:space="preserve">
    <value>Choose a theme to customize the appearance of the application.</value>
  </data>

  <data name="Tooltip_RefreshButton" xml:space="preserve">
    <value>Refresh the current view</value>
  </data>
</root>
```

## 사용 방법

### C# 코드에서 (WPF, Avalonia UI 공통)

```csharp
using XamlDS.ITK.Resources;

// 버튼 텍스트
string saveText = CommonStrings.Action_Save;

// 경고 메시지
string warning = Messages.Warning_UnsavedChanges;

// 검증 메시지 (파라미터 포함)
string validation = string.Format(Validations.Validation_RangeError, 0, 100);

// 도움말
string helpText = HelpTexts.Help_ThemeSelection;
```

### WPF XAML에서

```xml
<Window xmlns:res="clr-namespace:XamlDS.ITK.Resources;assembly=XamlDS.ITK">
    <Button Content="{x:Static res:CommonStrings.Action_Save}" />
    <TextBlock Text="{x:Static res:Messages.Info_ChangesSaved}" />
</Window>
```

### Avalonia UI XAML에서

```xml
<Window xmlns:res="clr-namespace:XamlDS.ITK.Resources;assembly=XamlDS.ITK">
    <Button Content="{x:Static res:CommonStrings.Action_Save}" />
    <TextBlock Text="{x:Static res:Messages.Info_ChangesSaved}" />
</Window>
```

## IDE별 .resx 편집기 지원

### Visual Studio ✅
- **네이티브 전용 편집기 제공**
- 테이블 형식으로 Key/Value 편집
- 이미지, 아이콘 등 바이너리 리소스도 GUI로 관리
- 자동으로 `.Designer.cs` 파일 생성
- 다국어 파일 간 비교 및 누락 항목 확인 기능

### JetBrains Rider ⚠️
- **제한적 지원**
- XML 텍스트 편집기로 열림 (GUI 편집기 없음)
- 구문 강조 및 XML 자동완성은 지원
- `.Designer.cs` 파일은 빌드 시 자동 생성됨
- **추가 플러그인 권장**: "ResX Manager" 플러그인

### VS Code ❌ → 🔧 (확장 필요)
- **기본 편집기 없음**
- XML 파일로 직접 편집
- **확장 기능 설치 권장**:
  - `resx-editor` (community 확장)
  - `ResX Viewer`

## 직접 XML 편집

.resx 파일은 XML이므로 어떤 텍스트 에디터에서도 편집 가능합니다.

### 직접 편집의 장점:
1. **IDE 독립적** - 어떤 에디터에서도 가능
2. **버전 관리 친화적** - Git diff 확인 용이
3. **자동화 가능** - 스크립트로 생성/수정 가능
4. **간단한 구조** - `<data name="Key">` 태그만 추가하면 됨

### 직접 편집 시 주의사항:
- **`xml:space="preserve"`** 속성 필수 (공백 보존)
- XML 특수문자 이스케이프 (`&lt;`, `&gt;`, `&amp;`)
- 인코딩은 **UTF-8** 유지

## 권장 워크플로우

| 작업 | 권장 IDE | 방법 |
|------|---------|------|
| 초기 리소스 생성 | Visual Studio | GUI 편집기 |
| 대량 번역 추가 | 텍스트 에디터 | XML 직접 편집 |
| 빠른 수정 | Rider/VS Code | XML 직접 편집 |
| 복잡한 리소스 관리 | Visual Studio | GUI 편집기 |

## 실용적인 팁

### 1. 템플릿 활용
초기 파일은 Visual Studio에서 생성하고, 이후 복사/수정:

```powershell
# 기본 파일 생성 후
cp Strings.resx Strings.ko-KR.resx
# XML 편집기로 value만 번역
```

### 2. 외부 번역 도구 연동
- Excel/CSV로 변환 → 번역 → XML로 재변환
- 번역 관리 플랫폼(Crowdin, Lokalise) 사용

### 3. 자동화 스크립트 예시

```powershell
# PowerShell로 리소스 항목 추가
$xml = [xml](Get-Content "Strings.resx")
$data = $xml.CreateElement("data")
$data.SetAttribute("name", "NewKey")
$data.SetAttribute("xml:space", "preserve")
$value = $xml.CreateElement("value")
$value.InnerText = "New Value"
$data.AppendChild($value)
$xml.root.AppendChild($data)
$xml.Save("Strings.resx")
```

## 공통 vs UI 특화 리소스

### ✅ 공통 리소스에 적합한 것 (XamlDS.ITK):
- 에러 메시지
- 공통 라벨/버튼 텍스트
- 검증 메시지
- 비즈니스 로직 관련 문구

### ⚠️ UI 특화 리소스는 분리:
- UI 프레임워크 특정 문구
- 플랫폼 특화 메뉴 구조
- 각 프레임워크의 고유 기능 설명

필요하다면 각 UI 프로젝트에 추가 리소스 파일을 둘 수도 있습니다:

```
XamlDS.ITK/Resources/CommonStrings.resx         (공통)
XamlDS.ITK.Aui/Resources/AuiStrings.resx        (Avalonia 전용)
XamlDS.ITK.WPF/Resources/WPFStrings.resx        (WPF 전용)
```

## 결론

- ✅ **.resx는 XamlDSWorks 프로젝트의 표준 리소스 형식**
- ✅ **XamlDS.ITK 프로젝트에 공통 리소스 배치**
- ✅ **기능/목적별로 리소스 파일 구분**
- ✅ **일관된 네이밍 규칙 사용 (Prefix 패턴)**
- ✅ **크로스 플랫폼 완벽 지원 (Windows, Linux, macOS)**
