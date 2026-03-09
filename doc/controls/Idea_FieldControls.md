# Field Controls Enhancement Ideas

> 이 문서는 NumericField와 TextField 커스텀 컨트롤의 향후 개선 방향에 대한 아이디어를 정리한 문서입니다.

## 🎯 핵심 개선 사항

### 1. FieldsPanel - Custom Layout Panel

#### **목적**

- 모든 자식 Field 컨트롤의 레이아웃을 통합 관리
- 일관된 스페이싱과 컬럼 너비 적용
- 반복적인 속성 설정 제거

#### **설계 방향**

```csharp
/// <summary>
/// A custom panel for managing layout of Field controls (NumericField, TextField)
/// Provides unified layout properties for all child fields
/// </summary>
public class FieldsPanel : Panel
{
    // Shared layout properties for all child fields
    public static readonly StyledProperty<double> LabelWidthProperty = ...;
    public static readonly StyledProperty<double> ValueWidthProperty = ...;
    public static readonly StyledProperty<double> UnitWidthProperty = ...;
    public static readonly StyledProperty<double> FieldSpacingProperty = ...;
    public static readonly StyledProperty<VerticalAlignment> FieldAlignmentProperty = ...;
    
    // Optional: Column alignment
    public static readonly StyledProperty<TextAlignment> LabelAlignmentProperty = ...;
    public static readonly StyledProperty<TextAlignment> ValueAlignmentProperty = ...;
    
    // Optional: Auto-sizing
    public static readonly StyledProperty<bool> AutoSizeLabelColumnProperty = ...;
    public static readonly StyledProperty<bool> AutoSizeValueColumnProperty = ...;
    
    // Optional: Visual enhancements
    public static readonly StyledProperty<bool> AlternateRowBackgroundProperty = ...;
    public static readonly StyledProperty<bool> ShowGroupSeparatorProperty = ...;
    public static readonly StyledProperty<IBrush?> SeparatorBrushProperty = ...;
}
```

#### **사용 예제**

Before: StackPanel 사용

```xml
<StackPanel>
    <controls:NumericField Label="Weight" Value="10.4" Unit="Kg" 
                          LabelWidth="120" ValueWidth="200" UnitWidth="72"/>
    <controls:NumericField Label="Height" Value="2.4" Unit="m"
                          LabelWidth="120" ValueWidth="200" UnitWidth="72"/>
    <controls:NumericField Label="Width" Value="1.4" Unit="m"
                          LabelWidth="120" ValueWidth="200" UnitWidth="72"/>
    <controls:TextField Label="Status" Value="Connected" Suffix="(Active)"
                       LabelWidth="120" ValueWidth="200"/>
</StackPanel>
```

After: FieldsPanel 사용

```xml
<itk:FieldsPanel LabelWidth="120" 
                 ValueWidth="200" 
                 UnitWidth="72" 
                 FieldSpacing="8">
    <itk:NumericField Label="Weight" Value="10.4" Unit="Kg"/>
    <itk:NumericField Label="Height" Value="2.4" Unit="m"/>
    <itk:NumericField Label="Width" Value="1.4" Unit="m"/>
    <itk:TextField Label="Status" Value="Connected" Suffix="(Active)"/>
</itk:FieldsPanel>
```

#### **구현 주요 포인트**

- `MeasureOverride`: 자식 컨트롤의 크기 측정 및 최대 너비 계산
- `ArrangeOverride`: 자식 컨트롤 배치 및 간격 적용
- `OnChildAdded/OnChildRemoved`: 자식 Field 컨트롤에 속성 자동 적용

---

### 2. Field State System

#### **상태 정의**

```csharp
/// <summary>
/// Represents the state of a field control
/// </summary>
public enum FieldState
{
    /// <summary>
    /// Default/Normal state
    /// </summary>
    Default,
    
    /// <summary>
    /// Informational state (Blue accent)
    /// </summary>
    Info,
    
    /// <summary>
    /// Warning state (Orange/Yellow accent)
    /// </summary>
    Warning,
    
    /// <summary>
    /// Critical/Error state (Red accent)
    /// </summary>
    Critical,
    
    /// <summary>
    /// Success/OK state (Green accent)
    /// </summary>
    Success
}
```

#### **NumericField/TextField에 추가할 속성**

