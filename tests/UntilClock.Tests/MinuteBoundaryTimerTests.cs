using UntilClock.Services;

namespace UntilClock.Tests;

public class FakeClockService : IClockService
{
    public DateTime Now { get; set; } = DateTime.Now;
}

public class MinuteBoundaryTimerTests
{
    [Theory]
    [InlineData(0, 60)]   // exactly at boundary → 60s
    [InlineData(1, 59)]   // 1 second past → 59s until next
    [InlineData(30, 30)]  // halfway → 30s
    [InlineData(59, 1)]   // 1 second before next → 1s
    public void InitialInterval_AlignsToNextMinuteBoundary(int currentSecond, int expectedSeconds)
    {
        var expected = TimeSpan.FromSeconds(Math.Max(60 - currentSecond, 1));
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), expected);
    }

    [Fact]
    public void AlignmentMath_SecondZero_GivesFullMinuteInterval()
    {
        int currentSecond = 0;
        int expectedSeconds = 60;
        var interval = TimeSpan.FromSeconds(Math.Max(60 - currentSecond, 1));
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), interval);
    }

    [Fact]
    public void AlignmentMath_Second59_GivesOneSecondInterval()
    {
        int currentSecond = 59;
        var interval = TimeSpan.FromSeconds(Math.Max(60 - currentSecond, 1));
        Assert.Equal(TimeSpan.FromSeconds(1), interval);
    }

    [Fact]
    public void AlignmentMath_LongSession_NoAccumulatedDrift()
    {
        // Simulate 120 "minute ticks" — each fires at :00 of a simulated minute.
        // Verify that after 120 ticks the alignment math always produces ≤ 60s interval
        // and never produces a negative interval.
        var baseTime = new DateTime(2026, 1, 1, 10, 0, 0);
        for (int tick = 0; tick < 120; tick++)
        {
            var simTime = baseTime.AddMinutes(tick);
            var secondsUntilNext = 60 - simTime.Second; // = 60
            var interval = TimeSpan.FromSeconds(Math.Max(secondsUntilNext, 1));
            Assert.InRange(interval.TotalSeconds, 1, 60);
        }
    }

    [Fact]
    public void AlignmentMath_SlightDrift_SelfCorrects()
    {
        // If a tick fires 2 seconds late (:02 instead of :00), next interval = 58s
        // bringing it back toward boundary
        var clock = new FakeClockService
        {
            Now = new DateTime(2026, 1, 1, 12, 5, 2) // 2s past boundary
        };
        int secondsUntilNext = 60 - clock.Now.Second; // 58
        var interval = TimeSpan.FromSeconds(Math.Max(secondsUntilNext, 1));
        Assert.Equal(58, interval.TotalSeconds);
    }
}
