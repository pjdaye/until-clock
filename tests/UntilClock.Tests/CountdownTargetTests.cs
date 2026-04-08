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
}
