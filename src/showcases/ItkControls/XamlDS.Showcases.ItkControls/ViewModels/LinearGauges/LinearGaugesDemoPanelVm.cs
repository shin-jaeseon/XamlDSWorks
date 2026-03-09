using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Fields;
using XamlDS.Itk.ViewModels.Gauges;
using XamlDS.Itk.ViewModels.Panels;

namespace XamlDS.Showcases.ItkControls.ViewModels.LinearGauges;

public class LinearGaugesDemoPanelVm : ViewModelBase
{
    private readonly PowerGeneratorDemoVm _demo1;
    private readonly PanelVm<ViewModelBase> _generatorPanel;

    public LinearGaugesDemoPanelVm(PowerGeneratorDemoVm demo1)
    {
        _demo1 = demo1;

        _generatorPanel = new PanelVm<ViewModelBase>()
        {
            Label = "Demo Generator",
            Layout = PanelLayout.Vertical()
        };

        var electricalPanel = new TextGaugesPanelVm()
        {
            Label = "ELECTRICAL",
            ShowGaugeLabel = true,
            GaugeStyle = GaugeStyle.Large,
            Layout = PanelLayout.Vertical()
        };
        electricalPanel.Children.Add(_demo1.OutputVoltage);
        // electricalPanel.Children.Add(_demo1.OutputcURRENT);
        electricalPanel.Children.Add(_demo1.FrequencyStatus);
        // electricalPanel.Children.Add(_demo1.ActivePower);
        // electricalPanel.Children.Add(_demo1.ReactivePower);
        _generatorPanel.Children.Add(electricalPanel);

        var enginePanel = new TextGaugesPanelVm()
        {
            Label = "ENGINE",
            ShowGaugeLabel = true,
            GaugeStyle = GaugeStyle.Large,
            Layout = PanelLayout.Vertical()
        };
        // enginePanel.Children.Add(_demo1.EngineSpeed);
        // enginePanel.Children.Add(_demo1.EngineTemperature);
        // enginePanel.Children.Add(_demo1.OilPressure);
        _generatorPanel.Children.Add(enginePanel);

        var statusPanel = new FieldsPanelVm()
        {
            Label = "STATUS",
            Layout = PanelLayout.Vertical()
        };
        // statusPanel.Children.Add(_demo1.FuelLevel);
        // statusPanel.Children.Add(_demo1.RunningHours);
        _generatorPanel.Children.Add(statusPanel);

    }

    public PanelVm<ViewModelBase> GeneratorPanel { get => _generatorPanel; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _demo1.Dispose();
        }
        base.Dispose(disposing);
    }
}
