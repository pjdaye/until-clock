using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Graphics;

namespace UntilClock
{
    public sealed partial class MainWindow : Window
    {
        public CountdownViewModel ViewModel { get; }

        private AppWindow? _appWindow;

        public MainWindow()
        {
            this.InitializeComponent();

            var savedTarget = SettingsService.LoadTargetTime();
            ViewModel = new CountdownViewModel(savedTarget);

            this.Title = "Until Clock";

            // Size the window as a compact widget.
            _appWindow = GetAppWindow();
            _appWindow?.Resize(new SizeInt32(480, 340));
        }

        // ─── Button handlers ─────────────────────────────────────────────────

        /// <summary>
        /// Shows a ContentDialog that lets the user pick a new target date/time.
        /// </summary>
        private async void SetTargetButton_Click(object sender, RoutedEventArgs e)
        {
            var current = ViewModel.TargetTime;

            var datePicker = new DatePicker
            {
                Date = new DateTimeOffset(current),
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            var timePicker = new TimePicker
            {
                Time = current.TimeOfDay,
                ClockIdentifier = "12HourClock",
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            var content = new StackPanel { Spacing = 10, MinWidth = 300 };
            content.Children.Add(new TextBlock { Text = "Date:" });
            content.Children.Add(datePicker);
            content.Children.Add(new TextBlock { Text = "Time:", Margin = new Thickness(0, 8, 0, 0) });
            content.Children.Add(timePicker);

            var dialog = new ContentDialog
            {
                Title = "Set Target Time",
                Content = content,
                PrimaryButtonText = "Set",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.Content.XamlRoot,
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var newTarget = datePicker.Date.Date + timePicker.Time;
                ViewModel.TargetTime = newTarget;
                SettingsService.SaveTargetTime(newTarget);
            }
        }

        /// <summary>
        /// Toggles the always-on-top (topmost) state of the window.
        /// </summary>
        private void AlwaysOnTopToggle_Click(object sender, RoutedEventArgs e)
        {
            if (_appWindow?.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsAlwaysOnTop = AlwaysOnTopToggle.IsChecked == true;
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private AppWindow? GetAppWindow()
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(handle);
            return AppWindow.GetFromWindowId(windowId);
        }
    }
}
