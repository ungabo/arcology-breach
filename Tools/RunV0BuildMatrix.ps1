param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe",
    [string]$LogPrefix = "",
    [switch]$SkipSceneRebuild
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-BuildVersion {
    param([string]$RootPath)

    $brandingPath = Join-Path $RootPath "Assets\_Project\Scripts\Utility\GameBranding.cs"
    $brandingText = Get-Content -LiteralPath $brandingPath -Raw

    if ($brandingText -notmatch 'BuildVersion\s*=\s*"([^"]+)"') {
        throw "Could not find GameBranding.BuildVersion in $brandingPath."
    }

    return $Matches[1]
}

function Get-DefaultLogPrefix {
    param([string]$Version)

    $cleanVersion = $Version.TrimStart("v")
    $segments = $cleanVersion.Split(".")
    $patchSegment = $segments[$segments.Length - 1]

    $patchNumber = 0
    if (-not [int]::TryParse($patchSegment, [ref]$patchNumber)) {
        return ($Version -replace '[^A-Za-z0-9]', '').ToLowerInvariant()
    }

    return "v" + $patchNumber.ToString("000")
}

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

function Assert-LogMarker {
    param(
        [string]$LogPath,
        [string]$Marker
    )

    if (-not (Test-Path -LiteralPath $LogPath)) {
        throw "Expected log was not written: $LogPath"
    }

    $markerMatch = Select-String -LiteralPath $LogPath -SimpleMatch -Pattern $Marker -Quiet
    if (-not $markerMatch) {
        throw "Expected marker '$Marker' was not found in $LogPath."
    }
}

function Invoke-UnityEditorStep {
    param(
        [string]$Method,
        [string]$LogPath,
        [string]$Marker
    )

    $arguments = @(
        "-batchmode",
        "-projectPath", $ProjectPath,
        "-executeMethod", $Method,
        "-quit",
        "-logFile", $LogPath
    )

    Write-Host "Running Unity editor step: $Method"
    $process = Start-Process -FilePath $UnityPath -ArgumentList (ConvertTo-ArgumentLine $arguments) -Wait -PassThru -NoNewWindow
    if ($process.ExitCode -ne 0) {
        throw "Unity editor step failed with exit code $($process.ExitCode): $Method"
    }

    Assert-LogMarker -LogPath $LogPath -Marker $Marker
    Write-Host "Passed: $Marker"
}

function Invoke-PlayerStep {
    param(
        [string]$ExecutablePath,
        [string]$Argument,
        [string]$LogPath,
        [string]$Marker
    )

    $arguments = @(
        "-batchmode",
        "-nographics",
        "-logFile", $LogPath,
        $Argument
    )

    Write-Host "Running player step: $Argument"
    $process = Start-Process -FilePath $ExecutablePath -ArgumentList (ConvertTo-ArgumentLine $arguments) -Wait -PassThru -NoNewWindow
    if ($process.ExitCode -ne 0) {
        throw "Player step failed with exit code $($process.ExitCode): $Argument"
    }

    Assert-LogMarker -LogPath $LogPath -Marker $Marker
    Write-Host "Passed: $Marker"
}

if (-not (Test-Path -LiteralPath $UnityPath)) {
    throw "Unity executable was not found: $UnityPath"
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$logsPath = Join-Path $ProjectPath "Logs"
New-Item -ItemType Directory -Force -Path $logsPath | Out-Null

$version = Get-BuildVersion -RootPath $ProjectPath
if ([string]::IsNullOrWhiteSpace($LogPrefix)) {
    $LogPrefix = Get-DefaultLogPrefix -Version $version
}

$windowsBuildPath = Join-Path $ProjectPath "Builds\Windows\$version\BrassworksBreach_$version.exe"

Write-Host "Brassworks Breach build matrix"
Write-Host "Project: $ProjectPath"
Write-Host "Version: $version"
Write-Host "Log prefix: $LogPrefix"

if (-not $SkipSceneRebuild) {
    Invoke-UnityEditorStep -Method "V0SceneBuilder.BuildV0" -LogPath (Join-Path $logsPath "$LogPrefix-scene.log") -Marker "V0 scenes rebuilt"
}

Invoke-UnityEditorStep -Method "V0LevelValidator.RunValidation" -LogPath (Join-Path $logsPath "$LogPrefix-level-validation.log") -Marker "V0_LEVEL_VALIDATION_PASS"
Invoke-UnityEditorStep -Method "V0SceneBuilder.RunSmokeTest" -LogPath (Join-Path $logsPath "$LogPrefix-smoke-test.log") -Marker "V0_SMOKE_TEST_PASS"
Invoke-UnityEditorStep -Method "V0SceneBuilder.BuildWindowsV0" -LogPath (Join-Path $logsPath "$LogPrefix-windows-build.log") -Marker "V0_WINDOWS_BUILD_PASS"

if (-not (Test-Path -LiteralPath $windowsBuildPath)) {
    throw "Windows build executable was not found: $windowsBuildPath"
}

Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0RuntimeSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-runtime-smoke.log") -Marker "V0_RUNTIME_SMOKE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0AutoPlaythrough" -LogPath (Join-Path $logsPath "$LogPrefix-auto-playthrough.log") -Marker "V0_AUTO_PLAYTHROUGH_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0CombatSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-combat-smoke.log") -Marker "V0_COMBAT_SMOKE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0CombatEdgeSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-combat-edge-smoke.log") -Marker "V0_COMBAT_EDGE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0RangedCombatSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-ranged-combat-smoke.log") -Marker "V0_RANGED_COMBAT_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0InteractionSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-interaction-smoke.log") -Marker "V0_INTERACTION_SMOKE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0PauseFlow" -LogPath (Join-Path $logsPath "$LogPrefix-pause-flow.log") -Marker "V0_PAUSE_FLOW_PASS"

Write-Host "V0_BUILD_MATRIX_PASS $version $windowsBuildPath"
