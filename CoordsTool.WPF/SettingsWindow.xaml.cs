using System;
using System.Windows;
using System.Windows.Input;
using CoordsTool.Core.UserData;

namespace CoordsTool.WPF;

public partial class SettingsWindow : Window
{
    // This buffer is meant to account for the taskbar when calculating the screen's bottom location
    private const int BottomScreenBufferHeight = 48;

    public SettingsWindow(UserSettings settings, Window owner)
    {
        InitializeComponent();
        DataContext = settings;
        Owner = owner;

        Left = Owner.Left;
        Top = GetTopPosition();
    }

    private double GetTopPosition()
    {
        // Ideally, the window would be located right under the owner
        var ownerBottomEdgePosition = Owner.Top + Owner.ActualHeight;

        // Estimate the position of the bottom of the user's screen and subtract the current window's height,
        // in order to ensure the window will not go off-screen
        var maxTopEdgePosition = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight -
                                 BottomScreenBufferHeight - Height;

        return Math.Min(ownerBottomEdgePosition, maxTopEdgePosition);
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