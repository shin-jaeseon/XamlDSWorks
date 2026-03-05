using XamlDS.Itk.Resources;
using XamlDS.Itk.ViewModels;

namespace XamlDS.DemoApp.ViewModels.Commands;

public sealed class OpenHomePanelCommandVm : CommandVm
{
    private AppMainPanelVm _appMainPanel;

    public OpenHomePanelCommandVm(AppMainPanelVm appMainPanel) : base("OpenHomePanel")
    {
        DisplayName = "Home";
        _appMainPanel = appMainPanel;
        _appMainPanel.PropertyChanged += OnAppMainPanelStatePropertyChanged;
    }

    public override string DisplayName { get => CommonStrings.Action_Home; }

    public override bool CanExecute(object? parameter)
    {
        if (_appMainPanel.CurrentPanelName == "Home")
            return false;
        return true;
    }

    public override void Execute(object? parameter)
    {
        if (CanExecute(parameter) == false)
            return;

        _appMainPanel.CurrentPanelName = "Home";
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
