using XamlDS.Itk.Enums;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.ViewModels.Panes;
using XamlDS.Itk.ViewModels.Selectors;

namespace LayoutWork.ViewModels;

public class AppWindowVm : DockPanelVm
{
    private readonly ContentPaneVm _currentPane;

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

        var singleSelectorPanel = new SingleSelectorPanelVm<String> { Layout = SelectorPanelLayout.Horizontal };
        singleSelectorPanel.Add("Option 1", "Option1");
        singleSelectorPanel.Add("Option 2", "Option2");
        singleSelectorPanel.Add("Option 3", "Option3");

        var multiSelectorPanel = new MultiSelectorPanelVm<String> { Layout = SelectorPanelLayout.Horizontal };
        multiSelectorPanel.Add("Option A", "OptionA");
        multiSelectorPanel.Add("Option B", "OptionB");
        multiSelectorPanel.Add("Option C", "OptionC");

        var selectorsPanel = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        selectorsPanel.Add(singleSelectorPanel);
        selectorsPanel.Add(multiSelectorPanel);

        bottomBar.Add(selectorsPanel);

        AddBottom(bottomBar);
        AddLeft(new MockPaneVm { Label = "Sidebar", Width = 256 });
        AddRight(new MockPaneVm { Label = "Right", Width = 256 });

        _currentPane = new ContentPaneVm();
        _currentPane.Content = new MockPaneVm { Label = "Current" };
        Add(CurrentPane);
    }

    public ContentPaneVm CurrentPane => _currentPane;
}
