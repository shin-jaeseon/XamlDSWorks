# MockPane: Rapid Prototyping Tool

## 개요

ITK의 MockPane은 실제 컴포넌트를 구현하지 않고도 UI 레이아웃을 즉시 시각화할 수 있는 프로토타이핑 도구입니다.

## 핵심 가치

### 1. **빠른 프로토타이핑**
- 실제 구현 없이 레이아웃 구조 검증
- 30분 내 완성 가능한 프로토타입
- 디자인 회의 중 즉석 레이아웃 변경

### 2. **점진적 구현 (Progressive Implementation)**
```csharp
// Phase 1: 전체 Mock
AddTop(new MockPaneVm { Label = "Dashboard", Height = 80 });
AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });

// Phase 2: 우선순위 높은 부분만 구현
AddTop(new DashboardVm());  // ✅ 실제 구현
AddLeft(new MockPaneVm { Label = "Sidebar" });  // ⏳ 대기

// Phase 3: 점진적 완성
AddLeft(new SidebarVm());  // ✅ 완료
```

### 3. **고객 피드백 도구**
- 페이지 전환과 화면 구성만 구현
- 세부 기능은 MockPane으로 대체
- 실시간 레이아웃 조정 및 피드백 반영

## 주요 특징

### ✅ Code-First UI Prototyping

**기존 방식:**
1. Figma/Sketch 디자인
2. XAML 하드코딩
3. 변경 시 처음부터 다시
4. 디자인-개발 간극

**ITK 방식:**
1. ViewModel으로 레이아웃 정의
2. MockPane으로 즉시 시각화
3. 실시간 피드백 반영
4. 점진적 실제 구현 전환

## 빠른 시작

### 기본 사용법

```csharp
using XamlDS.Itk.ViewModels.Panels;
using XamlDS.Itk.ViewModels.Panes;

public class PrototypeVm : DockPanelVm
{
    public PrototypeVm()
    {
        // 헤더 영역
        AddTop(new MockPaneVm { Label = "Header", Height = 60 });
        
        // 사이드바
        AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });
        
        // 메인 콘텐츠 (LastChildFill)
        AddCenter(new MockPaneVm { Label = "Main Content" });
        
        // 푸터
        AddBottom(new MockPaneVm { Label = "Footer", Height = 80 });
    }
}
```

### 동적 레이아웃

```csharp
public class AppWindowVm : DockPanelVm
{
    private readonly MockPaneVm sidebar = new() 
    { 
        Label = "Sidebar", 
        Width = 256 
    };
    
    private bool _hasSidebar = false;

    public AppWindowVm()
    {
        AddTop(new MockPaneVm { Label = "Header", Height = 60 });
        AddLeft(sidebar);
        AddCenter(new MockPaneVm { Label = "Content" });
        
        _hasSidebar = true;
    }

    public void ToggleSidebar()
    {
        if (_hasSidebar)
        {
            Remove(sidebar);
            _hasSidebar = false;
        }
        else
        {
            AddLeft(sidebar);
            _hasSidebar = true;
        }
    }
}
```

## 실전 활용 시나리오

### Scenario 1: 레이아웃 A/B 테스트

```csharp
public class AppLayoutManager
{
    private DockPanelVm _mainLayout;
    
    public void ShowLayoutOption(string option)
    {
        _mainLayout.Clear();
        
        switch (option)
        {
            case "Classic":
                // 전통적인 좌측 사이드바
                _mainLayout.AddTop(new MockPaneVm { Label = "Toolbar", Height = 40 });
                _mainLayout.AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });
                _mainLayout.AddCenter(new MockPaneVm { Label = "Content" });
                break;
                
            case "Modern":
                // 상단 네비게이션
                _mainLayout.AddTop(new MockPaneVm { Label = "Navigation", Height = 60 });
                _mainLayout.AddCenter(new MockPaneVm { Label = "Content" });
                break;
                
            case "Minimal":
                // 최소 UI
                _mainLayout.AddCenter(new MockPaneVm { Label = "Full Screen Content" });
                break;
        }
    }
}
```

### Scenario 2: 페이지 플로우 데모

