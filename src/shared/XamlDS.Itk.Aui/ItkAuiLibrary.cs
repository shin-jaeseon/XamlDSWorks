using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Commands;
using XamlDS.Itk.ViewModels.Layouts;
using XamlDS.Itk.ViewModels.Panels;
using XamlDS.Itk.ViewModels.Selectors;
using XamlDS.Itk.Views.Layouts;
using XamlDS.Itk.Views.Panels;
using XamlDS.Itk.Views.Selectors;

namespace XamlDS.Itk;

public sealed class ItkAuiLibrary : ItkAuiLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ThemeSettingsVm, ThemeSettingsAuiVm>();

        services.AddSingleton<QuitApplicationCommandVm, QuitApplicationAuiCommandVm>();
        services.AddSingleton<ToggleFullscreenWindowCommandVm, ToggleFullscreenWindowAuiCommandVm>();
    }

    protected override void AddViewTemplates()
    {
        // Layout Views - Explicit ViewModel → View mapping
        ItkDataTemplates.Add<DockPanelVm, DockPanelView>();
        ItkDataTemplates.Add<StackPanelVm, StackPanelView>();
        ItkDataTemplates.Add<ContentPanelVm, ContentPanelView>();

        // Panel Views
        ItkDataTemplates.Add<MockPanelVm, MockPanelView>();
        ItkDataTemplates.Add<GridLinePanelVm, GridLinePanelView>();

        // Conditional template for interface-based matching (lower priority)
        ItkDataTemplates.AddConditional<ISelectorPanelVm, SelectorPanelView>();
    }
}
