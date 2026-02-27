# EditorConfig 가이드

이 문서는 `.editorconfig` 파일의 IDE 호환성과 사용 방법에 대해 설명합니다.

## 📋 목차

- [개요](#개요)
- [IDE별 지원 상황](#ide별-지원-상황)
- [호환되는 표준 속성](#호환되는-표준-속성)
- [IDE별 확장 속성](#ide별-확장-속성)
- [권장 설정](#권장-설정)
- [프로젝트 적용 예시](#프로젝트-적용-예시)
- [IDE별 확인 방법](#ide별-확인-방법)

---

## 개요

`.editorconfig` 파일은 여러 개발자와 IDE 간에 일관된 코딩 스타일을 유지하기 위한 표준 설정 파일입니다.

### 주요 특징

- ✅ 크로스 플랫폼 지원
- ✅ IDE 독립적
- ✅ 프로젝트별 설정 가능
- ✅ Git 저장소에 포함 가능

---

## IDE별 지원 상황

| IDE | 지원 방식 | 호환성 | 비고 |
|-----|----------|--------|------|
| **Visual Studio** | 네이티브 지원 (2017+) | ⭐⭐⭐⭐⭐ | .NET 확장 속성 완벽 지원 |
| **Rider** | 네이티브 지원 | ⭐⭐⭐⭐⭐ | .NET 확장 속성 완벽 지원 |
| **VS Code** | 확장 플러그인 필요 | ⭐⭐⭐⭐ | 기본 속성 지원, .NET 확장 부분 지원 |

### ✅ 결론

세 IDE 모두 `.editorconfig` 파일을 지원하며, **기본 문법은 완전히 호환**됩니다. 하나의 파일로 모든 IDE를 지원할 수 있습니다.

---

## 호환되는 표준 속성

다음 속성들은 **모든 IDE에서 동일하게 작동**합니다:

### 기본 속성

```editorconfig
# .editorconfig

root = true

# 모든 파일에 적용
[*]
charset = utf-8-bom                    # 문자 인코딩
insert_final_newline = true            # 파일 끝에 빈 줄 추가
trim_trailing_whitespace = true        # 줄 끝 공백 제거
end_of_line = crlf                     # 줄 바꿈 방식 (Windows: crlf, Linux/Mac: lf)

# 인덴트 설정
indent_style = space                   # space 또는 tab
indent_size = 4                        # 인덴트 크기
```

### 파일 타입별 설정

```editorconfig
# C# 파일
[*.cs]
indent_style = space
indent_size = 4
end_of_line = crlf

# JSON 파일
[*.json]
indent_style = space
indent_size = 2

# XML/XAML 파일
[*.{xml,xaml,axaml}]
indent_style = space
indent_size = 2

# Markdown 파일
[*.md]
trim_trailing_whitespace = false       # Markdown은 줄 끝 공백이 의미가 있음
```

---

## IDE별 확장 속성

### Visual Studio & Rider - .NET 코딩 스타일

Visual Studio와 Rider는 **.NET 관련 확장 속성**을 풍부하게 지원합니다:

```editorconfig
[*.cs]

# ====================================
# C# 포맷팅 규칙
# ====================================

# 중괄호 위치
csharp_new_line_before_open_brace = all                    # 모든 중괄호 앞에 새 줄
csharp_new_line_before_else = true                         # else 앞에 새 줄
csharp_new_line_before_catch = true                        # catch 앞에 새 줄
csharp_new_line_before_finally = true                      # finally 앞에 새 줄

# 공백 규칙
csharp_space_after_cast = false                            # (int)x
csharp_space_after_keywords_in_control_flow_statements = true  # if (condition)

# ====================================
# .NET 코딩 규칙
# ====================================

# 'this.' 한정자 사용
dotnet_style_qualification_for_field = false:warning       # this.field (경고)
dotnet_style_qualification_for_property = false:warning    # this.Property (경고)
dotnet_style_qualification_for_method = false:warning      # this.Method() (경고)
dotnet_style_qualification_for_event = false:warning       # this.Event (경고)

# 'var' 키워드 사용
csharp_style_var_for_built_in_types = true:suggestion      # var i = 5;
csharp_style_var_when_type_is_apparent = true:suggestion   # var obj = new MyClass();
csharp_style_var_elsewhere = false:suggestion              # MyClass obj = GetObject();

# 식 본문 멤버
csharp_style_expression_bodied_methods = false:none        # public int Method() => expr;
csharp_style_expression_bodied_properties = true:suggestion # public int Prop => expr;

# ====================================
# 코드 분석 규칙
# ====================================

# CA1001: 삭제 가능한 필드가 있는 형식은 삭제 가능해야 합니다
dotnet_diagnostic.CA1001.severity = warning

# CA1016: Mark assemblies with assembly version
dotnet_diagnostic.CA1016.severity = warning

# IDE0001: Simplify name
dotnet_diagnostic.IDE0001.severity = suggestion
```

### 심각도 수준

```editorconfig
# 심각도 수준 옵션:
# - none: 규칙 비활성화
# - silent: 표시하지 않음
# - suggestion: ... 으로 제안
# - warning: 노란색 밑줄 경고
# - error: 빨간색 밑줄 오류
```

### VS Code 지원 범위

⚠️ **VS Code는 .NET 확장 속성을 부분적으로만 지원**합니다:

- ✅ 기본 속성(`indent_size`, `charset` 등)은 완벽 지원
- ⚠️ `csharp_*`, `dotnet_*` 속성은 C# 확장과 함께 사용 시 부분 지원
- ❌ 코드 분석 규칙(`dotnet_diagnostic.*`)은 미지원

하지만 이러한 속성이 있어도 **에러가 발생하지 않으므로**, 하나의 `.editorconfig` 파일로 모든 IDE를 지원할 수 있습니다.

---

## 권장 설정

### 모든 IDE 호환 설정 파일

```editorconfig
# .editorconfig
root = true

# ====================================
# 모든 IDE에서 호환되는 기본 설정
# ====================================

[*]
charset = utf-8-bom
insert_final_newline = true
trim_trailing_whitespace = true
end_of_line = crlf

# C# 파일
[*.cs]
indent_style = space
indent_size = 4

# JSON 파일
[*.json]
indent_style = space
indent_size = 2

# XML/XAML/Avalonia 파일
[*.{xml,xaml,axaml}]
indent_style = space
indent_size = 2

# Markdown 파일
[*.md]
trim_trailing_whitespace = false

# ====================================
# Visual Studio & Rider 전용 (.NET)
# VS Code는 무시하지만 에러는 없음
# ====================================

[*.cs]

# C# 포맷팅
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true

# 'this.' 한정자
dotnet_style_qualification_for_field = false:warning
dotnet_style_qualification_for_property = false:warning

# 'var' 사용
csharp_style_var_for_built_in_types = true:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
csharp_style_var_elsewhere = false:suggestion

# 코드 분석
dotnet_diagnostic.CA1001.severity = warning
dotnet_diagnostic.CA1016.severity = warning
```

---

## 프로젝트 적용 예시

### .NET 10 프로젝트용 설정

현재 프로젝트(.NET 10)에 최적화된 설정:

```editorconfig
root = true

# ====================================
# 공통 설정
# ====================================

[*]
charset = utf-8-bom
insert_final_newline = true
trim_trailing_whitespace = true
end_of_line = crlf

# ====================================
# C# 파일 (.NET 10)
# ====================================

[*.cs]
indent_style = space
indent_size = 4

# C# 14.0 기능 사용
csharp_style_prefer_primary_constructors = true:suggestion
csharp_style_prefer_collection_expression = true:suggestion

# Nullable 참조 형식
dotnet_diagnostic.CS8600.severity = warning
dotnet_diagnostic.CS8602.severity = warning
dotnet_diagnostic.CS8603.severity = warning

# ====================================
# Avalonia UI 파일
# ====================================

[*.{xaml,axaml}]
indent_style = space
indent_size = 2

# ====================================
# JSON 설정 파일
# ====================================

[*.json]
indent_style = space
indent_size = 2

[launchSettings.json]
indent_style = space
indent_size = 2

# ====================================
# Markdown 문서
# ====================================

[*.md]
trim_trailing_whitespace = false
charset = utf-8-bom

# ====================================
# 프로젝트 파일
# ====================================

[*.{csproj,props,targets}]
indent_style = space
indent_size = 2
```

---

## IDE별 확인 방법

### Visual Studio

#### 1. EditorConfig 적용 확인

1. 파일을 열었을 때 상단에 "EditorConfig 설정 활성화됨" 메시지 확인
2. `도구` → `옵션` → `텍스트 편집기` → `C#` → `코드 스타일`
3. "EditorConfig 및 .NET 코딩 규칙" 섹션에서 설정 확인

#### 2. EditorConfig 파일 편집

Visual Studio에는 EditorConfig 편집기가 내장되어 있습니다:
- `.editorconfig` 파일 우클릭 → **연결 프로그램** → **EditorConfig 편집기**

### Rider

#### 1. EditorConfig 지원 활성화

1. `Settings` (Ctrl+Alt+S)
2. `Editor` → `Code Style`
3. **"Enable EditorConfig support"** 체크

#### 2. 적용 확인

1. 파일을 열었을 때 자동으로 적용됨
2. `Settings` → `Editor` → `Code Style` → `C#`에서 현재 설정 확인
3. EditorConfig 아이콘이 표시되면 해당 설정이 `.editorconfig`에서 온 것

### VS Code

#### 1. 확장 설치

```bash
# EditorConfig 확장 설치
code --install-extension EditorConfig.EditorConfig

# C# 확장 설치 (선택사항, .NET 프로젝트용)
code --install-extension ms-dotnettools.csharp
```

#### 2. 적용 확인

1. 파일을 열면 자동으로 적용됨
2. 상태바 하단에서 현재 인덴트 설정 확인 (예: "Spaces: 4")
3. 출력 패널에서 "EditorConfig" 채널 확인

#### 3. VS Code 설정

`settings.json`에서 EditorConfig 우선순위 설정:

```json
{
  "editor.formatOnSave": true,
  "editorconfig.enable": true,
  "[csharp]": {
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  }
}
```

---

## 우선순위

`.editorconfig` 설정은 다음 순서로 적용됩니다:

1. **프로젝트 루트의 `.editorconfig`** (가장 높은 우선순위)
2. 상위 디렉토리의 `.editorconfig`
3. IDE의 전역 설정 (가장 낮은 우선순위)

### 예시

```
C:\Projects\MyProject
├── .editorconfig                 (root = true)
├── src
│   ├── .editorconfig            (특정 하위 디렉토리 설정)
│   └── MyApp
│       └── Program.cs           (두 .editorconfig 설정 모두 적용)
```

---

## 문제 해결

### Visual Studio에서 적용되지 않음

1. Visual Studio 재시작
2. `도구` → `옵션` → `텍스트 편집기` → `C#` → `코드 스타일`
3. **"EditorConfig에서 코딩 규칙 생성"** 확인

### Rider에서 적용되지 않음

1. `Settings` → `Editor` → `Code Style`
2. **"Enable EditorConfig support"** 체크 확인
3. Rider 재시작

### VS Code에서 적용되지 않음

1. EditorConfig 확장이 설치되어 있는지 확인
2. `settings.json`에서 `"editorconfig.enable": true` 확인
3. VS Code 재시작

---

## 추가 리소스

- [EditorConfig 공식 사이트](https://editorconfig.org/)
- [Visual Studio의 EditorConfig 설정](https://learn.microsoft.com/visualstudio/ide/create-portable-custom-editor-options)
- [.NET 코딩 규칙](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/code-style-rule-options)
- [Rider EditorConfig 지원](https://www.jetbrains.com/help/rider/Using_EditorConfig.html)

---

## 결론

✅ **핵심 포인트:**

1. Visual Studio, Rider, VS Code 모두 `.editorconfig` 지원
2. 기본 속성(`indent_size`, `charset` 등)은 완벽히 호환
3. .NET 확장 속성은 Visual Studio/Rider에서만 완전 지원
4. 하나의 `.editorconfig` 파일로 모든 IDE 지원 가능
5. 팀 프로젝트에서 일관된 코드 스타일 유지에 매우 유용

🎉 **권장사항:**

- 프로젝트 루트에 `.editorconfig` 파일 배치
- Git 저장소에 포함
- 팀원 전체가 동일한 설정 사용
- CI/CD 파이프라인에서 코드 스타일 검증