```csharp
public class WorkflowDemoVm : ViewModelBase
{
    private int _currentStep = 0;
    private DockPanelVm _layout;
    
    public WorkflowDemoVm(DockPanelVm layout)
    {
        _layout = layout;
        UpdateLayout();
    }
    
    public void NextStep()
    {
        _currentStep++;
        UpdateLayout();
    }
    
    public void PreviousStep()
    {
        if (_currentStep > 0)
        {
            _currentStep--;
            UpdateLayout();
        }
    }
    
    private void UpdateLayout()
    {
        _layout.Clear();
        
        switch (_currentStep)
        {
            case 0:
                // Step 1: 로그인 화면
                _layout.AddCenter(new MockPaneVm 
                { 
                    Label = "Login Screen", 
                    Width = 400, 
                    Height = 300 
                });
                break;
                
            case 1:
                // Step 2: 대시보드
                _layout.AddTop(new MockPaneVm { Label = "Header", Height = 60 });
                _layout.AddLeft(new MockPaneVm { Label = "Menu", Width = 200 });
                _layout.AddCenter(new MockPaneVm { Label = "Dashboard" });
                break;
                
            case 2:
                // Step 3: 상세 화면
                _layout.AddTop(new MockPaneVm { Label = "Header", Height = 60 });
                _layout.AddLeft(new MockPaneVm { Label = "Menu", Width = 200 });
                _layout.AddCenter(new MockPaneVm { Label = "Detail View" });
                _layout.AddRight(new MockPaneVm { Label = "Properties", Width = 300 });
                break;
        }
    }
}
```

### Scenario 3: 고객 데모용 레이아웃 스냅샷

```csharp
public class CustomerDemoVm : DockPanelVm
{
    public CustomerDemoVm()
    {
        // 비즈니스 애플리케이션 레이아웃
        CreateBusinessAppLayout();
    }
    
    private void CreateBusinessAppLayout()
    {
        // 상단 툴바
        AddTop(new MockPaneVm 
        { 
            Label = "Toolbar (File, Edit, View...)", 
            Height = 40 
        });
        
        // 왼쪽 탐색기
        AddLeft(new MockPaneVm 
        { 
            Label = "Project Explorer", 
            Width = 250 
        });
        
        // 하단 콘솔
        AddBottom(new MockPaneVm 
        { 
            Label = "Output Console", 
            Height = 150 
        });
        
        // 오른쪽 속성 패널
        AddRight(new MockPaneVm 
        { 
            Label = "Properties Panel", 
            Width = 300 
        });
        
        // 중앙 에디터 (LastChildFill)
        AddCenter(new MockPaneVm { Label = "Editor Area" });
    }
    
    public void ApplyMinimalLayout()
    {
        Clear();
        
        // 최소 레이아웃
        AddTop(new MockPaneVm { Label = "Toolbar", Height = 40 });
        AddCenter(new MockPaneVm { Label = "Editor Area" });
    }
}
```

## Mock에서 실제 구현으로 전환

### 단계별 전환 프로세스

```csharp
// Step 1: 전체 Mock으로 시작
public class AppWindowVm : DockPanelVm
{
    public AppWindowVm()
    {
        AddTop(new MockPaneVm { Label = "Toolbar", Height = 40 });
        AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });
        AddCenter(new MockPaneVm { Label = "Content" });
        AddBottom(new MockPaneVm { Label = "StatusBar", Height = 24 });
    }
}

// Step 2: 우선순위 컴포넌트 구현
public class AppWindowVm : DockPanelVm
{
    public AppWindowVm()
    {
        AddTop(new ToolbarVm());  // ✅ 실제 구현
        AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });
        AddCenter(new MockPaneVm { Label = "Content" });
        AddBottom(new MockPaneVm { Label = "StatusBar", Height = 24 });
    }
}

// Step 3: 점진적 완성
public class AppWindowVm : DockPanelVm
{
    public AppWindowVm()
    {
        AddTop(new ToolbarVm());      // ✅ 완료
        AddLeft(new SidebarVm());     // ✅ 완료
        AddCenter(new ContentVm());   // ✅ 완료
        AddBottom(new StatusBarVm()); // ✅ 완료
    }
}
```

### 동일한 인터페이스 유지

실제 구현으로 전환할 때 ViewModel 인터페이스를 유지하면 코드 변경이 최소화됩니다:

```csharp
// 공통 인터페이스
public interface ISidebarViewModel
{
    string Label { get; }
    double Width { get; set; }
}

// Mock 구현
public class MockSidebarVm : MockPaneVm, ISidebarViewModel
{
    public MockSidebarVm()
    {
        Label = "Sidebar";
        Width = 250;
    }
}

// 실제 구현
public class SidebarVm : ViewModelBase, ISidebarViewModel
{
    public string Label => "Sidebar";
    public double Width { get; set; } = 250;
    
    // 실제 기능 구현...
}

// 사용 (동일한 코드)
ISidebarViewModel sidebar = useMock 
    ? new MockSidebarVm() 
    : new SidebarVm();
    
AddLeft(sidebar as ViewModelBase);
```

