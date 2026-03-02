# Avalonia Property System: CLR Setter vs PropertyChanged

## 개요

Avalonia와 WPF의 **속성 시스템 동작 방식에는 중요한 차이**가 있습니다. 특히 **CLR Property Setter가 호출되는 시점**이 다릅니다.

## 핵심 차이점 요약

| 변경 경로 | WPF | Avalonia |
|-----------|-----|----------|
| **코드에서 직접 설정** | CLR Setter 호출 ✅ | CLR Setter 호출 ✅ |
| **바인딩에서 설정** | 경우에 따라 다름 ⚠️ | CLR Setter 호출 **안 됨** ❌ |
| **스타일에서 설정** | CLR Setter 호출 안 됨 ❌ | CLR Setter 호출 **안 됨** ❌ |
| **애니메이션** | 경우에 따라 다름 ⚠️ | CLR Setter 호출 **안 됨** ❌ |
| **권장 패턴** | Setter 또는 PropertyChanged | **PropertyChanged만** ✅ |
| **일관성** | ⚠️ 낮음 | ✅ **높음** |

## WPF의 동작

### 특징
- **일관성 없음**: 바인딩 시나리오에 따라 동작이 다름
- **예측 불가능**: 언제 Setter가 호출될지 보장 안 됨
- **내부 최적화**: 성능을 위해 경로를 다르게 처리

### 코드 예시

```csharp
// WPF
public int MyProperty
{
    get => (int)GetValue(MyPropertyProperty);
    set
    {
        Console.WriteLine("Setter called!");  // ⚠️ 때때로 호출됨
        SetValue(MyPropertyProperty, value);
    }
}
```

**동작:**
- 코드에서 직접 설정: `obj.MyProperty = 10` → **Setter 호출 ✅**
- 바인딩에서 설정: **경우에 따라 다름** (내부 최적화)
- 스타일에서 설정: **Setter 호출 안 됨 ❌**

## Avalonia의 동작

### 특징
- **일관성 있음**: 항상 동일하게 동작
- **예측 가능**: 명확한 규칙
- **성능 최적화**: 불필요한 오버헤드 제거

### 코드 예시

```csharp
// Avalonia
public int MyProperty
{
    get => GetValue(MyPropertyProperty);
    set
    {
        Console.WriteLine("Setter called!");  // ❌ 바인딩 시 호출 안 됨
        SetValue(MyPropertyProperty, value);
    }
}
```

**동작:**
- 코드에서 직접 설정: `obj.MyProperty = 10` → **Setter 호출 ✅**
- 바인딩에서 설정: **Setter 호출 안 됨 ❌**
- 스타일에서 설정: **Setter 호출 안 됨 ❌**
- 애니메이션: **Setter 호출 안 됨 ❌**

## 왜 Avalonia는 Setter를 우회하는가?

### 1. 성능 최적화

```csharp
// Avalonia 내부 동작 (의사 코드)
public void SetValueFromBinding<T>(AvaloniaProperty property, T value)
{
    // CLR Setter를 거치지 않고 직접 설정
    _propertyValues[property] = value;  // 빠름!
    
    // 이벤트만 발생
    RaisePropertyChanged(property, oldValue, value);
}
```

**이유:**
- CLR Setter 호출은 오버헤드
- 리플렉션 비용 제거
- 바인딩 업데이트가 빈번한 경우 성능 향상

### 2. 일관성 보장

```csharp
// Avalonia: 모든 경로가 동일하게 동작
SetValue(MyPropertyProperty, 10);           // Path 1
this.Bind(MyPropertyProperty, binding);     // Path 2
Styles.SetValue(MyPropertyProperty, 10);    // Path 3

// 모두 동일하게 PropertyChanged 이벤트만 발생
// CLR Setter는 호출 안 됨
```

### 3. 예측 가능성

**WPF의 문제:**
```csharp
// WPF: 예측 불가능한 동작
public int MyProperty
{
    get => (int)GetValue(MyPropertyProperty);
    set
    {
        // ⚠️ 이 로직이 항상 실행될까? 보장 안 됨!
        if (value < 0) value = 0;
        SetValue(MyPropertyProperty, value);
    }
}
```

**Avalonia의 해결:**
```csharp
// Avalonia: PropertyChanged 핸들러 사용 (항상 실행됨!)
static MyControl()
{
    MyPropertyProperty.Changed.AddClassHandler<MyControl>((x, e) =>
    {
        // ✅ 이 로직은 항상 실행됨 (모든 경로에서)
        var value = (int)e.NewValue!;
        if (value < 0)
        {
            x.SetValue(MyPropertyProperty, 0);
        }
    });
}
```

## 실제 동작 비교

### 테스트 코드

