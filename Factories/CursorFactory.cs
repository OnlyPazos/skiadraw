using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace skiadraw.Factories;

public static class CursorFactory
{
    public static Cursor FromGeometry(
        Geometry geometry,
        IBrush fill, IBrush stroke, double strokeThickness,
        int size, int hotspotX, int hotspotY)
    {
        var bitmap = new RenderTargetBitmap(new PixelSize(size, size), new Vector(96, 96));
        var bounds = geometry.Bounds;

        double margin = strokeThickness / 2 + 0.5;
        double scale = (size - 2 * margin) / Math.Max(bounds.Width, bounds.Height);

        using (var ctx = bitmap.CreateDrawingContext())
        {
            var transform = Matrix.CreateTranslation(-bounds.X, -bounds.Y)
                            * Matrix.CreateScale(scale, scale)
                            * Matrix.CreateTranslation(margin, margin);

            using (ctx.PushTransform(transform))
            {
                ctx.DrawGeometry(fill, new Pen(stroke, strokeThickness / scale, lineJoin: PenLineJoin.Round), geometry);
            }
        }

        return new Cursor(bitmap, new PixelPoint(hotspotX, hotspotY));
    }
}