using System;
using System.Collections.Generic;
using System.Text;

namespace XamlDS.ViewModels.Commands;

public abstract class QuitApplicationCommandVm : CommandVm
{
    protected QuitApplicationCommandVm() : base("QuitApplication")
    {
        DisplayName = "Quit";
        Description = "Quit the application.";
    }
}
