using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace VitalSignsMonitor;

public sealed class VitalSignsMonitorWpfLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkWpfLib = new ItkWpfLibrary();
        itkWpfLib.Register(hostBuilder);

        var vitalSignsMonitorLib = new VitalSignsMonitorLibrary();
        vitalSignsMonitorLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
    }
}
