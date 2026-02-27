using System;
using System.Collections.Generic;
using System.Text;
using XamlDS.DemoApp.ViewModels.Commands;
using XamlDS.ViewModels;

namespace XamlDS.DemoApp.ViewModels;

public class HomePanelVm : ViewModelBase
{
    public HomePanelVm(OpenItkControlsMainPanelCommandVm openItkControlsMainPanelCommandVm, OpenItkThemesMainPanelCommandVm openItkThemesMainPanelCommandVm)
    {
        OpenItkControlsMainPanelCvm = openItkControlsMainPanelCommandVm;
        OpenItkThemesMainPanelCvm = openItkThemesMainPanelCommandVm;
    }

    public OpenItkThemesMainPanelCommandVm OpenItkThemesMainPanelCvm { get; }
    public OpenItkControlsMainPanelCommandVm OpenItkControlsMainPanelCvm { get; }
}
