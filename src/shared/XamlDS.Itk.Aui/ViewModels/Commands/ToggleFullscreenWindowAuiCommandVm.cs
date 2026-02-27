using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using XamlDS.Itk.Resources;
using XamlDS.ViewModels.Commands;

namespace XamlDS.Itk.ViewModels.Commands;

public class ToggleFullscreenWindowAuiCommandVm : ToggleFullscreenWindowCommandVm
{
    private WindowState _previousWindowState = WindowState.Normal;

    public override void Execute(object? parameter)
    {
        // Try to get the main window from the application lifetime
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = desktop.MainWindow;
            if (mainWindow != null)
            {
                ToggleWindowFullscreen(mainWindow);
            }
        }
        // Alternative: handle window passed as command parameter
        else if (parameter is Window window)
        {
            ToggleWindowFullscreen(window);
        }
    }

    public override string DisplayName
    {
        get
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = desktop.MainWindow;
                if (mainWindow != null && mainWindow.WindowState == WindowState.FullScreen)
                {
                    return CommonStrings.Action_ExitFullscreen;
                }
            }
            return CommonStrings.Action_Fullscreen;
        }
        set => throw new NotSupportedException("DisplayName is dynamically determined.");
    }

    /// <summary>
    /// Toggle the window between fullscreen and previous window state
    /// </summary>
    /// <param name="window">The window to toggle fullscreen mode</param>
    private void ToggleWindowFullscreen(Window window)
    {
        if (window.WindowState == WindowState.FullScreen)
        {
            // Exit fullscreen and restore previous state
            window.WindowState = _previousWindowState;
        }
        else
        {
            // Store current state and enter fullscreen
            _previousWindowState = window.WindowState;
            window.WindowState = WindowState.FullScreen;
        }
        OnPropertyChanged(nameof(DisplayName)); // Update the display name based on new state
    }
}
