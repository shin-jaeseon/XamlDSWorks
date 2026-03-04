using System;
using System.Threading;

namespace XamlDS.Showcases.ItkControls.Services;

/// <summary>
/// Simulates real-time power generator monitoring data with random fluctuations.
/// </summary>
public class PowerGeneratorSimulator : IDisposable
{
    private readonly Timer _timer;
    private readonly Random _random = new();
    private bool _disposed;

    // Base values for simulation
    private double _outputVoltage = 380.0;
    private double _outputCurrent = 125.5;
    private double _frequency = 60.0;
    private double _activePower = 85.4;
    private double _reactivePower = 32.1;
    private double _engineSpeed = 1800.0;
    private double _engineTemperature = 82.5;
    private double _oilPressure = 4.2;
    private double _fuelLevel = 75.0;
    private double _runningHours = 1245.5;

    /// <summary>
    /// Event raised when generator data is updated.
    /// </summary>
    public event EventHandler<GeneratorDataEventArgs>? DataUpdated;

    /// <summary>
    /// Initializes a new instance of the PowerGeneratorSimulator class.
    /// </summary>
    /// <param name="updateIntervalMs">Update interval in milliseconds (default: 1000ms)</param>
    public PowerGeneratorSimulator(int updateIntervalMs = 1000)
    {
        _timer = new Timer(OnTimerTick, null, updateIntervalMs, updateIntervalMs);
    }

    private void OnTimerTick(object? state)
    {
        if (_disposed) return;

        // Simulate realistic fluctuations
        _outputVoltage = FluctuateValue(_outputVoltage, 380.0, 0.5, 370.0, 390.0);
        _outputCurrent = FluctuateValue(_outputCurrent, 125.5, 2.0, 100.0, 150.0);
        _frequency = FluctuateValue(_frequency, 60.0, 0.02, 59.5, 60.5);
        _activePower = FluctuateValue(_activePower, 85.4, 1.5, 70.0, 100.0);
        _reactivePower = FluctuateValue(_reactivePower, 32.1, 1.0, 20.0, 45.0);
        _engineSpeed = FluctuateValue(_engineSpeed, 1800.0, 5.0, 1750.0, 1850.0);
        _engineTemperature = FluctuateValue(_engineTemperature, 82.5, 0.8, 70.0, 105.0);
        _oilPressure = FluctuateValue(_oilPressure, 4.2, 0.1, 2.5, 5.5);
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
        _outputVoltage = 380.0;
        _outputCurrent = 125.5;
        _frequency = 60.0;
        _activePower = 85.4;
        _reactivePower = 32.1;
        _engineSpeed = 1800.0;
        _engineTemperature = 82.5;
        _oilPressure = 4.2;
        _fuelLevel = 75.0;
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
