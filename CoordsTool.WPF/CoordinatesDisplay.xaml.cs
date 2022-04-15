using System.Windows;
using System.Windows.Controls;
using CoordsTool.Core.Coordinates;

namespace CoordsTool.WPF;

public partial class CoordinatesDisplay : UserControl
{
    public static readonly DependencyProperty CoordinatesProperty = DependencyProperty.Register("Coordinates",
        typeof(MinecraftCoordinates), typeof(CoordinatesDisplay), new PropertyMetadata(default(MinecraftCoordinates)));

    public MinecraftCoordinates Coordinates
    {
        get => (MinecraftCoordinates)GetValue(CoordinatesProperty);
        set => SetValue(CoordinatesProperty, value);
    }

    public CoordinatesDisplay()
    {
        InitializeComponent();
    }
}