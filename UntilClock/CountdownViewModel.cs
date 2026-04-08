using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UntilClock
{
    /// <summary>
    /// ViewModel that manages countdown state and provides display-ready values
    /// for the main window. Ticks every second via a DispatcherTimer.
    /// </summary>
    public class CountdownViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;
        private DateTime _targetTime;

        private string _daysDisplay = "00";
        private string _hoursDisplay = "00";
        private string _minutesDisplay = "00";
        private string _secondsDisplay = "00";
        private string _targetDisplay = string.Empty;
        private Visibility _daysVisibility = Visibility.Collapsed;
        private Visibility _expiredVisibility = Visibility.Collapsed;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CountdownViewModel(DateTime targetTime)
        {
            _targetTime = targetTime;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (_, _) => UpdateDisplay();
            _timer.Start();

            UpdateDisplay();
        }

        /// <summary>Gets or sets the date/time the countdown is counting toward.</summary>
        public DateTime TargetTime
        {
            get => _targetTime;
            set
            {
                _targetTime = value;
                UpdateDisplay();
                OnPropertyChanged();
            }
        }

        /// <summary>Two-digit day count string (visible only when days &gt; 0).</summary>
        public string DaysDisplay
        {
            get => _daysDisplay;
            private set => SetProperty(ref _daysDisplay, value);
        }

        /// <summary>Two-digit hour string.</summary>
        public string HoursDisplay
        {
            get => _hoursDisplay;
            private set => SetProperty(ref _hoursDisplay, value);
        }

        /// <summary>Two-digit minute string.</summary>
        public string MinutesDisplay
        {
            get => _minutesDisplay;
            private set => SetProperty(ref _minutesDisplay, value);
        }

        /// <summary>Two-digit second string.</summary>
        public string SecondsDisplay
        {
            get => _secondsDisplay;
            private set => SetProperty(ref _secondsDisplay, value);
        }

        /// <summary>Human-readable label showing the target date/time.</summary>
        public string TargetDisplay
        {
            get => _targetDisplay;
            private set => SetProperty(ref _targetDisplay, value);
        }

        /// <summary>Controls visibility of the DAYS section (hidden when &lt; 1 day remaining).</summary>
        public Visibility DaysVisibility
        {
            get => _daysVisibility;
            private set => SetProperty(ref _daysVisibility, value);
        }

        /// <summary>Controls visibility of the "Time's up!" expired message.</summary>
        public Visibility ExpiredVisibility
        {
            get => _expiredVisibility;
            private set => SetProperty(ref _expiredVisibility, value);
        }

        private void UpdateDisplay()
        {
            var now = DateTime.Now;
            var remaining = _targetTime - now;

            TargetDisplay = $"Until: {_targetTime:ddd, MMM d, yyyy h:mm tt}";

            if (remaining <= TimeSpan.Zero)
            {
                ExpiredVisibility = Visibility.Visible;
                DaysVisibility = Visibility.Collapsed;
                DaysDisplay = "00";
                HoursDisplay = "00";
                MinutesDisplay = "00";
                SecondsDisplay = "00";
            }
            else
            {
                ExpiredVisibility = Visibility.Collapsed;

                int days = (int)remaining.TotalDays;
                int hours = remaining.Hours;
                int minutes = remaining.Minutes;
                int seconds = remaining.Seconds;

                DaysVisibility = days > 0 ? Visibility.Visible : Visibility.Collapsed;
                DaysDisplay = days.ToString("D2");
                HoursDisplay = hours.ToString("D2");
                MinutesDisplay = minutes.ToString("D2");
                SecondsDisplay = seconds.ToString("D2");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
