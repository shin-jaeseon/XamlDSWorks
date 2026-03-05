using Microsoft.Extensions.DependencyInjection;
using XamlDS.Itk.ViewModels;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.CheckBoxes;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.TextBoxs;

namespace XamlDS.Showcases.ItkThemes.ViewModels.Controls;

public class ControlsPanelVm : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    private ViewModelBase? _currentPanel;
    private string _currentPanelName = string.Empty;

    public ControlsPanelVm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        CurrentPanelName = "Buttons";
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
                    case "Buttons":
                        CurrentPanel = _serviceProvider.GetRequiredService<ButtonsPanelVm>();
                        break;
                    case "CheckBoxes":
                        CurrentPanel = _serviceProvider.GetRequiredService<CheckBoxesPanelVm>();
                        break;
                    case "RadioButtons":
                        CurrentPanel = _serviceProvider.GetRequiredService<RadioButtonsPanelVm>();
                        break;
                    case "Sliders":
                        CurrentPanel = _serviceProvider.GetRequiredService<SlidersPanelVm>();
                        break;
                    case "TextBoxs":
                        CurrentPanel = _serviceProvider.GetRequiredService<TextBoxsPanelVm>();
                        break;
                    default:
                        throw new ArgumentException("Invalid CurrentPanelName");
                }
            }
        }
    }
}
