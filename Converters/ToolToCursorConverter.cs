using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using skiadraw.Factories;
using skiadraw.Models;

namespace skiadraw.Converters;

public class ToolToCursorConverter : IValueConverter
{
    public static readonly ToolToCursorConverter Instance = new();
    private record CursorSpec(string ResourceKey, int HotspotX, int HotspotY);
    private static readonly Dictionary<Tool, CursorSpec> Specs = new()
    {
        [Tool.Hand]      = new CursorSpec("HandOpenRegular", 12, 12),
        [Tool.Selection] = new CursorSpec("CursorRegular", 2, 2),
        [Tool.Rectangle] = new CursorSpec("CrosshairRegular", 12, 12),
        [Tool.Diamond]   = new CursorSpec("CrosshairRegular", 12, 12),
        [Tool.Ellipse]   = new CursorSpec("CrosshairRegular", 12, 12),
        [Tool.Arrow]     = new CursorSpec("CrosshairRegular", 12, 12),
        [Tool.Line]      = new CursorSpec("CrosshairRegular", 12, 12),
        [Tool.Draw]      = new CursorSpec("PenRegular", 2, 22),
        [Tool.Eraser]    = new CursorSpec("EraserRegular", 4, 20),
    };

    private static readonly Dictionary<Tool, Cursor> Cache = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Tool tool)
            return new Cursor(StandardCursorType.Arrow);

        if (Cache.TryGetValue(tool, out var cached))
            return cached;

        var cursor = CreateCursor(tool);
        Cache[tool] = cursor;
        return cursor;
    }

    private static Cursor CreateCursor(Tool tool)
    {
        if (Specs.TryGetValue(tool, out var spec) &&
            Application.Current!.TryFindResource(spec.ResourceKey, out var res) &&
            res is Geometry geometry)
        {
            return CursorFactory.FromGeometry(
                geometry,
                fill: Brushes.Transparent, stroke: Brushes.White, strokeThickness: 2,
                size: 24, hotspotX: spec.HotspotX, hotspotY: spec.HotspotY);
        }

        // Plan B: cursor estándar si falta el recurso
        return new Cursor(tool == Tool.Hand ? StandardCursorType.Hand : StandardCursorType.Cross);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}