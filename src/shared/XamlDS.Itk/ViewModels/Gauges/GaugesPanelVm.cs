namespace XamlDS.Itk.ViewModels.Gauges;

public enum GaugeStyle
{
    Small,
    Medium,
    Large
}

/// <summary>
/// Base panel view model for displaying gauge controls.
/// </summary>
public abstract class GaugesPanelVm : ContentsPanelVm<GaugeVm>
{
    private bool _showGaugeLabel = true;
    private GaugeStyle _gaugeStyle = GaugeStyle.Medium;

    public bool ShowGaugeLabel
    {
        get => _showGaugeLabel;
        set => SetProperty(ref _showGaugeLabel, value);
    }

    public GaugeStyle GaugeStyle
    {
        get => _gaugeStyle;
        set => SetProperty(ref _gaugeStyle, value);
    }
}

public class TextGaugesPanelVm : GaugesPanelVm
{
}

public class LinearGaugesPanelVm : GaugesPanelVm
{
}

public class RadialGaugesPanelVm : GaugesPanelVm
{
}

public class CircularGaugesPanelVm : GaugesPanelVm
{
}
