namespace XamlDS.Itk.ViewModels.Inputs;

public abstract class InputVm : ViewModelBase
{
    private readonly string _name;
    private string _label = string.Empty;
    private string? _errorMessage;
    private bool _hasError;
    private bool _isDirty;

    protected InputVm(string name)
    {
        _name = name;
    }

    public string Name => _name;

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => SetProperty(ref _isDirty, value);
    }

    public abstract bool Validate();
    public abstract void Reset();
}
