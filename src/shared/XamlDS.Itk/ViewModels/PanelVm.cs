using System.Collections.ObjectModel;

namespace XamlDS.Itk.ViewModels;

public abstract class PanelVm<T> : ViewModelBase where T : class
{
    private readonly ObservableCollection<T> _children = new ObservableCollection<T>();
    private readonly ReadOnlyObservableCollection<T> _readonlyChildren;

    public PanelVm()
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
