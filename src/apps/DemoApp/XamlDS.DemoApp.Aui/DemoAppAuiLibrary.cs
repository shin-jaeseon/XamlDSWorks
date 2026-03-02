using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VitalSignsMonitor.Aui;
using XamlDS.Showcases.ItkControls;
using XamlDS.Showcases.ItkThemes;

namespace XamlDS.DemoApp;

internal sealed class DemoAppAuiLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var demoAppLib = new DemoAppLibrary();
        demoAppLib.Register(hostBuilder);

        var itkControlsLib = new ItkControlsAuiLibrary();
        itkControlsLib.Register(hostBuilder);

        var itkThemesLib = new ItkThemesAuiLibrary();
        itkThemesLib.Register(hostBuilder);

        var vitalSignsMonitorAuiLib = new VitalSignsMonitorAuiLibrary();
        vitalSignsMonitorAuiLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
    }
}
