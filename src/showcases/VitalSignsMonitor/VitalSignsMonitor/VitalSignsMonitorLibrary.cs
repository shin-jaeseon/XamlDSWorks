using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VitalSignsMonitor.ViewModels;
using XamlDS.Itk;

namespace VitalSignsMonitor;

public sealed class VitalSignsMonitorLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
        services.AddScoped<VitalSignsMonitorPanelVm>();
    }
}
