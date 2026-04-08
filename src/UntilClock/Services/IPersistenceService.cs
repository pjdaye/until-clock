using UntilClock.Models;

namespace UntilClock.Services;

/// <summary>Provides save/load operations for countdown state.</summary>
public interface IPersistenceService
{
    /// <summary>
    /// Saves countdown data. Throws only on unrecoverable IO errors.
    /// </summary>
    void Save(CountdownData data);

    /// <summary>
    /// Loads countdown data.
    /// Returns <c>null</c> if no data exists or the file is corrupt.
    /// </summary>
    CountdownData? Load();
}
