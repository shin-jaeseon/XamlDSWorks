using XamlDS.Itk.ViewModels;

namespace XamlDS.DemoApp.ViewModels.Commands;

public sealed class OpenItkThemesMainPanelCommandVm : CommandVm
{
    private AppMainPanelVm _appMainPanel;

    public OpenItkThemesMainPanelCommandVm(AppMainPanelVm appMainPanel) : base("OpenItkThemesMainPanel")
    {
        DisplayName = "Itk Themes";
        _appMainPanel = appMainPanel;
        _appMainPanel.PropertyChanged += OnAppMainPanelStatePropertyChanged;
    }
    public override bool CanExecute(object? parameter)
    {
        if (_appMainPanel.CurrentPanelName == "ItkThemes")
            return false;
        return true;
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        _appMainPanel.CurrentPanelName = "ItkThemes";
    }

    private void OnAppMainPanelStatePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_appMainPanel.CurrentPanelName))
        {
            RaiseCanExecuteChanged();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _appMainPanel.PropertyChanged -= OnAppMainPanelStatePropertyChanged;
        }
        base.Dispose(disposing);
    }
}
