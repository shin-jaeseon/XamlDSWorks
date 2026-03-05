using Microsoft.Extensions.DependencyInjection;
using XamlDS.Itk.ViewModels;
using XamlDS.Showcases.ItkControls.ViewModels.CircularGauges;
using XamlDS.Showcases.ItkControls.ViewModels.LinearGauges;
using XamlDS.Showcases.ItkControls.ViewModels.RadialGauges;
using XamlDS.Showcases.ItkControls.ViewModels.TextFields;

namespace XamlDS.Showcases.ItkControls.ViewModels;

public class ItkControlsPanelVm : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    private ViewModelBase? _currentPanel;
    private string _currentPanelName = string.Empty;


    public ItkControlsPanelVm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        CurrentPanelName = "TextFields";
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
                    case "TextFields":
                        CurrentPanel = _serviceProvider.GetRequiredService<TextFieldsPanelVm>();
                        break;
                    case "LinearGauges":
                        CurrentPanel = _serviceProvider.GetRequiredService<LinearGaugesPanelVm>();
                        break;
                    case "RadialGauges":
                        CurrentPanel = _serviceProvider.GetRequiredService<RadialGaugesPanelVm>();
                        break;
                    case "CircularGauges":
                        CurrentPanel = _serviceProvider.GetRequiredService<CircularGaugesPanelVm>();
                        break;
                    default:
                        throw new ArgumentException("Invalid CurrentPanelName");
                }
            }
        }
    }
}
