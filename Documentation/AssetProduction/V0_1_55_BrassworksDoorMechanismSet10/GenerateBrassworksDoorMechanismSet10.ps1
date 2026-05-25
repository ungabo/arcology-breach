param(
    [string]$UnityExe = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe",
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path,
    [switch]$KeepTempProject
)

$ErrorActionPreference = "Stop"
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

function Write-Utf8NoBom {
    param(
        [string]$Path,
        [string]$Value
    )

    [System.IO.Directory]::CreateDirectory([System.IO.Path]::GetDirectoryName($Path)) | Out-Null
    [System.IO.File]::WriteAllText($Path, $Value, $utf8NoBom)
}

$packageRoot = Join-Path $RepoRoot "AssetPacks\BrassworksBreach.BrassworksDoorMechanismSet10"
$assetPacksRoot = Join-Path $RepoRoot "AssetPacks"
$renderRoot = Join-Path $RepoRoot "Documentation\ConceptRenders\V0_1_55_BrassworksDoorMechanismSet10"
$planningRoot = Join-Path $RepoRoot "Documentation\Planning\V0_1_55_BrassworksDoorMechanismSet10ImportReadiness"
$qaRoot = Join-Path $RepoRoot "Documentation\QA\V0_1_55_BrassworksDoorMechanismSet10ImportReadiness"
$logRoot = Join-Path $RepoRoot "Logs"
$packageName = "com.brassworks.sidecar.brassworks-door-mechanism-set10"

if (!(Test-Path -LiteralPath $UnityExe)) {
    throw "Unity executable not found at $UnityExe"
}

New-Item -ItemType Directory -Force -Path $assetPacksRoot, $renderRoot, $planningRoot, $qaRoot, $logRoot | Out-Null

