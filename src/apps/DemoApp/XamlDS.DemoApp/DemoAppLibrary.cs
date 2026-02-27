using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.DemoApp.ViewModels;
using XamlDS.DemoApp.ViewModels.Commands;
using XamlDS.Showcases.ItkControls;
using XamlDS.Showcases.ItkThemes;

namespace XamlDS.DemoApp;

public sealed class DemoAppLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkThemesLib = new ItkThemesLibrary();
        itkThemesLib.Register(hostBuilder);

        var itkControlsLib = new ItkControlsLibrary();
        itkControlsLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
        services.AddTransient<HomePanelVm>();

        services.AddSingleton<AppBottomBarVm>();
        services.AddSingleton<AppMainPanelVm>();
        services.AddSingleton<AppWindowVm>();

        // Commands
        services.AddTransient<OpenHomePanelCommandVm>();
        services.AddTransient<OpenItkControlsMainPanelCommandVm>();
        services.AddTransient<OpenItkThemesMainPanelCommandVm>();
    }
}
