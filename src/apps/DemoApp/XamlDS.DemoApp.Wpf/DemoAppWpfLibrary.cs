using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VitalSignsMonitor;
using XamlDS.Showcases.ItkControls;
using XamlDS.Showcases.ItkThemes;

namespace XamlDS.DemoApp;

internal sealed class DemoAppWpfLibrary : XamlDS.Itk.XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var demoAppLib = new DemoAppLibrary();
        demoAppLib.Register(hostBuilder);

        var itkControlsWpfLib = new ItkControlsWpfLibrary();
        itkControlsWpfLib.Register(hostBuilder);

        var itkThemesWpfLib = new ItkThemesWpfLibrary();
        itkThemesWpfLib.Register(hostBuilder);

        var vitalSignsMonitorWpfLib = new VitalSignsMonitorWpfLibrary();
        vitalSignsMonitorWpfLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {

    }
}
