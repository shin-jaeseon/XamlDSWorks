using Microsoft.Extensions.Hosting;

namespace XamlDS.Itk;

public abstract class ItkWpfLibraryBase : ItkLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        base.Register(hostBuilder);
        RegisterView();
    }

    protected virtual void RegisterView() { }
}
