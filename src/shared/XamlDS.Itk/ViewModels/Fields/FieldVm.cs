namespace XamlDS.Itk.ViewModels.Fields;

public abstract class FieldVm : ViewModelBase
{
    private readonly string _name;
    private string _label = string.Empty;

    protected FieldVm(string name)
    {
        _name = name;
    }

    public string Name => _name;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }
}
