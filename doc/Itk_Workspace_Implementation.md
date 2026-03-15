# Itk Workspace 아키텍처 - 상세 구현 가이드

> 이미지 기반 실제 구현 설계

---

## 🎯 Workspace 유형 정의

### 1. UnitWorkspace (Tablet)

**특징**:
- 10~15인치 태블릿 크기
- 1인 사용 (개인 모니터링)
- 세로/가로 회전
- 간소화된 UI

**사용 시나리오**:
- 현장 점검 (Inspection)
- 모바일 모니터링
- 간단한 제어 작업
- 알람 확인

**Layout**:
```
┌────────────────┐
│   Status Bar   │
├────────────────┤
│                │
│  Main Content  │
│   (Compact)    │
│                │
├────────────────┤
│  Quick Actions │
└────────────────┘

세로: 10~12인치
가로: 12~15인치
```

### 2. MonoWorkspace (Single User)

**특징**:
- 22~48인치 대형 화면
- 1인 전용 독립 작업
- 세로 방향 고정
- 풀 기능 UI

**사용 시나리오**:
- 설계 작업 (Design)
- 운영 콘솔 (Operation)
- 모니터링 + 제어
- 데이터 분석

**Layout** (이미지 기반):
```
┌─────┬──────────────────────────────┬─────┐
│     │         Title Bar            │     │
│ Nav │──────────────────────────────│ Opt │
│     │                              │     │
│ Bar │      Main Content Area       │ Bar │
│     │      (Blue Panels)           │     │
│     │                              │     │
│ ■■  │  ┌────┬────┬──────┐          │ ■■  │
│ ■■  │  │    │    │      │          │ ■■  │
│ ■■  │  ├────┼────┤      │          │ ■■  │
│     │  │         │      │          │     │
│ ■   │  │         ├──────┤          │ ■   │
│ ■   │  │         │      │          │ ■   │
│ ■   │  └─────────┴──────┘          │ ■   │
│     │                              │     │
├─────┴──────────────────────────────┴─────┤
│          Application Menu                │
│  (Popup Grid - 하단 중앙 확장)            │
└───────────────────────────────────────────┘

세로: 1080~2160px
가로: 1920~3840px
비율: 16:9 또는 16:10
```

### 3. CoWorkspace (Multi User)

**특징**:
- 48인치 이상 초대형 화면
- 2~4인 동시 작업
- 360° 회전 가능
- 협업 특화

**사용 시나리오**:
- 협업 설계 (Collaborative Design)
- 공동 의사결정
- 팀 미팅
- 교육/트레이닝

**Layout** (이미지 기반):
```
        ┌──────────────────────────────┐
        │    User 3 Area (Brown)       │
        │    ■■ Nav  [Panels] Opt ■■   │
  ┌─────┤──────────────────────────────├─────┐
  │     │                              │     │
  │ U2  │     Shared Center Zone       │  U1 │
  │     │      (Green Circles)         │     │
  │Brown│                              │Blue │
  │     │  ○────○────○                 │     │
  │■■   │                              │  ■■ │
  │■■   │                  ○─────○     │  ■■ │
  │     │                              │     │
  └─────┤──────────────────────────────├─────┘
        │    User 4 Area (Purple)      │
        │    ■■ Nav  [Panels] Opt ■■   │
        └──────────────────────────────┘

크기: 48~100인치
비율: 정사각형 또는 16:9
회전: 360° (4 방향)
```

---

## 🎨 MonoWorkspace 상세 구현

### UI 구조 (이미지 분석)

#### 1. Navigation Bar (왼쪽)

**특징**:
- 고정 폭: ~80pt
- 세로 배치
- 3개 섹션

