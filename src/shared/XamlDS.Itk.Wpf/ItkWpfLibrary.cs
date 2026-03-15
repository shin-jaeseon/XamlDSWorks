using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Commands;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.Views.Layouts;

namespace XamlDS.Itk;

public sealed class ItkWpfLibrary : ItkWpfLibraryBase
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

    protected override void RegisterView()
    {
        // Register layout views
        ViewLocator.Register<ContentPanelVm, ContentPanelView>();
        ViewLocator.Register<DockPanelVm, DockPanelView>();
        ViewLocator.Register<StackPanelVm, StackPanelView>();
    }
}
