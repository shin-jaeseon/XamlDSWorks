using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using XamlDS.DemoApp.ViewModels;
using XamlDS.Showcases.ItkControls.ViewModels;
using XamlDS.Showcases.ItkControls.Views;
using XamlDS.Showcases.ItkThemes.ViewModels;
using XamlDS.Showcases.ItkThemes.Views;

namespace XamlDS.DemoApp.Views;

public partial class AppWindowView : Window
{
    public AppWindowView()
    {
        InitializeComponent();

        this.DataTemplates.Add(new FuncDataTemplate<HomePanelVm>((value, namescope) => new HomePanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<ItkControlsPanelVm>((value, namescope) => new ItkControlsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<ItkThemesPanelVm>((value, namescope) => new ItkThemesPanelView()));

        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}
