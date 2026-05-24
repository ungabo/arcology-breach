param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$UnityPath = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe",
    [string]$LogPrefix = "",
    [switch]$SkipSceneRebuild,
    [switch]$SkipPackage,
    [switch]$SkipQAPacket,
    [switch]$SkipIssueTriage,
    [switch]$SkipCandidateReadiness
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
    if ($segments.Length -ge 3) {
        $majorNumber = 0
        $minorNumber = 0
        $patchNumber = 0
        if ([int]::TryParse($segments[0], [ref]$majorNumber) -and [int]::TryParse($segments[1], [ref]$minorNumber) -and [int]::TryParse($segments[2], [ref]$patchNumber)) {
            return "v" + (($majorNumber * 100) + ($minorNumber * 10) + $patchNumber).ToString("000")
        }
    }

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

function Clear-StaleUnityLocks {
    $unityProcesses = Get-Process -Name Unity -ErrorAction SilentlyContinue
    if ($unityProcesses) {
        return
    }

    $lockPaths = @(
        (Join-Path $ProjectPath "Library\ArtifactDB-lock"),
        (Join-Path $ProjectPath "Library\SourceAssetDB-lock")
    )

    foreach ($lockPath in $lockPaths) {
        if (Test-Path -LiteralPath $lockPath) {
            Remove-Item -LiteralPath $lockPath -Force -ErrorAction SilentlyContinue
        }
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
    Clear-StaleUnityLocks
    $process = Start-Process -FilePath $UnityPath -ArgumentList (ConvertTo-ArgumentLine $arguments) -Wait -PassThru -NoNewWindow
    Start-Sleep -Milliseconds 750
    Clear-StaleUnityLocks
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
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0CombatScenarioSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-combat-scenario-smoke.log") -Marker "V0_COMBAT_SCENARIO_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0WeaponSwitchSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-weapon-switch-smoke.log") -Marker "V0_WEAPON_SWITCH_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0BellowsNodeSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-bellows-node-smoke.log") -Marker "V0_BELLOWS_NODE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0RangedCombatSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-ranged-combat-smoke.log") -Marker "V0_RANGED_COMBAT_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0BulwarkCombatSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-bulwark-combat-smoke.log") -Marker "V0_BULWARK_COMBAT_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0WardenCombatSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-warden-combat-smoke.log") -Marker "V0_WARDEN_COMBAT_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0InteractionSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-interaction-smoke.log") -Marker "V0_INTERACTION_SMOKE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0HazardSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-hazard-smoke.log") -Marker "V0_HAZARD_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0SecretSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-secret-smoke.log") -Marker "V0_SECRET_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0PauseFlow" -LogPath (Join-Path $logsPath "$LogPrefix-pause-flow.log") -Marker "V0_PAUSE_FLOW_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0MovementSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-movement-smoke.log") -Marker "V0_MOVEMENT_FEEL_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0BalanceSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-balance-smoke.log") -Marker "V0_BALANCE_ENVELOPE_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0Level01FlowSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-level01-flow-smoke.log") -Marker "V0_LEVEL01_FLOW_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0MidgameFlowSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-midgame-flow-smoke.log") -Marker "V0_MIDGAME_FLOW_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0ClimaxFlowSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-climax-flow-smoke.log") -Marker "V0_CLIMAX_FLOW_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0AudioMixSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-audio-mix-smoke.log") -Marker "V0_AUDIO_MIX_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0DisplaySettingsSmoke" -LogPath (Join-Path $logsPath "$LogPrefix-display-settings-smoke.log") -Marker "V0_DISPLAY_SETTINGS_PASS"
Invoke-PlayerStep -ExecutablePath $windowsBuildPath -Argument "-v0ReadabilitySmoke" -LogPath (Join-Path $logsPath "$LogPrefix-readability-smoke.log") -Marker "V0_READABILITY_SETTINGS_PASS"

if (-not $SkipPackage) {
    $packageScript = Join-Path $ProjectPath "Tools\PackageWindowsBuild.ps1"
    if (-not (Test-Path -LiteralPath $packageScript)) {
        throw "Windows package script was not found: $packageScript"
    }

    Write-Host "Running Windows package step"
    & $packageScript -ProjectPath $ProjectPath
}

if (-not $SkipQAPacket) {
    $qaPacketScript = Join-Path $ProjectPath "Tools\GenerateWindowsQAPacket.ps1"
    if (-not (Test-Path -LiteralPath $qaPacketScript)) {
        throw "Windows QA packet script was not found: $qaPacketScript"
    }

    Write-Host "Running Windows QA packet step"
    & $qaPacketScript -ProjectPath $ProjectPath
}

if (-not $SkipIssueTriage) {
    $issueTriageScript = Join-Path $ProjectPath "Tools\GenerateWindowsIssueTriagePacket.ps1"
    if (-not (Test-Path -LiteralPath $issueTriageScript)) {
        throw "Windows issue triage packet script was not found: $issueTriageScript"
    }

    Write-Host "Running Windows issue triage packet step"
    & $issueTriageScript -ProjectPath $ProjectPath
}

if (-not $SkipCandidateReadiness) {
    $candidateScript = Join-Path $ProjectPath "Tools\GenerateWindowsCandidateReadiness.ps1"
    if (-not (Test-Path -LiteralPath $candidateScript)) {
        throw "Windows candidate readiness script was not found: $candidateScript"
    }

    Write-Host "Running Windows candidate readiness step"
    & $candidateScript -ProjectPath $ProjectPath -LogPrefix $LogPrefix
}

Write-Host "V0_BUILD_MATRIX_PASS $version $windowsBuildPath"
