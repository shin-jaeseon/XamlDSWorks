# MockPane Control

## 개요

`MockPane`은 ITK의 Rapid Prototyping을 위한 핵심 컨트롤로, 실제 UI 컴포넌트를 구현하지 않고도 레이아웃을 시각화할 수 있게 해주는 플레이스홀더 컨트롤입니다.

## 구성 요소

### MockPaneVm (ViewModel)

**파일 위치**: `src/shared/XamlDS.Itk/ViewModels/Panes/MockPaneVm.cs`

```csharp
public class MockPaneVm : ViewModelBase
{
    private string _label = "Mock";
    private double _width = double.NaN;
    private double _height = double.NaN;

    /// <summary>
    /// Gets or sets the label displayed in the center of the mock pane.
    /// </summary>
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    /// <summary>
    /// Gets or sets the width of the mock pane.
    /// Use double.NaN for automatic sizing.
    /// </summary>
    public double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    /// <summary>
    /// Gets or sets the height of the mock pane.
    /// Use double.NaN for automatic sizing.
    /// </summary>
    public double Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }
}
```

### MockPaneView (View)

**파일 위치**: `src/shared/XamlDS.Itk.Aui/Views/Panes/MockPaneView.axaml`

```xml
<ControlTheme x:Key="{x:Type controls:MockPaneView}" TargetType="controls:MockPaneView">
    <Setter Property="Width" Value="{Binding Width}"/>
    <Setter Property="Height" Value="{Binding Height}"/>
    <Setter Property="Template">
        <ControlTemplate>
            <Grid>
                <Rectangle Fill="LightGray" Opacity="0.1" 
                          Stroke="#2FFF" StrokeThickness="4" Margin="1" />
                <itk:GridLinePane Opacity="0.1" Margin="1"/>
                <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
                    <TextBlock Text="{Binding Label}" FontSize="16" FontWeight="Bold"/>
                    <TextBlock Text="{Binding Width, StringFormat='Width: {0}'}" FontSize="10"/>
                    <TextBlock Text="{Binding Height, StringFormat='Height: {0}'}" FontSize="10"/>
                </StackPanel>
            </Grid>
        </ControlTemplate>
    </Setter>
</ControlTheme>
```

## 시각적 특징

### 레이어 구조

1. **배경 Rectangle**: 연한 회색 배경 (`LightGray`, Opacity 0.1)
2. **테두리**: 청록색 반투명 테두리 (`#2FFF`, 두께 4px)
3. **GridLinePane**: 격자 패턴 (Opacity 0.1)
4. **정보 표시**: 중앙에 Label, Width, Height 정보

### 디자인 의도

```
┌─────────────────────────────────┐
│ ░░░░░ (반투명 회색 배경) ░░░░░  │
│                                   │
│     ╔═════════════════════╗      │
│     ║   MockPane Label    ║      │
│     ║   Width: 250        ║      │
│     ║   Height: NaN       ║      │
│     ╚═════════════════════╝      │
│                                   │
│ ▓▓▓▓ (격자 패턴) ▓▓▓▓            │
└───────────────────────────────────┘
```

## 사용법

### 기본 사용

```csharp
using XamlDS.Itk.ViewModels.Panes;

// 자동 크기 (LastChildFill 영역에 적합)
var centerPane = new MockPaneVm { Label = "Main Content" };

// 고정 너비
var sidebar = new MockPaneVm 
{ 
    Label = "Sidebar", 
    Width = 250 
};

// 고정 높이
var toolbar = new MockPaneVm 
{ 
    Label = "Toolbar", 
    Height = 40 
};

// 고정 크기
var dialog = new MockPaneVm 
{ 
    Label = "Dialog", 
    Width = 400, 
    Height = 300 
};
```

### DockPanel과 함께 사용

```csharp
public class LayoutVm : DockPanelVm
{
    public LayoutVm()
    {
        // 상단: 고정 높이
        AddTop(new MockPaneVm 
        { 
            Label = "Header", 
            Height = 60 
        });
        
        // 왼쪽: 고정 너비
        AddLeft(new MockPaneVm 
        { 
            Label = "Sidebar", 
            Width = 250 
        });
        
        // 하단: 고정 높이
        AddBottom(new MockPaneVm 
        { 
            Label = "Footer", 
            Height = 80 
        });
        
        // 중앙: 자동 크기 (남은 공간 채움)
        AddCenter(new MockPaneVm 
        { 
            Label = "Content" 
        });
    }
}
```

