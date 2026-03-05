using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;
using XamlDS.Showcases.ItkThemes.ViewModels;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.CheckBoxes;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.TextBoxs;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase01;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase02;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase03;

namespace XamlDS.Showcases.ItkThemes;

public sealed class ItkThemesLibrary : XamlDS.Itk.XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddScoped<ItkThemesPanelVm>();

        services.AddTransient<Showcase01PanelVm>();
        services.AddTransient<Showcase02PanelVm>();
        services.AddTransient<Showcase03PanelVm>();
        services.AddTransient<ControlsPanelVm>();

        // Controls
        services.AddTransient<ButtonsPanelVm>();
        services.AddTransient<CheckBoxesPanelVm>();
        services.AddTransient<RadioButtonsPanelVm>();
        services.AddTransient<SlidersPanelVm>();
        services.AddTransient<TextBoxsPanelVm>();
    }
}
