using Avalonia;
using Microsoft.Extensions.Hosting;
using System;

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

        var layoutWorkLib = new LayoutWork.LayoutWorkLibrary();
        layoutWorkLib.Register(builder);

        var host = builder.Build();
        host.Start();

        // Make the host available to the Avalonia Application
        App.Host = host;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
