using XamlDS.Itk.ViewModels;

namespace XamlDS.Showcases.ItkControls.ViewModels.TextFields;

public class TextFieldsPanelVm : ViewModelBase
{
    private PowerGeneratorPaneVm _powerGeneratorPane;

    public TextFieldsPanelVm(PowerGeneratorPaneVm powerGeneratorPane)
    {
        _powerGeneratorPane = powerGeneratorPane;
    }

    public PowerGeneratorPaneVm PowerGeneratorPane => _powerGeneratorPane;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _powerGeneratorPane?.Dispose();
        }
        base.Dispose(disposing);
    }
}
