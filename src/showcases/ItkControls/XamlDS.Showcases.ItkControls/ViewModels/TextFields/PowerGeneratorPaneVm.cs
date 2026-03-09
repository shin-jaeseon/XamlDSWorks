using XamlDS.Itk.Simulators.Power;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Showcases.ItkControls.ViewModels.TextFields;

public class PowerGeneratorPaneVm : ViewModelBase
{
    private readonly PowerGeneratorSimulator _simulator;
    private readonly SynchronizationContext? _syncContext;

    // Observable properties for data binding
    private double _outputVoltage;
    private double _outputCurrent;
    private double _frequency;
    private double _activePower;
    private double _reactivePower;
    private double _engineSpeed;
    private double _engineTemperature;
    private double _oilPressure;
    private double _fuelLevel;
    private double _runningHours;

    // Status properties
    private string _voltageStatus = "Normal";
    private string _currentStatus = "Normal";
    private string _frequencyStatus = "Normal";
    private string _activePowerStatus = "Normal";
    private string _reactivePowerStatus = "Normal";
    private string _engineSpeedStatus = "Normal";
    private string _engineTemperatureStatus = "Normal";
    private string _oilPressureStatus = "Normal";
    private string _fuelLevelStatus = "Normal";

    public PowerGeneratorPaneVm()
    {
        // Capture the UI synchronization context
        _syncContext = SynchronizationContext.Current;

        // Initialize simulator with 1 second update interval
        _simulator = new PowerGeneratorSimulator(1000);
        _simulator.DataUpdated += OnSimulatorDataUpdated;
    }

    private void OnSimulatorDataUpdated(object? sender, GeneratorDataEventArgs e)
    {
        // Update UI on the UI thread
        if (_syncContext != null)
        {
            _syncContext.Post(_ => UpdateValues(e), null);
        }
        else
        {
            UpdateValues(e);
        }
    }

    private void UpdateValues(GeneratorDataEventArgs e)
    {
        OutputVoltage = e.OutputVoltage;
        OutputCurrent = e.OutputCurrent;
        Frequency = e.Frequency;
        ActivePower = e.ActivePower;
        ReactivePower = e.ReactivePower;
        EngineSpeed = e.EngineSpeed;
        EngineTemperature = e.EngineTemperature;
        OilPressure = e.OilPressure;
        FuelLevel = e.FuelLevel;
        RunningHours = e.RunningHours;

        // Update status based on values
        UpdateStatuses();
    }

    private void UpdateStatuses()
    {
        // Voltage: Normal (370-390V), Warning (365-370 or 390-395), Critical (<365 or >395)
        VoltageStatus = _outputVoltage switch
        {
            < 365 or > 395 => "Critical",
            < 370 or > 390 => "Warning",
            _ => "Normal"
        };

        // Current: Normal (<140A), Warning (140-150A), Critical (>150A)
        CurrentStatus = _outputCurrent switch
        {
            > 150 => "Critical",
            > 140 => "Warning",
            _ => "Normal"
        };

        // Frequency: Normal (59.8-60.2Hz), Warning (59.5-59.8 or 60.2-60.5), Critical (outside)
        FrequencyStatus = _frequency switch
        {
            < 59.5 or > 60.5 => "Critical",
            < 59.8 or > 60.2 => "Warning",
            _ => "Normal"
        };

        // Active Power: Normal (<95kW), Warning (95-100kW), Critical (>100kW)
        ActivePowerStatus = _activePower switch
        {
            > 100 => "Critical",
            > 95 => "Warning",
            _ => "Normal"
        };

        // Reactive Power: Normal (<40kVAr), Warning (40-45kVAr), Critical (>45kVAr)
        ReactivePowerStatus = _reactivePower switch
        {
            > 45 => "Critical",
            > 40 => "Warning",
            _ => "Normal"
        };

        // Engine Speed: Normal (1750-1850RPM), Warning (1700-1750 or 1850-1900), Critical (outside)
        EngineSpeedStatus = _engineSpeed switch
        {
            < 1700 or > 1900 => "Critical",
            < 1750 or > 1850 => "Warning",
            _ => "Normal"
        };

        // Engine Temperature: Normal (<85°C), Warning (85-95°C), Critical (>95°C)
        EngineTemperatureStatus = _engineTemperature switch
        {
            > 95 => "Critical",
            > 85 => "Warning",
            _ => "Normal"
        };

        // Oil Pressure: Normal (>3.5bar), Warning (3.0-3.5bar), Critical (<3.0bar)
        OilPressureStatus = _oilPressure switch
        {
            < 3.0 => "Critical",
            < 3.5 => "Warning",
            _ => "Normal"
        };

        // Fuel Level: Normal (>30%), Warning (15-30%), Critical (<15%)
        FuelLevelStatus = _fuelLevel switch
        {
            < 15 => "Critical",
            < 30 => "Warning",
            _ => "Normal"
        };
    }

