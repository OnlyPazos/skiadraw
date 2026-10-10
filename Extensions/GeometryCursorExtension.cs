using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using skiadraw.Factories;

namespace skiadraw.Extensions;

public class GeometryCursorExtension : MarkupExtension
{
    public Geometry? Geometry { get; set; }
    public IBrush Fill { get; set; } = Brushes.White;
    public IBrush Stroke { get; set; } = Brushes.Black;
    public double StrokeThickness { get; set; } = 2;
    public int Size { get; set; } = 24;
    public int HotspotX { get; set; }
    public int HotspotY { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Geometry is null) return new Cursor(StandardCursorType.Arrow);

        return CursorFactory.FromGeometry(Geometry, Fill, Stroke, StrokeThickness, Size, HotspotX, HotspotY);
    }
}