using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using XamlDS.Itk.Resources;
using XamlDS.ViewModels.Commands;

namespace XamlDS.Itk.ViewModels.Commands;

internal class QuitApplicationAuiCommandVm : QuitApplicationCommandVm
{
    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
        {
            desktopApp.Shutdown();
        }
    }

    public override string DisplayName { get => CommonStrings.Action_Quit; }
}
