using System.Numerics;
using XamlDS.Itk.Formatters;
using XamlDS.Itk.Units;
using XamlDS.Itk.ViewModels.Fields;

namespace XamlDS.Itk.Factories;

public class NumericFieldVmFactory : INumericFieldVmFactory
{
    public NumericFieldVm<int> CreateInteger(string name, NumericUnit? unit = null, IValueFormatter<int>? formatter = null)
    {
        return new NumericFieldVm<int>(name, unit, formatter);
    }

    public NumericFieldVm<T> CreatePercentage<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Percentage, formatter);
    }

    public NumericFieldVm<T> CreateTemperatureCelsius<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Temperature_Celsius, formatter);
    }

    public NumericFieldVm<T> CreateTemperatureFahrenheit<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Temperature_Fahrenheit, formatter);
    }

    public NumericFieldVm<T> CreateTemperatureKelvin<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Temperature_Kelvin, formatter);
    }

    public NumericFieldVm<T> CreateVoltageKilovolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Voltage_Kilovolt, formatter);
    }

    public NumericFieldVm<T> CreateVoltageMicrovolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Voltage_Microvolt, formatter);
    }

    public NumericFieldVm<T> CreateVoltageMillivolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Voltage_Millivolt, formatter);
    }

    public NumericFieldVm<T> CreateVoltageVolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>
    {
        return new NumericFieldVm<T>(name, NumericUnit.Voltage_Volt, formatter);
    }
}
