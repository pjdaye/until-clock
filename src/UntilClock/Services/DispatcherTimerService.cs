using System.Windows.Threading;

namespace UntilClock.Services;

/// <summary>
/// <see cref="ITimerService"/> backed by a WPF <see cref="DispatcherTimer"/>
/// so ticks arrive on the UI thread.
/// </summary>
[Obsolete("Use MinuteBoundaryTimerService for production. Kept for testing convenience.")]
public sealed class DispatcherTimerService : ITimerService, IDisposable
{
    private readonly DispatcherTimer _timer;

    public DispatcherTimerService(TimeSpan interval)
    {
        _timer = new DispatcherTimer { Interval = interval };
        _timer.Tick += (s, e) => Tick?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public event EventHandler? Tick;

    /// <inheritdoc />
    public TimeSpan Interval => _timer.Interval;

    /// <inheritdoc />
    public void Start() => _timer.Start();

    /// <inheritdoc />
    public void Stop() => _timer.Stop();

    /// <inheritdoc />
    public void Dispose() => _timer.Stop();
}
