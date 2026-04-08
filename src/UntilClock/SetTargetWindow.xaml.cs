using System.Windows;

namespace UntilClock;

public partial class SetTargetWindow : Window
{
    /// <summary>The confirmed target date; null if cancelled.</summary>
    public DateTime? SelectedDateTime { get; private set; }

    public SetTargetWindow(DateTime currentTarget)
    {
        InitializeComponent();
        DatePickerControl.SelectedDate = currentTarget;
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        if (DatePickerControl.SelectedDate is DateTime picked)
        {
            SelectedDateTime = picked;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Please select a date.", "Until Clock", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
