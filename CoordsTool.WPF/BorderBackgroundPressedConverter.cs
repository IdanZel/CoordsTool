using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CoordsTool.WPF;

public class BorderBackgroundPressedConverter : IValueConverter
{
    private const double OpacityChange = 0.1;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Brush brush)
        {
            throw new NotSupportedException();
        }

        var modifiedBrush = brush.CloneCurrentValue();
        modifiedBrush.Opacity += OpacityChange;

        return modifiedBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}