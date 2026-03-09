using XamlDS.Itk.ViewModels;

namespace XamlDS.Showcases.ItkControls.ViewModels.NumericFields;

public class NumericFieldsPanelVm : ViewModelBase
{
    private NumericDemo1PaneVm _powerGeneratorNF1Pane;
    public NumericFieldsPanelVm(NumericDemo1PaneVm numericDemo1Pane)
    {
        _powerGeneratorNF1Pane = numericDemo1Pane;
    }

    public NumericDemo1PaneVm NumericDemo1Pane => _powerGeneratorNF1Pane;
}
