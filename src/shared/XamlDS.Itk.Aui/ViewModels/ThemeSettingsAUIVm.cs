using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using XamlDS.Services;

namespace XamlDS.Itk.ViewModels;

public class ThemeSettingsAuiVm : ThemeSettingsVm
{
    private IMessenger _messenger;
    private const string BaseUri = "avares://XamlDS.Itk.Aui";

    public ThemeSettingsAuiVm(IMessenger messenger)
    {
        _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

        //var subscription = _messenger.Subscribe<LanguageChangedMessage>(msg =>
        //{
        //    var app = Application.Current;
        //    RemoveItkThemeStyles(app);
        //    LoadItkThemeStyles(app, this.CurrentTheme);
        //});
    }

    protected override void ApplyTheme(string themeName)
    {
        var app = Application.Current;
        if (app == null) return;

        RemoveItkThemeStyles(app);
        LoadItkThemeStyles(app, themeName);
    }

    /// <summary>
    /// Remove all Itk theme styles from application
    /// </summary>
    /// <remarks>
    /// Note: When StyleInclude loads a Styles.axaml file whose root element is Styles,
    /// the actual object added to app.Styles collection is the Styles object itself,
    /// not the StyleInclude wrapper. We need to handle both cases.
    /// </remarks>
    private static void RemoveItkThemeStyles(Application app)
    {
        for (int i = app.Styles.Count - 1; i >= 0; i--)
        {
            var style = app.Styles[i];

            if (style is StyleInclude styleInclude)
            {
                styleInclude.TryGetResource("IsItkTheme", null, out var isItkTheme);
                if (isItkTheme is bool isItk && isItk)
                {
                    app.Styles.RemoveAt(i);
                    return;
                }
            }
            else if (style is Styles styles)
            {
                if (styles.Resources.ContainsKey("IsItkTheme"))
                {
                    app.Styles.RemoveAt(i);
                    return;
                }
            }
        }
    }

    private static void LoadItkThemeStyles(Application app, string themeName)
    {
        var styleInclude = new StyleInclude(new Uri(BaseUri))
        {
            Source = new Uri($"{BaseUri}/Styles/{themeName}Styles.axaml")
        };
        app.Styles.Add(styleInclude);
    }
}
