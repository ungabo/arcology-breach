param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$AssetPacksPath,
    [string]$PackageNamePattern = "BrassworksBreach.*",
    [switch]$AllowEmpty,
    [switch]$Json
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Add-Finding {
    param(
        [System.Collections.Generic.List[object]]$Findings,
        [string]$Severity,
        [string]$Package,
        [string]$Path,
        [string]$Message
    )

    $Findings.Add([pscustomobject]@{
        severity = $Severity
        package = $Package
        path = $Path
        message = $Message
    }) | Out-Null
}

function Convert-ToRepoPath {
    param(
        [string]$RootPath,
        [string]$AbsolutePath
    )

    if ([string]::IsNullOrWhiteSpace($AbsolutePath)) {
        return ""
    }

    $root = [System.IO.Path]::GetFullPath($RootPath).TrimEnd('\', '/')
    $full = [System.IO.Path]::GetFullPath($AbsolutePath)
    if ($full.StartsWith($root, [System.StringComparison]::OrdinalIgnoreCase)) {
        return $full.Substring($root.Length).TrimStart('\', '/') -replace '\\', '/'
    }

    return $full -replace '\\', '/'
}

function Test-JsonFile {
    param(
        [string]$Path,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$Package,
        [string]$RootPath
    )

    try {
        $raw = Get-Content -LiteralPath $Path -Raw
        return $raw | ConvertFrom-Json
    }
    catch {
        Add-Finding -Findings $Findings -Severity "error" -Package $Package -Path (Convert-ToRepoPath -RootPath $RootPath -AbsolutePath $Path) -Message "JSON parse failed: $($_.Exception.Message)"
        return $null
    }
}

function Get-TextCandidateFiles {
    param([string]$RootPath)

    $textExtensions = @(
        ".asmdef",
        ".asset",
        ".cginc",
        ".compute",
        ".controller",
        ".cs",
        ".css",
        ".csv",
        ".html",
        ".json",
        ".mat",
        ".md",
        ".meta",
        ".prefab",
        ".shader",
        ".txt",
        ".unity",
        ".uss",
        ".uxml",
        ".xml",
        ".yaml",
        ".yml"
    )

    Get-ChildItem -LiteralPath $RootPath -Recurse -File -Force |
        Where-Object { $textExtensions -contains $_.Extension.ToLowerInvariant() }
}

function Test-PackageJson {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$ProjectRoot
    )

    $packageName = $PackageRoot.Name
    $packageJsonPath = Join-Path $PackageRoot.FullName "package.json"
    if (-not (Test-Path -LiteralPath $packageJsonPath)) {
        Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $packageJsonPath) -Message "Missing package.json at package root."
        return
    }

    $packageJson = Test-JsonFile -Path $packageJsonPath -Findings $Findings -Package $packageName -RootPath $ProjectRoot
    if ($null -eq $packageJson) {
        return
    }

    $requiredFields = @("name", "version", "displayName", "description", "unity")
    foreach ($field in $requiredFields) {
        $property = $packageJson.PSObject.Properties[$field]
        if ($null -eq $property -or [string]::IsNullOrWhiteSpace([string]$property.Value)) {
            Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $packageJsonPath) -Message "package.json is missing required field '$field'."
        }
    }

    if ($packageJson.PSObject.Properties["name"] -and ([string]$packageJson.name -notmatch '^(com\.brassworks\.sidecar\.|brassworksbreach\.)')) {
        Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $packageJsonPath) -Message "Package name does not use the expected Brassworks sidecar namespace."
    }

    if ($packageJson.PSObject.Properties["dependencies"] -and $null -ne $packageJson.dependencies) {
        $dependencyProperties = @($packageJson.dependencies.PSObject.Properties)
        $dependencyCount = $dependencyProperties.Count
        if ($dependencyCount -gt 0) {
            Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $packageJsonPath) -Message "Package declares $dependencyCount dependency/dependencies; confirm each one is approved by the integration gate."
        }
    }
}

