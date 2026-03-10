namespace XamlDS.Itk.ViewModels.Panes;

public class ContentPaneVm : ViewModelBase
{
    private object? _content;

    public object? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }
}
