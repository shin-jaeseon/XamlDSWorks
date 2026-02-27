# 파일 인코딩 가이드 (File Encoding Guide)

> Avalonia UI 크로스 플랫폼 프로젝트를 위한 파일 인코딩 완벽 가이드

## 📅 작성일
2025년 2월

---

## 🎯 핵심 요약

### **프로젝트 인코딩 정책**

| 파일 유형 | 인코딩 | 줄바꿈 | 이유 |
|----------|--------|--------|------|
| **C# (.cs)** | UTF-8 with BOM | CRLF | .NET 컴파일러 표준 |
| **XAML (.axaml)** | UTF-8 with BOM | CRLF | XML 표준 + Avalonia |
| **Markdown (.md)** | UTF-8 without BOM | LF | GitHub 호환성 |
| **JSON (.json)** | UTF-8 without BOM | LF | JSON 표준 |
| **Shell (.sh)** | UTF-8 without BOM | LF | Unix/Linux 표준 |

---

## 📚 목차

1. [BOM (Byte Order Mark)이란?](#bom-byte-order-mark이란)
2. [UTF-8 with BOM vs without BOM](#utf-8-with-bom-vs-without-bom)
3. [Windows-1252 vs UTF-8](#windows-1252-vs-utf-8)
4. [크로스 플랫폼 권장 설정](#크로스-플랫폼-권장-설정)
5. [.editorconfig 설정 가이드](#editorconfig-설정-가이드)
6. [실전 팁](#실전-팁)
7. [문제 해결](#문제-해결)

---

## 🔤 BOM (Byte Order Mark)이란?

### **정의**
BOM은 파일의 맨 앞에 추가되는 **보이지 않는 특수 문자**로, 파일의 인코딩 방식을 나타내는 표식입니다.

### **UTF-8 BOM의 실체**
```
UTF-8 with BOM:    EF BB BF [파일 내용]
                   ↑ 3바이트 BOM
                   
UTF-8 without BOM: [파일 내용]
                   ↑ BOM 없음
```

### **장점과 단점**

#### ✅ **장점 (with BOM)**
```
1. 인코딩 감지가 쉬움
   - 파일을 열면 즉시 "이건 UTF-8이야!" 확인 가능
   - Windows 메모장, Visual Studio가 자동 인식

2. Windows 호환성
   - Windows 환경에서 안정적
   - 한글 깨짐 방지

3. .NET 친화적
   - Roslyn 컴파일러가 BOM 선호
   - Visual Studio 기본 설정
```

#### ❌ **단점 (with BOM)**
```
1. Unix/Linux 환경에서 문제
   - Shell 스크립트: Shebang(#!/bin/bash)과 충돌
   - 예: /bin/bash^M: bad interpreter 오류

2. JSON/XML 파싱 문제
   - 일부 파서가 BOM을 데이터로 인식
   - API 응답에서 BOM이 포함될 수 있음

3. 파일 연결 문제
   - 여러 파일을 합칠 때 중간에 BOM 삽입
   - 불필요한 바이트 증가
```

---

## 🔄 UTF-8 with BOM vs without BOM

### **1. 영어만 사용하는 경우**

```
파일 내용: "Hello World"

UTF-8 with BOM:    EF BB BF 48 65 6C 6C 6F 20 57 6F 72 6C 64
                   ↑ BOM    ↑ Hello World

UTF-8 without BOM: 48 65 6C 6C 6F 20 57 6F 72 6C 64
                   ↑ Hello World

결과: BOM으로 인해 3바이트 추가
```

### **2. 한글을 사용하는 경우**

```
파일 내용: "안녕하세요"

UTF-8 with BOM:    EF BB BF EC 95 88 EB 85 95 ED 95 98 EC 84 B8 EC 9A 94
                   ↑ BOM    ↑ 안녕하세요 (각 글자 3바이트)

UTF-8 without BOM: EC 95 88 EB 85 95 ED 95 98 EC 84 B8 EC 9A 94
                   ↑ 안녕하세요

결과: 둘 다 한글 표현 가능, BOM만 3바이트 차이
```

### **3. Windows에서의 차이**

```
시나리오: 한글이 포함된 .txt 파일

UTF-8 with BOM:
✅ 메모장에서 열기 → 한글 정상 표시
✅ Visual Studio → 자동으로 UTF-8 인식
✅ PowerShell → 정상 읽기

UTF-8 without BOM:
⚠️ 메모장에서 열기 → ANSI로 오인, 한글 깨짐
⚠️ Visual Studio → 수동으로 인코딩 선택 필요
⚠️ PowerShell → 인코딩 지정 필요
```

---

## 🌍 Windows-1252 vs UTF-8

### **1. Windows-1252 (ANSI / CP-1252)**

```
특징:
├─ 1바이트 인코딩 (고정 길이)
├─ 256개 문자만 표현 (0x00 ~ 0xFF)
├─ 영어 + 서유럽 문자 지원
└─ ❌ 한글, 일본어, 중국어 표현 불가!

지원 문자:
✅ 영어: A-Z, a-z, 0-9
✅ 서유럽: é, ñ, ü, ö, ç
❌ 한글: 안녕하세요 → 표현 불가!
❌ 일본어: こんにちは → 표현 불가!
❌ 이모지: 😊 → 표현 불가!
```

### **2. UTF-8 (Unicode Transformation Format - 8bit)**

```
특징:
├─ 가변 길이 인코딩 (1~4바이트)
├─ 1,112,064개 문자 표현
├─ 전 세계 모든 언어 지원
└─ ✅ 모든 문자 + 이모지 지원!

지원 문자:
✅ 영어: A-Z (1바이트)
✅ 한글: 안녕하세요 (각 글자 3바이트)
✅ 일본어: こんにちは (각 글자 3바이트)
✅ 중국어: 你好 (각 글자 3바이트)
✅ 이모지: 😊 (4바이트)
```

### **3. 실제 예제**

```
파일 내용: "Hello 안녕"

Windows-1252:
48 65 6C 6C 6F 20 ?? ??
↑ Hello      ↑ 한글 표현 불가 (깨짐)

UTF-8:
48 65 6C 6C 6F 20 EC 95 88 EB 85 95
↑ Hello      ↑ 안녕 (정상 표현)
```

### **4. 한국에서 사용되는 인코딩 역사**

```
1980년대: EUC-KR (완성형)
          └─ 2,350개 한글만 지원
          └─ "똠", "쀍" 같은 글자 표현 불가

1990년대: CP949 (확장 완성형, Microsoft)
          └─ 11,172개 모든 한글 지원
          └─ Windows 한국어 기본 인코딩
          └─ Windows-1252 + 한글 추가

2000년대~현재: UTF-8 (유니코드)
               └─ 전 세계 모든 문자 지원
               └─ 웹 표준
               └─ 크로스 플랫폼 표준 ✅
```

---

## 🌏 크로스 플랫폼 권장 설정

### **파일 유형별 상세 가이드**

#### **1. C# 소스 코드 (.cs)**

```
권장: UTF-8 with BOM + CRLF

이유:
✅ .NET 컴파일러(Roslyn)가 BOM 선호
✅ Visual Studio 기본 설정
✅ JetBrains Rider 완벽 지원
✅ 한글 주석/문자열 안전
⚠️ 크로스 플랫폼에서도 문제 없음

예외: 없음 (항상 BOM 사용)
```

**예제:**
```csharp
// UTF-8 with BOM으로 저장
using System;

namespace MyApp
{
    /// <summary>
    /// 한글 주석도 안전하게 저장됨
    /// </summary>
    public class 한글클래스
    {
        public string Message => "안녕하세요";
    }
}
```

#### **2. XAML/AXAML 파일**

```
권장: UTF-8 with BOM + CRLF

이유:
✅ XML 기반 파일
✅ Avalonia 파서가 BOM 잘 처리
✅ Visual Studio XAML Designer 호환
✅ Rider XAML Preview 호환
✅ 한글 문자열 포함 가능

예외: 없음 (항상 BOM 사용)
```

**예제:**
```xml
<!-- UTF-8 with BOM으로 저장 -->
<UserControl xmlns="https://github.com/avaloniAui">
    <TextBlock Text="안녕하세요"/>
    <Button Content="한글 버튼"/>
</UserControl>
```

#### **3. Markdown 문서 (.md)**

```
권장: UTF-8 without BOM + LF

이유:
✅ GitHub가 UTF-8 without BOM 선호
✅ GitLab, Bitbucket 호환
✅ 대부분의 Markdown 파서 호환
⚠️ BOM이 있으면 일부 도구에서 렌더링 문제
✅ 한글 포함 가능 (BOM 없어도 OK)

예외: README.md, CHANGELOG.md 등
```

**예제:**
```markdown
# 제목 (UTF-8 without BOM으로 저장)

한글 내용도 BOM 없이 정상 표시됩니다.
```

#### **4. JSON 설정 파일 (.json)**

```
권장: UTF-8 without BOM + LF (필수!)

이유:
❌ JSON 표준(RFC 8259)은 BOM을 허용하지 않음
❌ BOM이 있으면 파싱 오류 발생
✅ Node.js, npm, yarn이 BOM 없는 파일 요구
✅ 웹 API 응답에 적합

예외: 없음 (절대 BOM 사용 금지!)
```

**예제:**
```json
{
  "name": "my-project",
  "version": "1.0.0",
  "description": "한글도 가능"
}
```

#### **5. Shell 스크립트 (.sh)**

```
권장: UTF-8 without BOM + LF (필수!)

이유:
❌ Shebang(#!/bin/bash)이 BOM과 충돌
❌ BOM이 있으면 실행 불가
✅ Linux/macOS 필수 설정
✅ CI/CD 파이프라인 호환

예외: 없음 (절대 BOM 사용 금지!)
```

**예제:**
```bash
#!/bin/bash
# UTF-8 without BOM으로 저장

echo "안녕하세요"  # 한글도 가능
```

#### **6. PowerShell 스크립트 (.ps1)**

```
권장: UTF-8 with BOM + CRLF

이유:
✅ PowerShell이 BOM 선호
✅ Windows 환경 최적화
⚠️ PowerShell 7+는 BOM 없이도 동작

예외: 크로스 플랫폼 스크립트는 UTF-8 without BOM
```

#### **7. 배치 스크립트 (.bat, .cmd)**

```
권장: UTF-8 with BOM + CRLF

이유:
✅ Windows 명령 프롬프트 호환
✅ 한글 출력 안전
⚠️ cmd.exe가 BOM을 선호

예외: 없음 (Windows 전용)
```

---

## ⚙️ .editorconfig 설정 가이드

### **현재 프로젝트 설정**

```ini
# 모든 파일 기본값 (크로스 플랫폼 최적화)
[*]
charset = utf-8                # BOM 없는 UTF-8
end_of_line = lf               # Unix 줄바꿈
insert_final_newline = true    # 파일 끝 개행
trim_trailing_whitespace = true # 후행 공백 제거

# Markdown (GitHub 호환)
[*.md]
charset = utf-8
end_of_line = lf
trim_trailing_whitespace = false  # MD에서는 공백 의미 있음

# JSON (웹 표준)
[*.json]
charset = utf-8
end_of_line = lf
indent_size = 2

# XAML/XML (.NET 표준)
[*.{xml,axaml,xaml}]
charset = utf-8-bom    # BOM 포함
end_of_line = crlf     # Windows 줄바꿈
indent_size = 2

# C# (.NET 표준)
[*.cs]
charset = utf-8-bom    # BOM 포함
end_of_line = crlf     # Windows 줄바꿈
indent_size = 4

# Shell 스크립트 (Unix 표준)
[*.sh]
charset = utf-8
end_of_line = lf

# Windows 스크립트
[*.{cmd,bat,ps1}]
charset = utf-8
end_of_line = crlf
```

### **설정 설명**

#### **charset 옵션**
```
utf-8          → UTF-8 without BOM
utf-8-bom      → UTF-8 with BOM
latin1         → ISO-8859-1 (사용 금지)
utf-16le       → UTF-16 Little Endian (특수 용도)
```

#### **end_of_line 옵션**
```
lf    → Line Feed (Unix/Linux/macOS: \n)
crlf  → Carriage Return + Line Feed (Windows: \r\n)
cr    → Carriage Return (구형 Mac: \r, 사용 금지)
```

---

## 🔧 실전 팁

### **1. Visual Studio 설정**

#### **기본 인코딩 설정**
```
Tools > Options > Environment > Documents
├─ "Save documents as Unicode..." 체크
└─ "Save new files as UTF-8 with signature" 권장

또는

File > Advanced Save Options...
├─ Encoding: Unicode (UTF-8 with signature) - Codepage 65001
└─ Line endings: Windows (CR LF)
```

#### **파일별 인코딩 변경**
```
1. 파일 열기
2. File > Advanced Save Options...
3. Encoding 선택:
   - C#/XAML: Unicode (UTF-8 with signature)
   - JSON/MD: Unicode (UTF-8 without signature)
4. 저장
```

---

### **2. JetBrains Rider 설정 (Linux/macOS)**

#### **기본 설정**
```
File > Settings > Editor > Code Style
├─ Line separator: LF (Unix and macOS)
├─ File encoding: UTF-8
└─ Add BOM: For C# files only

Editor > File Encodings
├─ Global Encoding: UTF-8
├─ Project Encoding: UTF-8
└─ Default encoding for properties files: UTF-8
```

#### **파일별 인코딩 변경**
```
1. 파일 하단 상태바에서 인코딩 클릭
2. "UTF-8" 또는 "UTF-8 with BOM" 선택
3. "Convert" 또는 "Reload" 선택
```

---

### **3. Git 설정 (.gitattributes)**

```gitattributes
# Auto detect text files and perform LF normalization
* text=auto

# Source code (UTF-8 with BOM, CRLF)
*.cs text diff=csharp eol=crlf
*.xaml text eol=crlf
*.axaml text eol=crlf

# JSON, YAML (UTF-8 without BOM, LF)
*.json text eol=lf
*.yml text eol=lf
*.yaml text eol=lf

# Markdown (UTF-8 without BOM, LF)
*.md text eol=lf

# Scripts (Platform specific)
*.sh text eol=lf
*.ps1 text eol=crlf
*.cmd text eol=crlf
*.bat text eol=crlf

# Binary files
*.png binary
*.jpg binary
*.dll binary
*.exe binary
```

---

### **4. 기존 파일 변환**

#### **Git을 사용한 일괄 변환**
```bash
# 모든 파일의 줄바꿈을 정규화
git add --renormalize .
git commit -m "Normalize line endings"

# 특정 파일만 변환
git add --renormalize *.cs
git commit -m "Normalize C# files"
```

#### **PowerShell을 사용한 변환**
```powershell
# UTF-8 without BOM → UTF-8 with BOM
$files = Get-ChildItem -Path "*.cs" -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $utf8BOM = New-Object System.Text.UTF8Encoding $true
    [System.IO.File]::WriteAllText($file.FullName, $content, $utf8BOM)
}

# UTF-8 with BOM → UTF-8 without BOM
$files = Get-ChildItem -Path "*.json" -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $utf8NoBOM = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($file.FullName, $content, $utf8NoBOM)
}
```

---

## 🚨 문제 해결

### **1. 한글이 깨지는 경우**

#### **증상**
```
안녕하세요 → ?????? 또는 ìëíì¸ì
```

#### **원인과 해결**
```
Windows-1252로 저장됨:
└─ 해결: UTF-8 with BOM으로 변환

EUC-KR로 저장됨:
└─ 해결: UTF-8로 변환 후 다시 저장

BOM 없는 UTF-8 (Windows 메모장):
└─ 해결: UTF-8 with BOM으로 변환
```

#### **Visual Studio 해결 방법**
```
1. File > Advanced Save Options...
2. Encoding: Unicode (UTF-8 with signature)
3. Save
```

---

### **2. Shell 스크립트 실행 오류**

#### **증상**
```bash
/bin/bash: ./script.sh: /bin/bash^M: bad interpreter
```

#### **원인과 해결**
```
원인:
├─ UTF-8 with BOM으로 저장됨
└─ 또는 CRLF 줄바꿈 사용

해결:
1. UTF-8 without BOM으로 변환
2. LF 줄바꿈으로 변환
```

#### **변환 명령**
```bash
# BOM 제거
sed '1s/^\xEF\xBB\xBF//' input.sh > output.sh

# CRLF → LF 변환
dos2unix script.sh

# 또는
sed -i 's/\r$//' script.sh
```

---

### **3. JSON 파싱 오류**

#### **증상**
```json
SyntaxError: Unexpected token  in JSON at position 0
```

#### **원인과 해결**
```
원인: UTF-8 with BOM으로 저장됨

해결:
1. UTF-8 without BOM으로 변환
2. JSON 파일은 절대 BOM 사용 금지
```

---

### **4. Git에서 줄바꿈 경고**

#### **증상**
```
warning: LF will be replaced by CRLF in file.txt
```

#### **원인과 해결**
```
원인: .gitattributes 설정과 실제 파일 불일치

해결:
1. .gitattributes 확인
2. git add --renormalize . 실행
3. 커밋
```

---

## 📊 빠른 참조표

### **인코딩 선택 플로우차트**

```
파일 유형 확인
    │
    ├─ C# 소스 코드?
    │   └─ UTF-8 with BOM + CRLF
    │
    ├─ XAML/XML?
    │   └─ UTF-8 with BOM + CRLF
    │
    ├─ JSON/YAML?
    │   └─ UTF-8 without BOM + LF
    │
    ├─ Markdown?
    │   └─ UTF-8 without BOM + LF
    │
    ├─ Shell 스크립트?
    │   └─ UTF-8 without BOM + LF
    │
    └─ Windows 스크립트?
        └─ UTF-8 with BOM + CRLF
```

---

## ✅ 체크리스트

### **프로젝트 설정**
- [x] `.editorconfig` 파일 추가
- [x] `.gitattributes` 파일 확인
- [ ] 팀원들과 공유
- [ ] CI/CD 파이프라인 검증

### **파일별 확인**
- [ ] C# 파일: UTF-8 BOM + CRLF
- [ ] XAML 파일: UTF-8 BOM + CRLF
- [ ] JSON 파일: UTF-8 + LF
- [ ] Markdown 파일: UTF-8 + LF
- [ ] Shell 스크립트: UTF-8 + LF

### **크로스 플랫폼 빌드 테스트**
- [ ] Windows에서 빌드
- [ ] Linux에서 빌드 (Rider)
- [ ] macOS에서 빌드 (Rider)
- [ ] GitHub Actions CI/CD

---

## 📚 참고 자료

### **공식 문서**
- [UTF-8 (RFC 3629)](https://tools.ietf.org/html/rfc3629)
- [JSON Standard (RFC 8259)](https://tools.ietf.org/html/rfc8259)
- [EditorConfig](https://editorconfig.org/)
- [Git Attributes](https://git-scm.com/docs/gitattributes)

### **Microsoft 문서**
- [.NET Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding)
- [Visual Studio Encoding](https://docs.microsoft.com/en-us/visualstudio/ide/encodings-and-line-breaks)

### **관련 도구**
- [dos2unix](https://waterlan.home.xs4all.nl/dos2unix.html)
- [file command (Linux)](https://man7.org/linux/man-pages/man1/file.1.html)

---

## 📝 변경 이력

### 2025-02-20
- 초안 작성
- UTF-8, BOM 개념 설명
- Windows-1252 vs UTF-8 비교
- 크로스 플랫폼 권장 설정
- .editorconfig 설정 가이드
- 실전 팁 및 문제 해결 방법 추가
- 빠른 참조표 및 체크리스트 추가

---

**문서 작성자**: GitHub Copilot  
**프로젝트**: XamlDS Avalonia UI  
**버전**: 1.0
