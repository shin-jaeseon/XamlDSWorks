using XamlDS.Itk.Enums;

namespace XamlDS.Itk.ViewModels.Panels;

public class StackPanelVm : PanelVm<ViewModelBase>
{
    private ItkOrientation _orientation = ItkOrientation.Vertical;

    public ItkOrientation Orientation
    {
        get => _orientation;
        set => SetProperty(ref _orientation, value);
    }

    public StackPanelVm Add(ViewModelBase child)
    {
        base.AddChild(child);
        return this;
    }

    public void Remove(ViewModelBase child)
    {
        base.RemoveChild(child);
    }
}
