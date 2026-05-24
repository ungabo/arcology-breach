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
    "Run $exeName to play.",
    "",
    "Controls:",
    "- Mouse: look",
    "- WASD: move",
    "- Left Mouse: fire Pressure Pistol",
    "- Right Mouse: Pressure Burst",
    "- E: interact",
    "- Esc: pause or access quit/restart",
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

$zipPath = Join-Path $packageFolder "${executableStem}_${version}_Windows.zip"
if (-not $SkipZip) {
    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    Compress-Archive -LiteralPath $stagingFolder -DestinationPath $zipPath -Force
}

$hash = $null
if (-not $SkipZip) {
    $hash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash
    Set-Content -LiteralPath ($zipPath + ".sha256.txt") -Value "$hash  $(Split-Path -Leaf $zipPath)" -Encoding ASCII
}

$manifest = [ordered]@{
    version = $version
    executable = $exeName
    build_folder = $buildFolder
    staging_folder = $stagingFolder
    zip_path = if ($SkipZip) { $null } else { $zipPath }
    sha256 = $hash
    generated_utc = (Get-Date).ToUniversalTime().ToString("o")
    required_artifacts = $requiredPaths
}

$manifestPath = Join-Path $packageFolder "${executableStem}_${version}_WindowsPackageManifest.json"
$manifest | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $manifestPath -Encoding UTF8

Write-Host "V0_WINDOWS_PACKAGE_PASS $version $zipPath"
