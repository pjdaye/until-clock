using System.Windows.Threading;

namespace UntilClock.Services;

/// <summary>
/// <see cref="ITimerService"/> that fires <see cref="Tick"/> aligned to
/// real-world minute boundaries (:00 seconds), then every 60 seconds.
/// Ticks arrive on the UI thread via <see cref="DispatcherTimer"/>.
/// </summary>
public sealed class MinuteBoundaryTimerService : ITimerService, IDisposable
{
    private readonly IClockService _clock;
    private readonly DispatcherTimer _timer;
    private bool _aligned;

    public MinuteBoundaryTimerService(IClockService clock)
    {
        _clock = clock;
        _timer = new DispatcherTimer();
        _timer.Tick += OnTick;
    }

    /// <inheritdoc />
    public event EventHandler? Tick;

    /// <inheritdoc />
    public TimeSpan Interval => TimeSpan.FromSeconds(60);

    /// <inheritdoc />
    public void Start()
    {
        _aligned = false;
        _timer.Interval = GetInitialInterval();
        _timer.Start();
    }

    /// <inheritdoc />
    public void Stop() => _timer.Stop();

    /// <inheritdoc />
    public void Dispose() => _timer.Stop();

    private void OnTick(object? sender, EventArgs e)
    {
        if (!_aligned)
        {
            // Switch to steady 60s interval after first alignment tick
            _aligned = true;
            _timer.Stop();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Start();
        }
        Tick?.Invoke(this, EventArgs.Empty);
    }

    private TimeSpan GetInitialInterval()
    {
        var now = _clock.Now;
        var secondsUntilNextMinute = 60 - now.Second;
        // Minimum 1 second to avoid immediate double-fire
        return TimeSpan.FromSeconds(Math.Max(secondsUntilNextMinute, 1));
    }
}
