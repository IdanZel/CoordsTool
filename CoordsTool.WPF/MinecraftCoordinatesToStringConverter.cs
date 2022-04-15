using System;
using System.Globalization;
using System.Windows.Data;
using CoordsTool.Core.Coordinates;

namespace CoordsTool.WPF;

public class MinecraftCoordinatesToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MinecraftCoordinates coordinates)
        {
            throw new NotSupportedException();
        }

        var convertDimension = parameter as bool? ?? false;

        return convertDimension
            ? coordinates.ConvertDimension()?.ToString() ?? string.Empty
            : coordinates.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}