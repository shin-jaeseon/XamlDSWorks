# ISA-101 테마 구현 가이드

## 개요

ANSI/ISA-101 표준을 준수하는 HMI (Human-Machine Interface) 테마 구현 가이드입니다.

## ISA-101 핵심 철학

> **"Only what's wrong should be colorful"**
> 
> 정상 상태는 무채색(회색 톤)으로, 이상 상태만 색상으로 강조하여
> 작업자가 문제를 즉시 인지할 수 있도록 합니다.

## 디자인 원칙

### 1. 색상 사용 규칙

#### ✅ 정상 상태 (Normal State)
```
- 배경: 연한 회색 (#E5E5E5)
- 장비/파이프: 회색 ~ 파란회색 톤
- 텍스트: 어두운 회색
- 값 표시: 연한 파란회색 박스
```

#### ⚠️ 이상 상태 (Abnormal State)
```
- 알람(Alarm): 주황색 (#FFB400)
- 경고(Warning): 노란색 (#FFDC64)
- 치명적(Critical): 빨간색 (#DC3232)
- 활성 양호(Active Good): 은은한 녹색
```

### 2. 정보 계층 구조

| Level | 용도 | 시각적 처리 |
|-------|------|-------------|
| 1 | 긴급 알람 | 밝은 색상 + 깜빡임 |
| 2 | 경고 | 중간 색상 |
| 3 | 정상 정보 | 회색 톤 |
| 4 | 배경 요소 | 연한 회색 |

### 3. 화면 구성

#### 정보 밀도
- **권장**: 화면당 20-30개의 핵심 정보
- **최대**: 40개를 넘지 않도록
- **상세 정보**: 드릴다운(클릭/터치)으로 제공

#### 터치 영역
- **최소 크기**: 44x44 dp
- **간격**: 최소 8 dp
- **버튼 라벨**: 최소 20pt

## 색상 팔레트

### 기본 색상

```xml
<!-- Background & Surfaces -->
<Color x:Key="ISA101.Background">#E5E5E5</Color>
<Color x:Key="ISA101.SurfaceLight">#F0F0F0</Color>
<Color x:Key="ISA101.SurfaceDark">#C8C8C8</Color>

<!-- Equipment (Normal State) -->
<Color x:Key="ISA101.Equipment.Normal">#8CA0B4</Color>
<Color x:Key="ISA101.Pipeline.Normal">#788CA0</Color>
<Color x:Key="ISA101.Valve.Normal">#A0A0A0</Color>

<!-- Text -->
<Color x:Key="ISA101.Text.Primary">#3C3C3C</Color>
<Color x:Key="ISA101.Text.Secondary">#646464</Color>
<Color x:Key="ISA101.Text.Disabled">#B4B4B4</Color>

<!-- Value Display -->
<Color x:Key="ISA101.Value.Background">#B4BEC8</Color>
<Color x:Key="ISA101.Value.Text">#285078</Color>

<!-- Abnormal States -->
<Color x:Key="ISA101.Alarm">#FFB400</Color>
<Color x:Key="ISA101.Warning">#FFDC64</Color>
<Color x:Key="ISA101.Critical">#DC3232</Color>
<Color x:Key="ISA101.ActiveGood">#64C864</Color>
```

### 상태별 색상 매핑

| 상태 | 색상 | 용도 |
|------|------|------|
| Normal | Gray | 정상 작동 |
| Active | Light Green | 활성 정상 |
| Warning | Yellow | 주의 필요 |
| Alarm | Orange | 알람 발생 |
| Critical | Red | 치명적 오류 |
| Off/Disabled | Light Gray | 비활성 |

## 컴포넌트 스타일 가이드

### 버튼 (CommandView)