    #region Properties

    public double OutputVoltage
    {
        get => _outputVoltage;
        set => SetProperty(ref _outputVoltage, value);
    }

    public double OutputCurrent
    {
        get => _outputCurrent;
        set => SetProperty(ref _outputCurrent, value);
    }

    public double Frequency
    {
        get => _frequency;
        set => SetProperty(ref _frequency, value);
    }

    public double ActivePower
    {
        get => _activePower;
        set => SetProperty(ref _activePower, value);
    }

    public double ReactivePower
    {
        get => _reactivePower;
        set => SetProperty(ref _reactivePower, value);
    }

    public double EngineSpeed
    {
        get => _engineSpeed;
        set => SetProperty(ref _engineSpeed, value);
    }

    public double EngineTemperature
    {
        get => _engineTemperature;
        set => SetProperty(ref _engineTemperature, value);
    }

    public double OilPressure
    {
        get => _oilPressure;
        set => SetProperty(ref _oilPressure, value);
    }

    public double FuelLevel
    {
        get => _fuelLevel;
        set => SetProperty(ref _fuelLevel, value);
    }

    public double RunningHours
    {
        get => _runningHours;
        set => SetProperty(ref _runningHours, value);
    }

    public string VoltageStatus
    {
        get => _voltageStatus;
        set => SetProperty(ref _voltageStatus, value);
    }

    public string CurrentStatus
    {
        get => _currentStatus;
        set => SetProperty(ref _currentStatus, value);
    }

    public string FrequencyStatus
    {
        get => _frequencyStatus;
        set => SetProperty(ref _frequencyStatus, value);
    }

    public string ActivePowerStatus
    {
        get => _activePowerStatus;
        set => SetProperty(ref _activePowerStatus, value);
    }

    public string ReactivePowerStatus
    {
        get => _reactivePowerStatus;
        set => SetProperty(ref _reactivePowerStatus, value);
    }

    public string EngineSpeedStatus
    {
        get => _engineSpeedStatus;
        set => SetProperty(ref _engineSpeedStatus, value);
    }

    public string EngineTemperatureStatus
    {
        get => _engineTemperatureStatus;
        set => SetProperty(ref _engineTemperatureStatus, value);
    }

    public string OilPressureStatus
    {
        get => _oilPressureStatus;
        set => SetProperty(ref _oilPressureStatus, value);
    }

    public string FuelLevelStatus
    {
        get => _fuelLevelStatus;
        set => SetProperty(ref _fuelLevelStatus, value);
    }

    #endregion

    #region Simulation Control Methods

    public void Refuel() => _simulator.Refuel();
    public void SimulateWarning() => _simulator.SimulateWarning();
    public void SimulateCritical() => _simulator.SimulateCritical();
    public void ResetToNormal() => _simulator.ResetToNormal();

    #endregion

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _simulator?.Dispose();
        }
        base.Dispose(disposing);
    }
}
