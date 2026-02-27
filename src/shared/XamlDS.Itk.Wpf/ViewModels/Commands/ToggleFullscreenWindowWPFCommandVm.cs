using XamlDS.Itk.Resources;
using XamlDS.ViewModels.Commands;

namespace XamlDS.Itk.ViewModels.Commands;

public class ToggleFullscreenWindowWpfCommandVm : ToggleFullscreenWindowCommandVm
{
    protected bool IsFullscreen
    {
        get
        {
            var appWindow = System.Windows.Application.Current.MainWindow;
            return appWindow != null && appWindow.WindowState == System.Windows.WindowState.Maximized && appWindow.WindowStyle == System.Windows.WindowStyle.None;
        }
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        var appWindow = System.Windows.Application.Current.MainWindow;
        if (appWindow != null)
        {
            if (IsFullscreen)
            {
                appWindow.WindowState = System.Windows.WindowState.Normal;
                appWindow.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            }
            else
            {
                appWindow.WindowStyle = System.Windows.WindowStyle.None;
                appWindow.WindowState = System.Windows.WindowState.Maximized;
            }
            OnPropertyChanged(nameof(DisplayName)); // Update the display name based on new state
        }
    }

    public override string DisplayName
    {
        get
        {
            return IsFullscreen ? CommonStrings.Action_ExitFullscreen : CommonStrings.Action_Fullscreen;
        }
        set => throw new NotSupportedException("DisplayName is dynamically determined.");
    }
}
