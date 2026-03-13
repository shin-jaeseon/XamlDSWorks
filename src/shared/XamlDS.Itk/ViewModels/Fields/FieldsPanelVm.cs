namespace XamlDS.Itk.ViewModels.Fields;


public enum FieldStyle
{
    Small,
    Medium,
    Large
}

/// <summary>
/// Panel view model for displaying field controls.
/// </summary>
public class FieldsPanelVm : ContentsPanelVm<FieldVm>
{
    private bool _showFieldLabel = true;
    private FieldStyle _fieldStyle = FieldStyle.Medium;

    public bool ShowFieldLabel
    {
        get => _showFieldLabel;
        set => SetProperty(ref _showFieldLabel, value);
    }

    public FieldStyle FieldStyle
    {
        get => _fieldStyle;
        set => SetProperty(ref _fieldStyle, value);
    }
}
