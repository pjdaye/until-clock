
using System.Text.Json;
using UntilClock.Models;

namespace UntilClock.Services;

/// <summary>
/// <see cref="IPersistenceService"/> backed by a JSON file under LocalAppData.
/// Uses atomic write (write to .tmp then replace) to survive interruptions.
/// </summary>
public sealed class JsonPersistenceService : IPersistenceService
{
    private readonly string _filePath;

    public JsonPersistenceService()
        : this(GetDefaultPath()) { }

    /// <summary>Allows injection of a custom path for testing.</summary>
    public JsonPersistenceService(string filePath)
    {
        _filePath = filePath;
    }

    /// <inheritdoc />
    public void Save(CountdownData data)
    {
        var dir = Path.GetDirectoryName(_filePath)!;
        Directory.CreateDirectory(dir);

        var tmpPath = _filePath + ".tmp";
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(tmpPath, json);
        File.Move(tmpPath, _filePath, overwrite: true);
    }

    /// <inheritdoc />
    public CountdownData? Load()
    {
        if (!File.Exists(_filePath))
            return null;

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<CountdownData>(json);
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            return null;
        }
    }

    private static string GetDefaultPath() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "UntilClock",
            "data.json");
}
