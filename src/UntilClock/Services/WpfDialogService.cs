using System.Windows;

namespace UntilClock.Services;

/// <summary>Production <see cref="IDialogService"/> that shows real WPF windows.</summary>
public sealed class WpfDialogService : IDialogService
{
    /// <inheritdoc />
    public DateTime? ShowSetTargetDialog(DateTime current)
    {
        var dialog = new SetTargetWindow(current)
        {
            Owner = Application.Current.MainWindow
        };
        return dialog.ShowDialog() == true ? dialog.SelectedDateTime : null;
    }
}
