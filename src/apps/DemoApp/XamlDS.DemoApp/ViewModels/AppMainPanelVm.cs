using Microsoft.Extensions.DependencyInjection;
using VitalSignsMonitor.ViewModels;
using XamlDS.Showcases.ItkControls.ViewModels;
using XamlDS.Showcases.ItkThemes.ViewModels;
using XamlDS.ViewModels;

namespace XamlDS.DemoApp.ViewModels;

public sealed class AppMainPanelVm : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private IServiceScope? _currentScope;
    private ViewModelBase? _currentPanel;
    private string _currentPanelName = String.Empty;

    public AppMainPanelVm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ViewModelBase? CurrentPanel
    {
        get => _currentPanel;
        private set => SetProperty(ref _currentPanel, value);
    }

    public string CurrentPanelName
    {
        get => _currentPanelName;
        set
        {
            if (SetProperty(ref _currentPanelName, value))
            {
                DisposeCurrentPanel();
                _currentScope = _serviceProvider.CreateScope();
                switch (value)
                {
                    case "Home":
                        CurrentPanel = _currentScope.ServiceProvider.GetRequiredService<HomePanelVm>();
                        break;
                    case "ItkControls":
                        CurrentPanel = _currentScope.ServiceProvider.GetRequiredService<ItkControlsPanelVm>();
                        break;
                    case "ItkThemes":
                        CurrentPanel = _currentScope.ServiceProvider.GetRequiredService<ItkThemesPanelVm>();
                        break;
                    case "VitalSignsMonitor":
                        CurrentPanel = _currentScope.ServiceProvider.GetRequiredService<VitalSignsMonitorPanelVm>();
                        break;
                    default:
                        throw new ArgumentException("Invalid CurrentPanelName");
                }
            }
        }
    }

    private void DisposeCurrentPanel()
    {
        _currentScope?.Dispose();
        _currentScope = null;
        CurrentPanel = null;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeCurrentPanel();
        }
        base.Dispose(disposing);
    }
}