## Best Practices

### 1. 명확한 라벨 사용
```csharp
// ❌ 나쁜 예
new MockPaneVm { Label = "Pane1" }

// ✅ 좋은 예
new MockPaneVm { Label = "User Profile Sidebar" }
```

### 2. 적절한 크기 지정
```csharp
// 고정 크기가 필요한 경우
AddTop(new MockPaneVm { Label = "Toolbar", Height = 40 });
AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });

// 자동 크기 (LastChildFill 영역)
AddCenter(new MockPaneVm { Label = "Content" });
```

### 3. 재사용 가능한 MockPane 정의
```csharp
public class CommonLayouts
{
    public static MockPaneVm StandardToolbar() => 
        new() { Label = "Toolbar", Height = 40 };
    
    public static MockPaneVm StandardSidebar() => 
        new() { Label = "Sidebar", Width = 250 };
    
    public static MockPaneVm StandardStatusBar() => 
        new() { Label = "Status Bar", Height = 24 };
}

// 사용
AddTop(CommonLayouts.StandardToolbar());
AddLeft(CommonLayouts.StandardSidebar());
```

## 고급 활용

### 조건부 레이아웃

```csharp
public class AdaptiveLayoutVm : DockPanelVm
{
    public void ApplyLayout(LayoutMode mode)
    {
        Clear();
        
        AddTop(new MockPaneVm { Label = "Header", Height = 60 });
        
        switch (mode)
        {
            case LayoutMode.Desktop:
                AddLeft(new MockPaneVm { Label = "Sidebar", Width = 250 });
                AddRight(new MockPaneVm { Label = "Tools", Width = 200 });
                break;
                
            case LayoutMode.Tablet:
                AddLeft(new MockPaneVm { Label = "Sidebar", Width = 200 });
                break;
                
            case LayoutMode.Mobile:
                // 사이드바 없음
                break;
        }
        
        AddCenter(new MockPaneVm { Label = "Content" });
    }
}

public enum LayoutMode
{
    Desktop,
    Tablet,
    Mobile
}
```

### 복잡한 중첩 레이아웃

```csharp
public class ComplexLayoutVm : DockPanelVm
{
    public ComplexLayoutVm()
    {
        // 상단 영역 (중첩 DockPanel)
        var topPanel = new DockPanelVm();
        topPanel.AddLeft(new MockPaneVm { Label = "Logo", Width = 200 });
        topPanel.AddCenter(new MockPaneVm { Label = "Navigation" });
        topPanel.AddRight(new MockPaneVm { Label = "User Menu", Width = 150 });
        AddTop(topPanel);
        
        // 왼쪽 사이드바 (중첩)
        var leftPanel = new DockPanelVm();
        leftPanel.AddTop(new MockPaneVm { Label = "Search", Height = 50 });
        leftPanel.AddCenter(new MockPaneVm { Label = "Tree View" });
        AddLeft(leftPanel);
        
        // 메인 콘텐츠
        AddCenter(new MockPaneVm { Label = "Main Content" });
    }
}
```

## 이점 요약

### 개발 팀
- ⚡ 빠른 레이아웃 프로토타이핑
- 🔄 점진적 구현 가능
- 🧪 레이아웃 로직 단위 테스트
- 📦 컴포넌트 독립적 개발

### 고객/이해관계자
- 👁️ 조기 시각적 피드백
- 🎯 명확한 레이아웃 구조 파악
- 💬 실시간 변경 요청 반영
- 📊 A/B 레이아웃 비교

### 프로젝트
- 📉 디자인-개발 간극 감소
- ⏱️ 반복 주기 단축
- 💰 초기 단계 비용 절감
- 🎨 더 많은 레이아웃 옵션 탐색

## 관련 문서

- [MockPane 컨트롤 상세 가이드](controls/MockPane.md)
- [DockPanelVm 사용법](panels/DockPanel.md)
- [ITK ViewModel-View 패턴](ITK-ViewModel-View-Pattern.md)

## 다음 단계

1. [MockPane 컨트롤](controls/MockPane.md)의 세부 기능 살펴보기
2. 샘플 프로젝트에서 실제 사용 예제 확인
3. 프로젝트에 적용하여 프로토타입 작성
4. 고객 피드백 수집 후 점진적 구현
