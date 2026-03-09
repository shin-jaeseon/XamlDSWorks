using XamlDS.Itk.Factories;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Showcases.ItkControls.ViewModels.NumericFields;

public class NumericDemo1PaneVm : ViewModelBase
{
    private readonly INumericFieldVmFactory _numericFieldVmFactory;

    public NumericDemo1PaneVm(INumericFieldVmFactory numericFieldVmFactory)
    {
        _numericFieldVmFactory = numericFieldVmFactory;
    }
}