```csharp
public class NumericField : TemplatedControl
{
    // ... existing properties ...
    
    /// <summary>
    /// Defines the <see cref="State"/> property.
    /// </summary>
    public static readonly StyledProperty<FieldState> StateProperty =
        AvaloniaProperty.Register<NumericField, FieldState>(
            nameof(State), 
            defaultValue: FieldState.Default);
    
    /// <summary>
    /// Gets or sets the state of the field (Default, Info, Warning, Critical, Success)
    /// </summary>
    public FieldState State
    {
        get => GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }
    
    /// <summary>
    /// Defines the <see cref="ShowStateIcon"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowStateIconProperty =
        AvaloniaProperty.Register<NumericField, bool>(
            nameof(ShowStateIcon), 
            defaultValue: false);
    
    /// <summary>
    /// Gets or sets whether to show the state icon
    /// </summary>
    public bool ShowStateIcon
    {
        get => GetValue(ShowStateIconProperty);
        set => SetValue(ShowStateIconProperty, value);
    }
}
```

#### **스타일 적용 (ITKBaseStyles.axaml)**

```xml
<!-- Default State (기존 스타일 유지) -->
<Style Selector="controls|NumericField">
    <!-- ... existing template ... -->
</Style>

<!-- Info State - Blue accent -->
<Style Selector="controls|NumericField[(State)=Info]">
    <Setter Property="Foreground" Value="#2196F3"/>
</Style>

<!-- Warning State - Orange/Yellow accent -->
<Style Selector="controls|NumericField[(State)=Warning]">
    <Setter Property="Foreground" Value="#FF9800"/>
    <Style.Animations>
        <Animation Duration="0:0:0.3" IterationCount="Infinite" PlaybackDirection="Alternate">
            <KeyFrame Cue="0%">
                <Setter Property="Opacity" Value="1.0"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Opacity" Value="0.7"/>
            </KeyFrame>
        </Animation>
    </Style.Animations>
</Style>

<!-- Critical State - Red accent -->
<Style Selector="controls|NumericField[(State)=Critical]">
    <Setter Property="Foreground" Value="#F44336"/>
    <Setter Property="FontWeight" Value="Bold"/>
</Style>

<!-- Success State - Green accent -->
<Style Selector="controls|NumericField[(State)=Success]">
    <Setter Property="Foreground" Value="#4CAF50"/>
</Style>
```

#### 사용 예제

```xml
<itk:FieldsPanel LabelWidth="120" ValueWidth="200" UnitWidth="72" FieldSpacing="8">
    <!-- Default state -->
    <itk:NumericField Label="Weight" 
                      Value="10.4" 
                      Unit="Kg" 
                      State="Default"/>
    
    <!-- Info state -->
    <itk:NumericField Label="Height" 
                      Value="2.4" 
                      Unit="m" 
                      State="Info"/>
    
    <!-- Warning state with icon -->
    <itk:NumericField Label="Temperature" 
                      Value="95.8" 
                      Unit="°C" 
                      State="Warning"
                      ShowStateIcon="True"/>
    
    <!-- Critical state -->
    <itk:NumericField Label="Pressure" 
                      Value="150.2" 
                      Unit="PSI" 
                      State="Critical"
                      ShowStateIcon="True"/>
    
    <!-- Success state -->
    <itk:TextField Label="Status" 
                   Value="Connected" 
                   State="Success" 
                   Suffix="(Active)"/>
</itk:FieldsPanel>
```

---

## 💡 추가 개선 아이디어

### 1. State 아이콘 시스템

```csharp
/// <summary>
/// Gets the icon path data based on current state
/// </summary>
public Geometry? StateIconData
{
    get
    {
        return State switch
        {
            FieldState.Info => (Geometry?)Application.Current?.FindResource("InfoIcon"),
            FieldState.Warning => (Geometry?)Application.Current?.FindResource("WarningIcon"),
            FieldState.Critical => (Geometry?)Application.Current?.FindResource("ErrorIcon"),
            FieldState.Success => (Geometry?)Application.Current?.FindResource("CheckIcon"),
            _ => null
        };
    }
}
```

Template에 아이콘 영역 추가

