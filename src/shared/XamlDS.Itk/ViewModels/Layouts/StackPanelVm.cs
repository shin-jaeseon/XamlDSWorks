using XamlDS.Itk.Enums;

namespace XamlDS.Itk.ViewModels.Layouts;

/// <summary>
/// Panel view model that arranges children in a stack layout.
/// </summary>
public class StackPanelVm : ContentsPanelVm<ViewModelBase>
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
