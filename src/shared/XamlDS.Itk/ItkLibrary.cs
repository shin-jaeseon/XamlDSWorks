using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.Factories;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Gauges;

namespace XamlDS.Itk;

public sealed class ItkLibrary : ItkLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddSingleton<Services.IMessenger, Services.Messenger>();
        services.AddSingleton<LanguageSettingsVm>();
        services.AddTransient<INumericFieldVmFactory, NumericFieldVmFactory>();
        services.AddTransient<IGaugeVmFactory, GaugeVmFactory>();

        services.AddTransient<TextGaugesPanelVm>();
        services.AddTransient<LinearGaugesPanelVm>();
        services.AddTransient<RadialGaugesPanelVm>();
        services.AddTransient<CircularGaugesPanelVm>();
    }
}
