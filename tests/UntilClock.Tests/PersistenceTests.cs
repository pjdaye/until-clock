

using UntilClock.Models;
using UntilClock.Services;

namespace UntilClock.Tests;

public class PersistenceTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _filePath;
    private readonly JsonPersistenceService _sut;

    public PersistenceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _filePath = Path.Combine(_tempDir, "data.json");
        _sut = new JsonPersistenceService(_filePath);
    }

    public void Dispose() => Directory.Delete(_tempDir, recursive: true);

    [Fact]
    public void Load_ReturnsNull_WhenFileDoesNotExist()
    {
        Assert.Null(_sut.Load());
    }

    [Fact]
    public void SaveAndLoad_RoundTrips_Data()
    {
        var now = new DateTime(2026, 6, 15, 10, 30, 0);
        var target = new DateTime(2026, 12, 31, 23, 59, 0);
        var data = new CountdownData { CountdownStartDateTime = now, TargetDateTime = target };

        _sut.Save(data);
        var loaded = _sut.Load();

        Assert.NotNull(loaded);
        Assert.Equal(target, loaded!.TargetDateTime);
        Assert.Equal(now, loaded.CountdownStartDateTime);
    }

    [Fact]
    public void Load_ReturnsNull_WhenFileIsCorrupt()
    {
        File.WriteAllText(_filePath, "not valid json {{{{");
        Assert.Null(_sut.Load());
    }

    [Fact]
    public void Load_ReturnsNull_WhenFileIsEmpty()
    {
        File.WriteAllText(_filePath, string.Empty);
        Assert.Null(_sut.Load());
    }

    [Fact]
    public void Save_IsAtomic_NoPartialFileLeft_OnSuccess()
    {
        var data = new CountdownData
        {
            TargetDateTime = DateTime.Now.AddDays(1),
            CountdownStartDateTime = DateTime.Now
        };
        _sut.Save(data);

        // .tmp file should not remain after successful save
        Assert.False(File.Exists(_filePath + ".tmp"));
        Assert.True(File.Exists(_filePath));
    }

    [Fact]
    public void Save_Overwrites_ExistingFile()
    {
        var data1 = new CountdownData { TargetDateTime = DateTime.Now.AddDays(1), CountdownStartDateTime = DateTime.Now };
        var data2 = new CountdownData { TargetDateTime = DateTime.Now.AddDays(5), CountdownStartDateTime = DateTime.Now };

        _sut.Save(data1);
        _sut.Save(data2);

        var loaded = _sut.Load();
        Assert.NotNull(loaded);
        Assert.Equal(data2.TargetDateTime.Date, loaded!.TargetDateTime.Date);
    }
}