```xml
<!-- ISA-101 Button Style -->
<Style Selector="Button.ISA101">
    <Setter Property="Background" Value="{StaticResource ISA101.SurfaceLight}"/>
    <Setter Property="Foreground" Value="{StaticResource ISA101.Text.Primary}"/>
    <Setter Property="BorderBrush" Value="{StaticResource ISA101.SurfaceDark}"/>
    <Setter Property="BorderThickness" Value="1"/>
    <Setter Property="CornerRadius" Value="4"/>
    <Setter Property="Padding" Value="12,8"/>
    <Setter Property="MinHeight" Value="44"/>
    <Setter Property="FontSize" Value="20"/>
</Style>

<!-- Warning State -->
<Style Selector="Button.ISA101.Warning">
    <Setter Property="Background" Value="{StaticResource ISA101.Warning}"/>
    <Setter Property="Foreground" Value="{StaticResource ISA101.Text.Primary}"/>
</Style>

<!-- Alarm State -->
<Style Selector="Button.ISA101.Alarm">
    <Setter Property="Background" Value="{StaticResource ISA101.Alarm}"/>
    <Setter Property="Foreground" Value="#FFFFFF"/>
    <!-- Add blinking animation -->
</Style>
```

### 값 표시 (ValueDisplayView)

```xml
<Border Background="{StaticResource ISA101.Value.Background}"
        BorderBrush="{StaticResource ISA101.SurfaceDark}"
        BorderThickness="1"
        CornerRadius="2"
        Padding="8,4">
    <StackPanel>
        <TextBlock Text="{Binding Value}" 
                   Foreground="{StaticResource ISA101.Value.Text}"
                   FontSize="18"
                   FontWeight="Bold"/>
        <TextBlock Text="{Binding Unit}"
                   Foreground="{StaticResource ISA101.Text.Secondary}"
                   FontSize="12"/>
    </StackPanel>
</Border>
```

### 게이지 (CircularGaugeView)

```csharp
// ISA-101 Gauge Zone Configuration
var temperatureGauge = new CircularGaugeVm
{
    Value = 75,
    Minimum = 0,
    Maximum = 150,
    Unit = "°C",
    // Normal zone: Gray
    Zones = 
    {
        new GaugeZone 
        { 
            Start = 0, 
            End = 100, 
            Color = ISA101Colors.EquipmentNormal,
            Label = "Normal"
        },
        // Warning zone: Yellow
        new GaugeZone 
        { 
            Start = 100, 
            End = 120, 
            Color = ISA101Colors.Warning,
            Label = "Warning"
        },
        // Critical zone: Red
        new GaugeZone 
        { 
            Start = 120, 
            End = 150, 
            Color = ISA101Colors.Critical,
            Label = "Critical"
        }
    },
    // Needle color: Gray for normal, changes based on zone
    NeedleColor = ISA101Colors.EquipmentNormal
};
```

## P&ID (Process Flow) 스타일

### 파이프라인

```xml
<!-- Normal State: Gray -->
<Line Stroke="{StaticResource ISA101.Pipeline.Normal}"
      StrokeThickness="3"/>

<!-- Flowing State: Still gray, but with animation -->
<Line Stroke="{StaticResource ISA101.Pipeline.Normal}"
      StrokeThickness="3"
      StrokeDashArray="10,5">
    <Line.Styles>
        <Style Selector="Line.Flowing">
            <Style.Animations>
                <Animation Duration="0:0:2" IterationCount="Infinite">
                    <KeyFrame Cue="0%">
                        <Setter Property="StrokeDashOffset" Value="0"/>
                    </KeyFrame>
                    <KeyFrame Cue="100%">
                        <Setter Property="StrokeDashOffset" Value="15"/>
                    </KeyFrame>
                </Animation>
            </Style.Animations>
        </Style>
    </Line.Styles>
</Line>

<!-- Alarm State: Orange -->
<Line Stroke="{StaticResource ISA101.Alarm}"
      StrokeThickness="3"/>
```

### 장비 심볼