```csharp
public class TestControl : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<TestControl, double>(nameof(Value));

    public double Value
    {
        get => GetValue(ValueProperty);
        set
        {
            Console.WriteLine($"Setter called: {value}");
            SetValue(ValueProperty, value);
        }
    }

    static TestControl()
    {
        ValueProperty.Changed.AddClassHandler<TestControl>((x, e) =>
        {
            Console.WriteLine($"PropertyChanged: {e.NewValue}");
        });
    }
}
```

### 시나리오별 출력

#### 1. 코드에서 직접 설정

```csharp
control.Value = 50;
```

**출력:**
```
Setter called: 50
PropertyChanged: 50
```

#### 2. 바인딩에서 설정

```xml
<TestControl Value="{Binding Temperature}" />
```
```csharp
viewModel.Temperature = 50;  // 바인딩 업데이트
```

**출력:**
```
PropertyChanged: 50
(Setter는 호출 안 됨!)
```

#### 3. 스타일에서 설정

```xml
<Style Selector="TestControl">
  <Setter Property="Value" Value="50" />
</Style>
```

**출력:**
```
PropertyChanged: 50
(Setter는 호출 안 됨!)
```

## Avalonia 내부 구조

```csharp
// Avalonia 소스 코드 (간소화)
public class AvaloniaObject
{
    private readonly Dictionary<AvaloniaProperty, object> _values = new();

    public void SetValue<T>(StyledProperty<T> property, T value)
    {
        var oldValue = GetValue(property);
        
        // 값 직접 저장 (CLR Setter 우회)
        _values[property] = value;
        
        // 이벤트 발생
        RaisePropertyChanged(property, oldValue, value);
    }

    // 바인딩 시스템이 호출하는 메서드
    internal void SetValueFromBinding<T>(AvaloniaProperty property, T value)
    {
        // 직접 SetValue 호출 (CLR Setter 우회)
        SetValue((StyledProperty<T>)property, value);
    }
}
```

## WPF vs Avalonia 철학

### WPF (하이브리드 접근)

```
CLR Property ←→ DependencyProperty
      ↑
  때때로 연결됨 (일관성 없음)
```

### Avalonia (명확한 분리)

```
CLR Property (편의용 래퍼만)
      ↓
StyledProperty (실제 저장소)
      ↓
PropertyChanged (모든 변경 감지) ✅
```

## 실전 예시: LinearGauge

### ❌ 잘못된 방법 (WPF 스타일)

```csharp
public class LinearGauge : TemplatedControl
{
    public double CurrentValue
    {
        get => GetValue(CurrentValueProperty);
        set
        {
            // ❌ 바인딩 시 호출 안 됨!
            var oldIndicatorWidth = ValueIndicatorWidth;
            SetValue(CurrentValueProperty, value);
            RaisePropertyChanged(ValueIndicatorWidthProperty, oldIndicatorWidth, ValueIndicatorWidth);
        }
    }
}
```

**문제:**
```xml
<!-- 이 바인딩은 Setter를 거치지 않음 -->
<controls:LinearGauge CurrentValue="{Binding Temperature.Value}" />
```

**결과:**
- `Temperature.Value` 변경 시
- `SetValue` 직접 호출됨
- CLR Setter 우회됨
- `ValueIndicatorWidth` 업데이트 **안 됨** ❌

### ✅ 올바른 방법 (Avalonia 스타일)

```csharp
public class LinearGauge : TemplatedControl
{
    public double CurrentValue
    {
        get => GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);  // 단순 래퍼
    }

    static LinearGauge()
    {
        // ✅ 모든 변경을 포착
        CurrentValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
        {
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, 0, gauge.ValueIndicatorWidth);
        });
        
        MinValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
        {
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, 0, gauge.ValueIndicatorWidth);
        });
        
        MaxValueProperty.Changed.AddClassHandler<LinearGauge>((gauge, e) =>
        {
            gauge.RaisePropertyChanged(ValueIndicatorWidthProperty, 0, gauge.ValueIndicatorWidth);
        });
    }
}
```

**작동:**
```xml
<!-- PropertyChanged 핸들러가 항상 호출됨 ✅ -->
<controls:LinearGauge CurrentValue="{Binding Temperature.Value}" />
```

**결과:**
- `Temperature.Value` 변경 시
- `CurrentValueProperty.Changed` 이벤트 발생
- `AddClassHandler`가 포착
- `ValueIndicatorWidth` 업데이트 **됨** ✅

## 공식 문서 인용

### WPF 문서

> "DependencyProperty의 CLR wrapper는 편의를 위한 것이며, 바인딩 시스템이 항상 이를 사용한다고 보장할 수 없습니다."

### Avalonia 문서

> "StyledProperty의 CLR wrapper는 코드에서의 사용 편의를 위한 것입니다. 바인딩, 스타일, 애니메이션은 항상 SetValue를 직접 호출합니다. **속성 변경 로직은 PropertyChanged 이벤트를 사용하세요.**"

## 모범 사례

### ✅ 권장 패턴

