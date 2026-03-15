using LayoutWork.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace LayoutWork;

public class LayoutWorkLibrary : ItkLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        //var itkLib = new ItkLibrary();
        //itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddTransient<AppWindowVm>();
    }
}
