using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace skiadraw.Models;

public static class ResourceHelper
{
    public static T GetResource<T>(string key, T fallback)
    {
        var app = Application.Current;
        if (app is not null &&
            app.TryFindResource(key, app.ActualThemeVariant, out var value) &&
            value is T typed)
        {
            return typed;
        }

        return fallback;
    }
}

public abstract partial class ShapeModel : ObservableObject
{
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _height;
    [ObservableProperty] private double _rotation;
    [ObservableProperty] private IBrush _fill;
    [ObservableProperty] private IBrush _stroke;
    [ObservableProperty] private string _strokeDash;
    [ObservableProperty] private double _strokeThickness;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private bool _isVisible;
    
    public Point Center => new(X + Width / 2, Y + Height / 2);

    public AvaloniaList<double> StrokeDashArray =>
        string.IsNullOrWhiteSpace(StrokeDash)
            ? new AvaloniaList<double>()
            : new AvaloniaList<double>(StrokeDash
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(p => double.Parse(p, CultureInfo.InvariantCulture)));

    protected ShapeModel()
    {
        var fillColor = ResourceHelper.GetResource("ColorSurface", Colors.Transparent);
        var strokeColor = ResourceHelper.GetResource("ColorSurfaceHover", Colors.Black);
        var fillBrush = new SolidColorBrush(fillColor);
        var strokeBrush = new SolidColorBrush(strokeColor);

        _fill = fillBrush;
        _stroke = strokeBrush;
        _strokeThickness = ResourceHelper.GetResource("StrokeMedium", 2.0);
        _strokeDash = ResourceHelper.GetResource("StrokeContinuous", "0,0");
    }
}

public partial class RectangleShape : ShapeModel
{
    [ObservableProperty] private double _radius = ResourceHelper.GetResource("RadiusExtraLarge", 0.0);
}

public partial class DiamondShape : ShapeModel
{
    [ObservableProperty] private double _radius = ResourceHelper.GetResource("RadiusExtraLarge", 0.0);
}

public partial class EllipseShape : ShapeModel
{
}