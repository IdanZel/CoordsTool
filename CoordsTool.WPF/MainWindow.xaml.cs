using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CoordsTool.Core.Coordinates;
using CoordsTool.Core.IO;
using CoordsTool.Core.UserData;
using CoordsTool.Network;
using static CoordsTool.WPF.MinecraftCoordinatesToStringConverter;

namespace CoordsTool.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int AssemblyVersionFieldCount = 3;

        private readonly UserSettings _settings;
        private readonly ClipboardMonitor _clipboardMonitor;
        private readonly Stack<UserCoordinates> _deletedCoordinates;

        private string? _latestReleaseUrl;

        public ObservableCollection<UserCoordinates> CoordinatesList { get; }

        public MainWindow()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener($"{DateTime.Now:yyyyMMdd-HHmmss}.log"));

            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
            Dispatcher.UnhandledException += ExceptionHandler;
            Application.Current.DispatcherUnhandledException += ExceptionHandler;
            TaskScheduler.UnobservedTaskException += ExceptionHandler;

            _clipboardMonitor = new ClipboardMonitor(OnClipboardUpdated);
            _deletedCoordinates = new Stack<UserCoordinates>();

            InitializeComponent();
            DataContext = this;

            var coordinatesList = UserDataFileManager.ReadCoordinatesList();
            CoordinatesList = new ObservableCollection<UserCoordinates>(coordinatesList);
            CoordinatesList.CollectionChanged += (_, _) => UserDataFileManager.WriteCoordinatesList(CoordinatesList);

            _settings = UserDataFileManager.ReadSettings();
            UpdateSettings();

            _latestReleaseUrl = null;

            var currentVersion = typeof(MainWindow).Assembly.GetName().Version?.ToString(AssemblyVersionFieldCount);
            Updates.CheckForUpdates(currentVersion).ContinueWith(OnCheckForUpdatesCompleted);
        }

        private static void ExceptionHandler(object? sender, EventArgs args)
        {
            var exception = args switch
            {
                UnhandledExceptionEventArgs unhandledArgs => unhandledArgs.ExceptionObject as Exception,
                DispatcherUnhandledExceptionEventArgs dispatcherUnhandledArgs => dispatcherUnhandledArgs.Exception,
                UnobservedTaskExceptionEventArgs unobservedArgs => unobservedArgs.Exception,
                _ => null
            };

            Trace.WriteLine("An exception was thrown by " + sender + Environment.NewLine + exception);
        }

        protected override void OnClosed(EventArgs e)
        {
            UserDataFileManager.WriteCoordinatesList(CoordinatesList);
            UserDataFileManager.WriteSettings(_settings);

            base.OnClosed(e);
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

        private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void OnClickMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSaveCoordinates(sender, e);
            }
        }

        private void OnDeleteCoordinates(object sender, RoutedEventArgs e)
        {
            if (sender is not Button { DataContext: UserCoordinates coordinates })
            {
                return;
            }

            CoordinatesList.Remove(coordinates);
            _deletedCoordinates.Push(coordinates);
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

        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_settings, this);
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

            TimeColumn.Visibility = _settings.DisplayTimeColumn ? Visibility.Visible : Visibility.Hidden;

            Topmost = _settings.AlwaysOnTop;

            UseChunkCoordinates[MinecraftDimension.Overworld] = _settings.UseChunkCoordinatesOverworld;
            UseChunkCoordinates[MinecraftDimension.Nether] = _settings.UseChunkCoordinatesNether;
            UseChunkCoordinates[MinecraftDimension.End] = _settings.UseChunkCoordinatesEnd;

            DisplayYLevel = _settings.DisplayYLevel;

            UserDataFileManager.WriteSettings(_settings);

            // This forces the CoordinatesList to refresh and apply the updated "UseChunkCoordinates" values
            CollectionViewSource.GetDefaultView(CoordinatesList).Refresh();
        }

        private void OnEditCoordinatesTableLabelCell(object? sender, DataGridBeginningEditEventArgs e)
        {
            if (e.EditingEventArgs is not MouseButtonEventArgs)
            {
                e.Cancel = true;
            }
        }

        private void OnClearAllDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (var coordinates in CoordinatesList)
            {
                _deletedCoordinates.Push(coordinates);
            }

            CoordinatesList.Clear();
        }

        private void OnClearAllSingleClick(object sender, RoutedEventArgs e)
        {
            SetToolTipIsOpen(sender, true);
        }

        private void OnClearAllMouseLeave(object sender, MouseEventArgs e)
        {
            SetToolTipIsOpen(sender, false);
        }

        private static void SetToolTipIsOpen(object sender, bool isOpen)
        {
            if (sender is Button { ToolTip: ToolTip toolTip })
            {
                toolTip.IsOpen = isOpen;
            }
        }

        private void OnClickRestore(object sender, RoutedEventArgs e)
        {
            if (_deletedCoordinates.Count == 0) return;

            var coordinates = _deletedCoordinates.Pop();
            CoordinatesList.Add(coordinates);
        }

        private void OnCheckForUpdatesCompleted(Task<(bool IsAvailable, string ReleaseUrl)> task)
        {
            if (task.IsFaulted)
            {
                Trace.WriteLine("Updates.CheckForUpdates threw an exception: " + task.Exception);
                return;
            }

            (bool isUpdateAvailable, string? releaseUrl) = task.Result;

            if (isUpdateAvailable)
            {
                ShowUpdateAvailable(releaseUrl);
            }
        }

        private void ShowUpdateAvailable(string latestReleaseUrl)
        {
            _latestReleaseUrl = latestReleaseUrl;

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateAvailableButton.Visibility = Visibility.Visible;
                UpdateAvailableButton.IsEnabled = true;
            });
        }

        private void OnClickUpdateAvailable(object sender, RoutedEventArgs e)
        {
            if (_latestReleaseUrl is null) return;

            Process.Start(new ProcessStartInfo(_latestReleaseUrl) { UseShellExecute = true });
        }
    }
}