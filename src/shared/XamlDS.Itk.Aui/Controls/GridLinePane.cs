using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace XamlDS.Itk.Controls;

/// <summary>
/// Enumeration that describes grid origin reference.
/// </summary>
public enum GridOrigin
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

/// <summary>
/// A lightweight control that draws a background grid. No child/content support.
/// </summary>
public class GridLinePane : Control
{
    /// <summary>
    /// Defines the <see cref="GridOpacity"/> property.
    /// </summary>
    public static readonly StyledProperty<double> GridOpacityProperty =
        AvaloniaProperty.Register<GridLinePane, double>(
            nameof(GridOpacity),
            defaultValue: 0.25,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    /// <summary>
    /// Defines the <see cref="GridSpacing"/> property.
    /// </summary>
    public static readonly StyledProperty<double> GridSpacingProperty =
        AvaloniaProperty.Register<GridLinePane, double>(
            nameof(GridSpacing),
            defaultValue: 64.0,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    /// <summary>
    /// Defines the <see cref="GridThickness"/> property.
    /// </summary>
    public static readonly StyledProperty<double> GridThicknessProperty =
        AvaloniaProperty.Register<GridLinePane, double>(
            nameof(GridThickness),
            defaultValue: 1.0,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    /// <summary>
    /// Defines the <see cref="GridLineBrush"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> GridLineBrushProperty =
        AvaloniaProperty.Register<GridLinePane, IBrush?>(
            nameof(GridLineBrush),
            defaultValue: Brushes.LightGray,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    /// <summary>
    /// Defines the <see cref="Origin"/> property.
    /// </summary>
    public static readonly StyledProperty<GridOrigin> OriginProperty =
        AvaloniaProperty.Register<GridLinePane, GridOrigin>(
            nameof(Origin),
            defaultValue: GridOrigin.Center,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    /// <summary>
    /// Defines the <see cref="SnapToDevicePixels"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> SnapToDevicePixelsProperty =
        AvaloniaProperty.Register<GridLinePane, bool>(
            nameof(SnapToDevicePixels),
            defaultValue: true,
            defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

    static GridLinePane()
    {
        AffectsRender<GridLinePane>(
            GridOpacityProperty,
            GridSpacingProperty,
            GridThicknessProperty,
            GridLineBrushProperty,
            OriginProperty,
            SnapToDevicePixelsProperty);
    }

    /// <summary>
    /// Gets or sets the opacity applied to grid lines.
    /// </summary>
    public double GridOpacity
    {
        get => GetValue(GridOpacityProperty);
        set => SetValue(GridOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between grid lines.
    /// </summary>
    public double GridSpacing
    {
        get => GetValue(GridSpacingProperty);
        set => SetValue(GridSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of grid lines.
    /// </summary>
    public double GridThickness
    {
        get => GetValue(GridThicknessProperty);
        set => SetValue(GridThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used for grid lines.
    /// </summary>
    public IBrush? GridLineBrush
    {
        get => GetValue(GridLineBrushProperty);
        set => SetValue(GridLineBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the origin reference for the grid.
    /// </summary>
    public GridOrigin Origin
    {
        get => GetValue(OriginProperty);
        set => SetValue(OriginProperty, value);
    }

    /// <summary>
    /// Gets or sets whether grid coordinates are snapped to device pixels.
    /// </summary>
    public bool SnapToDevicePixels
    {
        get => GetValue(SnapToDevicePixelsProperty);
        set => SetValue(SnapToDevicePixelsProperty, value);
    }

    // --- Geometry cache fields ---
    private StreamGeometry? _cachedGeometry;
    private Size _cachedSize = default;
    private double _cachedSpacing = double.NaN;
    private double _cachedScaleX = double.NaN;
    private double _cachedScaleY = double.NaN;
    private GridOrigin _cachedOrigin = (GridOrigin)(-1);
    private bool _cachedSnap = false;

    /// <summary>
    /// Ensures a cached StreamGeometry exists for the current render parameters.
    /// If any key parameter changed since the last creation, the cached geometry is regenerated.
    /// </summary>
    private void EnsureGeometry(double width, double height, double spacing, double scaleX, double scaleY, GridOrigin origin, bool snap)
    {
        var size = new Size(width, height);
        bool needRecreate = _cachedGeometry == null
            || _cachedSize != size
            || !AreClose(_cachedSpacing, spacing)
            || !AreClose(_cachedScaleX, scaleX)
            || !AreClose(_cachedScaleY, scaleY)
            || _cachedOrigin != origin
            || _cachedSnap != snap;

        if (!needRecreate)
            return;

        // Build new geometry
        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            // Calculate origin based on GridOrigin enum value
            double originX = origin switch
            {
                GridOrigin.Center => width / 2.0,
                GridOrigin.RightTop => width,
                GridOrigin.RightBottom => width,
                _ => 0.0 // LeftTop, LeftBottom
            };

            double originY = origin switch
            {
                GridOrigin.Center => height / 2.0,
                GridOrigin.LeftBottom => height,
                GridOrigin.RightBottom => height,
                _ => 0.0 // LeftTop, RightTop
            };

            static double SnapCoordLocal(double value, double scale, bool doSnap)
            {
                if (!doSnap)
                    return value;
                return Math.Round(value * scale) / scale;
            }

            // Vertical lines to the right
            for (double x = originX; x <= width; x += spacing)
            {
                double sx = SnapCoordLocal(x, scaleX, snap);
                ctx.BeginFigure(new Point(sx, SnapCoordLocal(0, scaleY, snap)), false);
                ctx.LineTo(new Point(sx, SnapCoordLocal(height, scaleY, snap)));
            }

            // Vertical lines to the left
            for (double x = originX - spacing; x >= 0; x -= spacing)
            {
                double sx = SnapCoordLocal(x, scaleX, snap);
                ctx.BeginFigure(new Point(sx, SnapCoordLocal(0, scaleY, snap)), false);
                ctx.LineTo(new Point(sx, SnapCoordLocal(height, scaleY, snap)));
            }

            // Horizontal lines downward
            for (double y = originY; y <= height; y += spacing)
            {
                double sy = SnapCoordLocal(y, scaleY, snap);
                ctx.BeginFigure(new Point(SnapCoordLocal(0, scaleX, snap), sy), false);
                ctx.LineTo(new Point(SnapCoordLocal(width, scaleX, snap), sy));
            }

            // Horizontal lines upward
            for (double y = originY - spacing; y >= 0; y -= spacing)
            {
                double sy = SnapCoordLocal(y, scaleY, snap);
                ctx.BeginFigure(new Point(SnapCoordLocal(0, scaleX, snap), sy), false);
                ctx.LineTo(new Point(SnapCoordLocal(width, scaleX, snap), sy));
            }
        }

        // Store cache
        _cachedGeometry = geometry;
        _cachedSize = size;
        _cachedSpacing = spacing;
        _cachedScaleX = scaleX;
        _cachedScaleY = scaleY;
        _cachedOrigin = origin;
        _cachedSnap = snap;
    }

    /// <summary>
    /// Renders the grid lines into the control's drawing context.
    /// Uses StreamGeometry for batching for better performance.
    /// </summary>
    /// <param name="context">The drawing context.</param>
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        double width = Bounds.Width;
        double height = Bounds.Height;
        if (width <= 0 || height <= 0)
            return;

        double spacing = Math.Max(0.0, GridSpacing);
        if (spacing <= 0.0)
            return;

        // Get render scaling for pixel snapping
        var visualRoot = this.VisualRoot;
        double scaleX = 1.0;
        double scaleY = 1.0;

        if (visualRoot is TopLevel topLevel)
        {
            scaleX = topLevel.RenderScaling;
            scaleY = topLevel.RenderScaling;
        }

        bool snap = SnapToDevicePixels;

        // Ensure cached geometry matches current render parameters
        EnsureGeometry(width, height, spacing, scaleX, scaleY, Origin, snap);

        if (_cachedGeometry == null)
            return;

        // Prepare brush + pen
        IBrush? baseBrush = GridLineBrush ?? Brushes.Transparent;

        // Create immutable brush with opacity
        IBrush? renderBrush = baseBrush;
        double opacity = Math.Clamp(GridOpacity, 0.0, 1.0);

        if (baseBrush is ISolidColorBrush solidBrush)
        {
            var color = solidBrush.Color;
            renderBrush = new SolidColorBrush(
                Color.FromArgb(
                    (byte)(color.A * opacity),
                    color.R,
                    color.G,
                    color.B));
        }

        var pen = new Pen(renderBrush, Math.Max(0.0, GridThickness));

        context.DrawGeometry(null, pen, _cachedGeometry);
    }

    private static bool AreClose(double a, double b)
    {
        if (double.IsNaN(a) && double.IsNaN(b))
            return true;
        if (double.IsNaN(a) || double.IsNaN(b))
            return false;
        const double eps = 1e-6;
        return Math.Abs(a - b) <= eps;
    }
}
