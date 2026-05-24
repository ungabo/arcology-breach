param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe",
    [string]$LogPrefix = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $UnityPath)) {
    throw "Unity executable was not found: $UnityPath"
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$logsPath = Join-Path $ProjectPath "Logs"
New-Item -ItemType Directory -Force -Path $logsPath | Out-Null

if ([string]::IsNullOrWhiteSpace($LogPrefix)) {
    $brandingPath = Join-Path $ProjectPath "Assets\_Project\Scripts\Utility\GameBranding.cs"
    $brandingText = Get-Content -LiteralPath $brandingPath -Raw
    if ($brandingText -notmatch 'BuildVersion\s*=\s*"v([0-9]+)\.([0-9]+)\.([0-9]+)"') {
        throw "Could not derive route-audit log prefix from $brandingPath."
    }

    $majorNumber = [int]$Matches[1]
    $minorNumber = [int]$Matches[2]
    $patchNumber = [int]$Matches[3]
    $LogPrefix = "v" + (($majorNumber * 100) + ($minorNumber * 10) + $patchNumber).ToString("000")
}

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

Write-Host "V0_ROUTE_AUDIT_PASS"
