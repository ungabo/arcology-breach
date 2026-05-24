param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$LogPrefix = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-Branding {
    param([string]$RootPath)

    $brandingPath = Join-Path $RootPath "Assets\_Project\Scripts\Utility\GameBranding.cs"
    $brandingText = Get-Content -LiteralPath $brandingPath -Raw

    if ($brandingText -notmatch 'BuildVersion\s*=\s*"([^"]+)"') {
        throw "Could not find GameBranding.BuildVersion in $brandingPath."
    }
    $version = $Matches[1]

    if ($brandingText -notmatch 'ExecutableStem\s*=\s*"([^"]+)"') {
        throw "Could not find GameBranding.ExecutableStem in $brandingPath."
    }
    $executableStem = $Matches[1]

    return @{
        Version = $version
        ExecutableStem = $executableStem
    }
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

    return ($Version -replace '[^A-Za-z0-9]', '').ToLowerInvariant()
}

function Require-Path {
    param(
        [string]$Path,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Windows candidate readiness failed: missing $Label at $Path"
    }
}

function Convert-ToRepoPath {
    param(
        [string]$RootPath,
        [string]$AbsolutePath
    )

    $root = [System.IO.Path]::GetFullPath($RootPath).TrimEnd('\', '/')
    $full = [System.IO.Path]::GetFullPath($AbsolutePath)
    if ($full.StartsWith($root, [System.StringComparison]::OrdinalIgnoreCase)) {
        return $full.Substring($root.Length).TrimStart('\', '/') -replace '\\', '/'
    }

    return $full
}

function Convert-ToInlineCode {
    param([string]$Value)

    $tick = [char]96
    return "$tick$Value$tick"
}

function Assert-LogMarker {
    param(
        [string]$LogPath,
        [string]$Marker
    )

    Require-Path -Path $LogPath -Label "verification log"
    if (-not (Select-String -LiteralPath $LogPath -SimpleMatch -Pattern $Marker -Quiet)) {
        throw "Windows candidate readiness failed: expected marker '$Marker' was not found in $LogPath."
    }
}

function Get-ZipEntryNames {
    param([string]$ZipPath)

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zipFile = [System.IO.Compression.ZipFile]::OpenRead($ZipPath)
    try {
        return @($zipFile.Entries | ForEach-Object { $_.FullName })
    }
    finally {
        $zipFile.Dispose()
    }
}

function Assert-ZipContainsLeaf {
    param(
        [string[]]$EntryNames,
        [string]$LeafName
    )

    $match = $EntryNames | Where-Object { $_.Replace('\', '/').EndsWith("/$LeafName", [System.StringComparison]::OrdinalIgnoreCase) -or $_.Equals($LeafName, [System.StringComparison]::OrdinalIgnoreCase) } | Select-Object -First 1
    if (-not $match) {
        throw "Windows candidate readiness failed: package ZIP does not contain $LeafName."
    }
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$branding = Get-Branding -RootPath $ProjectPath
$version = $branding.Version
$executableStem = $branding.ExecutableStem

if ([string]::IsNullOrWhiteSpace($LogPrefix)) {
    $LogPrefix = Get-DefaultLogPrefix -Version $version
}

$exePath = Join-Path $ProjectPath "Builds\Windows\$version\${executableStem}_$version.exe"
$routeAuditPath = Join-Path $ProjectPath "Documentation\QA\RouteAudit\ROUTE_AUDIT_$version.md"
$qaPacketPath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\QA_PACKET_$version.md"
$qaPacketManifestPath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\QA_PACKET_$version.json"
$issueTriagePath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\ISSUE_TRIAGE_$version.md"
$issueTriageManifestPath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\ISSUE_TRIAGE_$version.json"
$packageManifestPath = Join-Path $ProjectPath "Builds\WindowsPackages\$version\${executableStem}_${version}_WindowsPackageManifest.json"
$releaseNotesPath = Join-Path $ProjectPath "Documentation\Releases\RELEASE_NOTES_$version.md"
$readinessRoot = Join-Path $ProjectPath "Documentation\Releases\CandidateReadiness"
$logsPath = Join-Path $ProjectPath "Logs"

Require-Path -Path $exePath -Label "Windows executable"
Require-Path -Path $routeAuditPath -Label "route audit report"
Require-Path -Path $qaPacketPath -Label "Windows QA packet"
Require-Path -Path $qaPacketManifestPath -Label "Windows QA packet manifest"
Require-Path -Path $issueTriagePath -Label "Windows issue triage packet"
Require-Path -Path $issueTriageManifestPath -Label "Windows issue triage packet manifest"
Require-Path -Path $packageManifestPath -Label "Windows package manifest"

$packageManifest = Get-Content -LiteralPath $packageManifestPath -Raw | ConvertFrom-Json
$packageZip = [string]$packageManifest.zip_path
$packageHash = [string]$packageManifest.sha256
$packageLauncherPath = [string]$packageManifest.launcher
$packageReadmePath = [string]$packageManifest.readme
$packageQuickstartPath = [string]$packageManifest.quickstart
$packageSupportInfoPath = [string]$packageManifest.support_info
$packageReleaseIndexPath = [string]$packageManifest.release_index
$packageChecksumInstructionsPath = [string]$packageManifest.checksum_instructions
$packageSha256SidecarPath = [string]$packageManifest.sha256_sidecar
Require-Path -Path $packageZip -Label "Windows package ZIP"
Require-Path -Path $packageLauncherPath -Label "Windows package launcher"
Require-Path -Path $packageReadmePath -Label "Windows package README"
Require-Path -Path $packageQuickstartPath -Label "Windows package quickstart"
Require-Path -Path $packageSupportInfoPath -Label "Windows package support info"
Require-Path -Path $packageReleaseIndexPath -Label "Windows package release index"
Require-Path -Path $packageChecksumInstructionsPath -Label "Windows package checksum instructions"
Require-Path -Path $packageSha256SidecarPath -Label "Windows package SHA-256 sidecar"

$zipEntryNames = Get-ZipEntryNames -ZipPath $packageZip
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "LAUNCH_BRASSWORKS_BREACH.bat"
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "README_WINDOWS.txt"
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "QUICKSTART_WINDOWS.txt"
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "SUPPORT_INFO_WINDOWS.txt"
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "RELEASE_INDEX_WINDOWS.txt"
Assert-ZipContainsLeaf -EntryNames $zipEntryNames -LeafName "VERIFY_SHA256_WINDOWS.txt"

$logChecks = @(
    @{ Name = "Scene rebuild"; File = "$LogPrefix-scene.log"; Marker = "V0 scenes rebuilt" },
    @{ Name = "Level validation"; File = "$LogPrefix-level-validation.log"; Marker = "V0_LEVEL_VALIDATION_PASS" },
    @{ Name = "Editor smoke"; File = "$LogPrefix-smoke-test.log"; Marker = "V0_SMOKE_TEST_PASS" },
    @{ Name = "Windows build"; File = "$LogPrefix-windows-build.log"; Marker = "V0_WINDOWS_BUILD_PASS" },
    @{ Name = "Runtime smoke"; File = "$LogPrefix-runtime-smoke.log"; Marker = "V0_RUNTIME_SMOKE_PASS" },
    @{ Name = "Auto playthrough"; File = "$LogPrefix-auto-playthrough.log"; Marker = "V0_AUTO_PLAYTHROUGH_PASS" },
    @{ Name = "Combat smoke"; File = "$LogPrefix-combat-smoke.log"; Marker = "V0_COMBAT_SMOKE_PASS" },
    @{ Name = "Combat edge"; File = "$LogPrefix-combat-edge-smoke.log"; Marker = "V0_COMBAT_EDGE_PASS" },
    @{ Name = "Combat scenario"; File = "$LogPrefix-combat-scenario-smoke.log"; Marker = "V0_COMBAT_SCENARIO_PASS" },
    @{ Name = "Weapon switch"; File = "$LogPrefix-weapon-switch-smoke.log"; Marker = "V0_WEAPON_SWITCH_PASS" },
    @{ Name = "Bellows Node"; File = "$LogPrefix-bellows-node-smoke.log"; Marker = "V0_BELLOWS_NODE_PASS" },
    @{ Name = "Ranged combat"; File = "$LogPrefix-ranged-combat-smoke.log"; Marker = "V0_RANGED_COMBAT_PASS" },
    @{ Name = "Bulwark combat"; File = "$LogPrefix-bulwark-combat-smoke.log"; Marker = "V0_BULWARK_COMBAT_PASS" },
    @{ Name = "Warden combat"; File = "$LogPrefix-warden-combat-smoke.log"; Marker = "V0_WARDEN_COMBAT_PASS" },
    @{ Name = "Interaction"; File = "$LogPrefix-interaction-smoke.log"; Marker = "V0_INTERACTION_SMOKE_PASS" },
    @{ Name = "Hazards"; File = "$LogPrefix-hazard-smoke.log"; Marker = "V0_HAZARD_PASS" },
    @{ Name = "Secrets"; File = "$LogPrefix-secret-smoke.log"; Marker = "V0_SECRET_PASS" },
    @{ Name = "Pause flow"; File = "$LogPrefix-pause-flow.log"; Marker = "V0_PAUSE_FLOW_PASS" },
    @{ Name = "Movement feel"; File = "$LogPrefix-movement-smoke.log"; Marker = "V0_MOVEMENT_FEEL_PASS" },
    @{ Name = "Balance"; File = "$LogPrefix-balance-smoke.log"; Marker = "V0_BALANCE_ENVELOPE_PASS" },
    @{ Name = "Level01 flow"; File = "$LogPrefix-level01-flow-smoke.log"; Marker = "V0_LEVEL01_FLOW_PASS" },
    @{ Name = "Midgame flow"; File = "$LogPrefix-midgame-flow-smoke.log"; Marker = "V0_MIDGAME_FLOW_PASS" },
    @{ Name = "Climax flow"; File = "$LogPrefix-climax-flow-smoke.log"; Marker = "V0_CLIMAX_FLOW_PASS" },
    @{ Name = "Audio mix"; File = "$LogPrefix-audio-mix-smoke.log"; Marker = "V0_AUDIO_MIX_PASS" },
    @{ Name = "Display settings"; File = "$LogPrefix-display-settings-smoke.log"; Marker = "V0_DISPLAY_SETTINGS_PASS" },
    @{ Name = "Readability"; File = "$LogPrefix-readability-smoke.log"; Marker = "V0_READABILITY_SETTINGS_PASS" },
    @{ Name = "Gameplay feedback"; File = "$LogPrefix-gameplay-feedback-smoke.log"; Marker = "V0_GAMEPLAY_FEEDBACK_PASS" },
    @{ Name = "World label readability"; File = "$LogPrefix-world-label-readability-smoke.log"; Marker = "V0_WORLD_LABEL_READABILITY_PASS" }
)

foreach ($check in $logChecks) {
    Assert-LogMarker -LogPath (Join-Path $logsPath $check["File"]) -Marker $check["Marker"]
}

$generatedLocal = Get-Date -Format "yyyy-MM-dd HH:mm zzz"
$generatedUtc = (Get-Date).ToUniversalTime().ToString("o")
New-Item -ItemType Directory -Force -Path $readinessRoot | Out-Null
$readinessPath = Join-Path $readinessRoot "CANDIDATE_READINESS_$version.md"
$readinessManifestPath = Join-Path $readinessRoot "CANDIDATE_READINESS_$version.json"

$releaseNotesState = if (Test-Path -LiteralPath $releaseNotesPath) { "present" } else { "pending until docs refresh" }
$routeAuditRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $routeAuditPath)
$qaPacketRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $qaPacketPath)
$issueTriageRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $issueTriagePath)
$exeRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $exePath)
$packageRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageZip)
$launcherRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageLauncherPath)
$packageReadmeRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageReadmePath)
$quickstartRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageQuickstartPath)
$supportInfoRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageSupportInfoPath)
$releaseIndexRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageReleaseIndexPath)
$checksumInstructionsRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageChecksumInstructionsPath)
$sha256SidecarRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageSha256SidecarPath)
$hashCode = Convert-ToInlineCode -Value $packageHash
$releaseNotesRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $releaseNotesPath)
$generatedCode = Convert-ToInlineCode -Value $generatedLocal

