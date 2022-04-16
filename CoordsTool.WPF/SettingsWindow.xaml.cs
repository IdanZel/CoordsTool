using System.Windows;
using CoordsTool.Core.UserData;

namespace CoordsTool.WPF;

public partial class SettingsWindow : Window
{
    public SettingsWindow(UserSettings settings)
    {
        InitializeComponent();
        DataContext = settings;
    }
}