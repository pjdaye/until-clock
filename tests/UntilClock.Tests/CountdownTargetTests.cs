using UntilClock.Models;

namespace UntilClock.Tests;

public class CountdownTargetTests
{
    [Fact]
    public void Remaining_ReturnsPositiveSpan_WhenTargetIsInFuture()
    {
        var target = new CountdownTarget { Name = "Test", TargetDateTime = DateTime.Now.AddHours(1) };

        Assert.True(target.Remaining > TimeSpan.Zero);
        Assert.False(target.IsExpired);
    }

    [Fact]
    public void Remaining_ReturnsZero_WhenTargetIsInPast()
    {
        var target = new CountdownTarget { Name = "Test", TargetDateTime = DateTime.Now.AddSeconds(-1) };

        Assert.Equal(TimeSpan.Zero, target.Remaining);
        Assert.True(target.IsExpired);
    }

    [Fact]
    public void MinuteProgress_ReturnsZero_WhenExpired()
    {
        var target = new CountdownTarget { TargetDateTime = DateTime.Now.AddSeconds(-1) };
        Assert.Equal(0.0, target.MinuteProgress);
    }

    [Fact]
    public void HourProgress_ReturnsZero_WhenExpired()
    {
        var target = new CountdownTarget { TargetDateTime = DateTime.Now.AddSeconds(-1) };
        Assert.Equal(0.0, target.HourProgress);
    }

    [Fact]
    public void DayProgress_ReturnsZero_WhenExpired()
    {
        var target = new CountdownTarget
        {
            CountdownStartDateTime = DateTime.Now.AddDays(-2),
            TargetDateTime = DateTime.Now.AddSeconds(-1)
        };
        Assert.Equal(0.0, target.DayProgress);
    }

    [Fact]
    public void DayProgress_ReturnsClamped_WhenFuture()
    {
        var start = DateTime.Now.AddHours(-1);
        var target = new CountdownTarget
        {
            CountdownStartDateTime = start,
            TargetDateTime = start.AddHours(4)
        };
        // 1 hour elapsed out of 4 = 0.25, clamped to [0,1]
        Assert.InRange(target.DayProgress, 0.20, 0.35);
    }

    [Fact]
    public void MinuteProgress_IsClamped_BetweenZeroAndOne()
    {
        var target = new CountdownTarget { TargetDateTime = DateTime.Now.AddHours(2) };
        Assert.InRange(target.MinuteProgress, 0.0, 1.0);
    }
}
