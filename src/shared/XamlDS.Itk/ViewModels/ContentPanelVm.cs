namespace XamlDS.Itk.ViewModels;

public enum ContentPanelStyle
{
    Default,
    Border,
    Card
}

public class ContentPanelVm : ViewModelBase
{
    private double _width = double.NaN;
    private double _height = double.NaN;
    private ContentPanelStyle _style = ContentPanelStyle.Default;
    private object? _content;

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
    public ContentPanelStyle Style
    {
        get => _style;
        set => SetProperty(ref _style, value);
    }

    public object? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }
}
