using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Showcases.ItkControls;
using XamlDS.Showcases.ItkThemes;

namespace XamlDS.DemoApp;

internal sealed class DemoAppWpfLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var demoAppLib = new DemoAppLibrary();
        demoAppLib.Register(hostBuilder);

        var itkControlsLib = new ItkControlsWpfLibrary();
        itkControlsLib.Register(hostBuilder);

        var itkThemesLib = new ItkThemesWpfLibrary();
        itkThemesLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {

    }
}
