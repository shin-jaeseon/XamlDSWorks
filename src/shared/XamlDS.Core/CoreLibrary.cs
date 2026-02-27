using Microsoft.Extensions.DependencyInjection;

namespace XamlDS;

public sealed class CoreLibrary : XamlDSLibrary
{
    protected override void AddServices(IServiceCollection services)
    {
        services.AddSingleton<Services.IMessenger, Services.Messenger>();
    }
}
