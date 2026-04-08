namespace UntilClock.Services;

/// <summary>
/// Abstracts UI dialogs to allow ViewModel testing without showing real windows.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows the set-target dialog pre-populated with <paramref name="current"/>.
    /// Returns the chosen <see cref="DateTime"/> if confirmed, or <c>null</c> if cancelled.
    /// </summary>
    DateTime? ShowSetTargetDialog(DateTime current);
}
