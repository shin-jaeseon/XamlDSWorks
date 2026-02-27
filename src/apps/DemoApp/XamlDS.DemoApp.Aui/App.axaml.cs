using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using XamlDS.DemoApp.ViewModels;
using XamlDS.DemoApp.Views;
using XamlDS.Itk.Aui;

namespace XamlDS.DemoApp;

public partial class App : Application
{
    private ApplicationManager<AppWindowView, AppWindowVm>? _applicationManager;

    // Host will be set from Program.Main
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
