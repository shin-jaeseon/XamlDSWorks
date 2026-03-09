using Avalonia;
using Avalonia.Markup.Xaml;
using LayoutWork.Aui;
using LayoutWork.ViewModels;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.Aui;

namespace LayoutWork;

public partial class App : Application
{
    private ApplicationManager<AppWindowView, AppWindowVm>? _applicationManager;

    public static IHost? Host { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _applicationManager = new ApplicationManager<AppWindowView, AppWindowVm>(this, Host);
        base.OnFrameworkInitializationCompleted();
    }
}
