using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoordsTool.Core.Coordinates;
using CoordsTool.Core.IO;
using CoordsTool.Core.UserData;

namespace CoordsTool.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ClipboardMonitor _clipboardMonitor;
        private UserCoordinates? _currentEditedCoordinates;

        public ObservableCollection<UserCoordinates> CoordinatesList { get; }

        public MainWindow()
        {
            _clipboardMonitor = new ClipboardMonitor(OnClipboardUpdated);

            InitializeComponent();
            DataContext = this;

            CoordinatesList = new ObservableCollection<UserCoordinates>(UserDataFileManager.ReadCoordinatesList());
        }

        protected override void OnClosed(EventArgs e)
        {
            UserDataFileManager.WriteCoordinatesList(new List<UserCoordinates>(CoordinatesList));

            base.OnClosed(e);
        }

        private void AutoScrollCoordinatesTable()
        {
            if (VisualTreeHelper.GetChildrenCount(CoordinatesTable) == 0)
            {
                return;
            }

            var scrollViewer = VisualTreeHelper.GetChild(CoordinatesTable, 0) as ScrollViewer;
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
            if (_currentEditedCoordinates is not null)
            {
                ChangeUserCoordinatesLabel();
            }
            else if (!TryAddUserCoordinates())
            {
                return;
            }

            CoordinatesTextBox.Clear();
            LabelTextBox.Clear();
        }

        private void ChangeUserCoordinatesLabel()
        {
            var currentEditedCoordinatesIndex = CoordinatesList.IndexOf(_currentEditedCoordinates!);

            _currentEditedCoordinates!.Label = LabelTextBox.Text;

            CoordinatesList.Remove(_currentEditedCoordinates);
            CoordinatesList.Insert(currentEditedCoordinatesIndex, _currentEditedCoordinates);

            SetCoordinatesEditingIsEnabled(true);

            _currentEditedCoordinates = null;
        }

        private bool TryAddUserCoordinates()
        {
            if (!TryGetSelectedDimension(out var dimension))
            {
                SignalInvalid(DimensionButtonsBorder);
                return false;
            }

            if (!InputParser.TryParseManualInput(CoordinatesTextBox.Text, dimension, out var coordinates))
            {
                SignalInvalid(CoordinatesTextBox);
                return false;
            }

            CoordinatesList.Add(new UserCoordinates
            {
                Coordinates = coordinates,
                Label = LabelTextBox.Text,
                Type = UserCoordinatesType.Manual,
                TimeAdded = DateTime.Now
            });

            AutoScrollCoordinatesTable();

            return true;
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

        private void OnEditCoordinates(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not GridTableCell { DataContext: UserCoordinates coordinates })
            {
                return;
            }

            CoordinatesTextBox.Text = coordinates.Coordinates.ToString();
            SetSelectedDimension(coordinates.Coordinates.Dimension);

            SetCoordinatesEditingIsEnabled(false);

            LabelTextBox.Text = coordinates.Label;

            _currentEditedCoordinates = coordinates;
        }

        private void OnDeleteCoordinates(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not GridTableCell { DataContext: UserCoordinates coordinates })
            {
                return;
            }

            CoordinatesList.Remove(coordinates);
        }

        private void SetSelectedDimension(MinecraftDimension dimension)
        {
            var dimensionButton = dimension switch
            {
                MinecraftDimension.Overworld => OverworldRadioButton,
                MinecraftDimension.Nether => NetherRadioButton,
                MinecraftDimension.End or _ => EndRadioButton
            };

            dimensionButton.IsChecked = true;
        }

        private void SetCoordinatesEditingIsEnabled(bool isEnabled)
        {
            CoordinatesTextBox.IsEnabled = isEnabled;

            OverworldRadioButton.IsEnabled = isEnabled;
            NetherRadioButton.IsEnabled = isEnabled;
            EndRadioButton.IsEnabled = isEnabled;
        }

        private void ReadFromClipboardChecked(object sender, RoutedEventArgs e)
        {
            _clipboardMonitor.Enable();
        }

        private void ReadFromClipboardUnchecked(object sender, RoutedEventArgs e)
        {
            _clipboardMonitor.Disable();
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSaveCoordinates(sender, e);
            }
        }
    }
}