namespace XamlDS.Itk.Simulators.Power;


/// <summary>
/// Simulates real-time power generator monitoring data with random fluctuations.
/// </summary>
public class PowerGeneratorSimulator : IDisposable
{
    private readonly Timer _timer;
    private readonly Random _random = new();
    private bool _disposed;

    // Base values for simulation
    private double _outputVoltage;
    private double _outputCurrent;
    private double _frequency;
    private double _activePower;
    private double _reactivePower;
    private double _engineSpeed;
    private double _engineTemperature;
    private double _oilPressure;
    private double _fuelLevel;
    private double _runningHours = 1245.5;

    /// <summary>
    /// Gets the generator specification used by this simulator.
    /// </summary>
    public PowerGeneratorSpecification Specification { get; }

    /// <summary>
    /// Event raised when generator data is updated.
    /// </summary>
    public event EventHandler<GeneratorDataEventArgs>? DataUpdated;

    /// <summary>
    /// Initializes a new instance of the PowerGeneratorSimulator class.
    /// </summary>
    /// <param name="updateIntervalMs">Update interval in milliseconds (default: 1000ms)</param>
    /// <param name="specification">Generator specification (default: Industrial100kW60Hz)</param>
    public PowerGeneratorSimulator(int updateIntervalMs = 1000, PowerGeneratorSpecification? specification = null)
    {
        Specification = specification ?? PowerGeneratorSpecification.Industrial100kW60Hz;

        // Initialize values from specification
        _outputVoltage = Specification.VoltageNominal;
        _outputCurrent = Specification.CurrentNominal;
        _frequency = Specification.RatedFrequency;
        _activePower = Specification.ActivePowerNominal;
        _reactivePower = Specification.ReactivePowerNominal;
        _engineSpeed = Specification.EngineSpeedNominal;
        _engineTemperature = Specification.EngineTemperatureNominal;
        _oilPressure = Specification.OilPressureNominal;
        _fuelLevel = Specification.FuelLevelNominal;

        _timer = new Timer(OnTimerTick, null, updateIntervalMs, updateIntervalMs);
    }

    private void OnTimerTick(object? state)
    {
        if (_disposed) return;

        // Simulate realistic fluctuations
        _outputVoltage = FluctuateValue(_outputVoltage, Specification.VoltageNominal, 0.5,
            Specification.VoltageWarningLow, Specification.VoltageWarningHigh);
        _outputCurrent = FluctuateValue(_outputCurrent, Specification.CurrentNominal, 2.0,
            100.0, Specification.CurrentCritical);
        _frequency = FluctuateValue(_frequency, Specification.RatedFrequency, 0.02,
            Specification.FrequencyCriticalLow, Specification.FrequencyCriticalHigh);
        _activePower = FluctuateValue(_activePower, Specification.ActivePowerNominal, 1.5,
            70.0, Specification.ActivePowerCritical);
        _reactivePower = FluctuateValue(_reactivePower, Specification.ReactivePowerNominal, 1.0,
            20.0, Specification.ReactivePowerCritical);
        _engineSpeed = FluctuateValue(_engineSpeed, Specification.EngineSpeedNominal, 5.0,
            Specification.EngineSpeedWarningLow, Specification.EngineSpeedWarningHigh);
        _engineTemperature = FluctuateValue(_engineTemperature, Specification.EngineTemperatureNominal, 0.8,
            70.0, 105.0);
        _oilPressure = FluctuateValue(_oilPressure, Specification.OilPressureNominal, 0.1,
            2.5, 5.5);
        _fuelLevel = Math.Max(0, _fuelLevel - 0.01); // Gradually decrease
        _runningHours += 0.0002778; // ~1 second in hours

        // Raise event with updated data
        DataUpdated?.Invoke(this, new GeneratorDataEventArgs
        {
            OutputVoltage = _outputVoltage,
            OutputCurrent = _outputCurrent,
            Frequency = _frequency,
            ActivePower = _activePower,
            ReactivePower = _reactivePower,
            EngineSpeed = _engineSpeed,
            EngineTemperature = _engineTemperature,
            OilPressure = _oilPressure,
            FuelLevel = _fuelLevel,
            RunningHours = _runningHours
        });
    }

    /// <summary>
    /// Fluctuates a value around a target with realistic constraints.
    /// </summary>
    private double FluctuateValue(double current, double target, double maxChange, double min, double max)
    {
        // Add random change
        var change = (_random.NextDouble() - 0.5) * 2 * maxChange;

        // Gradually move toward target
        var toTarget = (target - current) * 0.1;

        var newValue = current + change + toTarget;

        // Clamp to min/max
        return Math.Clamp(newValue, min, max);
    }

    /// <summary>
    /// Resets fuel level to 100% (simulates refueling).
    /// </summary>
    public void Refuel()
    {
        _fuelLevel = 100.0;
    }

    /// <summary>
    /// Simulates a warning condition by reducing oil pressure.
    /// </summary>
    public void SimulateWarning()
    {
        _oilPressure = 3.2;
    }

    /// <summary>
    /// Simulates a critical condition by setting high temperature.
    /// </summary>
    public void SimulateCritical()
    {
        _engineTemperature = 98.0;
    }

    /// <summary>
    /// Resets all values to normal operating conditions.
    /// </summary>
    public void ResetToNormal()
    {
        _outputVoltage = Specification.VoltageNominal;
        _outputCurrent = Specification.CurrentNominal;
        _frequency = Specification.RatedFrequency;
        _activePower = Specification.ActivePowerNominal;
        _reactivePower = Specification.ReactivePowerNominal;
        _engineSpeed = Specification.EngineSpeedNominal;
        _engineTemperature = Specification.EngineTemperatureNominal;
        _oilPressure = Specification.OilPressureNominal;
        _fuelLevel = Specification.FuelLevelNominal;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _timer?.Dispose();
    }
}

/// <summary>
/// Event arguments containing generator monitoring data.
/// </summary>
public class GeneratorDataEventArgs : EventArgs
{
    public double OutputVoltage { get; init; }
    public double OutputCurrent { get; init; }
    public double Frequency { get; init; }
    public double ActivePower { get; init; }
    public double ReactivePower { get; init; }
    public double EngineSpeed { get; init; }
    public double EngineTemperature { get; init; }
    public double OilPressure { get; init; }
    public double FuelLevel { get; init; }
    public double RunningHours { get; init; }
}
