using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Itk;

/// <summary>
/// DataTemplateSelector that manages all ViewModel→View mappings.
/// Supports both type-specific and conditional template registration.
/// </summary>
public sealed class ItkTemplateSelector : DataTemplateSelector
{
    // Singleton instance for XAML reference
    public static ItkTemplateSelector Instance { get; } = new();

    private static readonly Dictionary<Type, DataTemplate> _templatesByVmType = new();
    private static readonly List<(Func<object, bool> Match, DataTemplate Template)> _conditionalTemplates = new();

    /// <summary>
    /// Register a DataTemplate that maps a ViewModel type to a View type.
    /// Uses XamlReader to create template with proper resource inheritance.
    /// </summary>
    public static void Add<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : FrameworkElement, new()
    {
        var vmType = typeof(TViewModel);
        var viewType = typeof(TView);

        if (_templatesByVmType.ContainsKey(vmType))
        {
            Debug.WriteLine($"[ItkDataTemplates] Overriding DataTemplate for {vmType.Name}");
        }

        // Create DataTemplate using XAML string for proper resource inheritance
        var assemblyName = viewType.Assembly.GetName().Name;
        var xaml = $@"
            <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                          xmlns:view='clr-namespace:{viewType.Namespace};assembly={assemblyName}'>
                <view:{viewType.Name} />
            </DataTemplate>";

        var template = (DataTemplate)XamlReader.Parse(xaml);
        _templatesByVmType[vmType] = template;
    }

    /// <summary>
    /// Register a conditional DataTemplate with type check.
    /// Conditional templates have lower priority than type-specific templates.
    /// Uses XamlReader to create template with proper resource inheritance.
    /// </summary>
    public static void AddConditional<TViewModel, TView>()
        where TViewModel : class
        where TView : FrameworkElement, new()
    {
        var viewType = typeof(TView);

        // Create DataTemplate using XAML string for proper resource inheritance
        var assemblyName = viewType.Assembly.GetName().Name;
        var xaml = $@"
            <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                          xmlns:view='clr-namespace:{viewType.Namespace};assembly={assemblyName}'>
                <view:{viewType.Name} />
            </DataTemplate>";

        var template = (DataTemplate)XamlReader.Parse(xaml);
        _conditionalTemplates.Add((item => item is TViewModel, template));
    }

    /// <summary>
    /// SelectTemplate override: searches in order of priority.
    /// 1. Exact type match in _templatesByVmType
    /// 2. Conditional templates in _conditionalTemplates
    /// 3. Base class implementation
    /// </summary>
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item == null)
            return base.SelectTemplate(item, container);

        var itemType = item.GetType();

        // 1. Try exact type match (highest priority)
        if (_templatesByVmType.TryGetValue(itemType, out var template))
            return template;

        // 2. Try conditional templates (lower priority)
        foreach (var (match, conditionalTemplate) in _conditionalTemplates)
        {
            if (match(item))
                return conditionalTemplate;
        }

        // 3. Fallback to base implementation
        return base.SelectTemplate(item, container);
    }

    /// <summary>
    /// Clear all registered DataTemplates.
    /// </summary>
    public static void Clear()
    {
        _templatesByVmType.Clear();
        _conditionalTemplates.Clear();
    }

    /// <summary>
    /// Get readonly view of registered templates for diagnostics.
    /// </summary>
    public static IReadOnlyDictionary<Type, DataTemplate> GetRegisteredTemplates()
        => _templatesByVmType;
}
