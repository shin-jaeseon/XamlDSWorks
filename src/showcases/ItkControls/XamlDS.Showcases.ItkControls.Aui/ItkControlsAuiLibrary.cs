using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace XamlDS.Showcases.ItkControls;

public sealed class ItkControlsAuiLibrary : XamlDS.Itk.XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkAuiLibrary = new ItkAuiLibrary();
        itkAuiLibrary.Register(hostBuilder);

        var itkControlsLib = new ItkControlsLibrary();
        itkControlsLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
    }
}
