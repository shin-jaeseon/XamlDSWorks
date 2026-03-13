namespace XamlDS.Itk.ViewModels;

public enum ContentPanelStyle
{
    Default,
    Border,
    Card
}

/// <summary>
/// Panel view model that contains a single content.
/// </summary>
public class ContentPanelVm : PanelVm
{
    private ContentPanelStyle _style = ContentPanelStyle.Default;
    private object? _content;

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
