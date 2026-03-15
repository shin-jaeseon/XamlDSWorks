using LayoutWork.ViewModels;
using LayoutWork.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace LayoutWork.Wpf;
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
        var layoutWorkWpfLib = new LayoutWorkWpfLibrary();
        layoutWorkWpfLib.Register(builder);
        var host = builder.Build();
        host.Start();

        MainWindow = new AppWindowView();
        MainWindow.DataContext = host.Services.GetRequiredService<AppWindowVm>();
        MainWindow.Show();

        base.OnStartup(e);
    }
}

