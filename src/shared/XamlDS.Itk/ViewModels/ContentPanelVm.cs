namespace XamlDS.Itk.ViewModels;

public enum ContentPanelStyle
{
    Default,
    Bordered
}

/// <summary>
/// Panel view model that contains a single content.
/// </summary>
public class ContentPanelVm : PanelVm
{
    private ContentPanelStyle _panelStyle = ContentPanelStyle.Default;
    private object? _content;

    public ContentPanelStyle PanelStyle
    {
        get => _panelStyle;
        set => SetProperty(ref _panelStyle, value);
    }

    public object? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }
}
