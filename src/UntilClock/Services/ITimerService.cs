namespace UntilClock.Services;

/// <summary>
/// Provides a periodic tick used to drive countdown updates.
/// </summary>
public interface ITimerService
{
    /// <summary>Raised on each tick at the configured interval.</summary>
    event EventHandler Tick;

    /// <summary>Gets the interval between ticks.</summary>
    TimeSpan Interval { get; }

    /// <summary>Starts the timer.</summary>
    void Start();

    /// <summary>Stops the timer.</summary>
    void Stop();
}
