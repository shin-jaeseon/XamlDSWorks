using Avalonia.Controls;
using Avalonia.Controls.Templates;
using XamlDS.Itk;
using XamlDS.Itk.ViewModels.Panels;
using XamlDS.Itk.ViewModels.Panes;
using XamlDS.Itk.Views.Panels;
using XamlDS.Itk.Views.Panes;

namespace LayoutWork.Aui;

public partial class AppWindowView : Window
{
    public AppWindowView()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        this.DataTemplates.Add(new FuncDataTemplate<DockPanelVm>((value, namescope) => new DockPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<StackPanelVm>((value, namescope) => new StackPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<TextPaneVm>((value, namescope) => new TextPaneView()));
        this.DataTemplates.Add(new FuncDataTemplate<MockPaneVm>((value, namescope) => new MockPaneView()));
    }
}

