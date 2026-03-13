namespace XamlDS.Itk.ViewModels;

/// <summary>
/// Base class for all panel view models.
/// </summary>
public abstract class PanelVm : ViewModelBase
{
    private double _width = double.NaN;
    private double _height = double.NaN;

    public double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    public double Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }
}
