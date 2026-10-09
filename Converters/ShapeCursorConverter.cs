using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Input;
using skiadraw.Models;

namespace skiadraw.Converters;

public class ShapeCursorConverter: IValueConverter
{
    public static readonly ShapeCursorConverter Instance = new();
    private static readonly Cursor SizeAll = new(StandardCursorType.SizeAll);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Tool.Selection
            ? SizeAll
            : ToolToCursorConverter.Instance.Convert(value, targetType, parameter, culture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}