function Test-RequiredPackageShape {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$ProjectRoot
    )

    $packageName = $PackageRoot.Name
    $requiredFolders = @("Runtime", "Documentation~")
    foreach ($folder in $requiredFolders) {
        $folderPath = Join-Path $PackageRoot.FullName $folder
        if (-not (Test-Path -LiteralPath $folderPath -PathType Container)) {
            Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $folderPath) -Message "Missing required package folder '$folder'."
        }
    }

    $recommendedFiles = @("README.md", "CHANGELOG.md")
    foreach ($file in $recommendedFiles) {
        $filePath = Join-Path $PackageRoot.FullName $file
        if (-not (Test-Path -LiteralPath $filePath -PathType Leaf)) {
            Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $filePath) -Message "Recommended package file '$file' is missing."
        }
    }

    $forbiddenRoots = @("ProjectSettings", "Packages", "Library", "Temp", "Logs", "UserSettings")
    foreach ($folder in $forbiddenRoots) {
        $folderPath = Join-Path $PackageRoot.FullName $folder
        if (Test-Path -LiteralPath $folderPath) {
            Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $folderPath) -Message "Forbidden Unity project folder found inside asset pack."
        }
    }
}

function Test-ManifestFiles {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$ProjectRoot
    )

    $packageName = $PackageRoot.Name
    $manifestFiles = @(Get-ChildItem -LiteralPath $PackageRoot.FullName -Recurse -File -Force -Filter "*.json" |
        Where-Object {
            $_.Name -match 'manifest' -or
            $_.DirectoryName -match [regex]::Escape("Documentation~") -and $_.DirectoryName -match 'Manifest'
        })

    if ($manifestFiles.Count -eq 0) {
        Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $PackageRoot.FullName) -Message "No sidecar manifest JSON found."
        return
    }

    $requiredFields = @(
        "pack_id",
        "display_name",
        "version",
        "build_id",
        "unity_version",
        "sidecar_project",
        "owner_lane",
        "primary_intake_owner",
        "canonical_root",
        "asset_counts",
        "dependencies",
        "required_primary_changes",
        "path_collisions_checked",
        "guid_collisions_checked",
        "import_smoke_status",
        "known_risks",
        "rollback_path"
    )

    foreach ($manifestFile in $manifestFiles) {
        $manifest = Test-JsonFile -Path $manifestFile.FullName -Findings $Findings -Package $packageName -RootPath $ProjectRoot
        if ($null -eq $manifest) {
            continue
        }

        foreach ($field in $requiredFields) {
            if ($null -eq $manifest.PSObject.Properties[$field]) {
                Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $manifestFile.FullName) -Message "Manifest is missing required field '$field'."
            }
        }

        if ($manifest.PSObject.Properties["path_collisions_checked"] -and $manifest.path_collisions_checked -ne $true) {
            Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $manifestFile.FullName) -Message "Manifest does not confirm path collision check."
        }

        if ($manifest.PSObject.Properties["guid_collisions_checked"] -and $manifest.guid_collisions_checked -ne $true) {
            Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $manifestFile.FullName) -Message "Manifest does not confirm GUID collision check."
        }
    }
}

function Test-ConflictMarkers {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$ProjectRoot
    )

    $packageName = $PackageRoot.Name
    foreach ($file in Get-TextCandidateFiles -RootPath $PackageRoot.FullName) {
        $matches = @(Select-String -LiteralPath $file.FullName -Pattern '^(<<<<<<<|=======|>>>>>>>)' -ErrorAction SilentlyContinue)
        foreach ($match in $matches) {
            Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $file.FullName) -Message "Conflict marker found at line $($match.LineNumber)."
        }
    }
}

