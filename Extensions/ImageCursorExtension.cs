using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace skiadraw.Extensions;

public class ImageCursorExtension : MarkupExtension
{
    public string Source { get; set; } = "";
    public int Size { get; set; } = 32;
    public int HotspotX { get; set; }
    public int HotspotY { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        using var stream = AssetLoader.Open(new Uri(Source));
        using var original = new Bitmap(stream);

        var scaled = original.CreateScaledBitmap(new PixelSize(Size, Size));

        return new Cursor(scaled, new PixelPoint(HotspotX, HotspotY));
    }
}