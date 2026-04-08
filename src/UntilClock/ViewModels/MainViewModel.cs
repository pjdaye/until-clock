using System.Windows.Input;
using UntilClock.Helpers;
using UntilClock.Models;
using UntilClock.Services;

namespace UntilClock.ViewModels;

/// <summary>
/// ViewModel for the main countdown clock window.
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly ITimerService _timer;
    private readonly IPersistenceService _persistence;
    private readonly IDialogService _dialogService;

    private CountdownTarget _target = new()
    {
        Name = "My Countdown",
        TargetDateTime = DateTime.Now.AddHours(1),
        CountdownStartDateTime = DateTime.Now
    };

    private TimeSpan _remaining;
    private string _displayText = string.Empty;
    private bool _isFirstLaunch;

    public MainViewModel(ITimerService timer, IPersistenceService persistence, IDialogService dialogService)
    {
        _timer = timer;
        _persistence = persistence;
        _dialogService = dialogService;
        _timer.Tick += OnTimerTick;

        SetTargetCommand = new RelayCommand(_ =>
        {
            var result = _dialogService.ShowSetTargetDialog(_target.TargetDateTime);
            if (result is DateTime chosen)
            {
                _target.TargetDateTime = chosen;
                _target.CountdownStartDateTime = DateTime.Now;
                OnPropertyChanged(nameof(TargetDateTime));
                OnPropertyChanged(nameof(CountdownStartDateTime));
                Refresh();
                Save();
            }
            // Cancel: no-op
        });
        ResetCommand = new RelayCommand(_ => Reset());

        var data = _persistence.Load();
        if (data != null)
        {
            _target.TargetDateTime = data.TargetDateTime;
            _target.CountdownStartDateTime = data.CountdownStartDateTime;
            IsFirstLaunch = false;
        }
        else
        {
            IsFirstLaunch = true;
        }

        Refresh();
        _timer.Start();
    }

    /// <summary>Gets the name of the active countdown.</summary>
    public string EventName
    {
        get => _target.Name;
        set
        {
            _target.Name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Gets or sets whether this is the first launch (no saved data found).</summary>
    public bool IsFirstLaunch
    {
        get => _isFirstLaunch;
        set => SetProperty(ref _isFirstLaunch, value);
    }

    /// <summary>Gets the target date/time for the active countdown.</summary>
    public DateTime TargetDateTime
    {
        get => _target.TargetDateTime;
        set
        {
            _target.TargetDateTime = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TargetDisplayText));
            Refresh();
            Save();
        }
    }

    /// <summary>Gets the remaining time as a <see cref="TimeSpan"/>.</summary>
    public TimeSpan Remaining
    {
        get => _remaining;
        private set => SetProperty(ref _remaining, value);
    }

    /// <summary>Gets the formatted display string (e.g. "2d 03:45:12").</summary>
    public string DisplayText
    {
        get => _displayText;
        private set => SetProperty(ref _displayText, value);
    }

    /// <summary>Gets whether the countdown has expired.</summary>
    public bool IsExpired => _target.IsExpired;

    /// <summary>Gets whether the countdown has completed (now >= target).</summary>
    public bool IsComplete => _target.IsExpired;

    /// <summary>Gets the days component of remaining time.</summary>
    public int RemainingDays => _remaining.Days;

    /// <summary>Gets the hours component of remaining time.</summary>
    public int RemainingHours => _remaining.Hours;

    /// <summary>Gets the minutes component of remaining time.</summary>
    public int RemainingMinutes => _remaining.Minutes;

    /// <summary>Gets the minute-level progress (0.0–1.0).</summary>
    public double MinuteProgress => _target.MinuteProgress;

    /// <summary>Gets the hour-level progress (0.0–1.0).</summary>
    public double HourProgress => _target.HourProgress;

    /// <summary>Gets the day-level progress (0.0–1.0).</summary>
    public double DayProgress => _target.DayProgress;

    /// <summary>Gets the formatted target date/time for display (e.g. "Apr 10, 2026 at 5:00 PM").</summary>
    public string TargetDisplayText => _target.TargetDateTime.ToString("MMM d, yyyy 'at' h:mm tt");

    /// <summary>Gets or sets the date/time the countdown was started.</summary>
    public DateTime CountdownStartDateTime
    {
        get => _target.CountdownStartDateTime;
        set
        {
            _target.CountdownStartDateTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Command to open the target date/time picker.</summary>
    public ICommand SetTargetCommand { get; }

    /// <summary>Command to reset the countdown to default.</summary>
    public ICommand ResetCommand { get; }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void OnTimerTick(object? sender, EventArgs e) => Refresh();

    private void Refresh()
    {
        Remaining = _target.Remaining;
        DisplayText = FormatRemaining(Remaining);
        OnPropertyChanged(nameof(IsExpired));
        OnPropertyChanged(nameof(IsComplete));
        OnPropertyChanged(nameof(RemainingDays));
        OnPropertyChanged(nameof(RemainingHours));
        OnPropertyChanged(nameof(RemainingMinutes));
        OnPropertyChanged(nameof(MinuteProgress));
        OnPropertyChanged(nameof(HourProgress));
        OnPropertyChanged(nameof(DayProgress));
        OnPropertyChanged(nameof(TargetDisplayText));
    }

    private void Reset()
    {
        _target = new CountdownTarget
        {
            Name = "My Countdown",
            TargetDateTime = DateTime.Now.AddHours(1),
            CountdownStartDateTime = DateTime.Now
        };
        OnPropertyChanged(nameof(EventName));
        OnPropertyChanged(nameof(TargetDateTime));
        OnPropertyChanged(nameof(CountdownStartDateTime));
        Refresh();
        Save();
    }

    private void Save()
    {
        _persistence.Save(new CountdownData
        {
            TargetDateTime = _target.TargetDateTime,
            CountdownStartDateTime = _target.CountdownStartDateTime
        });
    }

    private static string FormatRemaining(TimeSpan span) => CountdownFormatter.Format(span);
}
