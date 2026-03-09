using XamlDS.Itk.Factories;
using XamlDS.Itk.Simulators.Power;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Gauges;

namespace XamlDS.Showcases.ItkControls.ViewModels;

public class PowerGeneratorDemoVm : ViewModelBase
{
    private readonly PowerGeneratorSpecification _generatorSpecification;
    private readonly PowerGeneratorSimulator _simulator;
    private readonly SynchronizationContext? _syncContext;

    private readonly IGaugeVmFactory _gaugeVmFactory;
    private bool _isEnabled = false;
    private readonly GaugeVm _outputVoltage;
    private readonly GaugeVm _frequencyStatus;

    public PowerGeneratorDemoVm(IGaugeVmFactory gaugeVmFactory)
    {
        _gaugeVmFactory = gaugeVmFactory;

        _outputVoltage = _gaugeVmFactory.CreateVoltageVolt("Output Voltage");
        _frequencyStatus = _gaugeVmFactory.CreateFrequencyHertz("Frequency");

        // Capture the UI synchronization context
        _syncContext = SynchronizationContext.Current;

        // Initialize simulator with 1 second update interval
        _generatorSpecification = PowerGeneratorSpecification.Industrial100kW60Hz;
        _simulator = new PowerGeneratorSimulator(1000, _generatorSpecification);
        _simulator.DataUpdated += OnSimulatorDataUpdated;
        InitGauges();
    }

    private void InitGauges()
    {
        // Initialize a gauge for output voltage
        _outputVoltage.MinValue = _generatorSpecification.VoltageCriticalLow;
        _outputVoltage.MaxValue = _generatorSpecification.VoltageCriticalHigh;
        _outputVoltage.ThresholdLowCritical = _generatorSpecification.VoltageCriticalLow;
        _outputVoltage.ThresholdLowWarning = _generatorSpecification.VoltageWarningLow;
        _outputVoltage.ThresholdHighWarning = _generatorSpecification.VoltageWarningHigh;
        _outputVoltage.ThresholdHighCritical = _generatorSpecification.VoltageCriticalHigh;
        _outputVoltage.NominalValue = _generatorSpecification.VoltageNominal;
        _outputVoltage.ShowNominalMarker = true;
        _outputVoltage.IsEnabled = false;

        _frequencyStatus.MinValue = _generatorSpecification.FrequencyCriticalLow;
        _frequencyStatus.MaxValue = _generatorSpecification.FrequencyCriticalHigh;
        _frequencyStatus.ThresholdLowCritical = _generatorSpecification.FrequencyCriticalLow;
        _frequencyStatus.ThresholdLowWarning = _generatorSpecification.FrequencyWarningLow;
        _frequencyStatus.ThresholdHighWarning = _generatorSpecification.FrequencyWarningHigh;
        _frequencyStatus.ThresholdHighCritical = _generatorSpecification.FrequencyCriticalHigh;
        _frequencyStatus.IsEnabled = false;

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
            if (_isEnabled == false)
            {
                _isEnabled = true;
                OutputVoltage.IsEnabled = true; // Enable the gauge when we receive the first data
                _frequencyStatus.IsEnabled = true; // Enable the frequency gauge when we receive the first data
            }
            UpdateValues(e);
        }
    }

    private void UpdateValues(GeneratorDataEventArgs e)
    {
        OutputVoltage.Value = e.OutputVoltage;
        //OutputCurrent = e.OutputCurrent;
        _frequencyStatus.Value = e.Frequency;
        //ActivePower = e.ActivePower;
        //ReactivePower = e.ReactivePower;
        //EngineSpeed = e.EngineSpeed;
        //EngineTemperature = e.EngineTemperature;
        //OilPressure = e.OilPressure;
        //FuelLevel = e.FuelLevel;
        //RunningHours = e.RunningHours;
    }

    public GaugeVm OutputVoltage => _outputVoltage;

    public GaugeVm FrequencyStatus => _frequencyStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _simulator.DataUpdated -= OnSimulatorDataUpdated;
            _simulator?.Dispose();
        }
        base.Dispose(disposing);
    }
}
