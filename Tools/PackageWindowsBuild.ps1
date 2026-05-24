param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$PackageRoot = "",
    [switch]$SkipZip
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

function Assert-ChildPath {
    param(
        [string]$ParentPath,
        [string]$ChildPath
    )

    $parentFullPath = [System.IO.Path]::GetFullPath($ParentPath).TrimEnd('\', '/')
    $childFullPath = [System.IO.Path]::GetFullPath($ChildPath).TrimEnd('\', '/')
    if (-not $childFullPath.StartsWith($parentFullPath, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to operate outside package root. Parent: $parentFullPath Child: $childFullPath"
    }
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
if ([string]::IsNullOrWhiteSpace($PackageRoot)) {
    $PackageRoot = Join-Path $ProjectPath "Builds\WindowsPackages"
}

$branding = Get-Branding -RootPath $ProjectPath
$version = $branding.Version
$executableStem = $branding.ExecutableStem

$buildFolder = Join-Path $ProjectPath "Builds\Windows\$version"
$exeName = "${executableStem}_${version}.exe"
$exePath = Join-Path $buildFolder $exeName
$dataFolder = Join-Path $buildFolder "${executableStem}_${version}_Data"

$requiredPaths = @(
    $exePath,
    $dataFolder,
    (Join-Path $buildFolder "UnityPlayer.dll"),
    (Join-Path $buildFolder "MonoBleedingEdge")
)

foreach ($path in $requiredPaths) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Windows package failed: required build artifact is missing: $path"
    }
}

New-Item -ItemType Directory -Force -Path $PackageRoot | Out-Null
$packageFolder = Join-Path $PackageRoot $version
$stagingFolderName = "${executableStem}_${version}_Windows"
$stagingFolder = Join-Path $packageFolder $stagingFolderName

Assert-ChildPath -ParentPath $PackageRoot -ChildPath $packageFolder
Assert-ChildPath -ParentPath $PackageRoot -ChildPath $stagingFolder

if (Test-Path -LiteralPath $stagingFolder) {
    Remove-Item -LiteralPath $stagingFolder -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $stagingFolder | Out-Null
Get-ChildItem -LiteralPath $buildFolder | Copy-Item -Destination $stagingFolder -Recurse -Force

$readmePath = Join-Path $stagingFolder "README_WINDOWS.txt"
$readmeLines = @(
    "Brassworks Breach $version - Windows Build",
    "",
    "Recommended launch path: run LAUNCH_BRASSWORKS_BREACH.bat.",
    "Direct launch path: run $exeName from this same folder.",
    "",
    "Keep this folder together. The executable needs the Data folder, UnityPlayer.dll, and MonoBleedingEdge folder beside it.",
    "",
    "Controls:",
    "- Mouse: look",
    "- WASD: move",
    "- Left Mouse: fire Pressure Pistol",
    "- Right Mouse: Pressure Burst",
    "- E: interact",
    "- Esc: pause or access quit/restart",
    "",
    "Quit:",
    "- Press Esc in game, then use Quit from the pause menu.",
    "- Alt+F4 also closes the Windows player.",
    "",
    "Verification:",
    "- Route audit: V0_ROUTE_AUDIT_PASS",
    "- Full build matrix: V0_BUILD_MATRIX_PASS",
    "",
    "Notes:",
    "- Target: Windows mid/low gaming PC.",
    "- Android, WebGL, SteamVR, and Meta Quest ports are planned but deferred.",
    "- Current art is verified prototype art unless a specific asset doc says otherwise."
)
Set-Content -LiteralPath $readmePath -Value $readmeLines -Encoding UTF8

$launcherPath = Join-Path $stagingFolder "LAUNCH_BRASSWORKS_BREACH.bat"
$launcherLines = @(
    "@echo off",
    "setlocal",
    "cd /d ""%~dp0""",
    "start ""Brassworks Breach"" ""$exeName"""
)
Set-Content -LiteralPath $launcherPath -Value $launcherLines -Encoding ASCII

$quickstartPath = Join-Path $stagingFolder "QUICKSTART_WINDOWS.txt"
$quickstartLines = @(
    "Brassworks Breach $version - Quickstart",
    "",
    "1. Extract the whole ZIP before playing.",
    "2. Open the extracted folder.",
    "3. Double-click LAUNCH_BRASSWORKS_BREACH.bat.",
    "4. If the launcher is blocked by local policy, run $exeName directly from this folder.",
    "",
    "Core controls:",
    "- Mouse: look",
    "- WASD: move",
    "- Left Mouse: fire",
    "- Right Mouse: Pressure Burst",
    "- E: interact",
    "- Esc: pause, restart, or quit",
    "",
    "First-run notes:",
    "- This is an unsigned local Windows development build, so Windows may show a trust warning.",
    "- Do not move the EXE away from its Data folder.",
    "- This package targets a mid/low gaming PC and is not the Android, browser, or VR build."
)
Set-Content -LiteralPath $quickstartPath -Value $quickstartLines -Encoding UTF8

$supportInfoPath = Join-Path $stagingFolder "SUPPORT_INFO_WINDOWS.txt"
$supportInfoLines = @(
    "Brassworks Breach $version - Support Info",
    "",
    "Executable: $exeName",
    "Package folder: $stagingFolderName",
    "Package manifest: ${executableStem}_${version}_WindowsPackageManifest.json",
    "QA packet source: Documentation/QA/WindowsRouteQA/QA_PACKET_$version.md",
    "Issue triage source: Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_$version.md",
    "",
    "When reporting an issue, include:",
    "- Build version",
    "- Level name",
    "- What you were doing",
    "- Expected result",
    "- Actual result",
    "- Whether it blocks progress",
    "",
    "Known scope:",
    "- Windows candidate snapshot only.",
    "- Prototype art remains in places unless called out as promoted in an asset-production document.",
    "- Android, WebGL, SteamVR, and Meta Quest ports are planned but deferred."
)
Set-Content -LiteralPath $supportInfoPath -Value $supportInfoLines -Encoding UTF8

$releaseIndexPath = Join-Path $stagingFolder "RELEASE_INDEX_WINDOWS.txt"
$releaseIndexLines = @(
    "Brassworks Breach $version - Windows Release Index",
    "",
    "Start here when checking package contents.",
    "",
    "Launch files:",
    "- LAUNCH_BRASSWORKS_BREACH.bat: recommended launcher",
    "- ${exeName}: direct Unity player executable",
    "",
    "Required runtime files:",
    "- ${executableStem}_${version}_Data: Unity data folder",
    "- UnityPlayer.dll: Unity runtime library",
    "- MonoBleedingEdge: managed runtime folder",
    "",
    "Player-facing documents:",
    "- QUICKSTART_WINDOWS.txt: shortest launch/control notes",
    "- README_WINDOWS.txt: full Windows build notes",
    "- SUPPORT_INFO_WINDOWS.txt: issue-reporting details",
    "- VERIFY_SHA256_WINDOWS.txt: package hash verification instructions",
    "",
    "QA and release evidence in the repository:",
    "- Documentation/QA/WindowsRouteQA/QA_PACKET_$version.md",
    "- Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_$version.md",
    "- Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_$version.md",
    "- Documentation/Releases/RELEASE_NOTES_$version.md",
    "",
    "Distribution rule:",
    "- Share the ZIP package plus the generated .sha256.txt sidecar.",
    "- Do not share a loose EXE without the Data folder and runtime files."
)
Set-Content -LiteralPath $releaseIndexPath -Value $releaseIndexLines -Encoding UTF8

$checksumInstructionsPath = Join-Path $stagingFolder "VERIFY_SHA256_WINDOWS.txt"
$checksumInstructionsLines = @(
    "Brassworks Breach $version - SHA-256 Verification",
    "",
    "Use this when receiving or redistributing the Windows ZIP package.",
    "",
    "1. Keep the ZIP and its generated .sha256.txt sidecar in the same folder.",
    "2. Open PowerShell in that folder.",
    "3. Run:",
    "   Get-FileHash -Algorithm SHA256 '.\${executableStem}_${version}_Windows.zip'",
    "4. Compare the Hash value to the first value in:",
    "   ${executableStem}_${version}_Windows.zip.sha256.txt",
    "5. The same hash is also recorded in:",
    "   Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_$version.md",
    "",
    "If the values differ, do not treat that ZIP as the verified candidate package.",
    "",
    "Note: this text file is packaged inside the ZIP as verification guidance. The actual ZIP hash is generated next to the ZIP after packaging."
)
Set-Content -LiteralPath $checksumInstructionsPath -Value $checksumInstructionsLines -Encoding UTF8

$zipPath = Join-Path $packageFolder "${executableStem}_${version}_Windows.zip"
if (-not $SkipZip) {
    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    Compress-Archive -LiteralPath $stagingFolder -DestinationPath $zipPath -Force
}

$hash = $null
$sha256SidecarPath = if ($SkipZip) { $null } else { $zipPath + ".sha256.txt" }
if (-not $SkipZip) {
    $hash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash
    Set-Content -LiteralPath $sha256SidecarPath -Value "$hash  $(Split-Path -Leaf $zipPath)" -Encoding ASCII
}

$manifest = [ordered]@{
    version = $version
    executable = $exeName
    build_folder = $buildFolder
    staging_folder = $stagingFolder
    launcher = $launcherPath
    readme = $readmePath
    quickstart = $quickstartPath
    support_info = $supportInfoPath
    release_index = $releaseIndexPath
    checksum_instructions = $checksumInstructionsPath
    zip_path = if ($SkipZip) { $null } else { $zipPath }
    sha256_sidecar = $sha256SidecarPath
    sha256 = $hash
    generated_utc = (Get-Date).ToUniversalTime().ToString("o")
    required_artifacts = $requiredPaths
}

$manifestPath = Join-Path $packageFolder "${executableStem}_${version}_WindowsPackageManifest.json"
$manifest | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $manifestPath -Encoding UTF8

Write-Host "V0_WINDOWS_PACKAGE_PASS $version $zipPath"
