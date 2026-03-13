using Avalonia;
using Microsoft.Extensions.Hosting;
using System;
using XamlDS.Itk;

namespace LayoutWork;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var ItkAuiLib = new ItkAuiLibrary();
        ItkAuiLib.Register(builder);

        var layoutWorkLib = new LayoutWork.LayoutWorkLibrary();
        layoutWorkLib.Register(builder);

        var layoutWorkAuiLib = new LayoutWorkAuiLibrary();
        layoutWorkAuiLib.Register(builder);

        var host = builder.Build();
        host.Start();
        App.Host = host;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            //.WithInterFont()
            .WithDeveloperTools()
            .LogToTrace();
}
