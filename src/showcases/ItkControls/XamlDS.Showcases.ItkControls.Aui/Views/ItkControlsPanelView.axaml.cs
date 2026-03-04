using Avalonia.Controls;
using Avalonia.Controls.Templates;
using XamlDS.Showcases.ItkControls.ViewModels.CircularGauges;
using XamlDS.Showcases.ItkControls.ViewModels.LinearGauges;
using XamlDS.Showcases.ItkControls.ViewModels.RadialGauges;
using XamlDS.Showcases.ItkControls.ViewModels.TextFields;
using XamlDS.Showcases.ItkControls.Views.CircularGauges;
using XamlDS.Showcases.ItkControls.Views.LinearGauges;
using XamlDS.Showcases.ItkControls.Views.RadialGauges;
using XamlDS.Showcases.ItkControls.Views.TextFields;

namespace XamlDS.Showcases.ItkControls.Views;

public partial class ItkControlsPanelView : UserControl
{
    public ItkControlsPanelView()
    {
        InitializeComponent();

        this.DataTemplates.Add(
            new FuncDataTemplate<TextFieldsPanelVm>((value, namescope) => new TextFieldsPanelView()));
        this.DataTemplates.Add(
            new FuncDataTemplate<LinearGaugesPanelVm>((value, namescope) => new LinearGaugesPanelView()));
        this.DataTemplates.Add(
            new FuncDataTemplate<RadialGaugesPanelVm>((value, namescope) => new RadialGaugesPanelView()));
        this.DataTemplates.Add(
            new FuncDataTemplate<CircularGaugesPanelVm>((value, namescope) => new CircularGaugesPanelView()));
    }
}
