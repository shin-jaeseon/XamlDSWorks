using XamlDS.Itk.Themes;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.ViewModels.Panels;
using XamlDS.Itk.ViewModels.Selectors;

namespace LayoutWork.ViewModels;

public class SubAPanelVm : DockPanelVm
{
    public SubAPanelVm()
    {
        var leftMenuPanel = new SingleSelectorPanelVm<String> { Layout = SelectorPanelLayout.Vertical, Width = 100 };
        leftMenuPanel.SelectionChanged += OnLeftMenuPanelSelectionChanged;
        leftMenuPanel.Add("Sub A", "SubA", ThemeAccentBrush.AccentB);
        leftMenuPanel.Add("Sub B", "SubB", ThemeAccentBrush.Default);
        leftMenuPanel.Add("Sub C", "SubC", ThemeAccentBrush.Default);
        leftMenuPanel.Add("Sub D", "SubD", ThemeAccentBrush.Default);
        leftMenuPanel.SelectItem("SubA");
        AddLeft(leftMenuPanel);

        var contentPanel = new ContentPanelVm { Content = "Empty Content Panel", PanelStyle = ContentPanelStyle.Bordered };
        Add(contentPanel);
    }

    private void OnLeftMenuPanelSelectionChanged(object? sender, SelectionChangedEventArgs<string> e)
    {

    }
}

public class SubBPanelVm : DockPanelVm
{
    public SubBPanelVm()
    {
        AddLeft(new MockPanelVm { Label = "Sub B", Width = 100 });
        Add(new MockPanelVm { Label = "Sub B Center" });
    }
}

public class SubCPanelVm : DockPanelVm
{
    public SubCPanelVm()
    {
        AddLeft(new MockPanelVm { Label = "Sub C", Width = 100 });
        Add(new MockPanelVm { Label = "Sub C Center" });
    }
}
