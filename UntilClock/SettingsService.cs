using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UntilClock
{
    /// <summary>
    /// Persists and loads user settings (currently just the target countdown time)
    /// to a JSON file in the user's LocalApplicationData folder.
    /// </summary>
    public static class SettingsService
    {
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "UntilClock");

        private static readonly string SettingsFilePath = Path.Combine(
            SettingsDirectory, "settings.json");

        /// <summary>
        /// Loads the saved target time. Returns a sensible default (5:00 PM today)
        /// if no settings file exists or the file cannot be read.
        /// </summary>
        public static DateTime LoadTargetTime()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
                    if (settings?.TargetTime is not null)
                        return settings.TargetTime.Value;
                }
            }
            catch
            {
                // Fall through to the default value if any I/O or deserialization error occurs.
            }

            // Default: 5:00 PM today
            return DateTime.Today.AddHours(17);
        }

        /// <summary>Persists the target time to disk.</summary>
        public static void SaveTargetTime(DateTime targetTime)
        {
            try
            {
                Directory.CreateDirectory(SettingsDirectory);
                var settings = new AppSettings { TargetTime = targetTime };
                var json = JsonSerializer.Serialize(settings, JsonOptions);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch
            {
                // Silently ignore save failures — the countdown still works in-session.
            }
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
        };

        private sealed class AppSettings
        {
            [JsonPropertyName("targetTime")]
            public DateTime? TargetTime { get; set; }
        }
    }
}
