# Copilot Instructions

## Build, Test, and Run

```powershell
# Restore + build + test (full setup)
./setup.ps1

# Build only
dotnet build UntilClock.slnx

# Run all tests
dotnet test UntilClock.slnx

# Run a single test by name
dotnet test UntilClock.slnx --filter "FullyQualifiedName~Remaining_ReturnsZero_WhenTargetIsInPast"

# Launch the app
dotnet run --project src/UntilClock/UntilClock.csproj

# Run tests with coverage gate (fails if line coverage < 80%)
dotnet test UntilClock.slnx /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Threshold=80 /p:ThresholdType=line /p:ThresholdStat=total
```

Both projects target `net10.0-windows`. `EnableWindowsTargeting` is set so the solution can be restored on non-Windows machines, but the app and tests must run on Windows.

## Architecture

**Until Clock** is a WPF desktop app (`UseWPF=true`) that counts down to a named target date/time.

```
src/UntilClock/
  Models/         – Plain C# data models (no WPF dependencies)
  ViewModels/     – MVVM layer; inherits ViewModelBase
  Services/       – ITimerService / MinuteBoundaryTimerService (production); IClockService / SystemClockService;
                    IPersistenceService / JsonPersistenceService; IDialogService / WpfDialogService
  Converters/     – IValueConverter implementations for XAML bindings
  Controls/       – Custom WPF controls (e.g. CircularProgressRing)
  Helpers/        – Shared static utilities (e.g. CountdownFormatter)
  MainWindow.xaml – Single window; DataContext wired in code-behind

tests/UntilClock.Tests/
  CountdownTargetTests.cs  – xUnit tests targeting Models only
```

**Data flow:** `MinuteBoundaryTimerService` fires a `Tick` event aligned to minute boundaries on the UI thread → `MainViewModel.Refresh()` recomputes `Remaining` and `DisplayText` → WPF bindings update the view.

`MainWindow` manually constructs `MainViewModel` and passes service implementations to it — there is no DI container.

## Key Conventions

- **MVVM is hand-rolled.** `ViewModelBase` provides `OnPropertyChanged` (with `[CallerMemberName]`) and `SetProperty<T>`. Do not introduce a third-party MVVM toolkit without discussion.
- **`RelayCommand`** is the only `ICommand` implementation. Its `CanExecuteChanged` hooks into `CommandManager.RequerySuggested`.
- **`ITimerService`** exists to decouple ViewModels from WPF's `DispatcherTimer`. Tests should inject a fake/stub implementation rather than using `DispatcherTimerService` directly.
- **Models are WPF-free.** `CountdownTarget` lives in `UntilClock.Models` with no UI dependencies — keep it that way so it stays unit-testable without a WPF runtime.
- **Countdown format:** `"Xd HH:mm:ss"` when days > 0, otherwise `"HH:mm:ss"`. `CountdownFormatter.Format` (in `UntilClock.Helpers`) is the single source of truth — both `MainViewModel` and `TimeSpanToCountdownConverter` delegate to it.
- **XML doc comments** are present on all public members. Continue this practice.
- **Nullable reference types** (`<Nullable>enable</Nullable>`) and **implicit usings** are enabled project-wide.