**구성**:
```csharp
public class NavigationBarVm : StackPanelVm
{
    public NavigationBarVm()
    {
        Width = 80;
        Orientation = Orientation.Vertical;
        Background = Colors.DarkGray;
        
        // Top Section (Primary Actions)
        Add(new NavigationSectionVm
        {
            Items = new[]
            {
                new NavItemVm("Home", icon: "⌂", color: Colors.White),
                new NavItemVm("Monitor", icon: "📊", color: Colors.White),
                new NavItemVm("Control", icon: "⚙️", color: Colors.White),
            }
        });
        
        // Spacer
        Add(new SpacerVm { FlexGrow = 1 });
        
        // Bottom Section (Settings)
        Add(new NavigationSectionVm
        {
            Items = new[]
            {
                new NavItemVm("Settings", icon: "⚙", color: Colors.Cyan),
                new NavItemVm("Help", icon: "?", color: Colors.Cyan),
            }
        });
    }
}

public class NavItemVm : ButtonVm
{
    public NavItemVm(string text, string icon, Color color)
    {
        Width = 60;
        Height = 60;
        Content = icon;
        ToolTip = text;
        Background = color;
        Margin = new Thickness(10, 5);
    }
}
```

#### 2. Main Content Area (중앙)

**특징**:
- Flexible 크기
- 복잡한 레이아웃 가능
- 파란색 패널들

**구성** (이미지의 레이아웃):
```csharp
public class MainContentVm : DockPanelVm
{
    public MainContentVm()
    {
        Background = Colors.Black;
        
        // Top Row (3 panels)
        var topRow = new StackPanelVm 
        { 
            Orientation = Orientation.Horizontal,
            Height = 200
        };
        topRow.Add(new PanelVm { Width = 250, Background = Colors.Navy });
        topRow.Add(new PanelVm { Width = 250, Background = Colors.Navy });
        topRow.Add(new PanelVm { Width = 400, Background = Colors.Navy });
        
        // Middle Row (2 panels)
        var middleRow = new StackPanelVm 
        { 
            Orientation = Orientation.Horizontal,
            Height = 300
        };
        middleRow.Add(new PanelVm { Width = 500, Background = Colors.Navy });
        middleRow.Add(new StackPanelVm 
        { 
            Orientation = Orientation.Vertical,
            Children = 
            {
                new PanelVm { Height = 150, Background = Colors.Navy },
                new PanelVm { Height = 150, Background = Colors.Navy }
            }
        });
        
        // Right Column
        var rightColumn = new PanelVm 
        { 
            Width = 250, 
            Background = Colors.Navy 
        };
        
        // Dock layout
        Add(topRow).SetDock(DockPositon.Top);
        Add(rightColumn).SetDock(DockPositon.Right);
        Add(middleRow).SetDock(DockPositon.Center);
    }
}
```

#### 3. Option Bar (오른쪽)

**특징**:
- 고정 폭: ~80pt
- 옵션 버튼들
- Application Menu 토글

**구성**:
```csharp
public class OptionBarVm : StackPanelVm
{
    public OptionBarVm()
    {
        Width = 80;
        Orientation = Orientation.Vertical;
        Background = Colors.DarkGray;
        
        // Top Section
        Add(new OptionSectionVm
        {
            Items = new[]
            {
                new OptionItemVm("Menu", "☰", ShowApplicationMenu),
                new OptionItemVm("Filter", "🔍"),
                new OptionItemVm("Sort", "↕️"),
            }
        });
        
        // Spacer
        Add(new SpacerVm { FlexGrow = 1 });
        
        // Bottom Section
        Add(new OptionSectionVm
        {
            Items = new[]
            {
                new OptionItemVm("User", "👤", color: Colors.Cyan),
            }
        });
    }
    
    private void ShowApplicationMenu()
    {
        // Open Application Menu Popup (하단 중앙)
        var popup = new ApplicationMenuPopupVm();
        ShowPopup(popup, PopupPosition.BottomCenter);
    }
}
```

#### 4. Application Menu Popup (하단)

**특징**:
- Grid 형태 (5x4 예시)
- 큰 터치 영역
- 하단 중앙에서 확장

