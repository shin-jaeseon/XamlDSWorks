namespace XamlDS.Itk.ViewModels.Commands;

public abstract class QuitApplicationCommandVm : CommandVm
{
    protected QuitApplicationCommandVm() : base("QuitApplication")
    {
        DisplayName = "Quit";
        Description = "Quit the application.";
    }
}
