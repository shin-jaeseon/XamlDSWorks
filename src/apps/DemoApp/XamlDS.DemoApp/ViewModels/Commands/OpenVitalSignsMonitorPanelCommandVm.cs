using XamlDS.Itk.ViewModels;

namespace XamlDS.DemoApp.ViewModels.Commands;

public sealed class OpenVitalSignsMonitorPanelCommandVm : CommandVm
{
    private AppMainPanelVm _appMainPanel;

    public OpenVitalSignsMonitorPanelCommandVm(AppMainPanelVm appMainPanel) : base("OpenVitalSignsMonitorPanel")
    {
        DisplayName = "Vital Signs Monitor";
        _appMainPanel = appMainPanel;
        _appMainPanel.PropertyChanged += OnAppMainPanelStatePropertyChanged;
    }
    public override bool CanExecute(object? parameter)
    {
        if (_appMainPanel.CurrentPanelName == "VitalSignsMonitor")
            return false;
        return true;
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        _appMainPanel.CurrentPanelName = "VitalSignsMonitor";
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
