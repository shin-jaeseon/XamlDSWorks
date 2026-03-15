using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using XamlDS.DemoApp.ViewModels;
using XamlDS.DemoApp.Views;
using XamlDS.Showcases.ItkControls;

namespace XamlDS.DemoApp.Wpf;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var builder = Host.CreateApplicationBuilder(e.Args);
        //builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var demoAppWpfLib = new DemoAppWpfLibrary();
        demoAppWpfLib.Register(builder);

        //var host = Host.CreateDefaultBuilder(e.Args)
        //    .UseContentRoot(AppContext.BaseDirectory)
        //    .ConfigureAppConfiguration((context, config) =>
        //    {
        //        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //        config.AddEnvironmentVariables();
        //        if (e.Args != null)
        //        {
        //            config.AddCommandLine(e.Args);
        //        }
        //    })
        //    .Build();

        var host = builder.Build();
        host.Start();

        MainWindow = new AppWindowView();
        MainWindow.DataContext = host.Services.GetRequiredService<AppWindowVm>();
        MainWindow.Show();

        base.OnStartup(e);
    }
}