**구성** (이미지 기반):
```csharp
public class ApplicationMenuPopupVm : GridPanelVm
{
    public ApplicationMenuPopupVm()
    {
        Rows = 4;
        Columns = 5;
        Width = 800;
        Height = 400;
        Background = Colors.DarkGray;
        
        // Row 0: Categories
        Add(new MenuCategoryVm("Recent", Colors.Cyan), 0, 0);
        Add(new MenuCategoryVm("All Apps", Colors.Gray), 0, 1, columnSpan: 3);
        Add(new MenuCategoryVm("Search", Colors.Gray), 0, 4);
        
        // Row 1-3: App Grid
        var apps = new[]
        {
            ("App 1", Colors.Navy),
            ("App 2", Colors.Navy),
            ("App 3", Colors.Navy),
            // ... more apps
            ("App 12", Colors.Green),
            ("App 13", Colors.Green),
        };
        
        int row = 1;
        int col = 0;
        foreach (var (name, color) in apps)
        {
            Add(new AppTileVm(name, color), row, col);
            
            col++;
            if (col >= 5)
            {
                col = 0;
                row++;
            }
        }
    }
}

public class AppTileVm : ButtonVm
{
    public AppTileVm(string name, Color color)
    {
        Width = 150;
        Height = 100;
        Content = name;
        Background = color;
        Margin = new Thickness(5);
        FontSize = 16;
    }
}
```

---

## 🎨 CoWorkspace 상세 구현

### Multi-User Layout (이미지 분석)

#### 1. 전체 구조

**4개 독립 영역**:
- User 1 (Blue) - 오른쪽
- User 2 (Brown) - 왼쪽
- User 3 (Brown) - 위쪽
- User 4 (Purple) - 아래쪽

**중앙 공유 영역**:
- 녹색 원형 (Shared Objects)
- 모든 사용자 접근 가능

```csharp
public class CoWorkspaceVm : CanvasVm
{
    public CoWorkspaceVm()
    {
        Width = 4096;  // 예: 48인치 @ 4K
        Height = 4096; // 정사각형
        Background = Colors.Black;
        
        // Central Shared Zone
        var sharedZone = new SharedZoneVm
        {
            X = 1548, // Center X
            Y = 1548, // Center Y
            Width = 1000,
            Height = 1000
        };
        Add(sharedZone);
        
        // User 1 (Blue - Right)
        var user1Zone = new UserZoneVm(1, Colors.Navy, UserPosition.Right)
        {
            X = 2600,
            Y = 1200,
            Width = 1400,
            Height = 1600
        };
        Add(user1Zone);
        
        // User 2 (Brown - Left)
        var user2Zone = new UserZoneVm(2, Colors.Brown, UserPosition.Left)
        {
            X = 96,
            Y = 1200,
            Width = 1400,
            Height = 1600
        };
        Add(user2Zone);
        
        // User 3 (Brown - Top)
        var user3Zone = new UserZoneVm(3, Colors.Brown, UserPosition.Top)
        {
            X = 1200,
            Y = 96,
            Width = 1600,
            Height = 1000
        };
        Add(user3Zone);
        
        // User 4 (Purple - Bottom)
        var user4Zone = new UserZoneVm(4, Colors.Purple, UserPosition.Bottom)
        {
            X = 1200,
            Y = 3000,
            Width = 1600,
            Height = 1000
        };
        Add(user4Zone);
    }
}
```

#### 2. User Zone 구현

**각 사용자 독립 영역**:
- Navigation bar
- Content panels
- Option bar

