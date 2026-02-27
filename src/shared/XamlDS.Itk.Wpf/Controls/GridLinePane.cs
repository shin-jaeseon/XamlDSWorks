using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XamlDS.Itk.Controls;

/// <summary>
/// Enumeration that describes grid origin reference.
/// </summary>
public enum GridOrigin
{
    /// <summary>Origin at control center.</summary>
    Center,

    /// <summary>Origin at control top-left.</summary>
    LeftTop
}

/// <summary>
/// A lightweight control that draws a background grid. No child/content support.
/// </summary>
public class GridLinePane : Control
{
    static GridLinePane()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GridLinePane), new FrameworkPropertyMetadata(typeof(GridLinePane)));
    }

    /// <summary>
    /// Opacity applied to grid lines (0.0 - 1.0).
    /// </summary>
    public static readonly DependencyProperty GridOpacityProperty =
        DependencyProperty.Register(
            nameof(GridOpacity),
            typeof(double),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(0.25d, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Spacing between grid lines in device-independent units.
    /// </summary>
    public static readonly DependencyProperty GridSpacingProperty =
        DependencyProperty.Register(
            nameof(GridSpacing),
            typeof(double),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(48.0d, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Thickness of grid lines.
    /// </summary>
    public static readonly DependencyProperty GridThicknessProperty =
        DependencyProperty.Register(
            nameof(GridThickness),
            typeof(double),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(1.0d, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Brush used to draw grid lines.
    /// </summary>
    public static readonly DependencyProperty GridLineBrushProperty =
        DependencyProperty.Register(
            nameof(GridLineBrush),
            typeof(Brush),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Reference origin for the grid (Center or LeftTop).
    /// </summary>
    public static readonly DependencyProperty OriginProperty =
        DependencyProperty.Register(
            nameof(Origin),
            typeof(GridOrigin),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(GridOrigin.Center, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// When true, grid line coordinates will be snapped to the device pixel grid for crisper rendering.
    /// </summary>
    public static readonly DependencyProperty SnapToDevicePixelsProperty =
        DependencyProperty.Register(
            nameof(SnapToDevicePixels),
            typeof(bool),
            typeof(GridLinePane),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Gets or sets the opacity applied to grid lines.
    /// </summary>
    public double GridOpacity
    {
        get => (double)GetValue(GridOpacityProperty);
        set => SetValue(GridOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between grid lines.
    /// </summary>
    public double GridSpacing
    {
        get => (double)GetValue(GridSpacingProperty);
        set => SetValue(GridSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of grid lines.
    /// </summary>
    public double GridThickness
    {
        get => (double)GetValue(GridThicknessProperty);
        set => SetValue(GridThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used for grid lines.
    /// </summary>
    public Brush GridLineBrush
    {
        get => (Brush)GetValue(GridLineBrushProperty);
        set => SetValue(GridLineBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the origin reference for the grid.
    /// </summary>
    public GridOrigin Origin
    {
        get => (GridOrigin)GetValue(OriginProperty);
        set => SetValue(OriginProperty, value);
    }

    /// <summary>
    /// Gets or sets whether grid coordinates are snapped to device pixels.
    /// </summary>
    public bool SnapToDevicePixels
    {
        get => (bool)GetValue(SnapToDevicePixelsProperty);
        set => SetValue(SnapToDevicePixelsProperty, value);
    }

    // --- Geometry cache fields ---
    private StreamGeometry? _cachedGeometry;
    private Size _cachedSize = Size.Empty;
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
            double originX = (origin == GridOrigin.Center) ? width / 2.0 : 0.0;
            double originY = (origin == GridOrigin.Center) ? height / 2.0 : 0.0;

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
                ctx.BeginFigure(new Point(sx, SnapCoordLocal(0, scaleY, snap)), false, false);
                ctx.LineTo(new Point(sx, SnapCoordLocal(height, scaleY, snap)), true, false);
            }

            // Vertical lines to the left
            for (double x = originX - spacing; x >= 0; x -= spacing)
            {
                double sx = SnapCoordLocal(x, scaleX, snap);
                ctx.BeginFigure(new Point(sx, SnapCoordLocal(0, scaleY, snap)), false, false);
                ctx.LineTo(new Point(sx, SnapCoordLocal(height, scaleY, snap)), true, false);
            }

            // Horizontal lines downward
            for (double y = originY; y <= height; y += spacing)
            {
                double sy = SnapCoordLocal(y, scaleY, snap);
                ctx.BeginFigure(new Point(SnapCoordLocal(0, scaleX, snap), sy), false, false);
                ctx.LineTo(new Point(SnapCoordLocal(width, scaleX, snap), sy), true, false);
            }

            // Horizontal lines upward
            for (double y = originY - spacing; y >= 0; y -= spacing)
            {
                double sy = SnapCoordLocal(y, scaleY, snap);
                ctx.BeginFigure(new Point(SnapCoordLocal(0, scaleX, snap), sy), false, false);
                ctx.LineTo(new Point(SnapCoordLocal(width, scaleX, snap), sy), true, false);
            }
        }

        geometry.Freeze();

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
    /// Uses StreamGeometry for batching and freezes resources for better performance.
    /// </summary>
    /// <param name="drawingContext">The drawing context.</param>
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        double width = ActualWidth;
        double height = ActualHeight;
        if (width <= 0 || height <= 0)
            return;

        double spacing = Math.Max(0.0, GridSpacing);
        if (spacing <= 0.0)
            return;

        // DPI scale for snapping
        var dpi = VisualTreeHelper.GetDpi(this);
        double scaleX = dpi.DpiScaleX;
        double scaleY = dpi.DpiScaleY;

        bool snap = SnapToDevicePixels;

        // Ensure cached geometry matches current render parameters
        EnsureGeometry(width, height, spacing, scaleX, scaleY, Origin, snap);

        if (_cachedGeometry == null)
            return;

        // Prepare brush + pen (pen can vary independently of geometry)
        Brush baseBrush = GridLineBrush ?? Brushes.Transparent;
        Brush brush = baseBrush.Clone();
        brush.Opacity = Math.Max(0.0, Math.Min(1.0, GridOpacity));
        brush.Freeze();

        Pen pen = new Pen(brush, Math.Max(0.0, GridThickness));
        pen.Freeze();

        drawingContext.DrawGeometry(null, pen, _cachedGeometry);
    }

    //private static double SnapCoord(double value, double scale, bool snap)
    //{
    //    if (!snap)
    //        return value;
    //    // Round to nearest device pixel then convert back to device-independent units
    //    return Math.Round(value * scale) / scale;
    //}

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
