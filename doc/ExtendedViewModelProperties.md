# Extended ViewModel Properties

ViewModel 계층에서 Attached Property와 유사한 동작을 제공하는 ExtendedProperty 시스템입니다.

## 개념

### SelfOwnedProperty (Context-Independent)
ViewModel 자체가 소유하는 속성으로, 어디서 사용되든 동일한 값을 유지합니다.

**예시**: ThemeAccentColor, CustomStyle, Metadata

### ParentOwnedProperty (Context-Dependent)  
부모 ViewModel이 자식에 대해 소유하는 속성으로, 동일한 자식 ViewModel이 다른 부모에서 다른 값을 가질 수 있습니다.

**예시**: Dock, GridRow, GridColumn, LayoutInfo

## 사용 예시

### 1. ThemeAccentColor (Self-Owned)

```csharp
using XamlDS.Itk.ViewModels.ExtendedProperties;

var gaugeVm = new GaugeVm();

// Set theme color
ThemeAccentColorProperty.Set(gaugeVm, ThemeAccentColor.Orange);

// Get theme color
var color = ThemeAccentColorProperty.Get(gaugeVm); // Orange

// Try get
if (ThemeAccentColorProperty.TryGet(gaugeVm, out var themeColor))
{
    Console.WriteLine($"Theme: {themeColor}");
}
```

### 2. Dock Position (Parent-Owned)

```csharp
using XamlDS.Itk.ViewModels.ExtendedProperties;

var panelVm = new PanelVm<ViewModelBase>();
var child1 = new GaugeVm();
var child2 = new FieldVm();

panelVm.Children.Add(child1);
panelVm.Children.Add(child2);

// Set dock positions (context: panelVm)
DockProperty.Set(panelVm, child1, Dock.Left);
DockProperty.Set(panelVm, child2, Dock.Right);

// Get dock positions
var dock1 = DockProperty.Get(panelVm, child1); // Left
var dock2 = DockProperty.Get(panelVm, child2); // Right

// 동일한 child1을 다른 부모에서 다르게 사용 가능
var anotherPanel = new PanelVm<ViewModelBase>();
anotherPanel.Children.Add(child1);
DockProperty.Set(anotherPanel, child1, Dock.Top); // 다른 Dock!

// panelVm에서는 여전히 Left
var stillLeft = DockProperty.Get(panelVm, child1); // Left
// anotherPanel에서는 Top
var nowTop = DockProperty.Get(anotherPanel, child1); // Top
```

### 3. Grid Layout (Parent-Owned)

```csharp
var gridPanel = new PanelVm<ViewModelBase>
{
    Layout = PanelLayout.Grid(3, 2) // 3 columns, 2 rows
};

var gauge1 = new GaugeVm();
var gauge2 = new GaugeVm();
var gauge3 = new GaugeVm();

gridPanel.Children.Add(gauge1);
gridPanel.Children.Add(gauge2);
gridPanel.Children.Add(gauge3);

// Set grid positions
GridRowProperty.Set(gridPanel, gauge1, 0);
GridColumnProperty.Set(gridPanel, gauge1, 0);

GridRowProperty.Set(gridPanel, gauge2, 0);
GridColumnProperty.Set(gridPanel, gauge2, 1);

GridRowProperty.Set(gridPanel, gauge3, 1);
GridColumnProperty.Set(gridPanel, gauge3, 0);

// Get grid positions
var row = GridRowProperty.Get(gridPanel, gauge1); // 0
var col = GridColumnProperty.Get(gridPanel, gauge1); // 0
```

## 자동 Disposal

ExtendedProperty는 ViewModel의 Dispose 시 자동으로 정리됩니다:

```csharp
var parentVm = new PanelVm<ViewModelBase>();
var childVm = new GaugeVm();

DockProperty.Set(parentVm, childVm, Dock.Left);
ThemeAccentColorProperty.Set(childVm, ThemeAccentColor.Blue);

// Dispose 시 자동 정리
parentVm.Dispose(); // DockProperty 자동 제거
childVm.Dispose();  // ThemeAccentColorProperty 자동 제거
```

## 커스텀 ExtendedProperty 만들기

### Self-Owned Property 예시

```csharp
public class CustomStyleProperty : SelfOwnedProperty<string>
{
    public static readonly CustomStyleProperty Instance = new();
    public override string PropertyName => "CustomStyle";

    public static void Set(ViewModelBase target, string style)
        => Instance.SetValue(target, style);

    public static string? Get(ViewModelBase target)
        => Instance.GetValue(target);
}
```

### Parent-Owned Property 예시

```csharp
public class ZIndexProperty : ParentOwnedProperty<int>
{
    public static readonly ZIndexProperty Instance = new();
    public override string PropertyName => "ZIndex";

    public static void Set(ViewModelBase parent, ViewModelBase child, int zIndex)
        => Instance.SetValue(parent, child, zIndex);

    public static int Get(ViewModelBase parent, ViewModelBase child)
        => Instance.GetValue(parent, child) ?? 0;
}
```

## 주의사항

1. **ConditionalWeakTable 사용**: 메모리 누수 방지를 위해 약한 참조 사용
2. **타입 제약**: 
   - `SelfOwnedProperty<T>`: T는 class (참조 타입)
   - `ParentOwnedProperty<T>`: T는 struct (값 타입)
   - Enum은 wrapper 클래스 필요 (ThemeAccentColorValue 참고)
3. **Thread Safety**: ConditionalWeakTable은 thread-safe
4. **Performance**: 약간의 오버헤드 있음 (Dictionary lookup)
