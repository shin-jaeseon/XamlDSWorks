using XamlDS.Itk.Enums;
using XamlDS.Itk.ViewModels.Panels;
using XamlDS.Itk.ViewModels.Panes;

namespace LayoutWork.ViewModels;

public class AppWindowVm : DockPanelVm
{
    public AppWindowVm()
    {
        var bottomBar = new DockPanelVm();
        var bottomLeft = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        bottomLeft.Add(new MockPaneVm { Label = "HOME", Width = 96, Height = 48 });
        bottomBar.AddLeft(bottomLeft);

        var bottomRight = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        bottomRight.Add(new MockPaneVm { Label = "EXIT", Width = 96, Height = 48 });
        bottomRight.Add(new MockPaneVm { Label = "OPTIONS", Width = 96, Height = 48 });
        bottomBar.AddRight(bottomRight);

        bottomBar.Add(new MockPaneVm { Label = "Status", Height = 48 });

        AddBottom(bottomBar);
        AddLeft(new MockPaneVm { Label = "Sidebar", Width = 256 });
        AddRight(new MockPaneVm { Label = "Right", Width = 256 });
        Add(new MockPaneVm { Label = "Center" });
    }
}
