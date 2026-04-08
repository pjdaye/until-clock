[CmdletBinding()]
param(
    [switch]$RunApp
)

$ErrorActionPreference = "Stop"

$solution = "UntilClock.slnx"
$appProject = "src/UntilClock/UntilClock.csproj"

function Invoke-Step {
    param(
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][string]$Command
    )

    Write-Host "`n==> $Name" -ForegroundColor Cyan
    Invoke-Expression $Command

    if ($LASTEXITCODE -ne 0) {
        throw "Step failed: $Name (exit code $LASTEXITCODE)"
    }
}

try {
    Invoke-Step -Name "Restore" -Command "dotnet restore $solution"
    Invoke-Step -Name "Build" -Command "dotnet build $solution"
    Invoke-Step -Name "Test" -Command "dotnet test $solution"

    if ($RunApp) {
        Invoke-Step -Name "Run App" -Command "dotnet run --project $appProject"
    }

    Write-Host "`nSetup completed successfully." -ForegroundColor Green
    exit 0
}
catch {
    Write-Host "`nSetup failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