function Test-MetaConsistency {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$ProjectRoot
    )

    $packageName = $PackageRoot.Name
    $ignoredNames = @("package.json", "README.md", "CHANGELOG.md", "LICENSE.md")
    $assetRoots = @("Runtime", "Samples~")

    foreach ($assetRoot in $assetRoots) {
        $rootPath = Join-Path $PackageRoot.FullName $assetRoot
        if (-not (Test-Path -LiteralPath $rootPath -PathType Container)) {
            continue
        }

        $sourceFiles = @(Get-ChildItem -LiteralPath $rootPath -Recurse -File -Force | Where-Object {
            $_.Extension -ne ".meta" -and
            ($ignoredNames -notcontains $_.Name)
        })

        foreach ($sourceFile in $sourceFiles) {
            $metaPath = "$($sourceFile.FullName).meta"
            if (-not (Test-Path -LiteralPath $metaPath -PathType Leaf)) {
                Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $sourceFile.FullName) -Message "Source asset has no adjacent .meta file."
            }
        }
    }

    $metaFiles = @(Get-ChildItem -LiteralPath $PackageRoot.FullName -Recurse -File -Force -Filter "*.meta")
    $guidMap = @{}
    foreach ($metaFile in $metaFiles) {
        $sourcePath = $metaFile.FullName.Substring(0, $metaFile.FullName.Length - 5)
        if (-not (Test-Path -LiteralPath $sourcePath)) {
            Add-Finding -Findings $Findings -Severity "warning" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $metaFile.FullName) -Message ".meta file has no adjacent source file or folder."
        }

        $guidMatch = Select-String -LiteralPath $metaFile.FullName -Pattern '^guid:\s*([0-9a-fA-F]+)' -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($guidMatch) {
            $guid = $guidMatch.Matches[0].Groups[1].Value.ToLowerInvariant()
            if (-not $guidMap.ContainsKey($guid)) {
                $guidMap[$guid] = New-Object System.Collections.Generic.List[string]
            }
            $guidMap[$guid].Add($metaFile.FullName)
        }
    }

    foreach ($entry in $guidMap.GetEnumerator()) {
        if ($entry.Value.Count -gt 1) {
            foreach ($duplicatePath in $entry.Value) {
                Add-Finding -Findings $Findings -Severity "error" -Package $packageName -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $duplicatePath) -Message "Duplicate Unity meta GUID detected inside asset pack: $($entry.Key)."
            }
        }
    }
}

if ([string]::IsNullOrWhiteSpace($AssetPacksPath)) {
    $AssetPacksPath = Join-Path $ProjectPath "AssetPacks"
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$findings = New-Object System.Collections.Generic.List[object]
$packages = @()

if (-not (Test-Path -LiteralPath $AssetPacksPath -PathType Container)) {
    if ($AllowEmpty) {
        Write-Host "Sidecar validation: AssetPacks folder not found, and -AllowEmpty was supplied."
        exit 0
    }

    throw "AssetPacks folder was not found: $AssetPacksPath"
}

$AssetPacksPath = (Resolve-Path -LiteralPath $AssetPacksPath).Path
$packages = @(Get-ChildItem -LiteralPath $AssetPacksPath -Directory -Force | Where-Object { $_.Name -like $PackageNamePattern })

if ($packages.Count -eq 0) {
    if ($AllowEmpty) {
        Write-Host "Sidecar validation: no packages matched '$PackageNamePattern' under $AssetPacksPath, and -AllowEmpty was supplied."
        exit 0
    }

    throw "No sidecar packages matched '$PackageNamePattern' under $AssetPacksPath"
}

foreach ($package in $packages) {
    Test-PackageJson -PackageRoot $package -Findings $findings -ProjectRoot $ProjectPath
    Test-RequiredPackageShape -PackageRoot $package -Findings $findings -ProjectRoot $ProjectPath
    Test-ManifestFiles -PackageRoot $package -Findings $findings -ProjectRoot $ProjectPath
    Test-ConflictMarkers -PackageRoot $package -Findings $findings -ProjectRoot $ProjectPath
    Test-MetaConsistency -PackageRoot $package -Findings $findings -ProjectRoot $ProjectPath
}

$errorCount = @($findings | Where-Object { $_.severity -eq "error" }).Count
$warningCount = @($findings | Where-Object { $_.severity -eq "warning" }).Count
$status = if ($errorCount -eq 0) { "pass" } else { "fail" }
$packageNames = [string[]]@($packages | ForEach-Object { $_.Name })
$findingItems = $findings.ToArray()

$result = [pscustomobject]@{
    status = $status
    project_path = $ProjectPath
    asset_packs_path = $AssetPacksPath
    package_pattern = $PackageNamePattern
    package_count = $packages.Count
    errors = $errorCount
    warnings = $warningCount
    packages = $packageNames
    findings = $findingItems
}

if ($Json) {
    $result | ConvertTo-Json -Depth 8
}
else {
    Write-Host "Sidecar asset-pack validation"
    Write-Host "Project: $ProjectPath"
    Write-Host "Asset packs: $AssetPacksPath"
    Write-Host "Pattern: $PackageNamePattern"
    Write-Host "Packages checked: $($packages.Count)"
    Write-Host "Errors: $errorCount"
    Write-Host "Warnings: $warningCount"

    foreach ($finding in $findings) {
        $label = $finding.severity.ToUpperInvariant()
        Write-Host "[$label] $($finding.package) $($finding.path) - $($finding.message)"
    }
}

if ($errorCount -gt 0) {
    exit 1
}

exit 0