```csharp
public class MyControl : TemplatedControl
{
    // 1. 속성 등록
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<MyControl, double>(nameof(Value), 0.0);

    // 2. CLR 래퍼 (단순하게)
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    // 3. 정적 생성자에서 핸들러 등록
    static MyControl()
    {
        ValueProperty.Changed.AddClassHandler<MyControl>((control, e) =>
        {
            // ✅ 모든 변경을 포착하는 로직
            control.OnValueChanged(e);
        });
    }

    // 4. 변경 처리 메서드
    private void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = (double)e.NewValue!;
        
        // 검증
        if (newValue < 0)
        {
            SetValue(ValueProperty, 0);
            return;
        }

        // 의존 속성 업데이트
        RaisePropertyChanged(DependentPropertyProperty, null, CalculateDependentValue());
    }
}
```

### ❌ 비권장 패턴

```csharp
public class MyControl : TemplatedControl
{
    // ❌ Setter에 로직 작성
    public double Value
    {
        get => GetValue(ValueProperty);
        set
        {
            // ❌ 바인딩 시 호출 안 됨!
            if (value < 0) value = 0;
            SetValue(ValueProperty, value);
        }
    }
}
```

## WPF에서 Avalonia 마이그레이션 가이드

### WPF 코드

```csharp
// WPF
public class WpfControl : Control
{
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(WpfControl));

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set
        {
            // WPF: Setter에 로직
            if (value < Min) value = Min;
            if (value > Max) value = Max;
            SetValue(ValueProperty, value);
        }
    }
}
```

### Avalonia 변환

```csharp
// Avalonia
public class AvaloniaControl : Control
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<AvaloniaControl, double>(nameof(Value), 0.0);

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);  // 단순 래퍼
    }

    static AvaloniaControl()
    {
        // ✅ 로직을 PropertyChanged로 이동
        ValueProperty.Changed.AddClassHandler<AvaloniaControl>((control, e) =>
        {
            var value = (double)e.NewValue!;
            
            // 검증 로직
            if (value < control.Min)
            {
                control.SetValue(ValueProperty, control.Min);
            }
            else if (value > control.Max)
            {
                control.SetValue(ValueProperty, control.Max);
            }
        });
    }
}
```

## 성능 비교

### CLR Setter 호출 (WPF 방식)

```
바인딩 → 리플렉션 → CLR Setter → SetValue → 이벤트
         ^^^^^^^^   ^^^^^^^^^^
         느림       오버헤드
```

### 직접 SetValue (Avalonia 방식)

```
바인딩 → SetValue → 이벤트
         빠름!
```

**성능 향상:**
- 리플렉션 제거: ~30-50% 빠름
- 메서드 호출 오버헤드 제거
- 바인딩 업데이트가 빈번한 경우 큰 차이

## 디버깅 팁

### Setter가 호출되지 않는 문제 진단

```csharp
public double Value
{
    get => GetValue(ValueProperty);
    set
    {
        Debug.WriteLine("Setter called!");  // 여기가 호출 안 되면?
        SetValue(ValueProperty, value);
    }
}

static MyControl()
{
    ValueProperty.Changed.AddClassHandler<MyControl>((x, e) =>
    {
        Debug.WriteLine($"PropertyChanged: {e.NewValue}");  // 여기는 항상 호출됨
    });
}
```

**진단 방법:**
1. Setter에 브레이크포인트 → 안 걸리면 바인딩 문제
2. PropertyChanged에 브레이크포인트 → 항상 걸림
3. **해결**: Setter 로직을 PropertyChanged로 이동

## 요약

### Avalonia의 명확한 규칙

1. ✅ **CLR Setter**: 코드에서만 호출 (편의 기능)
2. ✅ **PropertyChanged**: 모든 변경을 포착 (신뢰할 수 있는 방법)
3. ✅ **성능**: 불필요한 오버헤드 제거
4. ✅ **예측 가능성**: 항상 동일한 동작
5. ✅ **일관성**: 모든 경로가 동일하게 처리

### 체크리스트

- [ ] CLR Setter는 단순 래퍼로만 사용
- [ ] 모든 로직을 PropertyChanged 핸들러로 이동
- [ ] 정적 생성자에서 AddClassHandler 등록
- [ ] 바인딩 테스트 시 PropertyChanged 확인
- [ ] WPF에서 이식 시 Setter 로직 점검

## 참고 자료

- [Avalonia Documentation - Properties](https://docs.avaloniaui.net/docs/guides/basics/mvvm#properties)
- [Avalonia GitHub - AvaloniaObject Source](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Base/AvaloniaObject.cs)
- [WPF vs Avalonia Differences](https://docs.avaloniaui.net/docs/guides/platforms/wpf-differences)

---

**핵심 메시지**: Avalonia에서는 **PropertyChanged 패턴만 신뢰**하세요. CLR Setter는 편의용 래퍼일 뿐입니다! 🎯
