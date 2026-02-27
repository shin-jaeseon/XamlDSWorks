using System;
using System.Collections.Generic;
using System.Text;
using XamlDS.Itk.ResourceKeys;
using XamlDS.ViewModels;

namespace XamlDS.Itk.ViewModels;

public abstract class ThemeSettingsVm : ViewModelBase
{
    private IEnumerable<string> _availableThemes = new[] { ThemeNames.ItkLight, ThemeNames.ItkDark, ThemeNames.ItkGreenPunk, ThemeNames.ItkBluePunk };
    private string _currentTheme = string.Empty;

    protected ThemeSettingsVm()
    {
        CurrentTheme = ThemeNames.ItkDark;
    }
    public IEnumerable<string> AvailableThemes => _availableThemes;

    public string CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (SetProperty(ref _currentTheme, value))
            {
                ApplyTheme(value);
            }
        }
    }

    protected abstract void ApplyTheme(string themeName);
}
