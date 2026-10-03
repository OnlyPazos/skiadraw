using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace skiadraw.Extensions;

public class GeometryCursorExtension : MarkupExtension
{
    public Geometry? Geometry { get; set; }
    public IBrush Fill { get; set; } = Brushes.White;
    public IBrush Stroke { get; set; } = Brushes.Black;
    public double StrokeThickness { get; set; } = 1.5;
    public int Size { get; set; } = 24;
    public int HotspotX { get; set; }
    public int HotspotY { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Geometry is null)
            return new Cursor(StandardCursorType.Arrow);

        var bitmap = new RenderTargetBitmap(new PixelSize(Size, Size), new Vector(96, 96));
        var bounds = Geometry.Bounds;

        double margin = StrokeThickness / 2 + 0.5;
        double scale = (Size - 2 * margin) / Math.Max(bounds.Width, bounds.Height);

        using (var ctx = bitmap.CreateDrawingContext())
        {
            var transform = Matrix.CreateTranslation(-bounds.X, -bounds.Y)
                            * Matrix.CreateScale(scale, scale)
                            * Matrix.CreateTranslation(1, 1);

            using (ctx.PushTransform(transform))
            {
                ctx.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness / scale), Geometry);
            }
        }

        return new Cursor(bitmap, new PixelPoint(HotspotX, HotspotY));
    }
}