```xml
<Grid ColumnDefinitions="Auto,Auto,Auto,Auto">
    <!-- State Icon Column -->
    <PathIcon Grid.Column="0"
              Data="{Binding RelativeSource={RelativeSource TemplatedParent}, Path=StateIconData}"
              Foreground="{TemplateBinding Foreground}"
              Width="16" Height="16"
              Margin="0,0,4,0"
              IsVisible="{TemplateBinding ShowStateIcon}"/>
    
    <!-- Label Column -->
    <TextBlock Grid.Column="1" ... />
    
    <!-- Value Column -->
    <TextBlock Grid.Column="2" ... />
    
    <!-- Unit Column -->
    <TextBlock Grid.Column="3" ... />
</Grid>
```

### 2. State 변경 애니메이션

```xaml
<Style Selector="controls|NumericField[(State)=Warning]">
    <Style.Animations>
        <!-- Fade in/out animation -->
        <Animation Duration="0:0:0.5" IterationCount="Infinite" PlaybackDirection="Alternate">
            <KeyFrame Cue="0%">
                <Setter Property="Opacity" Value="1.0"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Opacity" Value="0.6"/>
            </KeyFrame>
        </Animation>
        
        <!-- Color transition -->
        <Animation Duration="0:0:0.3">
            <KeyFrame Cue="0%">
                <Setter Property="Foreground" Value="{StaticResource Fg0Brush}"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Foreground" Value="#FF9800"/>
            </KeyFrame>
        </Animation>
    </Style.Animations>
</Style>
```

### 3. FieldsPanel 고급 기능

```csharp
/// <summary>
/// Automatically calculate label column width based on content
/// </summary>
public bool AutoSizeLabelColumn { get; set; }

/// <summary>
/// Automatically calculate value column width based on content
/// </summary>
public bool AutoSizeValueColumn { get; set; }

/// <summary>
/// Apply alternating background color to rows
/// </summary>
public bool AlternateRowBackground { get; set; }

/// <summary>
/// Background brush for alternate rows
/// </summary>
public IBrush? AlternateRowBrush { get; set; }

/// <summary>
/// Show separator line between fields
/// </summary>
public bool ShowGroupSeparator { get; set; }

/// <summary>
/// Separator brush
/// </summary>
public IBrush? SeparatorBrush { get; set; }

/// <summary>
/// Separator thickness
/// </summary>
public double SeparatorThickness { get; set; } = 1.0;
```

### 4. Validation 통합

```csharp
/// <summary>
/// Validation rule for the field value
/// </summary>
public IValueValidator? Validator { get; set; }

/// <summary>
/// Minimum allowed value (for numeric fields)
/// </summary>
public double? MinValue { get; set; }

/// <summary>
/// Maximum allowed value (for numeric fields)
/// </summary>
public double? MaxValue { get; set; }

/// <summary>
/// Validation error message
/// </summary>
public string? ValidationError { get; set; }

/// <summary>
/// Automatically set state based on validation result
/// </summary>
public bool AutoSetStateOnValidation { get; set; } = true;
```

---

## 🎯 구현 우선순위

### Phase 1: 기본 State 시스템 (1주)

- [x] NumericField, TextField 완성
- [ ] FieldState enum 추가
- [ ] State property를 Field 컨트롤에 추가
- [ ] State별 기본 스타일 정의 (색상만)
- [ ] 간단한 사용 예제 작성

**완료 기준**: State 속성 변경 시 색상이 변경됨

### Phase 2: FieldsPanel 기본 구현 (1-2주)

- [ ] FieldsPanel 클래스 생성
- [ ] 기본 레이아웃 로직 구현 (MeasureOverride, ArrangeOverride)
- [ ] LabelWidth, ValueWidth, UnitWidth 속성 구현
- [ ] FieldSpacing 속성 구현
- [ ] 자식 Field에 속성 자동 적용
- [ ] FieldsPanel 기본 스타일 정의

**완료 기준**: FieldsPanel 내부의 Field 컨트롤들이 일관된 레이아웃으로 표시됨

### Phase 3: State 아이콘 및 애니메이션 (1주)

- [ ] State 아이콘 리소스 정의
- [ ] ShowStateIcon 속성 구현
- [ ] Template에 아이콘 영역 추가
- [ ] State 변경 애니메이션 구현
- [ ] Warning state 깜빡임 효과

**완료 기준**: State 아이콘이 표시되고 애니메이션이 동작함

