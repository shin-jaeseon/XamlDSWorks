namespace XamlDS.Itk.Simulators.Power;

/// <summary>
/// Defines the specifications and thresholds for a power generator model.
/// </summary>
public class PowerGeneratorSpecification
{
    /// <summary>
    /// Model name or specification identifier (e.g., "Industrial 100kW 60Hz", "Cummins C100D6")
    /// </summary>
    public string Model { get; init; } = "Generic";

    /// <summary>
    /// Rated power output in kilowatts
    /// </summary>
    public double RatedPowerKw { get; init; }

    /// <summary>
    /// Rated frequency in hertz (50Hz or 60Hz)
    /// </summary>
    public double RatedFrequency { get; init; }

    #region Voltage Thresholds (V)

    public double VoltageNominal { get; init; }
    public double VoltageCriticalLow { get; init; }
    public double VoltageWarningLow { get; init; }
    public double VoltageWarningHigh { get; init; }
    public double VoltageCriticalHigh { get; init; }

    #endregion

    #region Current Thresholds (A)

    public double CurrentNominal { get; init; }
    public double CurrentWarning { get; init; }
    public double CurrentCritical { get; init; }

    #endregion

    #region Frequency Thresholds (Hz)

    public double FrequencyCriticalLow { get; init; }
    public double FrequencyWarningLow { get; init; }
    public double FrequencyWarningHigh { get; init; }
    public double FrequencyCriticalHigh { get; init; }

    #endregion

    #region Active Power Thresholds (kW)

    public double ActivePowerNominal { get; init; }
    public double ActivePowerWarning { get; init; }
    public double ActivePowerCritical { get; init; }

    #endregion

    #region Reactive Power Thresholds (kVAr)

    public double ReactivePowerNominal { get; init; }
    public double ReactivePowerWarning { get; init; }
    public double ReactivePowerCritical { get; init; }

    #endregion

    #region Engine Speed Thresholds (RPM)

    public double EngineSpeedNominal { get; init; }
    public double EngineSpeedCriticalLow { get; init; }
    public double EngineSpeedWarningLow { get; init; }
    public double EngineSpeedWarningHigh { get; init; }
    public double EngineSpeedCriticalHigh { get; init; }

    #endregion

    #region Engine Temperature Thresholds (°C)

    public double EngineTemperatureNominal { get; init; }
    public double EngineTemperatureWarning { get; init; }
    public double EngineTemperatureCritical { get; init; }

    #endregion

    #region Oil Pressure Thresholds (bar)

    public double OilPressureNominal { get; init; }
    public double OilPressureCritical { get; init; }
    public double OilPressureWarning { get; init; }

    #endregion

    #region Fuel Level Thresholds (%)

    public double FuelLevelNominal { get; init; }
    public double FuelLevelWarning { get; init; }
    public double FuelLevelCritical { get; init; }

    #endregion

    #region Pre-defined Specifications

    /// <summary>
    /// Pre-defined specification for 100kW industrial diesel generator (60Hz).
    /// <para><strong>Specification Summary:</strong></para>
    /// <list type="bullet">
    /// <item><description>Rated Voltage: 380V (3-phase)</description></item>
    /// <item><description>Rated Current: 125.5A</description></item>
    /// <item><description>Power Output: 80-100kW (100kVA class)</description></item>
    /// <item><description>Frequency: 60Hz (Korea/North America standard)</description></item>
    /// <item><description>Engine Speed: 1800 RPM (4-pole synchronous generator)</description></item>
    /// </list>
    /// <para><strong>Application &amp; Category:</strong></para>
    /// <list type="bullet">
    /// <item><description>Category: Medium-sized industrial/commercial diesel generator</description></item>
    /// <item><description>Capacity: 80-100kW (approx. 100kVA)</description></item>
    /// <item><description>Use Cases:</description>
    ///   <list type="bullet">
    ///     <item><description>Backup power for small to medium hospitals</description></item>
    ///     <item><description>UPS auxiliary for data centers</description></item>
    ///     <item><description>Emergency power for small to medium factories</description></item>
    ///     <item><description>Commercial buildings (10-20 floors)</description></item>
    ///     <item><description>Apartment complex elevators and water pumps</description></item>
    ///   </list>
    /// </item>
    /// </list>
    /// <para><strong>Real Product Examples:</strong></para>
    /// <list type="bullet">
    /// <item><description>Cummins C100D6 (100kVA)</description></item>
    /// <item><description>Caterpillar DE88E0 (88kW)</description></item>
    /// <item><description>Perkins 404A-22G1 (80kW)</description></item>
    /// </list>
    /// </summary>
    public static PowerGeneratorSpecification Industrial100kW60Hz => new()
    {
        Model = "Industrial 100kW 60Hz",
        RatedPowerKw = 100,
        RatedFrequency = 60,

        // Voltage thresholds
        VoltageNominal = 380.0,
        VoltageCriticalLow = 365.0,
        VoltageWarningLow = 370.0,
        VoltageWarningHigh = 390.0,
        VoltageCriticalHigh = 395.0,

        // Current thresholds
        CurrentNominal = 125.5,
        CurrentWarning = 140.0,
        CurrentCritical = 150.0,

        // Frequency thresholds
        FrequencyCriticalLow = 59.5,
        FrequencyWarningLow = 59.8,
        FrequencyWarningHigh = 60.2,
        FrequencyCriticalHigh = 60.5,

        // Active power thresholds
        ActivePowerNominal = 85.4,
        ActivePowerWarning = 95.0,
        ActivePowerCritical = 100.0,

        // Reactive power thresholds
        ReactivePowerNominal = 32.1,
        ReactivePowerWarning = 40.0,
        ReactivePowerCritical = 45.0,

        // Engine speed thresholds
        EngineSpeedNominal = 1800.0,
        EngineSpeedCriticalLow = 1700.0,
        EngineSpeedWarningLow = 1750.0,
        EngineSpeedWarningHigh = 1850.0,
        EngineSpeedCriticalHigh = 1900.0,

        // Engine temperature thresholds
        EngineTemperatureNominal = 82.5,
        EngineTemperatureWarning = 85.0,
        EngineTemperatureCritical = 95.0,

        // Oil pressure thresholds
        OilPressureNominal = 4.2,
        OilPressureCritical = 3.0,
        OilPressureWarning = 3.5,

        // Fuel level thresholds
        FuelLevelNominal = 75.0,
        FuelLevelWarning = 30.0,
        FuelLevelCritical = 15.0
    };

    #endregion
}
