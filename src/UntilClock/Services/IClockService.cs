namespace UntilClock.Services;

/// <summary>Abstracts the system clock to enable deterministic testing.</summary>
public interface IClockService
{
    /// <summary>Gets the current local date and time.</summary>
    DateTime Now { get; }
}
