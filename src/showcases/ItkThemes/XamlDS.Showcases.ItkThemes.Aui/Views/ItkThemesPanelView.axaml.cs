using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase01;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase02;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase03;
using XamlDS.Showcases.ItkThemes.Views.Controls;
using XamlDS.Showcases.ItkThemes.Views.Showcase01;
using XamlDS.Showcases.ItkThemes.Views.Showcase02;
using XamlDS.Showcases.ItkThemes.Views.Showcase03;

namespace XamlDS.Showcases.ItkThemes.Views;

public partial class ItkThemesPanelView : UserControl
{
    public ItkThemesPanelView()
    {
        InitializeComponent();

        this.DataTemplates.Add(new FuncDataTemplate<ControlsPanelVm>((value, namescope) => new ControlsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<Showcase01PanelVm>((value, namescope) => new Showcase01PanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<Showcase02PanelVm>((value, namescope) => new Showcase02PanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<Showcase03PanelVm>((value, namescope) => new Showcase03PanelView()));
    }
}