## 속성 상세

### Label

**타입**: `string`  
**기본값**: `"Mock"`  
**용도**: 패널의 역할이나 이름을 표시

```csharp
// 명확한 라벨 사용 권장
new MockPaneVm { Label = "User Profile Panel" }
new MockPaneVm { Label = "Product List" }
new MockPaneVm { Label = "Shopping Cart" }
```

**Best Practice**:
- ✅ 구체적이고 설명적인 이름 사용
- ✅ 실제 컴포넌트 이름과 유사하게 작성
- ❌ "Pane1", "Test", "Temp" 같은 모호한 이름 지양

### Width

**타입**: `double`  
**기본값**: `double.NaN` (자동 크기)  
**용도**: 패널의 너비 설정

```csharp
// 자동 크기 (기본값)
new MockPaneVm { Width = double.NaN }

// 고정 너비
new MockPaneVm { Width = 250 }
new MockPaneVm { Width = 300 }

// DockPanel의 LastChildFill 영역에는 NaN 권장
AddCenter(new MockPaneVm { Label = "Content" });  // Width는 NaN
```

**사용 시나리오**:
- `NaN`: LastChildFill 영역, 자동 조정 필요 영역
- 고정값: Sidebar, Toolbar, 고정 크기 패널

### Height

**타입**: `double`  
**기본값**: `double.NaN` (자동 크기)  
**용도**: 패널의 높이 설정

```csharp
// 자동 크기 (기본값)
new MockPaneVm { Height = double.NaN }

// 고정 높이
new MockPaneVm { Height = 60 }   // Header
new MockPaneVm { Height = 40 }   // Toolbar
new MockPaneVm { Height = 24 }   // Status Bar

// DockPanel의 LastChildFill 영역에는 NaN 권장
AddCenter(new MockPaneVm { Label = "Content" });  // Height는 NaN
```

## 고급 사용 패턴

### 1. 재사용 가능한 Mock 정의

```csharp
public static class MockPanePresets
{
    public static MockPaneVm Header(string label = "Header") => 
        new() { Label = label, Height = 60 };
    
    public static MockPaneVm Toolbar(string label = "Toolbar") => 
        new() { Label = label, Height = 40 };
    
    public static MockPaneVm Sidebar(string label = "Sidebar") => 
        new() { Label = label, Width = 250 };
    
    public static MockPaneVm WideSidebar(string label = "Wide Sidebar") => 
        new() { Label = label, Width = 350 };
    
    public static MockPaneVm StatusBar(string label = "Status Bar") => 
        new() { Label = label, Height = 24 };
    
    public static MockPaneVm Dialog(string label = "Dialog") => 
        new() { Label = label, Width = 400, Height = 300 };
    
    public static MockPaneVm Content(string label = "Content") => 
        new() { Label = label };
}

// 사용
public class AppWindowVm : DockPanelVm
{
    public AppWindowVm()
    {
        AddTop(MockPanePresets.Header());
        AddLeft(MockPanePresets.Sidebar());
        AddCenter(MockPanePresets.Content());
        AddBottom(MockPanePresets.StatusBar());
    }
}
```

### 2. 동적 크기 조정

```csharp
public class ResizableMockVm : MockPaneVm
{
    public ResizableMockVm()
    {
        Label = "Resizable Panel";
        Width = 250;
    }
    
    public void IncreaseWidth()
    {
        Width += 50;
    }
    
    public void DecreaseWidth()
    {
        if (Width > 100)
            Width -= 50;
    }
    
    public void ResetWidth()
    {
        Width = 250;
    }
}
```

### 3. 조건부 표시

```csharp
public class ConditionalLayoutVm : DockPanelVm
{
    private MockPaneVm? _sidebar;
    private bool _showSidebar = true;
    
    public ConditionalLayoutVm()
    {
        AddTop(new MockPaneVm { Label = "Header", Height = 60 });
        
        _sidebar = new MockPaneVm { Label = "Sidebar", Width = 250 };
        AddLeft(_sidebar);
        
        AddCenter(new MockPaneVm { Label = "Content" });
    }
    
    public void ToggleSidebar()
    {
        if (_showSidebar && _sidebar != null)
        {
            Remove(_sidebar);
        }
        else if (_sidebar != null)
        {
            AddLeft(_sidebar);
        }
        
        _showSidebar = !_showSidebar;
    }
}
```

## 실제 구현으로 전환

### 단계별 전환 가이드

#### Step 1: Mock으로 시작

