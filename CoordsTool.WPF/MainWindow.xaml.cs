using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CoordsTool.Core.Coordinates;
using CoordsTool.Core.IO;
using CoordsTool.Core.UserData;
using static CoordsTool.WPF.MinecraftCoordinatesToStringConverter;

namespace CoordsTool.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserSettings _settings;
        private readonly ClipboardMonitor _clipboardMonitor;

        public ObservableCollection<UserCoordinates> CoordinatesList { get; }

        public MainWindow()
        {
            _clipboardMonitor = new ClipboardMonitor(OnClipboardUpdated);

            InitializeComponent();
            DataContext = this;

            var coordinatesList = UserDataFileManager.ReadCoordinatesList();
            CoordinatesList = new ObservableCollection<UserCoordinates>(coordinatesList);

            _settings = UserDataFileManager.ReadSettings();
            UpdateSettings();
        }

        protected override void OnClosed(EventArgs e)
        {
            UserDataFileManager.WriteCoordinatesList(new List<UserCoordinates>(CoordinatesList));
            UserDataFileManager.WriteSettings(_settings);

            base.OnClosed(e);
        }

        private void AutoScrollCoordinatesTable()
        {
            if (VisualTreeHelper.GetChildrenCount(CoordinatesTable) == 0 || 
                VisualTreeHelper.GetChild(CoordinatesTable, 0) is not Border border)
            {
                return;
            }

            var scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
            scrollViewer?.ScrollToEnd();
        }

        private void OnClipboardUpdated(string text)
        {
            if (!InputParser.TryParseF3C(text, out var coordinates))
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                CoordinatesList.Add(new UserCoordinates
                {
                    Coordinates = coordinates,
                    Label = string.Empty,
                    Type = UserCoordinatesType.Auto,
                    TimeAdded = DateTime.Now
                });

                AutoScrollCoordinatesTable();
            });
        }

        private void OnSaveCoordinates(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedDimension(out var dimension))
            {
                SignalInvalid(DimensionButtonsBorder);
                return;
            }

            if (!InputParser.TryParseManualInput(CoordinatesTextBox.Text, dimension, out var coordinates))
            {
                SignalInvalid(CoordinatesTextBox);
                return;
            }

            CoordinatesList.Add(new UserCoordinates
            {
                Coordinates = coordinates,
                Label = LabelTextBox.Text,
                Type = UserCoordinatesType.Manual,
                TimeAdded = DateTime.Now
            });

            AutoScrollCoordinatesTable();

            CoordinatesTextBox.Clear();
            LabelTextBox.Clear();
        }

        private bool TryGetSelectedDimension(out MinecraftDimension dimension)
        {
            if (OverworldRadioButton.IsChecked ?? false)
            {
                dimension = MinecraftDimension.Overworld;
            }
            else if (NetherRadioButton.IsChecked ?? false)
            {
                dimension = MinecraftDimension.Nether;
            }
            else if (EndRadioButton.IsChecked ?? false)
            {
                dimension = MinecraftDimension.End;
            }
            else
            {
                dimension = default;
                return false;
            }

            return true;
        }

        private static async void SignalInvalid(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);

            await Task.Delay(200);

            control.ClearValue(BorderBrushProperty);
            control.ClearValue(BorderThicknessProperty);
        }

        private static async void SignalInvalid(Border border)
        {
            border.BorderBrush = Brushes.Red;

            await Task.Delay(200);

            border.ClearValue(BorderBrushProperty);
        }

        private void OnDeleteCoordinates(object sender, RoutedEventArgs e)
        {
            if (sender is not Button { DataContext: UserCoordinates coordinates })
            {
                return;
            }

            CoordinatesList.Remove(coordinates);
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSaveCoordinates(sender, e);
            }
        }

        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_settings);
            settingsWindow.ShowDialog();

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (_settings.ShouldReadFromClipboard)
            {
                _clipboardMonitor.Enable();
            }
            else
            {
                _clipboardMonitor.Disable();
            }

            UseChunkCoordinates[MinecraftDimension.Overworld] = _settings.UseChunkCoordinatesOverworld;
            UseChunkCoordinates[MinecraftDimension.Nether] = _settings.UseChunkCoordinatesNether;
            UseChunkCoordinates[MinecraftDimension.End] = _settings.UseChunkCoordinatesEnd;

            // This forces the CoordinatesList to refresh and apply the updated "UserChunkCoordinates" values
            CollectionViewSource.GetDefaultView(CoordinatesList).Refresh();
        }

        private void OnClickMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}