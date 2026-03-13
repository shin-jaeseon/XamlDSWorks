using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Diagnostics;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Itk;

/// <summary>
/// Central registry for DataTemplates used in the Itk framework.
/// Supports both type-specific and conditional DataTemplate registration.
/// </summary>
public static class ItkDataTemplates
{
    private static readonly Dictionary<Type, IDataTemplate> _templatesByVmType = new();
    private static readonly List<IDataTemplate> _conditionalTemplates = new();

    /// <summary>
    /// Register a DataTemplate that maps a ViewModel type to a View type.
    /// If a template for the same ViewModel already exists, it will be replaced with a debug warning.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel type</typeparam>
    /// <typeparam name="TView">The View type (must have parameterless constructor)</typeparam>
    public static void Add<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : Control, new()
    {
        var vmType = typeof(TViewModel);

        if (_templatesByVmType.ContainsKey(vmType))
        {
            Debug.WriteLine($"[ItkDataTemplates] Overriding DataTemplate for {vmType.Name}");
        }

        _templatesByVmType[vmType] = new FuncDataTemplate<TViewModel>((vm, scope) => new TView());
    }

    /// <summary>
    /// Register a conditional DataTemplate with type check.
    /// Conditional templates have lower priority than type-specific templates.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel base type or interface</typeparam>
    /// <typeparam name="TView">The View type (must have parameterless constructor)</typeparam>
    public static void AddConditional<TViewModel, TView>()
        where TViewModel : class
        where TView : Control, new()
    {
        _conditionalTemplates.Add(
            new FuncDataTemplate<TViewModel>((vm, scope) => new TView())
        );
    }

    /// <summary>
    /// Apply all registered DataTemplates to the Application.
    /// Type-specific templates are added first (higher priority),
    /// followed by conditional templates (lower priority).
    /// </summary>
    public static void ApplyTo(Application app)
    {
        // Type-specific templates first (higher priority)
        foreach (var template in _templatesByVmType.Values)
            app.DataTemplates.Add(template);

        // Conditional templates last (lower priority)
        foreach (var template in _conditionalTemplates)
            app.DataTemplates.Add(template);
    }

    /// <summary>
    /// Clear all registered DataTemplates. Useful for testing.
    /// </summary>
    public static void Clear()
    {
        _templatesByVmType.Clear();
        _conditionalTemplates.Clear();
    }

    /// <summary>
    /// Get readonly view of registered type-specific templates for diagnostics.
    /// </summary>
    public static IReadOnlyDictionary<Type, IDataTemplate> GetRegisteredTemplates()
        => _templatesByVmType;
}