$resolvedAssetPacksRoot = (Resolve-Path -LiteralPath $assetPacksRoot).Path.TrimEnd("\")
$expectedPackageRoot = Join-Path $resolvedAssetPacksRoot "BrassworksBreach.BrassworksDoorMechanismSet10"
if ([System.IO.Path]::GetFullPath($packageRoot).TrimEnd("\") -ne [System.IO.Path]::GetFullPath($expectedPackageRoot).TrimEnd("\")) {
    throw "Refusing to reset unexpected package path: $packageRoot"
}

if (Test-Path -LiteralPath $packageRoot) {
    Remove-Item -LiteralPath $packageRoot -Recurse -Force
}

$packageDirectories = @(
    (Join-Path $packageRoot "Runtime"),
    (Join-Path $packageRoot "Runtime\Materials"),
    (Join-Path $packageRoot "Runtime\Meshes"),
    (Join-Path $packageRoot "Runtime\Textures"),
    (Join-Path $packageRoot "Runtime\Prefabs"),
    (Join-Path $packageRoot "Runtime\Metadata"),
    (Join-Path $packageRoot "Documentation~\Manifest"),
    (Join-Path $packageRoot "Documentation~\Previews"),
    (Join-Path $packageRoot "Samples~\PreviewScene")
)
New-Item -ItemType Directory -Force -Path $packageDirectories | Out-Null

$packageJson = @"
{
  "name": "$packageName",
  "version": "0.1.55-p001",
  "displayName": "Brassworks Breach Brassworks Door Mechanism Set 10",
  "description": "Unity-only visual sidecar package for detailed steampunk door mechanism objects: gear hubs, locking bars, piston braces, riveted hinges, pressure wheels, bolt collars, track rails, amber lamp capsules, gauge/valve subassemblies, and pressure-lock modules.",
  "unity": "6000.4",
  "author": {
    "name": "Brassworks Breach Sidecar Production"
  },
  "keywords": [
    "brassworks",
    "sidecar",
    "visual-only",
    "steampunk",
    "door-mechanism",
    "gears",
    "locking-bars",
    "pistons",
    "gauges",
    "valves"
  ],
  "dependencies": {},
  "samples": [
    {
      "displayName": "Preview Notes",
      "description": "Import-safe notes for Set10 brassworks door mechanism visual prefabs.",
      "path": "Samples~/PreviewScene"
    }
  ]
}
"@
Write-Utf8NoBom -Path (Join-Path $packageRoot "package.json") -Value $packageJson

$readme = @"
# Brassworks Breach Brassworks Door Mechanism Set 10

Unity-only visual sidecar candidate for detailed steampunk door mechanism objects.

This package is intentionally isolated. It does not alter the main project manifest, scenes, build scripts, or shared status documents.
"@
Write-Utf8NoBom -Path (Join-Path $packageRoot "README.md") -Value $readme

$changelog = @"
# Changelog

## 0.1.55-p001

- Added first isolated visual-only package candidate for brassworks door mechanism components.
- Generated Unity prefabs, materials, texture maps, mesh assets, preview renders, manifest, and import-readiness QA docs.
"@
Write-Utf8NoBom -Path (Join-Path $packageRoot "CHANGELOG.md") -Value $changelog

$sampleReadme = @"
# Preview Notes

All prefabs are visual-only. They are intended for quarantine import review and later composition into corridor/vault door scenes.
"@
Write-Utf8NoBom -Path (Join-Path $packageRoot "Samples~\PreviewScene\README.md") -Value $sampleReadme

$tempProject = Join-Path $env:TEMP ("BDM10_UnityProject_" + [Guid]::NewGuid().ToString("N"))
$generateLog = Join-Path $logRoot "bdm10-generate.log"

$tempAssets = Join-Path $tempProject "Assets"
$tempPackages = Join-Path $tempProject "Packages"
$tempProjectSettings = Join-Path $tempProject "ProjectSettings"
New-Item -ItemType Directory -Force -Path $tempAssets, $tempPackages, $tempProjectSettings | Out-Null
Write-Utf8NoBom -Path (Join-Path $tempProjectSettings "ProjectVersion.txt") -Value "m_EditorVersion: 6000.4.6f1`nm_EditorVersionWithRevision: 6000.4.6f1 (0b051c2e5d54)`n"

$packageUriPath = $packageRoot.Replace("\", "/")
$tempManifest = @"
{
  "dependencies": {
    "$packageName": "file:$packageUriPath",
    "com.unity.modules.animation": "1.0.0",
    "com.unity.modules.audio": "1.0.0",
    "com.unity.modules.imageconversion": "1.0.0",
    "com.unity.modules.physics": "1.0.0"
  }
}
"@
Write-Utf8NoBom -Path (Join-Path $tempPackages "manifest.json") -Value $tempManifest

Copy-Item -LiteralPath (Join-Path $PSScriptRoot "BDM10Generator.cs") -Destination (Join-Path $tempAssets "BDM10Generator.cs") -Force

$env:BDM10_REPO_ROOT = $RepoRoot
& $UnityExe -batchmode -projectPath $tempProject -executeMethod BrassworksBreach.AssetProduction.BDM10Generator.GenerateAll -quit -logFile $generateLog
$exitCode = $LASTEXITCODE
Remove-Item Env:\BDM10_REPO_ROOT -ErrorAction SilentlyContinue
$generatePassed = (Test-Path -LiteralPath $generateLog) -and ((Get-Content -LiteralPath $generateLog -Raw) -match "BDM10_GENERATE_PASS")
if ($exitCode -ne 0 -and !$generatePassed) {
    throw "BDM10 generation failed. See $generateLog"
}

if (!$KeepTempProject) {
    $resolvedTemp = [System.IO.Path]::GetFullPath($tempProject)
    $resolvedTempRoot = [System.IO.Path]::GetFullPath($env:TEMP).TrimEnd("\")
    if (!$resolvedTemp.StartsWith($resolvedTempRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove unexpected temp project: $tempProject"
    }
    Remove-Item -LiteralPath $tempProject -Recurse -Force
}

Write-Host "BDM10_GENERATE_PASS"
Write-Host "Package: $packageRoot"
Write-Host "Concept renders: $renderRoot"
Write-Host "QA: $qaRoot"
