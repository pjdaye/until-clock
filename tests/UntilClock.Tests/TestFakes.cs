using UntilClock.Models;
using UntilClock.Services;

namespace UntilClock.Tests;

public class FakeTimerService : ITimerService
{
    public event EventHandler? Tick;
    public TimeSpan Interval => TimeSpan.FromSeconds(60);
    public void Start() { }
    public void Stop() { }
    public void FireTick() => Tick?.Invoke(this, EventArgs.Empty);
}

public class FakePersistenceService : IPersistenceService
{
    private CountdownData? _stored;
    public bool SaveCalled { get; set; }

    public void Save(CountdownData data)
    {
        _stored = data;
        SaveCalled = true;
    }

    public CountdownData? Load() => _stored;
}

public class FakeDialogService : IDialogService
{
    private readonly DateTime? _result;
    public bool WasCalled { get; private set; }

    public FakeDialogService(DateTime? result = null) => _result = result;

    public DateTime? ShowSetTargetDialog(DateTime current)
    {
        WasCalled = true;
        return _result;
    }
}
