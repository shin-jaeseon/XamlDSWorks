using XamlDS.Itk.Enums;
using XamlDS.Itk.Themes;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.ViewModels.Panes;
using XamlDS.Itk.ViewModels.Selectors;

namespace LayoutWork.ViewModels;

public class AppWindowVm : DockPanelVm
{
    private readonly ContentPanelVm _currentPane;

    public AppWindowVm()
    {
        _currentPane = new ContentPanelVm();
        _currentPane.Content = new MockPaneVm { Label = "Current" };

        var bottomBar = new DockPanelVm();
        var bottomLeft = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        bottomLeft.Add(new MockPaneVm { Label = "HOME", Width = 96, Height = 48 });
        bottomBar.AddLeft(bottomLeft);

        var bottomRight = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        bottomRight.Add(new MockPaneVm { Label = "EXIT", Width = 96, Height = 48 });
        bottomRight.Add(new MockPaneVm { Label = "OPTIONS", Width = 96, Height = 48 });
        bottomBar.AddRight(bottomRight);

        var singleSelectorPanel = new SingleSelectorPanelVm<String> { Layout = SelectorPanelLayout.Horizontal };
        singleSelectorPanel.SelectionChanged += OnSingleSelectorPanelSelectionChanged;
        singleSelectorPanel.Add("SubPanel A", "SubPanelA", ThemeAccentBrush.AccentA);
        singleSelectorPanel.Add("SubPanel B", "SubPanelB");
        singleSelectorPanel.Add("SubPanel C", "SubPanelC");
        singleSelectorPanel.SelectItem("SubPanelA");

        var multiSelectorPanel = new MultiSelectorPanelVm<String> { Layout = SelectorPanelLayout.Horizontal };
        multiSelectorPanel.Add("Option A", "OptionA");
        multiSelectorPanel.Add("Option B", "OptionB", ThemeAccentBrush.AccentB);
        multiSelectorPanel.Add("Option C", "OptionC", ThemeAccentBrush.AccentC);
        multiSelectorPanel.ToggleItem("OptionA");
        multiSelectorPanel.ToggleItem("OptionC");

        var selectorsPanel = new StackPanelVm { Orientation = ItkOrientation.Horizontal };
        selectorsPanel.Add(singleSelectorPanel);
        selectorsPanel.Add(multiSelectorPanel);

        bottomBar.Add(selectorsPanel);
        AddBottom(bottomBar);

        Add(CurrentPane);
    }

    public ContentPanelVm CurrentPane => _currentPane;

    private void OnSingleSelectorPanelSelectionChanged(object? sender, SelectionChangedEventArgs<String> e)
    {
        if (CurrentPane.Content != null)
        {
            var oldContent = CurrentPane.Content;
            (oldContent as IDisposable)?.Dispose();
            CurrentPane.Content = null;
        }
        switch (e.NewItem)
        {
            case "SubPanelA":
                CurrentPane.Content = new SubAPanelVm();
                break;
            case "SubPanelB":
                CurrentPane.Content = new SubBPanelVm();
                break;
            case "SubPanelC":
                CurrentPane.Content = new SubCPanelVm();
                break;
        }
    }
}
