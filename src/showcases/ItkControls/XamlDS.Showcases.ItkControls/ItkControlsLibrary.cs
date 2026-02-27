using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using XamlDS.Itk;
using XamlDS.Showcases.ItkControls.ViewModels;

namespace XamlDS.Showcases.ItkControls;

public sealed class ItkControlsLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkLib = new ItkLibrary();
        itkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
        services.AddTransient<ItkControlsPanelVm>();
    }
}
