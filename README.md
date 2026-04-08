# until-clock

A simple countdown clock widget for the Windows desktop, written in **C#** using **WinUI 3** (Windows App SDK).

## Features

- Counts down to any date and time you specify
- Displays days, hours, minutes, and seconds (the days unit is hidden when less than one day remains)
- Shows a "🎉 Time's up!" celebration message when the countdown expires
- **Always-on-top** toggle (📌) so the widget floats above other windows
- Persists the target time across sessions (stored in `%LocalAppData%\UntilClock\settings.json`)
- Adapts to the system light/dark theme via WinUI 3's Fluent design resources

## Requirements

| Requirement | Version |
|---|---|
| Windows | 10 version 1903 (build 19041) or later |
| .NET | 8.0 |
| Windows App SDK | 1.5 |

The [Windows App SDK 1.5 runtime](https://learn.microsoft.com/windows/apps/windows-app-sdk/downloads) must be installed on the target machine (or use a self-contained publish profile).

## Building

1. Install [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **Windows application development** workload (includes Windows App SDK & WinUI 3 tools).
2. Open `UntilClock.sln`.
3. Select the desired platform (`x64`, `x86`, or `ARM64`) in the toolbar.
4. Press **F5** to build and run.

### Command-line (dotnet CLI)

```powershell
cd UntilClock
dotnet build -p:Platform=x64
dotnet run -p:Platform=x64
```

## Usage

1. Launch **UntilClock.exe**.
2. Click **Set Target Time** to choose the date and time you are counting down to.
3. The clock updates every second. Click 📌 to keep it on top of other windows.

## Project Structure

```
UntilClock.sln
UntilClock/
  UntilClock.csproj       — project file (WinUI 3, net8.0-windows)
  app.manifest            — DPI awareness & OS compatibility declarations
  App.xaml / .cs          — Application entry point
  MainWindow.xaml / .cs   — Main window UI and interaction logic
  CountdownViewModel.cs   — Countdown state and display properties (INotifyPropertyChanged)
  SettingsService.cs      — Persists/loads the target time as JSON
```
