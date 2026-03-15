using Microsoft.Extensions.Hosting;
using XamlDS.Itk;

namespace LayoutWork.Wpf;

class LayoutWorkWpfLibrary : ItkWpfLibraryBase
{
    public override void Register(HostApplicationBuilder hostBuilder)
    {
        var itkWpfLib = new ItkWpfLibrary();
        itkWpfLib.Register(hostBuilder);

        var layoutWorkLib = new LayoutWorkLibrary();
        layoutWorkLib.Register(hostBuilder);

        base.Register(hostBuilder);
    }

    protected override void RegisterView()
    {
        base.RegisterView();

        // Main window mapping
        //ViewPresenter.Register<AppWindowVm, DockPanelView>();
    }
}
