using System;
using System.Globalization;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CoordsTool.Avalonia.Converters;

/// <summary>
/// <para>
/// Converts a string path to a bitmap asset.
/// </para>
/// <para>
/// See: https://github.com/AvaloniaUI/Avalonia/issues/3860#issuecomment-652712504
/// </para>
/// </summary>
public class BitmapAssetValueConverter : IValueConverter
{
    public static BitmapAssetValueConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            null => null,
            string path when targetType == typeof(IImage) => GetBitmapAsset(path),
            _ => throw new NotSupportedException()
        };
    }

    private static object? GetBitmapAsset(string path)
    {
        // Using "GetExecutingAssembly" instead of "GetEntryAssembly" to circumvent preview issues
        var assemblyName = Assembly.GetExecutingAssembly()!.GetName().Name!;
        var uri = new Uri($"avares://{assemblyName}{path}");
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        return new Bitmap(assets!.Open(uri));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}