### Phase 4: FieldsPanel 고급 기능 (1-2주)

- [ ] AutoSizeLabelColumn 구현
- [ ] AutoSizeValueColumn 구현
- [ ] AlternateRowBackground 구현
- [ ] ShowGroupSeparator 구현
- [ ] 성능 최적화

**완료 기준**: 고급 레이아웃 기능이 모두 동작함

### Phase 5: Validation 통합 (1-2주)

- [ ] IValueValidator 인터페이스 설계
- [ ] MinValue, MaxValue 검증
- [ ] ValidationError 표시
- [ ] AutoSetStateOnValidation 구현
- [ ] 에러 툴팁 표시

**완료 기준**: 유효성 검사 실패 시 자동으로 State가 변경되고 에러 메시지가 표시됨

---

## 📋 실제 사용 시나리오

### **시나리오 1: 산업용 센서 모니터링**

```xml
<itk:FieldsPanel LabelWidth="150" 
                 ValueWidth="120" 
                 UnitWidth="60" 
                 FieldSpacing="12"
                 ShowGroupSeparator="True">
    
    <!-- Temperature Monitoring -->
    <itk:NumericField Label="Temperature" 
                      Value="{Binding Temperature}" 
                      Unit="°C"
                      State="{Binding TemperatureState}"
                      ShowStateIcon="True"
                      MinValue="0"
                      MaxValue="100"
                      Format="F1"/>
    
    <!-- Pressure Monitoring -->
    <itk:NumericField Label="Pressure" 
                      Value="{Binding Pressure}" 
                      Unit="Bar"
                      State="{Binding PressureState}"
                      ShowStateIcon="True"
                      MinValue="0"
                      MaxValue="10"
                      Format="F2"/>
    
    <!-- Flow Rate Monitoring -->
    <itk:NumericField Label="Flow Rate" 
                      Value="{Binding FlowRate}" 
                      Unit="L/min"
                      State="{Binding FlowState}"
                      ShowStateIcon="True"
                      MinValue="0"
                      MaxValue="500"
                      Format="F0"/>
    
    <!-- System Status -->
    <itk:TextField Label="System Status" 
                   Value="{Binding SystemStatus}"
                   State="{Binding StatusState}"
                   ShowStateIcon="True"/>
    
    <!-- Connection Status -->
    <itk:TextField Label="Connection" 
                   Value="{Binding ConnectionStatus}"
                   State="{Binding ConnectionState}"
                   Prefix="Status:"
                   ShowStateIcon="True"/>
</itk:FieldsPanel>
```

ViewModel 예제

```csharp
public class SensorMonitorViewModel : ViewModelBase
{
    private double _temperature = 85.3;
    private FieldState _temperatureState = FieldState.Warning;
    
    public double Temperature
    {
        get => _temperature;
        set
        {
            this.RaiseAndSetIfChanged(ref _temperature, value);
            UpdateTemperatureState();
        }
    }
    
    public FieldState TemperatureState
    {
        get => _temperatureState;
        set => this.RaiseAndSetIfChanged(ref _temperatureState, value);
    }
    
    private void UpdateTemperatureState()
    {
        if (Temperature > 90)
            TemperatureState = FieldState.Critical;
        else if (Temperature > 80)
            TemperatureState = FieldState.Warning;
        else if (Temperature > 20)
            TemperatureState = FieldState.Success;
        else
            TemperatureState = FieldState.Default;
    }
}
```

### **시나리오 2: 설정 화면**

```xaml
<itk:FieldsPanel LabelWidth="180" 
                 ValueWidth="200" 
                 FieldSpacing="16"
                 AlternateRowBackground="True">
    
    <itk:TextField Label="Device Name" 
                   Value="{Binding DeviceName}"
                   State="Default"/>
    
    <itk:TextField Label="IP Address" 
                   Value="{Binding IpAddress}"
                   State="{Binding IpAddressState}"/>
    
    <itk:NumericField Label="Timeout (seconds)" 
                      Value="{Binding Timeout}" 
                      Unit="sec"
                      MinValue="1"
                      MaxValue="300"
                      Format="F0"/>
    
    <itk:NumericField Label="Retry Count" 
                      Value="{Binding RetryCount}"
                      MinValue="0"
                      MaxValue="10"
                      Format="F0"/>
</itk:FieldsPanel>
```

