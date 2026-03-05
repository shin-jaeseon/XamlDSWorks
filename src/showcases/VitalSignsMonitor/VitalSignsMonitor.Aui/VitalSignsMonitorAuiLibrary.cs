using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace VitalSignsMonitor.Aui;

public sealed class VitalSignsMonitorAuiLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkAuiLib = new ItkAuiLibrary();
        itkAuiLib.Register(hostBuilder);

        var vitalSignsMonitorLib = new VitalSignsMonitorLibrary();
        vitalSignsMonitorLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
    }
}
