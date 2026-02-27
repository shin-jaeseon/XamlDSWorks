using XamlDS.Itk.ViewModels;
using XamlDS.ViewModels;

namespace XamlDS.DemoApp.ViewModels;

public sealed class AppWindowVm : ViewModelBase
{
    private readonly LanguageSettingsVm _languageSettings;

    public AppWindowVm(LanguageSettingsVm languageSettings, AppBottomBarVm appBottomBar, AppMainPanelVm appMainPanel, HomePanelVm panel)
    {
        _languageSettings = languageSettings;

        _languageSettings.AddLanguage("en-US");
        _languageSettings.AddLanguage("ko-KR");
        _languageSettings.AddLanguage("ja-JP");
        _languageSettings.AddLanguage("zh-CN");
        _languageSettings.AddLanguage("zh-TW");
        _languageSettings.AddLanguage("ar");
        _languageSettings.CurrentLanguage = _languageSettings.AvailableLanguages.FirstOrDefault();


        AppBottomBar = appBottomBar;
        AppMainPanel = appMainPanel;
        AppMainPanel.CurrentPanelName = "Home";
    }

    public AppBottomBarVm AppBottomBar { get; }
    public AppMainPanelVm AppMainPanel { get; }
}
