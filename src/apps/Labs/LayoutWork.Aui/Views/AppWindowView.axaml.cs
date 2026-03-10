using Avalonia.Controls;
using Avalonia.Controls.Templates;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.ViewModels.Panes;
using XamlDS.Itk.ViewModels.Selectors;
using XamlDS.Itk.Views.Layouts;
using XamlDS.Itk.Views.Panes;
using XamlDS.Itk.Views.Selectors;

namespace LayoutWork.Aui;

public partial class AppWindowView : Window
{
    public AppWindowView()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        this.DataTemplates.Add(new FuncDataTemplate<DockPanelVm>((value, namescope) => new DockPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<StackPanelVm>((value, namescope) => new StackPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<MockPaneVm>((value, namescope) => new MockPaneView()));
        this.DataTemplates.Add(new FuncDataTemplate<ContentPaneVm>((value, namescope) => new ContentPaneView()));

        // Match any SingleSelectorPanelVm<T> regardless of T
        this.DataTemplates.Add(new FuncDataTemplate(
            data => data?.GetType().IsGenericType == true &&
                    data.GetType().GetGenericTypeDefinition() == typeof(SingleSelectorPanelVm<>),
            (data, namescope) => new SingleSelectorPanelView()));

        // Match any MultiSelectorPanelVm<T> regardless of T
        this.DataTemplates.Add(new FuncDataTemplate(
            data => data?.GetType().IsGenericType == true &&
                    data.GetType().GetGenericTypeDefinition() == typeof(MultiSelectorPanelVm<>),
            (data, namescope) => new MultiSelectorPanelView()));
    }
}

