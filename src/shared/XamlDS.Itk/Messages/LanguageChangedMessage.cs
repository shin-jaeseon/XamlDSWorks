using System.Globalization;

namespace XamlDS.Itk.Messages;

public sealed record LanguageChangedMessage(CultureInfo? OldLanguage, CultureInfo NewLanguage);
