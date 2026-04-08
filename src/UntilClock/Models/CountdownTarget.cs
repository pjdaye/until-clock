namespace UntilClock.Models;

/// <summary>
/// Represents a named target date/time to count down toward.
/// </summary>
public class CountdownTarget
{
    /// <summary>Gets or sets the display name for this countdown.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the target date and time to count down to.</summary>
    public DateTime TargetDateTime { get; set; }

    /// <summary>
    /// Returns the remaining <see cref="TimeSpan"/> until <see cref="TargetDateTime"/>.
    /// Returns <see cref="TimeSpan.Zero"/> once the target has passed.
    /// </summary>
    public TimeSpan Remaining => TargetDateTime > DateTime.Now
        ? TargetDateTime - DateTime.Now
        : TimeSpan.Zero;

    /// <summary>Gets whether the countdown has finished.</summary>
    public bool IsExpired => DateTime.Now >= TargetDateTime;

    /// <summary>Gets or sets the date/time the countdown was started (used for day-level progress calculation).</summary>
    public DateTime CountdownStartDateTime { get; set; }

    /// <summary>
    /// Gets the minute-level progress (0.0–1.0): fraction of current hour remaining.
    /// E.g. 45 remaining minutes → 0.75.
    /// </summary>
    public double MinuteProgress => IsExpired ? 0.0 : Math.Clamp(Remaining.Minutes / 60.0, 0.0, 1.0);

    /// <summary>
    /// Gets the hour-level progress (0.0–1.0): fraction of current day remaining.
    /// E.g. 18 remaining hours → 0.75.
    /// </summary>
    public double HourProgress => IsExpired ? 0.0 : Math.Clamp(Remaining.Hours / 24.0, 0.0, 1.0);

    /// <summary>
    /// Gets the day-level progress (0.0–1.0): fraction of total countdown duration elapsed.
    /// Uses <see cref="CountdownStartDateTime"/> as baseline.
    /// Returns 0.0 if <see cref="CountdownStartDateTime"/> equals <see cref="TargetDateTime"/>.
    /// </summary>
    public double DayProgress
    {
        get
        {
            if (IsExpired) return 0.0;
            var totalDuration = TargetDateTime - CountdownStartDateTime;
            if (totalDuration <= TimeSpan.Zero) return 0.0;
            var elapsed = DateTime.Now - CountdownStartDateTime;
            return Math.Clamp(elapsed / totalDuration, 0.0, 1.0);
        }
    }
}
