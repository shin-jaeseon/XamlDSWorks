using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaControlFactory.ViewModels;
using AvaloniaControlFactory.Views;

namespace AvaloniaControlFactory;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindowView();
            desktop.MainWindow.DataContext = new MainWindowVm();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
