namespace XamlDS.Itk.ViewModels.Panels;

public class MockPanelVm : ViewModelBase
{
    private string _label = string.Empty;
    private double _width = double.NaN;
    private double _height = double.NaN;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

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