### **시나리오 3: 대시보드 요약 정보**

```xaml
<itk:FieldsPanel LabelWidth="120" 
                 ValueWidth="150" 
                 UnitWidth="80"
                 FieldSpacing="8"
                 Classes="Compact">
    
    <itk:NumericField Label="Total Users" 
                      Value="{Binding TotalUsers}" 
                      Format="N0"
                      State="Info"/>
    
    <itk:NumericField Label="Active Sessions" 
                      Value="{Binding ActiveSessions}" 
                      Format="N0"
                      State="Success"/>
    
    <itk:NumericField Label="CPU Usage" 
                      Value="{Binding CpuUsage}" 
                      Unit="%"
                      State="{Binding CpuState}"
                      ShowStateIcon="True"
                      Format="F1"/>
    
    <itk:NumericField Label="Memory Usage" 
                      Value="{Binding MemoryUsage}" 
                      Unit="GB"
                      State="{Binding MemoryState}"
                      ShowStateIcon="True"
                      Format="F2"/>
    
    <itk:TextField Label="Last Update" 
                   Value="{Binding LastUpdateTime}"
                   State="Default"/>
</itk:FieldsPanel>
```

---

## 🔧 기술적 고려사항

### **1. 성능 최적화**

- FieldsPanel의 MeasureOverride/ArrangeOverride를 효율적으로 구현
- 자식 컨트롤이 많을 경우 가상화 고려
- State 변경 시 전체 리렌더링 방지

### **2. 접근성 (Accessibility)**

- State 정보를 스크린 리더에 전달
- 키보드 네비게이션 지원
- 고대비 테마 지원

### **3. 테마 통합**

- ITKLight, ITKDark, ITKGreenPunk, ITKBluePunk 테마와 호환
- State 색상을 테마 리소스로 정의
- 다크/라이트 테마에서 가독성 보장

### **4. 디자인 타임 지원**

- Visual Studio Designer에서 미리보기 지원
- Design.PreviewWith를 통한 샘플 데이터 제공

---

## 📚 참고 자료

### **Avalonia UI 관련**

- [Custom Controls Guide](https://docs.avaloniAui.net/docs/guides/custom-controls/)
- [Custom Panels](https://docs.avaloniAui.net/docs/concepts/custom-controls/how-to-create-custom-panels)
- [Styling](https://docs.avaloniAui.net/docs/basics/user-interface/styling)
- [Animations](https://docs.avaloniAui.net/docs/animations/)

### **유사 컨트롤 참조**

- WPF PropertyGrid
- DevExpress PropertyGridControl
- Telerik PropertyGrid

---

## ✅ 체크리스트

### **Phase 1 완료 조건**

- [ ] FieldState enum 정의
- [ ] NumericField.State 속성 추가
- [ ] TextField.State 속성 추가
- [ ] 5가지 State별 스타일 정의
- [ ] README.md에 사용법 추가
- [ ] 테스트 View 작성

### **Phase 2 완료 조건**

- [ ] FieldsPanel 클래스 생성
- [ ] 레이아웃 로직 구현
- [ ] 속성 자동 적용 구현
- [ ] 스타일 정의
- [ ] README.md 업데이트
- [ ] 테스트 View 업데이트

### **전체 프로젝트 완료 조건**

- [ ] 모든 Phase 완료
- [ ] 단위 테스트 작성
- [ ] 성능 테스트 통과
- [ ] 문서 완성
- [ ] 예제 프로젝트 작성
- [ ] 코드 리뷰 완료

---

## 🎨 디자인 가이드

### **색상 팔레트 (기본)**

```text
Default:  Fg0Brush (테마 기본 색상)
Info:     #2196F3 (Blue)
Warning:  #FF9800 (Orange)
Critical: #F44336 (Red)
Success:  #4CAF50 (Green)
```

### **타이포그래피**

```text
Label:     기본 폰트, 왼쪽 정렬
Value:     기본 폰트, 오른쪽 정렬 (숫자), 왼쪽 정렬 (텍스트)
Unit:      기본 폰트, 왼쪽 정렬
Critical:  Bold 폰트 가중치
```

### **간격**

```text
FieldSpacing:    8px (기본), 12px (넓게), 4px (좁게)
Column Margin:   8px (기본)
Padding:         12px (BodyPanel 내부)
```
