using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UntilClock.Controls;

/// <summary>
/// A circular progress ring drawn with a WPF <see cref="Path"/> arc.
/// <see cref="Value"/> ranges from 0.0 (empty) to 1.0 (full).
/// </summary>
public sealed class CircularProgressRing : UserControl
{
    // ── Dependency properties ────────────────────────────────────────────────

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(CircularProgressRing),
            new PropertyMetadata(0.0, OnGeometryChanged));

    public static readonly DependencyProperty RingRadiusProperty =
        DependencyProperty.Register(nameof(RingRadius), typeof(double), typeof(CircularProgressRing),
            new PropertyMetadata(40.0, OnGeometryChanged));

    public static readonly DependencyProperty ThicknessProperty =
        DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(CircularProgressRing),
            new PropertyMetadata(6.0, OnGeometryChanged));

    public static readonly DependencyProperty RingColorProperty =
        DependencyProperty.Register(nameof(RingColor), typeof(Brush), typeof(CircularProgressRing),
            new PropertyMetadata(Brushes.DodgerBlue, OnColorChanged));

    public static readonly DependencyProperty TrackColorProperty =
        DependencyProperty.Register(nameof(TrackColor), typeof(Brush), typeof(CircularProgressRing),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)), OnColorChanged));

    // ── CLR wrappers ─────────────────────────────────────────────────────────

    /// <summary>Progress value, 0.0–1.0.</summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, Math.Clamp(value, 0.0, 1.0));
    }

    /// <summary>Outer radius of the ring in device-independent pixels.</summary>
    public double RingRadius
    {
        get => (double)GetValue(RingRadiusProperty);
        set => SetValue(RingRadiusProperty, value);
    }

    /// <summary>Stroke thickness of the ring.</summary>
    public double Thickness
    {
        get => (double)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    /// <summary>Color of the progress arc.</summary>
    public Brush RingColor
    {
        get => (Brush)GetValue(RingColorProperty);
        set => SetValue(RingColorProperty, value);
    }

    /// <summary>Color of the background track.</summary>
    public Brush TrackColor
    {
        get => (Brush)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    // ── Children ─────────────────────────────────────────────────────────────

    private readonly Path _trackPath;
    private readonly Path _progressPath;

    public CircularProgressRing()
    {
        _trackPath = new Path { StrokeThickness = 0, Fill = Brushes.Transparent };
        _progressPath = new Path { StrokeStartLineCap = PenLineCap.Round, StrokeEndLineCap = PenLineCap.Round };

        var canvas = new Canvas();
        canvas.Children.Add(_trackPath);
        canvas.Children.Add(_progressPath);
        Content = canvas;

        Loaded += (_, _) => UpdateGeometry();
    }

    // ── Callbacks ────────────────────────────────────────────────────────────

    private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((CircularProgressRing)d).UpdateGeometry();

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((CircularProgressRing)d).UpdateColors();

    // ── Geometry ─────────────────────────────────────────────────────────────

    private void UpdateGeometry()
    {
        var radius = RingRadius;
        var thickness = Thickness;
        var size = (radius + thickness / 2) * 2;

        Width = size;
        Height = size;

        var cx = size / 2;
        var cy = size / 2;
        var r = radius;

        // Full-circle track
        _trackPath.Data = BuildArc(cx, cy, r, 0, 359.9999);
        _trackPath.StrokeThickness = thickness;
        _trackPath.Stroke = TrackColor;

        // Progress arc — start at top (−90°), sweep clockwise
        var angle = Math.Clamp(Value, 0.0, 1.0) * 359.9999;
        _progressPath.Data = angle < 0.001
            ? Geometry.Empty
            : BuildArc(cx, cy, r, 0, angle);
        _progressPath.StrokeThickness = thickness;
        _progressPath.Stroke = RingColor;
    }

    private void UpdateColors()
    {
        _trackPath.Stroke = TrackColor;
        _progressPath.Stroke = RingColor;
    }

    /// <summary>
    /// Builds a clockwise arc <see cref="PathGeometry"/> starting from the top of the circle.
    /// </summary>
    private static PathGeometry BuildArc(double cx, double cy, double r, double startDeg, double sweepDeg)
    {
        // Start angle offset: −90° so 0 = top
        const double offset = -90.0;
        var startRad = (startDeg + offset) * Math.PI / 180.0;
        var endRad = (startDeg + sweepDeg + offset) * Math.PI / 180.0;

        var start = new Point(cx + r * Math.Cos(startRad), cy + r * Math.Sin(startRad));
        var end = new Point(cx + r * Math.Cos(endRad), cy + r * Math.Sin(endRad));

        var isLargeArc = sweepDeg > 180.0;

        var figure = new PathFigure { StartPoint = start, IsClosed = false };
        figure.Segments.Add(new ArcSegment(
            end,
            new Size(r, r),
            0,
            isLargeArc,
            SweepDirection.Clockwise,
            isStroked: true));

        return new PathGeometry(new[] { figure });
    }
}
