namespace XamlDS.Itk.ViewModels.ExtendedProperties;

/// <summary>
/// Base class for extended ViewModel properties (similar to WPF/Avalonia Attached Properties).
/// Extended properties allow attaching additional data to ViewModels without modifying their structure.
/// </summary>
public abstract class ExtendedViewModelProperty : IDisposable
{
    protected bool _disposed;

    /// <summary>
    /// Gets the unique name of this property.
    /// </summary>
    public abstract string PropertyName { get; }

    /// <summary>
    /// Disposes resources held by this property.
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
