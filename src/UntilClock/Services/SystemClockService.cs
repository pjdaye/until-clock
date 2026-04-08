namespace UntilClock.Services;

/// <summary>Production <see cref="IClockService"/> backed by <see cref="DateTime.Now"/>.</summary>
public sealed class SystemClockService : IClockService
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;
}
