using System.Collections.ObjectModel;
using System.Globalization;
using XamlDS.Itk.Messages;
using XamlDS.Services;
using XamlDS.ViewModels;

namespace XamlDS.Itk.ViewModels;

public sealed class LanguageSettingsVm : ViewModelBase
{
    private readonly IMessenger _messenger;
    private CultureInfo? _currentLanguage;

    private readonly ObservableCollection<CultureInfo> _availableLanguages = new ObservableCollection<CultureInfo>();
    private readonly ReadOnlyObservableCollection<CultureInfo> _availableLanguagesReadOnly;
    public ReadOnlyObservableCollection<CultureInfo> AvailableLanguages => _availableLanguagesReadOnly;

    public LanguageSettingsVm(IMessenger messenger)
    {
        _messenger = messenger;
        _availableLanguagesReadOnly = new ReadOnlyObservableCollection<CultureInfo>(_availableLanguages);
    }

    /// <summary>
    /// Gets or sets the current language.
    /// Must be one of the cultures in AvailableLanguages.
    /// Setting this property will apply the culture to the current thread.
    /// </summary>
    public CultureInfo? CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!_availableLanguages.Any(c => c.Name.Equals(value.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"The culture '{value.Name}' is not in AvailableLanguages.");

            var oldLanguage = _currentLanguage;
            if (SetProperty(ref _currentLanguage, value))
            {
                ApplyLanguage(oldLanguage, value);
            }
        }
    }

    /// <summary>
    /// Adds a culture to the list of available languages.
    /// If this is the first language added and CurrentLanguage is null, it will be set automatically.
    /// </summary>
    public void AddLanguage(CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);

        if (_availableLanguages.Any(c => c.Name.Equals(culture.Name, StringComparison.OrdinalIgnoreCase)))
            return;
        _availableLanguages.Add(culture);
        if (CurrentLanguage == null)
            CurrentLanguage = culture;
    }

    /// <summary>
    /// Adds a culture to the list of available languages.
    /// </summary>
    /// <param name="cultureName">The culture name (e.g., "en-US", "ko-KR")</param>
    /// <exception cref="CultureNotFoundException">Thrown when the culture name is invalid.</exception>
    public void AddLanguage(string cultureName)
    {
        ArgumentNullException.ThrowIfNull(cultureName);
        AddLanguage(new CultureInfo(cultureName));
    }

    private void ApplyLanguage(CultureInfo? oldCulture, CultureInfo newCulture)
    {
        if (newCulture == null)
            return;

        if (Thread.CurrentThread.CurrentCulture.Name == newCulture.Name)
            return;

        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
        CultureInfo.DefaultThreadCurrentCulture = newCulture;
        CultureInfo.DefaultThreadCurrentUICulture = newCulture;

        _messenger.Send(new LanguageChangedMessage(oldCulture, newCulture));
    }
}
