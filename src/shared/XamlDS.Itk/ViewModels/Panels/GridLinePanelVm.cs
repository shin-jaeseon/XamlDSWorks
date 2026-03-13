namespace XamlDS.Itk.ViewModels.Panels;

/// <summary>
/// Enumeration that describes grid origin reference.
/// </summary>
public enum GridLineOrigin
{
    /// <summary>Origin at control center.</summary>
    Center,

    /// <summary>Origin at control top-left.</summary>
    LeftTop,

    /// <summary>Origin at control bottom-left.</summary>
    LeftBottom,

    /// <summary>Origin at control top-right.</summary>
    RightTop,

    /// <summary>Origin at control bottom-right.</summary>
    RightBottom,
}

public class GridLinePanelVm : PanelVm
{
    private double _columnThickness = 1.0;
    private double _rowThickness = 1.0;
    private double _columnOpacity = 1.0;
    private double _rowOpacity = 1.0;
    private double _columnSpacing = 20.0;
    private double _rowSpacing = 20.0;
    private GridLineOrigin _gridOrigin = GridLineOrigin.Center;

    public double ColumnThickness
    {
        get => _columnThickness;
        set => SetProperty(ref _columnThickness, value);
    }

    public double RowThickness
    {
        get => _rowThickness;
        set => SetProperty(ref _rowThickness, value);
    }

    public double ColumnOpacity
    {
        get => _columnOpacity;
        set => SetProperty(ref _columnOpacity, value);
    }

    public double RowOpacity
    {
        get => _rowOpacity;
        set => SetProperty(ref _rowOpacity, value);
    }

    public double ColumnSpacing
    {
        get => _columnSpacing;
        set => SetProperty(ref _columnSpacing, value);
    }

    public double RowSpacing
    {
        get => _rowSpacing;
        set => SetProperty(ref _rowSpacing, value);
    }

    public GridLineOrigin GridOrigin
    {
        get => _gridOrigin;
        set => SetProperty(ref _gridOrigin, value);
    }
}
