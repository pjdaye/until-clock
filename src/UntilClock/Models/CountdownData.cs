namespace UntilClock.Models;

/// <summary>Persisted countdown state, serialized to/from JSON.</summary>
public class CountdownData
{
    /// <summary>The target date/time to count down toward.</summary>
    public DateTime TargetDateTime { get; set; }

    /// <summary>When this countdown was started (used for day-level progress).</summary>
    public DateTime CountdownStartDateTime { get; set; }
}
