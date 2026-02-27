using XamlDS.ViewModels;

namespace XamlDS.DemoApp.ViewModels.Commands;

public sealed class OpenItkControlsMainPanelCommandVm : CommandVm
{
    private AppMainPanelVm _appMainPanel;

    public OpenItkControlsMainPanelCommandVm(AppMainPanelVm appMainPanel) : base("OpenItkControlsMainPanel")
    {
        DisplayName = "Itk Controls";
        _appMainPanel = appMainPanel;
        _appMainPanel.PropertyChanged += OnAppMainPanelStatePropertyChanged;
    }
    public override bool CanExecute(object? parameter)
    {
        if (_appMainPanel.CurrentPanelName == "ItkControls")
            return false;
        return true;
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        _appMainPanel.CurrentPanelName = "ItkControls";
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
