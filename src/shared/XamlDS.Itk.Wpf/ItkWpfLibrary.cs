using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Commands;

namespace XamlDS.Itk;

public sealed class ItkWpfLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ThemeSettingsVm, ThemeSettingsWpfVm>();

        services.AddSingleton<QuitApplicationCommandVm, QuitApplicationWpfCommandVm>();
        services.AddSingleton<ToggleFullscreenWindowCommandVm, ToggleFullscreenWindowWpfCommandVm>();
    }
}
