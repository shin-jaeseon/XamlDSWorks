namespace XamlDS.Itk.ViewModels.Panes;



public class TextPaneVm : ViewModelBase
{
    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set
        {
            SetProperty(ref _text, value);
        }
    }
}
