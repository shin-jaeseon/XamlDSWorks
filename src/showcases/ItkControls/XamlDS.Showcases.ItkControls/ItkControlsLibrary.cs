using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;
using XamlDS.Showcases.ItkControls.ViewModels;
using XamlDS.Showcases.ItkControls.ViewModels.CircularGauges;
using XamlDS.Showcases.ItkControls.ViewModels.LinearGauges;
using XamlDS.Showcases.ItkControls.ViewModels.NumericFields;
using XamlDS.Showcases.ItkControls.ViewModels.RadialGauges;
using XamlDS.Showcases.ItkControls.ViewModels.TextFields;

namespace XamlDS.Showcases.ItkControls;

public sealed class ItkControlsLibrary : XamlDS.Itk.XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddTransient<ItkControlsPanelVm>();
        services.AddTransient<TextFieldsPanelVm>();

        services.AddTransient<NumericFieldsPanelVm>();
        services.AddTransient<NumericDemo1PaneVm>();

        services.AddTransient<CircularGaugesPanelVm>();
        services.AddTransient<LinearGaugesDemoPanelVm>();
        services.AddTransient<PowerGeneratorDemoVm>();
        services.AddTransient<RadialGaugesPanelVm>();
        services.AddTransient<PowerGeneratorPaneVm>();
    }
}
