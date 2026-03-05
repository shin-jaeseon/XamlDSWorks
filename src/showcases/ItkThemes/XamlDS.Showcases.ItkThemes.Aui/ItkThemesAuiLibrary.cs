using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace XamlDS.Showcases.ItkThemes;

public sealed class ItkThemesAuiLibrary : XamlDS.Itk.XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkAuiLib = new ItkAuiLibrary();
        itkAuiLib.Register(hostBuilder);

        var itkThemesLib = new ItkThemesLibrary();
        itkThemesLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
    }
}
