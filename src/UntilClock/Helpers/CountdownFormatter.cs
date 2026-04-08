namespace UntilClock.Helpers;

/// <summary>Shared formatting helpers for countdown time display.</summary>
public static class CountdownFormatter
{
    /// <summary>
    /// Formats a <see cref="TimeSpan"/> as a human-readable countdown string.
    /// Returns <c>"00:00:00"</c> for zero or negative spans.
    /// Examples: <c>"2d 03:45:12"</c>, <c>"00:45:12"</c>.
    /// </summary>
    public static string Format(TimeSpan span)
    {
        if (span <= TimeSpan.Zero)
            return "00:00:00";

        return span.Days > 0
            ? $"{span.Days}d {span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}"
            : $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }
}
