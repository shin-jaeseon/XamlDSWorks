using System.Collections.ObjectModel;

namespace XamlDS.Itk.ViewModels;

/// <summary>
/// Base class for panel view models that contain multiple children.
/// </summary>
/// <typeparam name="T">The type of child view models.</typeparam>
public abstract class ContentsPanelVm<T> : PanelVm where T : class
{
    private readonly ObservableCollection<T> _children = new ObservableCollection<T>();
    private readonly ReadOnlyObservableCollection<T> _readonlyChildren;

    public ContentsPanelVm()
    {
        _readonlyChildren = new ReadOnlyObservableCollection<T>(_children);
    }

    public ReadOnlyObservableCollection<T> Children => _readonlyChildren;

    protected void AddChild(T child)
    {
        _children.Add(child);
    }

    protected void RemoveChild(T child)
    {
        _children.Remove(child);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var child in _children)
            {
                if (child is IDisposable disposableChild)
                {
                    disposableChild.Dispose();
                }
            }
        }
        base.Dispose(disposing);
    }
}
