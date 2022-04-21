using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CoordsTool.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnTitleBarMouseDown(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            
        }

        private void OnSaveCoordinates(object? sender, RoutedEventArgs e)
        {
            
        }

        private void OnDeleteCoordinates(object? sender, RoutedEventArgs e)
        {
            
        }

        private void OnSettingsButtonClick(object? sender, RoutedEventArgs e)
        {
            
        }
    }
}