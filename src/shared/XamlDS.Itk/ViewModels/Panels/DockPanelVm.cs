using XamlDS.Itk.ViewModels.ExtendedProperties;

namespace XamlDS.Itk.ViewModels.Panels;


public class DockPanelVm : PanelVm<ViewModelBase>
{
    public DockPanelVm Add(ViewModelBase child, DockPositon dock = DockPositon.Left)
    {
        DockProperty.Set(this, child, dock);
        base.AddChild(child);
        return this;
    }

    public DockPanelVm AddTop(ViewModelBase child) => Add(child, DockPositon.Top);
    public DockPanelVm AddBottom(ViewModelBase child) => Add(child, DockPositon.Bottom);
    public DockPanelVm AddLeft(ViewModelBase child) => Add(child, DockPositon.Left);
    public DockPanelVm AddRight(ViewModelBase child) => Add(child, DockPositon.Right);

    public void Remove(ViewModelBase child)
    {
        DockProperty.Clear(this, child);
        base.RemoveChild(child);
    }

    public void SetDock(ViewModelBase child, DockPositon dock)
    {
        if (!Children.Contains(child))
            throw new ArgumentException("Child is not in this panel", nameof(child));

        DockProperty.Set(this, child, dock);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DockProperty.ClearAll(this);  // 전체 정리
        }
        base.Dispose(disposing);
    }
}
