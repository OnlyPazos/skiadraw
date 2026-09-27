using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace skiadraw.Converters;

public class StrokeMarginConverter : IValueConverter
{
    public static readonly StrokeMarginConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var strokeThickness = value is double d ? d : 0;
        var extraGap = 4.0;
        var margin = (strokeThickness / 2) + extraGap;
        return new Thickness(-margin);

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}