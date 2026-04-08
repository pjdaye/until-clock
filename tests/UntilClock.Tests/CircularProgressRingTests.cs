using UntilClock.Models;

namespace UntilClock.Tests;

public class CircularProgressRingTests
{
    // MinuteProgress = Remaining.Minutes / 60.0 (where .Minutes is the 0–59 component)

    [Theory]
    [InlineData(45)]  // ~45 min remaining → MinuteProgress ≈ 0.75
    [InlineData(30)]  // ~30 min remaining → MinuteProgress ≈ 0.50
    [InlineData(10)]  // ~10 min remaining → MinuteProgress ≈ 0.17
    public void MinuteProgress_IsClamped_BetweenZeroAndOne(int minutesRemaining)
    {
        var target = new CountdownTarget
        {
            TargetDateTime = DateTime.Now.AddMinutes(minutesRemaining).AddSeconds(30)
        };
        Assert.InRange(target.MinuteProgress, 0.0, 1.0);
    }

    [Fact]
    public void HourProgress_WithZeroHoursRemaining_IsZeroWhenExpired()
    {
        var target = new CountdownTarget { TargetDateTime = DateTime.Now.AddSeconds(-1) };
        Assert.Equal(0.0, target.HourProgress);
    }

    [Fact]
    public void DayProgress_IncreasesOverTime()
    {
        // start=2h ago, target=2h from now → total=4h, elapsed=2h → ≈0.5
        var start = DateTime.Now.AddHours(-2);
        var target = new CountdownTarget
        {
            CountdownStartDateTime = start,
            TargetDateTime = DateTime.Now.AddHours(2)
        };
        Assert.InRange(target.DayProgress, 0.40, 0.60);
    }

    [Fact]
    public void DayProgress_ReturnsZero_WhenStartEqualsTarget()
    {
        var now = DateTime.Now;
        var target = new CountdownTarget
        {
            CountdownStartDateTime = now,
            TargetDateTime = now
        };
        Assert.Equal(0.0, target.DayProgress);
    }

    [Fact]
    public void AllProgress_AreZero_WhenExpired()
    {
        var target = new CountdownTarget
        {
            CountdownStartDateTime = DateTime.Now.AddDays(-2),
            TargetDateTime = DateTime.Now.AddSeconds(-1)
        };
        Assert.Equal(0.0, target.MinuteProgress);
        Assert.Equal(0.0, target.HourProgress);
        Assert.Equal(0.0, target.DayProgress);
    }
}