$markerRows = @()
foreach ($check in $logChecks) {
    $name = [string]$check["Name"]
    $marker = Convert-ToInlineCode -Value ([string]$check["Marker"])
    $file = Convert-ToInlineCode -Value ([string]$check["File"])
    $markerRows += "| $name | $marker | $file |"
}

$readinessLines = @(
    "# Brassworks Breach - Windows Candidate Readiness $version",
    "",
    "Generated: $generatedCode",
    "",
    "## Candidate Artifacts",
    "",
    "- Executable: $exeRepo",
    "- Package: $packageRepo",
    "- Package SHA-256: $hashCode",
    "- Route audit: $routeAuditRepo",
    "- QA packet: $qaPacketRepo",
    "- Issue triage packet: $issueTriageRepo",
    "- Release notes: $releaseNotesRepo ($releaseNotesState)",
    "- Package launcher: $launcherRepo",
    "- Package README: $packageReadmeRepo",
    "- Package quickstart: $quickstartRepo",
    "- Package support info: $supportInfoRepo",
    "- Package release index: $releaseIndexRepo",
    "- Package checksum instructions: $checksumInstructionsRepo",
    "- Package SHA-256 sidecar: $sha256SidecarRepo",
    "",
    "## Automated Verification Markers",
    "",
    "| Area | Marker | Log |",
    "| --- | --- | --- |"
) + $markerRows + @(
    "",
    "## Candidate Rules",
    "",
    "- Ship only the ZIP package, not a loose executable alone.",
    "- Keep the launcher, quickstart, README, support info, Data folder, UnityPlayer.dll, and MonoBleedingEdge folder together after extraction.",
    "- Keep the SHA-256 hash with any shared package and use VERIFY_SHA256_WINDOWS.txt to compare it.",
    "- Use RELEASE_INDEX_WINDOWS.txt as the package contents index before sharing a candidate.",
    "- Use the QA packet as the manual route-test starting point.",
    "- Treat this as a Windows candidate snapshot, not Android, WebGL, SteamVR, or Meta Quest readiness.",
    "- Any manual blocker or confusion note should become a tracked task before a v1.0 release label.",
    "",
    "## Status",
    "",
    "Candidate readiness automation passed for this Windows build.",
    "",
    "Next-step directive: continue immediately with the next highest-impact unfinished task."
)

Set-Content -LiteralPath $readinessPath -Value $readinessLines -Encoding UTF8

$manifest = [ordered]@{
    version = $version
    generated_utc = $generatedUtc
    executable = $exePath
    package_zip = $packageZip
    package_sha256 = $packageHash
    package_launcher = $packageLauncherPath
    package_readme = $packageReadmePath
    package_quickstart = $packageQuickstartPath
    package_support_info = $packageSupportInfoPath
    package_release_index = $packageReleaseIndexPath
    package_checksum_instructions = $packageChecksumInstructionsPath
    package_sha256_sidecar = $packageSha256SidecarPath
    route_audit = $routeAuditPath
    qa_packet = $qaPacketPath
    issue_triage_packet = $issueTriagePath
    release_notes = $releaseNotesPath
    release_notes_state = $releaseNotesState
    readiness_report = $readinessPath
    log_prefix = $LogPrefix
    checked_markers = $logChecks
}

$manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $readinessManifestPath -Encoding UTF8

Write-Host "V0_WINDOWS_CANDIDATE_PASS $version $readinessPath"
