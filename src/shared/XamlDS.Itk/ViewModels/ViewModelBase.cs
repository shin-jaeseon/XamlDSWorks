// <summary>
// Base class for all ViewModel classes, implementing INotifyPropertyChanged.
// Provides methods for property change notification and value setting.
// </summary>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using XamlDS.Itk.ViewModels.ExtendedProperties;

namespace XamlDS.Itk.ViewModels;


/// <summary>
/// Base class for all ViewModel classes, implementing INotifyPropertyChanged.
/// Provides methods for property change notification and value setting,
/// compatible with Avalonia, Wpf, and WinUI3.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
{
    private bool _disposed;
    private List<ExtendedViewModelProperty>? _extendedProperties;

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets the property and notifies listeners if the value changes.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="field">The backing field of the property.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="propertyName">The name of the property (optional).</param>
    /// <returns>True if the value was changed, false otherwise.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Registers an extended property for automatic disposal.
    /// Internal use only - called by ExtendedProperty classes.
    /// Raises PropertyChanged event with "Ex_{PropertyName}" format for View CodeBehind usage.
    /// </summary>
    internal void RegisterExtendedProperty(ExtendedViewModelProperty property)
    {
        _extendedProperties ??= new List<ExtendedViewModelProperty>();
        if (!_extendedProperties.Contains(property))
        {
            _extendedProperties.Add(property);
        }

        // Notify View CodeBehind that an extended property has been registered
        this.OnPropertyChanged(string.Format("Ex_{0}", property.PropertyName));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Dispose all registered extended properties
            if (_extendedProperties != null)
            {
                foreach (var prop in _extendedProperties)
                {
                    prop.Dispose();
                }
                _extendedProperties.Clear();
                _extendedProperties = null;
            }
        }

        _disposed = true;
    }

    ~ViewModelBase()
    {
        Dispose(false);
    }
}
