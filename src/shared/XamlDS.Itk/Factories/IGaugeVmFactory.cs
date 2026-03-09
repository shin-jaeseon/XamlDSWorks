using XamlDS.Itk.Formatters;
using XamlDS.Itk.Operators;
using XamlDS.Itk.Units;
using XamlDS.Itk.ViewModels.Gauges;

namespace XamlDS.Itk.Factories;

public interface IGaugeVmFactory
{
    GaugeVm Create(string name, NumericUnit unit, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);

    GaugeVm CreateFrequencyHertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateFrequencyKilohertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateFrequencyMegahertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateFrequencyGigahertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);

    GaugeVm CreateTemperatureCelsius(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateTemperatureFahrenheit(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateTemperatureKelvin(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);

    GaugeVm CreateVoltageKilovolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateVoltageMicrovolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateVoltageMillivolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
    GaugeVm CreateVoltageVolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null);
}
