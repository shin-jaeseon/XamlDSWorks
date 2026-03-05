using XamlDS.Itk.Resources;

namespace XamlDS.Itk.ViewModels.Commands;

internal class QuitApplicationWpfCommandVm : QuitApplicationCommandVm
{
    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;
        System.Windows.Application.Current.Shutdown();
    }

    public override string DisplayName { get => CommonStrings.Action_Quit; }
}
