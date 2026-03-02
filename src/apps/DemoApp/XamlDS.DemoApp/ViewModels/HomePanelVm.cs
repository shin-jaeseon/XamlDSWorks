using XamlDS.DemoApp.ViewModels.Commands;
using XamlDS.ViewModels;

namespace XamlDS.DemoApp.ViewModels;

public class HomePanelVm : ViewModelBase
{
    public HomePanelVm(
        OpenItkControlsMainPanelCommandVm openItkControlsMainPanelCommandVm,
        OpenItkThemesMainPanelCommandVm openItkThemesMainPanelCommandVm,
        OpenVitalSignsMonitorPanelCommandVm openVitalSignsMonitorPanelCommandVm)
    {
        OpenItkControlsMainPanelCvm = openItkControlsMainPanelCommandVm;
        OpenItkThemesMainPanelCvm = openItkThemesMainPanelCommandVm;
        OpenVitalSignsMonitorPanelCvm = openVitalSignsMonitorPanelCommandVm;
    }

    public OpenItkThemesMainPanelCommandVm OpenItkThemesMainPanelCvm { get; }
    public OpenItkControlsMainPanelCommandVm OpenItkControlsMainPanelCvm { get; }
    public OpenVitalSignsMonitorPanelCommandVm OpenVitalSignsMonitorPanelCvm { get; }
}
