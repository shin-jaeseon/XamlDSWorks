using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace LayoutWork;

internal class LayoutWorkAuiLibrary : ItkAuiLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var ItkAuiLib = new ItkAuiLibrary();
        ItkAuiLib.Register(hostBuilder);

        var layoutWorkLib = new LayoutWork.LayoutWorkLibrary();
        layoutWorkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void RegisterView()
    {
    }
}
