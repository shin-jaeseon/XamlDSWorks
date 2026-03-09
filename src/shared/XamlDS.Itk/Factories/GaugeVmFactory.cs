using XamlDS.Itk.Formatters;
using XamlDS.Itk.Operators;
using XamlDS.Itk.Units;
using XamlDS.Itk.ViewModels.Gauges;

namespace XamlDS.Itk.Factories;

public class GaugeVmFactory : IGaugeVmFactory
{
    public GaugeVm Create(string name, NumericUnit unit, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, unit, formatter, precisionOperator);
    }

    public GaugeVm CreateFrequencyHertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Frequency_Hertz, formatter, precisionOperator);
    }

    public GaugeVm CreateFrequencyKilohertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Frequency_Kilohertz, formatter, precisionOperator);
    }
    public GaugeVm CreateFrequencyMegahertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Frequency_Megahertz, formatter, precisionOperator);
    }

    public GaugeVm CreateFrequencyGigahertz(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Frequency_Gigahertz, formatter, precisionOperator);
    }

    public GaugeVm CreateTemperatureCelsius(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Temperature_Celsius, formatter, precisionOperator);
    }

    public GaugeVm CreateTemperatureFahrenheit(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Temperature_Fahrenheit, formatter, precisionOperator);
    }

    public GaugeVm CreateTemperatureKelvin(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Temperature_Kelvin, formatter, precisionOperator);
    }

    public GaugeVm CreateVoltageKilovolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Voltage_Kilovolt, formatter, precisionOperator);
    }

    public GaugeVm CreateVoltageMicrovolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Voltage_Microvolt, formatter, precisionOperator);
    }

    public GaugeVm CreateVoltageMillivolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Voltage_Millivolt, formatter, precisionOperator);
    }

    public GaugeVm CreateVoltageVolt(string name, IValueFormatter<double>? formatter = null, IPrecisionOperator? precisionOperator = null)
    {
        return new GaugeVm(name, NumericUnit.Voltage_Volt, formatter, precisionOperator);
    }
}
