using System.Windows;
using System.Windows.Input;
using CoordsTool.Core.UserData;

namespace CoordsTool.WPF;

public partial class SettingsWindow : Window
{
    public SettingsWindow(UserSettings settings)
    {
        InitializeComponent();
        DataContext = settings;
    }

    private void OnClickOK(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
}