param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe",
    [string]$LogPrefix = "v012"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $UnityPath)) {
    throw "Unity executable was not found: $UnityPath"
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$logsPath = Join-Path $ProjectPath "Logs"
New-Item -ItemType Directory -Force -Path $logsPath | Out-Null

$logPath = Join-Path $logsPath "$LogPrefix-route-audit.log"
$arguments = @(
    "-batchmode",
    "-projectPath", $ProjectPath,
    "-executeMethod", "V0RouteAudit.RunRouteAudit",
    "-quit",
    "-logFile", $logPath
)

function ConvertTo-ArgumentLine {
    param([string[]]$Arguments)

    return ($Arguments | ForEach-Object {
        if ($_ -match '[\s"]') {
            '"' + ($_ -replace '"', '\"') + '"'
        }
        else {
            $_
        }
    }) -join " "
}

Write-Host "Running Unity route audit"
$process = Start-Process -FilePath $UnityPath -ArgumentList (ConvertTo-ArgumentLine $arguments) -Wait -PassThru -NoNewWindow
if ($process.ExitCode -ne 0) {
    throw "Unity route audit failed with exit code $($process.ExitCode)."
}

if (-not (Select-String -LiteralPath $logPath -SimpleMatch -Pattern "V0_ROUTE_AUDIT_PASS" -Quiet)) {
    throw "Expected marker 'V0_ROUTE_AUDIT_PASS' was not found in $logPath."
}

Write-Host "V0_ROUTE_AUDIT_PASS Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.2.md"
