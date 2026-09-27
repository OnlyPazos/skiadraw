using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Input;
using skiadraw.Models;

namespace skiadraw.Converters;

public class ToolToCursorConverter : IValueConverter
{
    public static readonly ToolToCursorConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Tool tool) return new Cursor(StandardCursorType.Arrow);

        var cursorType = tool switch
        {
            Tool.Hand => StandardCursorType.Hand,
            Tool.Selection => StandardCursorType.Arrow,
            Tool.Rectangle => StandardCursorType.Cross,
            Tool.Diamond => StandardCursorType.Cross,
            Tool.Ellipse => StandardCursorType.Cross,
            Tool.Arrow => StandardCursorType.Cross,
            Tool.Line => StandardCursorType.Cross,
            Tool.Draw => StandardCursorType.Cross,
            Tool.Text => StandardCursorType.Cross,
            Tool.Eraser => StandardCursorType.Cross,
            _ => StandardCursorType.Arrow
        };

        return new Cursor(cursorType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}