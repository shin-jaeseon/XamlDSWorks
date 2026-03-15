# ViewPresenter 문서

> ViewLocator + ViewPresenter: WPF와 Avalonia를 위한 완벽한 MVVM ViewModel-to-View 바인딩 솔루션

## 📚 문서

- **[ViewLocator_ViewPresenter_Guide.md](ViewLocator_ViewPresenter_Guide.md)** - 완전 가이드 (전체 문서)

## 🎯 빠른 참조

### 등록

```csharp
// ItkWpfLibrary.cs 또는 ItkAuiLibrary.cs
protected override void RegisterView()
{
    ViewLocator.Register<DockPanelVm, DockPanelView>();
    ViewLocator.Register<StackPanelVm, StackPanelView>();
}
```

### XAML 사용

```xml
<!-- 기본 -->
<views:ViewPresenter />

<!-- 명시적 바인딩 -->
<views:ViewPresenter DataContext="{Binding CurrentPage}" />
```

### Factory 함수

```csharp
ViewLocator.RegisterFunc<DeviceVm>(vm => 
    vm.IsDetailed ? new DetailView() : new BriefView()
);
```

## 🎨 핵심 개념

| 컴포넌트 | 역할 | 파일 위치 |
|----------|------|-----------|
| **ViewLocator** | ViewModel → View 찾기 | `XamlDS.Itk.Wpf/ViewLocator.cs`<br>`XamlDS.Itk.Aui/ViewLocator.cs` |
| **ViewPresenter** | View 렌더링 | `XamlDS.Itk.Wpf/Views/ViewPresenter.cs`<br>`XamlDS.Itk.Aui/Views/ViewPresenter.cs` |

## ✨ 주요 특징

- ✅ 성능: ~0.3ms (DataTemplateSelector 대비 20배 빠름)
- ✅ 리소스 상속: Application 레벨 스타일 자동 적용
- ✅ 부모 클래스 검색: 코드 중복 제거
- ✅ Factory 함수: 조건부 View 생성
- ✅ WPF + Avalonia: 동일한 구조

## 📖 자세한 내용

전체 가이드는 **[ViewLocator_ViewPresenter_Guide.md](ViewLocator_ViewPresenter_Guide.md)**를 참고하세요.

---

**XamlDS.Itk - Industrial UI Toolkit**
