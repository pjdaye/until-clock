using System.Globalization;
using System.Windows.Data;

namespace UntilClock.Converters;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a human-readable countdown string.
/// E.g. "2d 03:45:12" or "00:45:12".
/// </summary>
[ValueConversion(typeof(TimeSpan), typeof(string))]
public sealed class TimeSpanToCountdownConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TimeSpan span || span <= TimeSpan.Zero)
            return "00:00:00";

        return span.Days > 0
            ? $"{span.Days}d {span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}"
            : $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
