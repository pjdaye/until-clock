using System.Windows;
using UntilClock.Services;
using UntilClock.ViewModels;

namespace UntilClock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(
            new MinuteBoundaryTimerService(new SystemClockService()),
            new JsonPersistenceService(),
            new WpfDialogService());
    }
}