```csharp
public class UserProfileVm : DockPanelVm
{
    public UserProfileVm()
    {
        AddTop(new MockPaneVm { Label = "Profile Header", Height = 100 });
        AddLeft(new MockPaneVm { Label = "Avatar", Width = 150 });
        AddCenter(new MockPaneVm { Label = "User Info" });
    }
}
```

#### Step 2: 부분 구현

```csharp
public class UserProfileVm : DockPanelVm
{
    public UserProfileVm()
    {
        // ✅ 실제 구현
        AddTop(new ProfileHeaderVm 
        { 
            UserName = "John Doe",
            Email = "john@example.com"
        });
        
        // ⏳ 아직 Mock
        AddLeft(new MockPaneVm { Label = "Avatar", Width = 150 });
        AddCenter(new MockPaneVm { Label = "User Info" });
    }
}
```

#### Step 3: 완전 구현

```csharp
public class UserProfileVm : DockPanelVm
{
    public UserProfileVm(User user)
    {
        // ✅ 모두 실제 구현
        AddTop(new ProfileHeaderVm(user));
        AddLeft(new AvatarVm(user.AvatarUrl));
        AddCenter(new UserInfoVm(user));
    }
}
```

## 디버깅 및 문제 해결

### 크기가 표시되지 않는 경우

```csharp
// ❌ 문제: 크기가 0으로 표시
var pane = new MockPaneVm { Label = "Test" };
// Width = NaN, Height = NaN

// ✅ 해결: 명시적 크기 설정 또는 LastChildFill 영역에 배치
AddLeft(new MockPaneVm { Label = "Test", Width = 250 });
// 또는
AddCenter(new MockPaneVm { Label = "Test" });  // LastChildFill
```

### GridLinePane이 보이지 않는 경우

GridLinePane이 프로젝트에 없는 경우 제거하고 다른 배경 패턴 사용:

```xml
<Grid>
    <Rectangle Fill="LightGray" Opacity="0.1" 
              Stroke="#2FFF" StrokeThickness="4" Margin="1" />
    <!-- GridLinePane 대신 -->
    <Border Background="#10000000" BorderBrush="#20000000" BorderThickness="1">
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="*"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
        </Grid>
    </Border>
    <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
        <TextBlock Text="{Binding Label}" FontSize="16" FontWeight="Bold"/>
        <TextBlock Text="{Binding Width, StringFormat='Width: {0}'}" FontSize="10"/>
        <TextBlock Text="{Binding Height, StringFormat='Height: {0}'}" FontSize="10"/>
    </StackPanel>
</Grid>
```

## 확장 가능성

### 커스텀 MockPane 만들기

```csharp
public class ColoredMockPaneVm : MockPaneVm
{
    private string _backgroundColor = "LightGray";
    
    public string BackgroundColor
    {
        get => _backgroundColor;
        set => SetProperty(ref _backgroundColor, value);
    }
}
```

```xml
<!-- ColoredMockPaneView.axaml -->
<ControlTheme x:Key="{x:Type controls:ColoredMockPaneView}" 
              TargetType="controls:ColoredMockPaneView">
    <Setter Property="Template">
        <ControlTemplate>
            <Grid>
                <Rectangle Fill="{Binding BackgroundColor}" Opacity="0.1" 
                          Stroke="#2FFF" StrokeThickness="4" Margin="1" />
                <!-- 나머지 내용 동일 -->
            </Grid>
        </ControlTemplate>
    </Setter>
</ControlTheme>
```

### 인터랙티브 Mock 만들기

```csharp
public class InteractiveMockPaneVm : MockPaneVm
{
    private ICommand? _clickCommand;
    
    public ICommand? ClickCommand
    {
        get => _clickCommand;
        set => SetProperty(ref _clickCommand, value);
    }
    
    public void SimulateClick()
    {
        ClickCommand?.Execute(null);
    }
}
```

## 관련 문서

- [MockPane Rapid Prototyping 가이드](../MockPane-Rapid-Prototyping.md)
- [DockPanelVm 사용법](../panels/DockPanel.md)
- [ITK ViewModel-View 패턴](../ITK-ViewModel-View-Pattern.md)

## 버전 이력

- **v1.0**: 초기 릴리스
  - 기본 MockPane 기능
  - Label, Width, Height 속성
  - GridLinePane 통합

## 기여 및 피드백

MockPane에 대한 개선 아이디어나 피드백은 GitHub Issues를 통해 제출해 주세요.
