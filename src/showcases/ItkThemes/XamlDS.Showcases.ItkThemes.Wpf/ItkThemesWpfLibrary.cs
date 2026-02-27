using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using XamlDS.Itk;

namespace XamlDS.Showcases.ItkThemes;

public sealed class ItkThemesWpfLibrary : XamlDSLibrary
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkWpfLib = new ItkWpfLibrary();
        itkWpfLib.Register(hostBuilder);

        var itkThemesLib = new ItkThemesLibrary();
        itkThemesLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }
    protected override void AddServices(IServiceCollection services)
    {
    }
}