```xml
<!-- Tank: Normal -->
<Path Fill="{StaticResource ISA101.Equipment.Normal}"
      Stroke="{StaticResource ISA101.SurfaceDark}"
      StrokeThickness="2"
      Data="M 10,10 L 90,10 L 90,80 A 40,20 0 0 1 10,80 Z"/>

<!-- Pump: Normal -->
<Ellipse Fill="{StaticResource ISA101.Equipment.Normal}"
         Stroke="{StaticResource ISA101.SurfaceDark}"
         StrokeThickness="2"
         Width="60" Height="60"/>

<!-- Valve: Normal -->
<Path Fill="{StaticResource ISA101.Valve.Normal}"
      Stroke="{StaticResource ISA101.SurfaceDark}"
      StrokeThickness="2"
      Data="M 30,10 L 50,30 L 30,50 L 10,30 Z"/>
```

## 애니메이션 가이드

### 알람 깜빡임

```xml
<Style Selector="Border.Alarm">
    <Style.Animations>
        <Animation Duration="0:0:1" IterationCount="Infinite">
            <KeyFrame Cue="0%">
                <Setter Property="Opacity" Value="1"/>
            </KeyFrame>
            <KeyFrame Cue="50%">
                <Setter Property="Opacity" Value="0.3"/>
            </KeyFrame>
            <KeyFrame Cue="100%">
                <Setter Property="Opacity" Value="1"/>
            </KeyFrame>
        </Animation>
    </Style.Animations>
</Style>
```

### 공정 흐름 애니메이션

- **정상 흐름**: 은은한 점선 이동 (회색)
- **이상 흐름**: 깜빡임 + 색상 변경

## 타이포그래피

### 폰트 크기

| 용도 | 크기 | 가중치 |
|------|------|--------|
| 큰 제목 | 24pt | Bold |
| 제목 | 20pt | SemiBold |
| 버튼 | 20pt | Regular |
| 본문 | 16pt | Regular |
| 값 표시 | 18pt | Bold |
| 단위 | 12pt | Regular |
| 캡션 | 12pt | Regular |

### 폰트 선택

```
우선순위:
1. Noto Sans (다국어 지원)
2. Segoe UI
3. Arial
4. sans-serif
```

## 레이아웃 가이드

### 그리드 시스템

```
기본 그리드: 8dp
터치 최소 크기: 44x44 dp (5.5 grid units)
간격: 8dp (1 grid unit)
여백: 16dp (2 grid units)
```

### 화면 구성

```
┌─────────────────────────────────────┐
│ Header (Title, Alarm Summary)       │ 60dp
├─────────────────────────────────────┤
│                                     │
│                                     │
│  Main Process View                  │
│  (P&ID or Control Panel)            │
│                                     │
│                                     │
├─────────────────────────────────────┤
│ Footer (Navigation, Status)         │ 60dp
└─────────────────────────────────────┘
```

## 체크리스트

ISA-101 테마 적용 시 확인사항:

### 색상
- [ ] 정상 상태는 회색 톤만 사용
- [ ] 이상 상태만 색상 사용
- [ ] 색상 의미가 일관적

### 레이아웃
- [ ] 터치 영역 최소 44x44 dp
- [ ] 간격 최소 8 dp
- [ ] 정보 밀도 적절 (20-30개)

### 타이포그래피
- [ ] 폰트 크기 최소 16pt
- [ ] 버튼 텍스트 최소 20pt
- [ ] 명확한 계층 구조

### 상태 표시
- [ ] 정상/이상 상태 명확히 구분
- [ ] 알람 시 깜빡임 효과
- [ ] 일관된 상태 색상

### 사용성
- [ ] 중요 정보 시각적 우선순위
- [ ] 빠른 상황 인지 가능
- [ ] 작업자 피로도 최소화

## 참고 자료

- ANSI/ISA-101: Human Machine Interfaces for Process Automation Systems
- ASM Consortium: "The High Performance HMI Handbook"
- "Effective HMI Design" by Bill Hollifield

## 구현 예시

실제 구현 예시는 다음 파일을 참조:
- `XamlDS.ITK.Aui/Themes/ISA101Theme.axaml`
- `XamlDS.ITK/Resources/ISA101Colors.cs`
- `XamlDS.Showcases.ITKThemes.Aui/Views/ISA101ThemeView.axaml`

