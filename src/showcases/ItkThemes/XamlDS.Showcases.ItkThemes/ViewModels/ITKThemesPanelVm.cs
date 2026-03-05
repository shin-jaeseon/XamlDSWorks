using Microsoft.Extensions.DependencyInjection;
using XamlDS.Itk.ViewModels;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase01;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase02;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase03;

namespace XamlDS.Showcases.ItkThemes.ViewModels;

public class ItkThemesPanelVm : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    private ViewModelBase? _currentPanel;
    private string _currentPanelName = String.Empty;

    public ItkThemesPanelVm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        CurrentPanelName = "Controls";
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
                _currentPanel?.Dispose();
                _currentPanel = null;
                switch (value)
                {
                    case "Controls":
                        CurrentPanel = _serviceProvider.GetRequiredService<ControlsPanelVm>();
                        break;
                    case "Showcase01":
                        CurrentPanel = _serviceProvider.GetRequiredService<Showcase01PanelVm>();
                        break;
                    case "Showcase02":
                        CurrentPanel = _serviceProvider.GetRequiredService<Showcase02PanelVm>();
                        break;
                    case "Showcase03":
                        CurrentPanel = _serviceProvider.GetRequiredService<Showcase03PanelVm>();
                        break;
                    default:
                        throw new ArgumentException("Invalid CurrentPanelName");
                }
            }
        }
    }
}
