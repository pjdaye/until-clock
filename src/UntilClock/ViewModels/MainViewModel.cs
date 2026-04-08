using System.Windows.Input;
using UntilClock.Models;
using UntilClock.Services;

namespace UntilClock.ViewModels;

/// <summary>
/// ViewModel for the main countdown clock window.
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly ITimerService _timer;

    private CountdownTarget _target = new()
    {
        Name = "My Countdown",
        TargetDateTime = DateTime.Now.AddHours(1)
    };

    private TimeSpan _remaining;
    private string _displayText = string.Empty;

    public MainViewModel(ITimerService timer)
    {
        _timer = timer;
        _timer.Tick += OnTimerTick;

        SetTargetCommand = new RelayCommand(_ => { /* TODO: open date-picker dialog */ });
        ResetCommand = new RelayCommand(_ => Reset());

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

    /// <summary>Gets the target date/time for the active countdown.</summary>
    public DateTime TargetDateTime
    {
        get => _target.TargetDateTime;
        set
        {
            _target.TargetDateTime = value;
            OnPropertyChanged();
            Refresh();
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

    /// <summary>Command to open the target date/time picker.</summary>
    public ICommand SetTargetCommand { get; }

    /// <summary>Command to reset the countdown to default.</summary>
    public ICommand ResetCommand { get; }

    // -----------------------------------------------------------------------
    private void OnTimerTick(object? sender, EventArgs e) => Refresh();

    private void Refresh()
    {
        Remaining = _target.Remaining;
        DisplayText = FormatRemaining(Remaining);
        OnPropertyChanged(nameof(IsExpired));
    }

    private void Reset()
    {
        _target = new CountdownTarget
        {
            Name = "My Countdown",
            TargetDateTime = DateTime.Now.AddHours(1)
        };
        OnPropertyChanged(nameof(EventName));
        OnPropertyChanged(nameof(TargetDateTime));
        Refresh();
    }

    private static string FormatRemaining(TimeSpan span)
    {
        if (span <= TimeSpan.Zero)
            return "00:00:00";

        return span.Days > 0
            ? $"{span.Days}d {span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}"
            : $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }
}