```csharp
public class UserZoneVm : DockPanelVm
{
    public UserZoneVm(int userId, Color color, UserPosition position)
    {
        UserId = userId;
        UserColor = color;
        Position = position;
        
        // Rotation based on position
        RotationAngle = position switch
        {
            UserPosition.Right => 0,
            UserPosition.Bottom => 90,
            UserPosition.Left => 180,
            UserPosition.Top => 270,
            _ => 0
        };
        
        // Navigation Bar (사용자 방향 기준 왼쪽)
        var navBar = new NavigationBarVm { Width = 80 };
        Add(navBar).SetDock(GetDockForPosition(position, Side.Left));
        
        // Option Bar (사용자 방향 기준 오른쪽)
        var optBar = new OptionBarVm { Width = 80 };
        Add(optBar).SetDock(GetDockForPosition(position, Side.Right));
        
        // Content Area
        var content = CreateUserContent(userId, color);
        Add(content).SetDock(DockPositon.Center);
    }
    
    private DockPositon GetDockForPosition(UserPosition pos, Side side)
    {
        // 회전에 따른 Dock 위치 계산
        return (pos, side) switch
        {
            (UserPosition.Right, Side.Left) => DockPositon.Left,
            (UserPosition.Bottom, Side.Left) => DockPositon.Top,
            (UserPosition.Left, Side.Left) => DockPositon.Right,
            (UserPosition.Top, Side.Left) => DockPositon.Bottom,
            // ... other combinations
            _ => DockPositon.Left
        };
    }
}

public enum UserPosition { Right, Bottom, Left, Top }
public enum Side { Left, Right, Top, Bottom }
```

#### 3. Shared Zone 구현

**중앙 공유 영역**:
- 모든 사용자 접근 가능
- 드래그 가능한 객체
- 충돌 감지

```csharp
public class SharedZoneVm : CanvasVm
{
    private CollaborationManager _collabManager;
    
    public SharedZoneVm()
    {
        Background = Colors.Transparent; // 배경 투명
        _collabManager = new CollaborationManager();
        
        // Add shared objects (green circles in image)
        Add(new SharedObjectVm 
        { 
            X = 200, 
            Y = 200, 
            Diameter = 300,
            Color = Colors.DarkGreen
        });
        
        Add(new SharedObjectVm 
        { 
            X = 600, 
            Y = 400, 
            Diameter = 400,
            Color = Colors.DarkGreen
        });
        
        // More shared objects...
    }
    
    public void OnObjectTouched(SharedObjectVm obj, int userId)
    {
        // Check if object is locked by another user
        if (_collabManager.IsLocked(obj) && 
            _collabManager.GetOwner(obj) != userId)
        {
            ShowConflictIndicator(obj, userId);
            return;
        }
        
        // Lock for this user
        _collabManager.Lock(obj, userId);
        HighlightObject(obj, GetUserColor(userId));
    }
}

public class SharedObjectVm : ContentPanelVm
{
    public double Diameter { get; set; }
    public bool IsLocked { get; set; }
    public int? LockedByUserId { get; set; }
    
    public SharedObjectVm()
    {
        // Circular shape
        BorderRadius = Diameter / 2;
        Width = Diameter;
        Height = Diameter;
    }
}
```

---

## 🎯 Touch Interaction 구현

### MonoWorkspace Touch Gestures

```csharp
public class MonoWorkspaceTouchHandler
{
    private TouchGestureManager _gestureManager;
    
    public MonoWorkspaceTouchHandler(MonoWorkspaceVm workspace)
    {
        _gestureManager = new TouchGestureManager();
        
        // Single tap: Select panel
        _gestureManager.OnTap += point => 
        {
            var panel = workspace.HitTest(point);
            if (panel != null)
            {
                workspace.SelectPanel(panel);
            }
        };
        
        // Long press: Show context menu
        _gestureManager.OnLongPress += point =>
        {
            var panel = workspace.HitTest(point);
            if (panel != null)
            {
                workspace.ShowContextMenu(panel, point);
            }
        };
        
        // Drag: Move panel
        _gestureManager.OnDrag += drag =>
        {
            var panel = workspace.SelectedPanel;
            if (panel != null && panel.IsMoveable)
            {
                panel.X += drag.End.X - drag.Start.X;
                panel.Y += drag.End.Y - drag.Start.Y;
            }
        };
        
        // Pinch: Resize panel
        _gestureManager.OnPinch += pinch =>
        {
            var panel = workspace.SelectedPanel;
            if (panel != null && panel.IsResizable)
            {
                panel.Width *= pinch.ScaleFactor;
                panel.Height *= pinch.ScaleFactor;
            }
        };
    }
}
```

### CoWorkspace Multi-User Touch

