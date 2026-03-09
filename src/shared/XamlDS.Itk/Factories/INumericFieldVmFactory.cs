using System.Numerics;
using XamlDS.Itk.Formatters;
using XamlDS.Itk.Units;
using XamlDS.Itk.ViewModels.Fields;

namespace XamlDS.Itk.Factories;

public interface INumericFieldVmFactory
{
    /// <summary>
    /// Create a ViewModel configured for integer input
    /// </summary>
    NumericFieldVm<int> CreateInteger(string name, NumericUnit? unit = null, IValueFormatter<int>? formatter = null);

    /// <summary>
    /// Create a ViewModel configured for percentage input
    /// </summary>
    NumericFieldVm<T> CreatePercentage<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;

    NumericFieldVm<T> CreateTemperatureCelsius<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
    NumericFieldVm<T> CreateTemperatureFahrenheit<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
    NumericFieldVm<T> CreateTemperatureKelvin<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;

    NumericFieldVm<T> CreateVoltageVolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
    NumericFieldVm<T> CreateVoltageMillivolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
    NumericFieldVm<T> CreateVoltageKilovolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
    NumericFieldVm<T> CreateVoltageMicrovolt<T>(string name, IValueFormatter<T>? formatter = null) where T : INumber<T>, IComparable<T>, IEquatable<T>;
}
