# until-clock

A simple countdown clock for the desktop

## Local setup

Run the bootstrap script from the repository root:

```powershell
./setup.ps1
```

This runs:

- `dotnet restore UntilClock.slnx`
- `dotnet build UntilClock.slnx`
- `dotnet test UntilClock.slnx`

Optional: run the desktop app after setup:

```powershell
./setup.ps1 -RunApp
```