```csharp
public class CoWorkspaceTouchHandler
{
    private Dictionary<int, TouchGestureManager> _gestureManagers;
    private CollaborationManager _collabManager;
    
    public CoWorkspaceTouchHandler(CoWorkspaceVm workspace)
    {
        _gestureManagers = new Dictionary<int, TouchGestureManager>();
        _collabManager = new CollaborationManager();
        
        // Initialize gesture manager for each user
        for (int userId = 1; userId <= 4; userId++)
        {
            var manager = new TouchGestureManager();
            _gestureManagers[userId] = manager;
            
            // User-specific gesture handlers
            manager.OnTap += point => HandleUserTap(userId, point);
            manager.OnDrag += drag => HandleUserDrag(userId, drag);
        }
    }
    
    private void HandleUserTap(int userId, TouchPoint point)
    {
        // Determine which zone was tapped
        var zone = DetermineZone(point);
        
        if (zone == Zone.Shared)
        {
            // Shared zone - check conflicts
            var obj = HitTestSharedZone(point);
            if (obj != null)
            {
                if (_collabManager.TryLock(obj, userId))
                {
                    HighlightObject(obj, GetUserColor(userId));
                }
                else
                {
                    ShowConflictIndicator(obj, userId);
                }
            }
        }
        else if (zone == GetUserZone(userId))
        {
            // Own zone - normal interaction
            HandleNormalTap(userId, point);
        }
        // Ignore taps in other user zones
    }
}
```

---

## 📋 구현 체크리스트

### UnitWorkspace
- [ ] 세로/가로 회전 레이아웃
- [ ] 컴팩트 네비게이션
- [ ] 터치 최적화 (60pt minimum)
- [ ] 빠른 액션 버튼

### MonoWorkspace
- [ ] 3열 레이아웃 (Nav/Content/Option)
- [ ] Application Menu Popup
- [ ] 복잡한 패널 레이아웃
- [ ] 80pt touch targets
- [ ] Context menu

### CoWorkspace
- [ ] 4개 User Zone
- [ ] 360° rotation
- [ ] Shared Zone
- [ ] Multi-user collision detection
- [ ] User color indicators
- [ ] Component locking
- [ ] Gesture conflict resolution

---

## 🎨 스타일 가이드 (이미지 기반)

### 색상 팔레트

**MonoWorkspace**:
- Background: `#000000` (Black)
- Panels: `#1E3A5F` (Navy Blue)
- Nav/Option Bars: `#2A2A2A` (Dark Gray)
- Active Items: `#00BFFF` (Cyan)
- Text: `#FFFFFF` (White)

**CoWorkspace**:
- Background: `#000000` (Black)
- User 1 (Blue): `#1E3A5F` (Navy)
- User 2 (Brown): `#6B4423` (Brown)
- User 3 (Brown): `#6B4423` (Brown)
- User 4 (Purple): `#4B0082` (Purple)
- Shared Zone: `#2F4F2F` (Dark Green)
- Active: `#00BFFF` (Cyan)

### 간격 및 크기

**MonoWorkspace**:
- Navigation Bar: 80pt
- Option Bar: 80pt
- Nav Item: 60x60pt
- Nav Item Margin: 10pt horizontal, 5pt vertical
- Panel Gap: 10~20pt

**CoWorkspace**:
- User Zone minimum: 1400x1000pt
- Shared Zone: 1000x1000pt
- Navigation Bar: 80pt (per user)
- Shared Object: 300~400pt diameter

---

## 🚀 다음 단계

1. **UnitWorkspace Prototype** (1주)
   - 기본 레이아웃
   - 회전 지원
   - 터치 제스처

2. **MonoWorkspace Implementation** (2주)
   - Navigation Bar
   - Application Menu
   - Panel system
   - Touch optimization

3. **CoWorkspace MVP** (4주)
   - User Zone layout
   - Shared Zone
   - Multi-user detection
   - Basic collaboration

**이미지를 기반으로 실제 구현이 명확해졌습니다!** 🎉
