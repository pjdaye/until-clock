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
}
