namespace XamlDS.Itk.Units;

/// <summary>
/// Provides extension methods for NumericUnit enum.
/// </summary>
public static class NumericUnitExtensions
{
    /// <summary>
    /// Converts a NumericUnit value to its corresponding unit string representation.
    /// </summary>
    /// <param name="unit">The NumericUnit value to convert.</param>
    /// <returns>The unit string representation (e.g., "mm", "kg", "°C").</returns>
    public static string ToUnitString(this NumericUnit unit)
    {
        return unit switch
        {
            NumericUnit.Percentage => "%",

            // Frequency units
            NumericUnit.Frequency_Hertz => "Hz",
            NumericUnit.Frequency_Kilohertz => "kHz",
            NumericUnit.Frequency_Megahertz => "MHz",
            NumericUnit.Frequency_Gigahertz => "GHz",

            // Length units
            NumericUnit.Length_Millimeter => "mm",
            NumericUnit.Length_Centimeter => "cm",
            NumericUnit.Length_Meter => "m",
            NumericUnit.Length_Kilometer => "km",
            NumericUnit.Length_Inch => "in",
            NumericUnit.Length_Foot => "ft",
            NumericUnit.Length_Yard => "yd",
            NumericUnit.Length_Mile => "mi",
            NumericUnit.Length_NauticalMile => "nmi",

            // Mass units
            NumericUnit.Mass_Milligram => "mg",
            NumericUnit.Mass_Gram => "g",
            NumericUnit.Mass_Kilogram => "kg",
            NumericUnit.Mass_Tonne => "t",
            NumericUnit.Mass_Pound => "lb",
            NumericUnit.Mass_Ounce => "oz",

            // Pressure units
            NumericUnit.Pressure_Atmosphere => "atm",
            NumericUnit.Pressure_bar => "bar",
            NumericUnit.Pressure_kgfcm2 => "kgf/cm²",
            NumericUnit.Pressure_kPa => "kPa",
            NumericUnit.Pressure_MPa => "MPa",
            NumericUnit.Pressure_Pascal => "Pa",
            NumericUnit.Pressure_psi => "psi",
            NumericUnit.Pressure_Torr => "Torr",

            // Temperature units
            NumericUnit.Temperature_Celsius => "°C",
            NumericUnit.Temperature_Fahrenheit => "°F",
            NumericUnit.Temperature_Kelvin => "K",

            // Voltage units
            NumericUnit.Voltage_Volt => "V",
            NumericUnit.Voltage_Millivolt => "mV",
            NumericUnit.Voltage_Kilovolt => "kV",
            NumericUnit.Voltage_Microvolt => "µV",

            // Volume units
            NumericUnit.Volume_Liter => "L",
            NumericUnit.Volume_Milliliter => "mL",
            NumericUnit.Volume_CubicMeter => "m³",
            NumericUnit.Volume_CubicCentimeter => "cm³",

            // Default
            NumericUnit.None => string.Empty,
            _ => string.Empty
        };
    }
}
