using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Itk;


/// <summary>
/// Locates and creates the appropriate View for a given ViewModel.
/// Supports type-based mappings, factory functions, and conditional mappings.
/// </summary>
public static class ViewLocator
{
    private static readonly Dictionary<Type, Func<object, FrameworkElement?>> _factoriesByVmType = new();
    private static readonly Dictionary<Type, Type> _viewTypesByVmType = new();
    private static readonly List<(Func<object, bool> Match, Type ViewType)> _conditionalMappings = new();
    private static readonly Dictionary<Type, Type?> _viewTypeResultCache = new();

    /// <summary>
    /// Register a ViewModel-to-View type mapping.
    /// </summary>
    public static void Register<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : FrameworkElement, new()
    {
        var vmType = typeof(TViewModel);
        var viewType = typeof(TView);

        // Invalidate cache for this type
        if (_viewTypeResultCache.ContainsKey(vmType))
        {
            _viewTypeResultCache.Remove(vmType);
        }

        if (_viewTypesByVmType.ContainsKey(vmType))
        {
            Debug.WriteLine($"[ViewLocator] Overriding mapping for {vmType.Name} -> {viewType.Name}");
        }

        _viewTypesByVmType[vmType] = viewType;
    }

    /// <summary>
    /// Register a factory function for conditional View creation.
    /// Note: Factory functions are NOT cached as they may return different Views based on ViewModel state.
    /// </summary>
    public static void RegisterFunc<TViewModel>(Func<TViewModel, FrameworkElement?> factory)
        where TViewModel : ViewModelBase
    {
        var vmType = typeof(TViewModel);

        // Remove from cache as factory might return different results
        if (_viewTypeResultCache.ContainsKey(vmType))
        {
            _viewTypeResultCache.Remove(vmType);
        }

        if (_factoriesByVmType.ContainsKey(vmType))
        {
            Debug.WriteLine($"[ViewLocator] Overriding factory for {vmType.Name}");
        }

        _factoriesByVmType[vmType] = vm => factory((TViewModel)vm);
    }

    /// <summary>
    /// Register a conditional ViewModel-to-View mapping.
    /// </summary>
    public static void RegisterConditional<TViewModel, TView>()
        where TViewModel : class
        where TView : Control, new()
    {
        var viewType = typeof(TView);
        _viewTypeResultCache.Clear();
        _conditionalMappings.Add((item => item is TViewModel, viewType));
    }

    /// <summary>
    /// Clear all registered mappings and factories.
    /// </summary>
    public static void Clear()
    {
        _factoriesByVmType.Clear();
        _viewTypesByVmType.Clear();
        _conditionalMappings.Clear();
        _viewTypeResultCache.Clear();
    }

    /// <summary>
    /// Get readonly view of registered type mappings for diagnostics.
    /// </summary>
    public static IReadOnlyDictionary<Type, Type> GetRegisteredMappings()
        => _viewTypesByVmType;

    /// <summary>
    /// Locate and create the appropriate View for the given ViewModel.
    /// Returns null if no mapping is found.
    /// 
    /// Search priority:
    /// 1. Factory function
    /// 2. Exact type match
    /// 3. Parent class hierarchy
    /// 4. Conditional match
    /// 
    /// Results are cached for performance (except factory functions).
    /// </summary>
    public static FrameworkElement? GetView(object viewModel)
    {
        if (viewModel == null)
            return null;

        var vmType = viewModel.GetType();

        // Check cache first (except for factory functions)
        if (_viewTypeResultCache.TryGetValue(vmType, out var cachedViewType))
        {
            return cachedViewType != null ? CreateView(cachedViewType) : null;
        }

        // 1. Try factory function (highest priority)
        // Factory functions are NOT cached as they may return different Views based on state
        if (_factoriesByVmType.TryGetValue(vmType, out var factory))
        {
            Debug.WriteLine($"[ViewLocator] Using factory for {vmType.Name}");
            return factory(viewModel);  // No caching for factory results
        }

        // 2. Try exact type match
        if (_viewTypesByVmType.TryGetValue(vmType, out var viewType))
        {
            _viewTypeResultCache[vmType] = viewType;
            return CreateView(viewType);
        }

        // 3. Try parent class hierarchy
        var currentType = vmType.BaseType;
        while (currentType != null && currentType != typeof(object))
        {
            if (_viewTypesByVmType.TryGetValue(currentType, out var parentViewType))
            {
                Debug.WriteLine($"[ViewLocator] Found View for parent type: {vmType.Name} -> {currentType.Name} -> {parentViewType.Name}");
                _viewTypeResultCache[vmType] = parentViewType;
                return CreateView(parentViewType);
            }
            currentType = currentType.BaseType;
        }

        // 4. Try conditional mappings
        foreach (var (match, conditionalViewType) in _conditionalMappings)
        {
            if (match(viewModel))
            {
                _viewTypeResultCache[vmType] = conditionalViewType;
                return CreateView(conditionalViewType);
            }
        }

        // Cache "not found" result
        _viewTypeResultCache[vmType] = null;

        // Not found
        return null;
    }

    private static FrameworkElement? CreateView(Type viewType)
    {
        try
        {
            if (Activator.CreateInstance(viewType) is FrameworkElement view)
            {
                return view;
            }

            Debug.WriteLine($"[ViewLocator] Failed to create instance of {viewType.Name}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ViewLocator] Exception creating View: {ex.Message}");
            return null;
        }
    }
}
