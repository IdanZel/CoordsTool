using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using CoordsTool.Core.Coordinates;

namespace CoordsTool.WPF;

public class MinecraftCoordinatesToStringConverter : IValueConverter
{
    // A static property is used here to avoid making changes to the CoordinatesDisplay control.
    // The "correct" way to do this would probably be to change this converter to an IMultiValueConverter,
    // add "UseChunkCoordinates" properties to CoordinatesDisplay (for each dimension) and pass both the
    // coordinates and appropriate "UseChunkCoordinates" value to the converter using MultiBinding.
    // In either way, the goal is to apply changes to coordinates in the UI only (as opposed to changing the data).
    public static readonly Dictionary<MinecraftDimension, bool> UseChunkCoordinates = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MinecraftCoordinates coordinates)
        {
            throw new NotSupportedException();
        }

        var convertDimension = parameter as bool? ?? false;

        return ConvertCoordinates(coordinates, convertDimension).ToString();
    }

    private MinecraftCoordinates ConvertCoordinates(MinecraftCoordinates coordinates, bool convertDimension)
    {
        var convertedCoordinates = convertDimension
            ? coordinates.ConvertDimension()
            : coordinates;

        return UseChunkCoordinates.TryGetValue(convertedCoordinates.Dimension, out var useChunkCoordinates) &&
               useChunkCoordinates
            ? convertedCoordinates.ToChunkCoordinates()
            : convertedCoordinates;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}