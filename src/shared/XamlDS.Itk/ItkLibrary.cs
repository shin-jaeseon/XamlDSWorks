using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Itk;

public sealed class ItkLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var coreLib = new CoreLibrary();
        coreLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddSingleton<LanguageSettingsVm>();
    }
}
