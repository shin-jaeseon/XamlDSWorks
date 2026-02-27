# ITK (Industrial Toolkit) 구현 로드맵

## 목차
- [ITK 비전](#itk-비전)
- [전략적 접근](#전략적-접근)
- [구현 단계](#구현-단계)
- [테마 전략](#테마-전략)
- [비즈니스 모델](#비즈니스-모델)

---

## ITK 비전

### 목표
**산업용 터치 인터페이스 애플리케이션을 위한 크로스 플랫폼 표준 UI Toolkit**

### 핵심 가치 제안

1. **생산성 극대화**
   - XAML 코드 작성 최소화
   - ViewModel 중심 개발
   - 재사용 가능한 (Vm, View) Pack 제공

2. **UI 프레임워크 독립성**
   - WPF, Avalonia UI 동시 지원
   - ViewModel 100% 재사용
   - Framework 교체 비용 최소화

3. **산업 표준화**
   - 일관된 사용자 인터페이스
   - 작업자 교육 시간 단축
   - 장비 간 UI 일관성 확보
   - 안전성 향상

4. **개발 비용 절감**
   - 커스텀 컨트롤 개발 최소화
   - 검증된 컴포넌트 재사용
   - 유지보수 용이

### 최종 사용자 이점

```
작업자 A: 장비 X 사용 경험 있음
         ↓
       ITK 표준 인터페이스
         ↓
작업자 A: 장비 Y를 즉시 사용 가능
         → 교육 시간 75% 감소
         → 작업 오류 감소
         → 안전 사고 위험 감소
```

---

## 전략적 접근

### 2-Tier 아키텍처

#### Tier 1: ITK 컨트롤과 테마
**대상**: 기존 WPF/Avalonia 개발자, 현재 진행 중인 프로젝트

**제공 내용**:
- 스타일링된 기본 컨트롤 (Button, TextBox, ListBox 등)
- 산업용 테마 세트
- 터치 최적화 UI 요소

**사용 방식**:
```xml
<Button Content="Save" 
        Theme="{StaticResource ITKButtonTheme}"/>
```

**장점**:
- ✅ 기존 프로젝트에 점진적 통합 가능
- ✅ 학습 곡선 낮음
- ✅ XAML 커스터마이징 자유도 높음

#### Tier 2: ITK (Vm, View) Pack
**대상**: 새 프로젝트, ITK 표준 채택 프로젝트

**제공 내용**:
- ViewModel과 View 세트
- 완전한 기능을 가진 컴포넌트
- XAML 코드 최소화

**사용 방식**:
```xml
<itk:CommandView DataContext="{Binding SaveCvm}"/>
<itk:CircularGaugeView DataContext="{Binding TemperatureGauge}"/>
```

**장점**:
- ✅ 개발 속도 극대화
- ✅ UI 프레임워크 교체 용이
- ✅ 표준화된 동작 및 디자인

### 프로젝트 구조

```
XamlDS.ITK (공통 ViewModel 라이브러리)
    ↓ 참조
├─ XamlDS.ITK.Aui (Avalonia UI Views)
├─ XamlDS.ITK.WPF (WPF Views)
└─ XamlDS.ITK.Themes (공통 리소스)

응용 애플리케이션
    ↓ 참조
├─ YourApp (공통 ViewModel)
├─ YourApp.Aui (Avalonia UI)
└─ YourApp.WPF (WPF)
```

---

## 구현 단계

### Phase 1: Foundation ✅
**기간**: 현재 단계  
**상태**: 진행 중

#### 완료된 작업
- ✅ 프로젝트 구조 확립
- ✅ CommandVm 기반 아키텍처
- ✅ 리소스 시스템 (.resx) 구축
- ✅ 테마 시스템 기본 구조

#### 다음 작업
- 🔄 첫 번째 Pack: CommandView
- 🔄 클릭 사운드 시스템
- 🔄 액션 로깅 시스템
- 🔄 상태 애니메이션

#### Deliverables
```
XamlDS.ITK/
  ├─ ViewModels/
  │   └─ CommandVm.cs
  ├─ Resources/
  │   └─ CommonStrings.resx
  └─ Input/
      └─ RelayCommand.cs

XamlDS.ITK.Aui/
  ├─ Views/
  │   └─ CommandView.axaml (신규)
  └─ Themes/
      └─ ITKThemes.axaml
```

---

### Phase 2: Core Controls Pack
**기간**: 1-2개월  
**목표**: 기본 UI 요소 완성

#### 2.1 Input & Action Controls

##### CommandView
**기능**:
- Button 기반 명령 실행
- 클릭 사운드 재생
- 액션 로그 기록
- 상태 애니메이션 (Normal, Warning, Error)
- 비활성화 상태 시각화

**ViewModel**:
```csharp
public class CommandVm : ViewModelBase
{
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public ICommand Command { get; }
    public bool PlayClickSound { get; set; } = true;
    public bool LogAction { get; set; } = true;
    public CommandState State { get; set; }
}
```

**사용 예시**:
```xml
<itk:CommandView DataContext="{Binding SaveCvm}"/>
```

##### ToggleCommandView
**기능**:
- ON/OFF 토글
- 상태 표시 (체크마크, 색상)
- 그룹 토글 지원 (Radio 동작)

**ViewModel**:
```csharp
public class ToggleCommandVm : CommandVm
{
    public bool IsChecked { get; set; }
    public string CheckedDisplayName { get; set; }
    public string UncheckedDisplayName { get; set; }
}
```

#### 2.2 Display Controls

##### StatusView
**기능**:
- 장비/시스템 상태 표시
- 색상 코딩 (정상, 경고, 에러)
- 아이콘 + 텍스트
- 깜빡임 애니메이션 (경고/에러)

**ViewModel**:
```csharp
public class StatusVm : ViewModelBase
{
    public string StatusText { get; set; }
    public StatusLevel Level { get; set; } // Normal, Warning, Error, Critical
    public string IconKey { get; set; }
    public bool Blink { get; set; }
}
```

##### ValueDisplayView
**기능**:
- 숫자/텍스트 값 표시
- 단위 표시
- 범위 기반 색상 변경
- 포맷팅 (소수점, 천 단위 구분)

**ViewModel**:
```csharp
public class ValueDisplayVm : ViewModelBase
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public string Format { get; set; } = "F2";
    public ValueRange NormalRange { get; set; }
    public ValueRange WarningRange { get; set; }
}
```

#### 2.3 Input Controls

##### NumericInputView
**기능**:
- 터치 최적화 숫자 입력
- 가상 키패드
- 범위 검증
- 단위 선택

**ViewModel**:
```csharp
public class NumericInputVm : ViewModelBase
{
    public double Value { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public double Step { get; set; } = 1.0;
    public string Unit { get; set; }
    public bool ShowKeypad { get; set; } = true;
}
```

##### SelectionView
**기능**:
- 터치 최적화 선택 (ComboBox/ListBox)
- 큰 터치 영역
- 아이콘 지원
- 검색 기능

**ViewModel**:
```csharp
public class SelectionVm<T> : ViewModelBase
{
    public ObservableCollection<T> Items { get; }
    public T SelectedItem { get; set; }
    public bool EnableSearch { get; set; }
    public Func<T, string> DisplayMember { get; set; }
}
```

#### Phase 2 Deliverables
```
XamlDS.ITK/
  └─ ViewModels/
      ├─ CommandVm.cs (확장)
      ├─ ToggleCommandVm.cs
      ├─ StatusVm.cs
      ├─ ValueDisplayVm.cs
      ├─ NumericInputVm.cs
      └─ SelectionVm.cs

XamlDS.ITK.Aui/
  └─ Views/
      ├─ CommandView.axaml
      ├─ ToggleCommandView.axaml
      ├─ StatusView.axaml
      ├─ ValueDisplayView.axaml
      ├─ NumericInputView.axaml
      └─ SelectionView.axaml
```

---

### Phase 3: Industrial Instruments Pack
**기간**: 2-3개월  
**목표**: 산업용 계측/제어 컴포넌트

#### 3.1 Gauge Controls

##### CircularGaugeView
**기능**:
- 원형 게이지
- 바늘 또는 아크 표시
- 색상 구역 (Zones)
- 눈금 및 레이블
- 애니메이션

**ViewModel**:
```csharp
public class CircularGaugeVm : ViewModelBase
{
    public double Value { get; set; }
    public double Minimum { get; set; } = 0;
    public double Maximum { get; set; } = 100;
    public string Unit { get; set; }
    public ObservableCollection<GaugeZone> Zones { get; }
    public bool ShowNeedle { get; set; } = true;
    public bool ShowTicks { get; set; } = true;
    public GaugeStyle Style { get; set; } = GaugeStyle.Modern;
}

public class GaugeZone
{
    public double Start { get; set; }
    public double End { get; set; }
    public Color Color { get; set; }
    public string Label { get; set; }
}
```

##### LinearGaugeView
**기능**:
- 수평/수직 선형 게이지
- 바 또는 슬라이더 표시
- 색상 구역
- 목표 값 마커

**ViewModel**:
```csharp
public class LinearGaugeVm : ViewModelBase
{
    public double Value { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
    public ObservableCollection<GaugeZone> Zones { get; }
    public double? TargetValue { get; set; }
}
```

##### TankLevelView
**기능**:
- 탱크 레벨 시각화
- 액체 애니메이션
- 최소/최대 레벨 표시
- 용량 계산

**ViewModel**:
```csharp
public class TankLevelVm : ViewModelBase
{
    public double CurrentLevel { get; set; }
    public double Capacity { get; set; }
    public double MinLevel { get; set; }
    public double MaxLevel { get; set; }
    public string LiquidName { get; set; }
    public Color LiquidColor { get; set; } = Colors.Blue;
    public TankShape Shape { get; set; } = TankShape.Cylinder;
}
```

#### 3.2 Control Elements

##### ValveControlView
**기능**:
- 밸브 개폐 제어
- 상태 표시 (열림, 닫힘, 중간)
- 수동/자동 모드
- 애니메이션

**ViewModel**:
```csharp
public class ValveControlVm : ViewModelBase
{
    public ValveState State { get; set; } // Closed, Open, Intermediate
    public bool IsAutoMode { get; set; }
    public double Position { get; set; } // 0-100%
    public ICommand OpenCommand { get; }
    public ICommand CloseCommand { get; }
    public ValveType Type { get; set; } = ValveType.Gate;
}
```

##### MotorStatusView
**기능**:
- 모터 상태 시각화
- 회전 애니메이션
- RPM 표시
- 전류/전압 표시

**ViewModel**:
```csharp
public class MotorStatusVm : ViewModelBase
{
    public MotorState State { get; set; } // Stopped, Running, Error
    public double RPM { get; set; }
    public double Current { get; set; }
    public double Voltage { get; set; }
    public bool ShowAnimation { get; set; } = true;
}
```

#### 3.3 Chart Controls

##### TemperatureChartView
**기능**:
- 실시간 온도 차트
- 다중 채널
- 시간 축 스크롤
- 확대/축소

**ViewModel**:
```csharp
public class TemperatureChartVm : ViewModelBase
{
    public ObservableCollection<ChartSeries> Series { get; }
    public TimeSpan TimeWindow { get; set; } = TimeSpan.FromMinutes(10);
    public bool AutoScroll { get; set; } = true;
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
}

public class ChartSeries
{
    public string Name { get; set; }
    public ObservableCollection<DataPoint> Points { get; }
    public Color Color { get; set; }
}
```

#### Phase 3 Deliverables
```
XamlDS.ITK/
  └─ ViewModels/
      ├─ CircularGaugeVm.cs
      ├─ LinearGaugeVm.cs
      ├─ TankLevelVm.cs
      ├─ ValveControlVm.cs
      ├─ MotorStatusVm.cs
      └─ TemperatureChartVm.cs

XamlDS.ITK.Aui/
  └─ Views/
      ├─ CircularGaugeView.axaml
      ├─ LinearGaugeView.axaml
      ├─ TankLevelView.axaml
      ├─ ValveControlView.axaml
      ├─ MotorStatusView.axaml
      └─ TemperatureChartView.axaml
```

---

### Phase 4: Advanced Pack
**기간**: 3-6개월  
**목표**: 복잡한 산업용 애플리케이션 컴포넌트

#### 4.1 Process Visualization

##### ProcessFlowView
**기능**:
- 공정 흐름도 (P&ID)
- 동적 연결선
- 실시간 상태 업데이트
- 드래그 앤 드롭 편집 (디자인 모드)

**ViewModel**:
```csharp
public class ProcessFlowVm : ViewModelBase
{
    public ObservableCollection<ProcessNode> Nodes { get; }
    public ObservableCollection<ProcessConnection> Connections { get; }
    public bool IsEditMode { get; set; }
}

public class ProcessNode
{
    public string Id { get; set; }
    public ProcessNodeType Type { get; set; } // Pump, Valve, Tank, etc.
    public Point Position { get; set; }
    public object DataContext { get; set; } // 실제 장비 ViewModel
}
```

#### 4.2 Data Management

##### AlarmListView
**기능**:
- 알람 목록 표시
- 우선순위별 정렬
- 필터링 및 검색
- 확인(ACK) 기능
- 알람 히스토리

**ViewModel**:
```csharp
public class AlarmListVm : ViewModelBase
{
    public ObservableCollection<AlarmItem> ActiveAlarms { get; }
    public ObservableCollection<AlarmItem> AlarmHistory { get; }
    public ICommand AcknowledgeCommand { get; }
    public ICommand ClearCommand { get; }
}

public class AlarmItem : ViewModelBase
{
    public DateTime Timestamp { get; set; }
    public AlarmLevel Level { get; set; } // Info, Warning, Error, Critical
    public string Message { get; set; }
    public string Source { get; set; }
    public bool IsAcknowledged { get; set; }
}
```

##### TrendChartView
**기능**:
- 장기 트렌드 차트
- 시간 범위 선택
- 다중 축
- 통계 정보
- 데이터 내보내기

**ViewModel**:
```csharp
public class TrendChartVm : ViewModelBase
{
    public ObservableCollection<TrendSeries> Series { get; }
    public DateTimeRange TimeRange { get; set; }
    public bool ShowStatistics { get; set; }
    public ICommand ExportCommand { get; }
}
```

#### 4.3 Recipe & Batch Control

##### RecipeEditorView
**기능**:
- 레시피 편집
- 단계별 파라미터 설정
- 검증
- 템플릿

**ViewModel**:
```csharp
public class RecipeEditorVm : ViewModelBase
{
    public ObservableCollection<RecipeStep> Steps { get; }
    public RecipeMetadata Metadata { get; set; }
    public ICommand AddStepCommand { get; }
    public ICommand RemoveStepCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand ValidateCommand { get; }
}

public class RecipeStep
{
    public int StepNumber { get; set; }
    public string Name { get; set; }
    public ObservableCollection<RecipeParameter> Parameters { get; }
    public TimeSpan Duration { get; set; }
}
```

##### BatchControlView
**기능**:
- 배치 실행 제어
- 진행 상태 표시
- 일시 정지/재개
- 로그 기록

**ViewModel**:
```csharp
public class BatchControlVm : ViewModelBase
{
    public Recipe CurrentRecipe { get; set; }
    public int CurrentStep { get; set; }
    public BatchState State { get; set; } // Ready, Running, Paused, Completed, Error
    public double Progress { get; set; } // 0-100%
    public ObservableCollection<BatchLog> Logs { get; }
    public ICommand StartCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand StopCommand { get; }
}
```

#### Phase 4 Deliverables
```
XamlDS.ITK/
  └─ ViewModels/
      ├─ ProcessFlowVm.cs
      ├─ AlarmListVm.cs
      ├─ TrendChartVm.cs
      ├─ RecipeEditorVm.cs
      └─ BatchControlVm.cs

XamlDS.ITK.Aui/
  └─ Views/
      ├─ ProcessFlowView.axaml
      ├─ AlarmListView.axaml
      ├─ TrendChartView.axaml
      ├─ RecipeEditorView.axaml
      └─ BatchControlView.axaml
```

---

## 테마 전략

### 산업 표준 테마

#### Theme 1: ISA-101 Compliant ⭐ (권장)
- **특징**: ANSI/ISA-101 HMI 설계 표준 준수
- **철학**: "Only what's wrong should be colorful"
- **색상**: 
  - 배경: 연한 회색 (#E5E5E5)
  - 정상 상태: 회색 톤 (무채색)
  - 이상 상태: 주황/노랑/빨강으로 명확히 구분
  - 텍스트: 어두운 회색
- **대상**: 
  - 산업 표준 준수가 필요한 프로젝트
  - 화학/석유/가스/전력 플랜트
  - 안전 규정 준수 필요 환경
- **장점**:
  - 작업자 피로도 최소화
  - 이상 상태 즉시 인지
  - 국제 표준 준수
  - 장시간 모니터링 최적화

#### Theme 2: Modern (기본)
- **특징**: 플랫 디자인, 현대적
- **색상**: 밝은 배경, 고대비 텍스트, 다양한 악센트 색상
- **대상**: 일반 산업 환경, 컬러풀한 UI 선호

#### Theme 3: Classic
- **특징**: 전통적 산업용 (Siemens/Rockwell 스타일)
- **색상**: 회색 톤, 3D 효과, 그라데이션
- **대상**: 기존 시스템과의 일관성 필요

#### Theme 4: HighContrast
- **특징**: 고대비, 큰 폰트
- **색상**: 검정 배경, 밝은 텍스트
- **대상**: 열악한 조명 환경, 야간 작업

#### Theme 5: TouchOptimized
- **특징**: 대형 터치 요소 (최소 44x44dp)
- **색상**: 명확한 시각적 피드백
- **대상**: 터치 전용 디바이스

#### Theme 6: Automotive
- **특징**: 자동차 대시보드 스타일
- **색상**: 어두운 배경, 네온 악센트
- **대상**: 차량 내 시스템

#### Theme 7: Pharmaceutical
- **특징**: 클린 룸 최적화
- **색상**: 흰색/연한 파란색
- **대상**: 제약/식품 산업

### 안전 색상 표준

ISA-101 및 ISO/ANSI 표준 준수:

```csharp
// ISA-101 Compliant Color Palette
public static class ISA101Colors
{
    // Background & Surfaces
    public static Color Background => Color.FromRgb(229, 229, 229);      // #E5E5E5 Light gray
    public static Color SurfaceLight => Color.FromRgb(240, 240, 240);    // #F0F0F0
    public static Color SurfaceDark => Color.FromRgb(200, 200, 200);     // #C8C8C8

    // Equipment & Lines (Normal State) - Grayscale only
    public static Color EquipmentNormal => Color.FromRgb(140, 160, 180); // Blue-gray
    public static Color PipelineNormal => Color.FromRgb(120, 140, 160);  // Muted blue-gray
    public static Color ValveNormal => Color.FromRgb(160, 160, 160);     // Gray

    // Text
    public static Color TextPrimary => Color.FromRgb(60, 60, 60);        // Dark gray
    public static Color TextSecondary => Color.FromRgb(100, 100, 100);   // Medium gray
    public static Color TextDisabled => Color.FromRgb(180, 180, 180);    // Light gray

    // Value Display Boxes
    public static Color ValueBox => Color.FromRgb(180, 190, 200);        // Light blue-gray
    public static Color ValueText => Color.FromRgb(40, 80, 120);         // Dark blue

    // Abnormal States Only (ISA-101 Philosophy: "Only what's wrong should be colorful")
    public static Color Alarm => Color.FromRgb(255, 180, 0);             // Orange - Active alarm
    public static Color Warning => Color.FromRgb(255, 220, 100);         // Yellow - Warning
    public static Color Critical => Color.FromRgb(220, 50, 50);          // Red - Critical error
    public static Color ActiveGood => Color.FromRgb(100, 200, 100);      // Subtle green - Active good state
}

// General Safety Colors (ISO/ANSI)
public static class SafetyColors
{
    // Emergency (긴급 정지, 위험)
    public static Color Emergency => Color.FromRgb(204, 0, 0); // Red

    // Warning (주의, 경고)
    public static Color Warning => Color.FromRgb(255, 204, 0); // Yellow

    // Safe (정상 작동)
    public static Color Safe => Color.FromRgb(0, 153, 51); // Green

    // Mandatory (필수 조치)
    public static Color Mandatory => Color.FromRgb(0, 102, 204); // Blue

    // Information (정보)
    public static Color Information => Color.FromRgb(0, 153, 204); // Cyan
}
```

### ISA-101 디자인 원칙

#### 1. 색상 사용 최소화
- **정상 상태**: 무채색 (회색 톤)만 사용
- **이상 상태**: 색상 사용 (주황, 노랑, 빨강)
- **목적**: 작업자가 이상 상태를 즉시 인지

#### 2. 시각적 계층 구조
```
Level 1 (최상위): 경보 - 밝은 색상 + 깜빡임
Level 2 (중요): 경고 - 중간 색상
Level 3 (정상): 정보 - 회색 톤
Level 4 (배경): 무시 가능 - 연한 회색
```

#### 3. 일관성
- 모든 화면에서 동일한 색상 의미
- 동일한 장비는 동일한 심볼
- 동일한 상태는 동일한 표현

#### 4. 정보 밀도 관리
- 화면당 핵심 정보 20-30개 이하
- 과도한 정보 표시 방지
- 드릴다운 방식으로 상세 정보 제공

### 터치 인터페이스 가이드라인

1. **최소 터치 영역**: 44x44 dp
2. **간격**: 최소 8 dp
3. **폰트 크기**: 최소 16 pt (본문), 20 pt (버튼)
4. **피드백**: 즉각적인 시각/청각 피드백

---

## 비즈니스 모델

### 오픈 소스 + 상용 하이브리드

#### 무료 버전 (MIT License)
```
✅ XamlDS.ITK.Core          - 기본 ViewModel 라이브러리
✅ XamlDS.ITK.Controls      - 기본 컨트롤 및 테마
✅ XamlDS.ITK.Basic Pack    - Phase 2 Core Controls
```

**목표**: 커뮤니티 구축, 채택률 증가

#### 상용 라이센스 (Professional/Enterprise)
```
💎 XamlDS.ITK.Industrial Pack  - Phase 3 (계측/제어)
💎 XamlDS.ITK.Process Pack     - Phase 4 (공정 제어)
💎 XamlDS.ITK.Premium Themes   - 고급 테마
💎 기술 지원 및 컨설팅
💎 커스텀 컴포넌트 개발
```

**가격 모델**:
- Professional: 개발자당 연간 라이센스
- Enterprise: 조직 전체 라이센스 + 우선 지원
- OEM: 재배포 라이센스

### 시장 진입 전략

#### Phase 1-2: 커뮤니티 구축
- 무료 버전 배포
- 샘플 프로젝트 및 문서
- GitHub 스타 1,000+ 목표

#### Phase 3: 상용 출시
- Industrial Pack 출시
- 초기 고객 확보 (할인 제공)
- 케이스 스터디 작성

#### Phase 4: 생태계 확장
- 파트너십 (SI 업체, 장비 제조사)
- 교육 프로그램
- ITK 인증 개발자 제도

### 경쟁 우위

| 요소 | ITK | Siemens WinCC | Rockwell FactoryTalk |
|------|-----|---------------|----------------------|
| 크로스 플랫폼 | ✅ (Windows, Linux, macOS) | ❌ (Windows only) | ❌ (Windows only) |
| UI 프레임워크 | ✅ (WPF, Avalonia) | ❌ (전용) | ❌ (전용) |
| 오픈 소스 | ✅ (Core) | ❌ | ❌ |
| 진입 장벽 | 낮음 (.NET 개발자) | 높음 (전용 교육) | 높음 (전용 교육) |
| 비용 | 저렴 (무료~) | 고가 | 고가 |

---

## 성공 지표

### Phase별 목표

#### Phase 1 (Foundation)
- ✅ 프로젝트 구조 완성
- ✅ 첫 CommandView 동작
- 🎯 GitHub 공개 및 README 작성

#### Phase 2 (Core Controls)
- 🎯 5개 이상의 (Vm, View) Pack 완성
- 🎯 샘플 애플리케이션 2개 이상
- 🎯 GitHub 스타 100+

#### Phase 3 (Industrial Instruments)
- 🎯 10개 이상의 산업용 컴포넌트
- 🎯 실제 프로젝트 적용 사례 3개
- 🎯 GitHub 스타 500+

#### Phase 4 (Advanced Pack)
- 🎯 완전한 산업용 솔루션 제공
- 🎯 상용 라이센스 출시
- 🎯 파트너사 10개 이상

---

## 참고 자료

### 유사 프로젝트
- [Siemens WinCC](https://www.siemens.com/wincc)
- [Rockwell FactoryTalk](https://www.rockwellautomation.com/factorytalk)
- [Schneider EcoStruxure](https://www.se.com/ecostruxure)
- [Ignition by Inductive Automation](https://inductiveautomation.com/ignition)

### 표준 및 가이드라인
- ISO 11064: 제어실 설계 표준
- ANSI/ISA-101: HMI 설계 표준
- Material Design: 터치 인터페이스 가이드라인
- Microsoft Fluent Design: 현대적 UI 디자인

### 기술 스택
- .NET 10
- WPF
- Avalonia UI
- MVVM Pattern
- Reactive Extensions (Rx.NET)

---

## 변경 이력

| 버전 | 날짜 | 변경 내용 |
|------|------|----------|
| 1.0 | 2025-01-XX | 초안 작성 |

---

## 라이센스

이 문서는 XamlDSWorks 프로젝트의 일부입니다.

