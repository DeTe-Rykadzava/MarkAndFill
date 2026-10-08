using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MarkAndFill.Base.Converters;

public class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color) return new SolidColorBrush(color);

        if (value is string colorString && !string.IsNullOrWhiteSpace(colorString))
            if (Color.TryParse(colorString, out var parsedColor))
                return new SolidColorBrush(parsedColor);

        return Brushes.Transparent; // Значение по умолчанию, если конвертация не удалась
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}