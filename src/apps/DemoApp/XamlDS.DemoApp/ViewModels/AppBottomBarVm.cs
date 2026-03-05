using XamlDS.DemoApp.ViewModels.Commands;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Commands;

namespace XamlDS.DemoApp.ViewModels;

public sealed class AppBottomBarVm : ViewModelBase
{

    public AppBottomBarVm(LanguageSettingsVm languageSettings,
        QuitApplicationCommandVm quitApplicationCvm,
        ToggleFullscreenWindowCommandVm toggleFullscreenWindowCvm,
        OpenHomePanelCommandVm openHomePanelCvm,
        ThemeSettingsVm themeSettings)
    {
        LanguageSettings = languageSettings;
        QuitApplicationCvm = quitApplicationCvm;
        ToggleFullscreenWindowCvm = toggleFullscreenWindowCvm;
        OpenHomePanelCvm = openHomePanelCvm;
        ThemeSettings = themeSettings;
    }

    public LanguageSettingsVm LanguageSettings { get; }

    public QuitApplicationCommandVm QuitApplicationCvm { get; }

    public OpenHomePanelCommandVm OpenHomePanelCvm { get; }

    public ToggleFullscreenWindowCommandVm ToggleFullscreenWindowCvm { get; }

    public ThemeSettingsVm ThemeSettings { get; }
}
