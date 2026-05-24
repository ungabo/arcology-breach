param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$AssetPacksPath,
    [string]$PackageNamePattern = "BrassworksBreach.*",
    [string]$OutputDirectory,
    [string]$OutputMarkdown,
    [string]$OutputJson,
    [switch]$AllowEmpty,
    [switch]$ConsoleJson
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

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

function Get-PropertyValue {
    param(
        [object]$Object,
        [string]$Name
    )

    if ($null -eq $Object) {
        return $null
    }

    $property = $Object.PSObject.Properties[$Name]
    if ($null -eq $property) {
        return $null
    }

    return $property.Value
}

function Get-ArrayPropertyValue {
    param(
        [object]$Object,
        [string]$Name
    )

    $value = Get-PropertyValue -Object $Object -Name $Name
    if ($null -eq $value) {
        return @()
    }

    if ($value -is [string]) {
        return @($value)
    }

    return @($value)
}

function Read-JsonOrFinding {
    param(
        [string]$Path,
        [System.Collections.Generic.List[object]]$Findings,
        [string]$Package,
        [string]$ProjectRoot
    )

    try {
        $raw = Get-Content -LiteralPath $Path -Raw
        return $raw | ConvertFrom-Json
    }
    catch {
        Add-Finding -Findings $Findings -Severity "error" -Package $Package -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $Path) -Message "JSON parse failed: $($_.Exception.Message)"
        return $null
    }
}

function Test-PlaceholderReference {
    param([string]$Reference)

    if ([string]::IsNullOrWhiteSpace($Reference)) {
        return $true
    }

    return ($Reference -match '(^|/)(generated_by_|TBD|TODO|PLACEHOLDER)')
}

function Resolve-ManifestReference {
    param(
        [string]$ProjectRoot,
        [System.IO.DirectoryInfo]$PackageRoot,
        [string[]]$PackageNames,
        [string]$Reference
    )

    $normalized = $Reference -replace '\\', '/'
    $isPlaceholder = Test-PlaceholderReference -Reference $normalized
    if ($isPlaceholder) {
        return [pscustomobject]@{
            reference = $Reference
            disk_path = ""
            repo_path = $normalized
            exists = $false
            is_placeholder = $true
        }
    }

    if ([System.IO.Path]::IsPathRooted($Reference)) {
        $fullPath = [System.IO.Path]::GetFullPath($Reference)
        return [pscustomobject]@{
            reference = $Reference
            disk_path = $fullPath
            repo_path = Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $fullPath
            exists = Test-Path -LiteralPath $fullPath
            is_placeholder = $false
        }
    }

    foreach ($packageName in $PackageNames) {
        if ([string]::IsNullOrWhiteSpace($packageName)) {
            continue
        }

        $packagePrefix = "Packages/$packageName/"
        if ($normalized.StartsWith($packagePrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            $relativeInsidePackage = $normalized.Substring($packagePrefix.Length)
            $fullPath = Join-Path $PackageRoot.FullName ($relativeInsidePackage -replace '/', [System.IO.Path]::DirectorySeparatorChar)
            return [pscustomobject]@{
                reference = $Reference
                disk_path = $fullPath
                repo_path = Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $fullPath
                exists = Test-Path -LiteralPath $fullPath
                is_placeholder = $false
            }
        }
    }

    if ($normalized.StartsWith("AssetPacks/", [System.StringComparison]::OrdinalIgnoreCase) -or
        $normalized.StartsWith("Documentation/", [System.StringComparison]::OrdinalIgnoreCase) -or
        $normalized.StartsWith("Tools/", [System.StringComparison]::OrdinalIgnoreCase)) {
        $fullPath = Join-Path $ProjectRoot ($normalized -replace '/', [System.IO.Path]::DirectorySeparatorChar)
        return [pscustomobject]@{
            reference = $Reference
            disk_path = $fullPath
            repo_path = Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $fullPath
            exists = Test-Path -LiteralPath $fullPath
            is_placeholder = $false
        }
    }

    $packageRelativePath = Join-Path $PackageRoot.FullName ($normalized -replace '/', [System.IO.Path]::DirectorySeparatorChar)
    return [pscustomobject]@{
        reference = $Reference
        disk_path = $packageRelativePath
        repo_path = Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $packageRelativePath
        exists = Test-Path -LiteralPath $packageRelativePath
        is_placeholder = $false
    }
}

function Get-ExpectedCount {
    param(
        [object]$Manifest,
        [string[]]$CountKeys
    )

    $assetCounts = Get-PropertyValue -Object $Manifest -Name "asset_counts"
    foreach ($key in $CountKeys) {
        $value = Get-PropertyValue -Object $assetCounts -Name $key
        if ($null -ne $value) {
            return [int]$value
        }
    }

    return $null
}

function Get-ObservedFiles {
    param(
        [System.IO.DirectoryInfo]$PackageRoot,
        [string]$Category
    )

    $runtimeRoot = Join-Path $PackageRoot.FullName "Runtime"
    if (-not (Test-Path -LiteralPath $runtimeRoot -PathType Container)) {
        return @()
    }

    if ($Category -eq "generated_prefabs") {
        $folder = Join-Path $runtimeRoot "Prefabs"
        if (Test-Path -LiteralPath $folder -PathType Container) {
            return @(Get-ChildItem -LiteralPath $folder -Recurse -File -Force -Filter "*.prefab")
        }
        return @()
    }

    if ($Category -eq "generated_materials") {
        $folder = Join-Path $runtimeRoot "Materials"
        if (Test-Path -LiteralPath $folder -PathType Container) {
            return @(Get-ChildItem -LiteralPath $folder -Recurse -File -Force -Filter "*.mat")
        }
        return @()
    }

    if ($Category -eq "generated_meshes") {
        $folder = Join-Path $runtimeRoot "Meshes"
        if (Test-Path -LiteralPath $folder -PathType Container) {
            return @(Get-ChildItem -LiteralPath $folder -Recurse -File -Force | Where-Object { $_.Extension -ne ".meta" })
        }
        return @()
    }

    return @()
}

function Get-ExpectedReferences {
    param(
        [object]$Manifest,
        [string]$PropertyName
    )

    $items = @(Get-ArrayPropertyValue -Object $Manifest -Name $PropertyName)
    if ($items.Count -gt 0) {
        return @($items | ForEach-Object { [string]$_ })
    }

    if ($PropertyName -eq "generated_prefabs") {
        $prefabObjects = @(Get-ArrayPropertyValue -Object $Manifest -Name "prefabs")
        $prefabPaths = @()
        foreach ($prefabObject in $prefabObjects) {
            $path = Get-PropertyValue -Object $prefabObject -Name "path"
            if ($null -ne $path) {
                $prefabPaths += [string]$path
            }
        }
        return $prefabPaths
    }

    return @()
}

function Test-ManifestCategory {
    param(
        [string]$ProjectRoot,
        [System.IO.DirectoryInfo]$PackageRoot,
        [string[]]$PackageNames,
        [object]$Manifest,
        [string]$ManifestPath,
        [string]$Category,
        [string[]]$CountKeys,
        [System.Collections.Generic.List[object]]$Findings
    )

    $expectedReferences = @(Get-ExpectedReferences -Manifest $Manifest -PropertyName $Category)
    $expectedCount = Get-ExpectedCount -Manifest $Manifest -CountKeys $CountKeys
    $resolved = @($expectedReferences | ForEach-Object {
        Resolve-ManifestReference -ProjectRoot $ProjectRoot -PackageRoot $PackageRoot -PackageNames $PackageNames -Reference $_
    })
    $placeholderReferences = @($resolved | Where-Object { $_.is_placeholder })
    $missingReferences = @($resolved | Where-Object { -not $_.is_placeholder -and -not $_.exists })
    $existingReferences = @($resolved | Where-Object { $_.exists })
    if ($Category -eq "preview_renders") {
        $observedFiles = @($existingReferences | ForEach-Object {
            [pscustomobject]@{
                FullName = $_.disk_path
            }
        })
    }
    else {
        $observedFiles = @(Get-ObservedFiles -PackageRoot $PackageRoot -Category $Category)
    }

    foreach ($missing in $missingReferences) {
        Add-Finding -Findings $Findings -Severity "warning" -Package $PackageRoot.Name -Path $missing.repo_path -Message "Manifest lists missing $Category asset: $($missing.reference)"
    }

    foreach ($placeholder in $placeholderReferences) {
        Add-Finding -Findings $Findings -Severity "warning" -Package $PackageRoot.Name -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $ManifestPath) -Message "Manifest uses placeholder $Category reference: $($placeholder.reference)"
    }

    if ($null -ne $expectedCount -and $expectedCount -gt $observedFiles.Count) {
        Add-Finding -Findings $Findings -Severity "warning" -Package $PackageRoot.Name -Path (Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $PackageRoot.FullName) -Message "$Category count expects $expectedCount but only $($observedFiles.Count) matching file(s) are on disk."
    }

    return [pscustomobject]@{
        category = $Category
        expected_count = $expectedCount
        manifest_reference_count = $expectedReferences.Count
        manifest_existing_count = $existingReferences.Count
        manifest_missing_count = $missingReferences.Count
        manifest_placeholder_count = $placeholderReferences.Count
        observed_disk_count = $observedFiles.Count
        missing_references = @($missingReferences | ForEach-Object { $_.reference })
        placeholder_references = @($placeholderReferences | ForEach-Object { $_.reference })
        observed_files = @($observedFiles | ForEach-Object { Convert-ToRepoPath -RootPath $ProjectRoot -AbsolutePath $_.FullName })
    }
}

function Get-MarkdownCell {
    param([object]$Value)

    $text = [string]$Value
    if ([string]::IsNullOrWhiteSpace($text)) {
        return ""
    }

    return ($text -replace '\|', '\|' -replace "`r?`n", " ")
}

function New-MarkdownReport {
    param(
        [object]$Report
    )

    $builder = New-Object System.Text.StringBuilder
    [void]$builder.AppendLine("# V0.1.38 Sidecar Quarantine Readiness Report")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("Generated: $($Report.generated_at)")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("Purpose: inventory sidecar asset packages before primary-lane quarantine import. This report is static and non-destructive; it does not edit Unity manifests, import packages, or modify the primary project.")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("## Summary")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("| Field | Value |")
    [void]$builder.AppendLine("| --- | --- |")
    [void]$builder.AppendLine("| Project | $(Get-MarkdownCell $Report.project_path) |")
    [void]$builder.AppendLine("| Asset packs | $(Get-MarkdownCell $Report.asset_packs_path) |")
    [void]$builder.AppendLine("| Package pattern | $(Get-MarkdownCell $Report.package_pattern) |")
    [void]$builder.AppendLine("| Packages checked | $($Report.package_count) |")
    [void]$builder.AppendLine("| Errors | $($Report.errors) |")
    [void]$builder.AppendLine("| Warnings | $($Report.warnings) |")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("## Package Readiness")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("| Package | UPM name | Version | Manifests | Decision | Errors | Warnings |")
    [void]$builder.AppendLine("| --- | --- | --- | ---: | --- | ---: | ---: |")
    foreach ($package in $Report.packages) {
        [void]$builder.AppendLine("| $(Get-MarkdownCell $package.package_folder) | $(Get-MarkdownCell $package.upm_name) | $(Get-MarkdownCell $package.version) | $($package.manifest_count) | $(Get-MarkdownCell $package.decision) | $($package.errors) | $($package.warnings) |")
    }
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("## Manifest Asset Checks")
    foreach ($package in $Report.packages) {
        [void]$builder.AppendLine("")
        [void]$builder.AppendLine("### $($package.package_folder)")
        if ($package.manifests.Count -eq 0) {
            [void]$builder.AppendLine("")
            [void]$builder.AppendLine("No package-local manifest was found.")
            continue
        }

        foreach ($manifest in $package.manifests) {
            [void]$builder.AppendLine("")
            [void]$builder.AppendLine("Manifest: $($manifest.path)")
            [void]$builder.AppendLine("")
            [void]$builder.AppendLine("| Category | Expected | Manifest refs | Existing refs | Missing refs | Placeholders | Disk count |")
            [void]$builder.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: | ---: |")
            foreach ($category in $manifest.categories) {
                $expected = ""
                if ($null -ne $category.expected_count) {
                    $expected = [string]$category.expected_count
                }
                [void]$builder.AppendLine("| $($category.category) | $expected | $($category.manifest_reference_count) | $($category.manifest_existing_count) | $($category.manifest_missing_count) | $($category.manifest_placeholder_count) | $($category.observed_disk_count) |")
            }
        }
    }
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("## Findings")
    [void]$builder.AppendLine("")
    if ($Report.findings.Count -eq 0) {
        [void]$builder.AppendLine("No findings.")
    }
    else {
        [void]$builder.AppendLine("| Severity | Package | Path | Message |")
        [void]$builder.AppendLine("| --- | --- | --- | --- |")
        foreach ($finding in $Report.findings) {
            [void]$builder.AppendLine("| $($finding.severity) | $(Get-MarkdownCell $finding.package) | $(Get-MarkdownCell $finding.path) | $(Get-MarkdownCell $finding.message) |")
        }
    }
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("## How To Use This Report")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("- ready_for_primary_quarantine: static inventory is clean enough for primary-lane quarantine import after the clean throwaway import evidence is reviewed.")
    [void]$builder.AppendLine("- needs_generation_or_remediation: the package is useful but still has placeholder or missing generated assets; run its sidecar generator/render pass before quarantine import.")
    [void]$builder.AppendLine("- blocked_static_errors: fix package/manifest structure before any Unity import work.")
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("Next-step directive: continue immediately with the next highest-impact unfinished task.")

    return $builder.ToString()
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
if ([string]::IsNullOrWhiteSpace($AssetPacksPath)) {
    $AssetPacksPath = Join-Path $ProjectPath "AssetPacks"
}
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $ProjectPath "Documentation\QA\V0_1_38_QuarantineImportPrep"
}
if ([string]::IsNullOrWhiteSpace($OutputMarkdown)) {
    $OutputMarkdown = Join-Path $OutputDirectory "QUARANTINE_READINESS_REPORT_v0.1.38.md"
}
if ([string]::IsNullOrWhiteSpace($OutputJson)) {
    $OutputJson = Join-Path $OutputDirectory "QUARANTINE_READINESS_REPORT_v0.1.38.json"
}

$findings = New-Object System.Collections.Generic.List[object]

if (-not (Test-Path -LiteralPath $AssetPacksPath -PathType Container)) {
    if (-not $AllowEmpty) {
        Add-Finding -Findings $findings -Severity "error" -Package "AssetPacks" -Path (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $AssetPacksPath) -Message "AssetPacks folder not found."
    }
    $packages = @()
}
else {
    $AssetPacksPath = (Resolve-Path -LiteralPath $AssetPacksPath).Path
    $packages = @(Get-ChildItem -LiteralPath $AssetPacksPath -Directory -Force | Where-Object { $_.Name -like $PackageNamePattern })
    if ($packages.Count -eq 0 -and -not $AllowEmpty) {
        Add-Finding -Findings $findings -Severity "warning" -Package "AssetPacks" -Path (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $AssetPacksPath) -Message "No sidecar packages matched '$PackageNamePattern'."
    }
}

$packageReports = New-Object System.Collections.Generic.List[object]

foreach ($package in $packages) {
    $packageFindingsStart = $findings.Count
    $packageJsonPath = Join-Path $package.FullName "package.json"
    $packageJson = $null
    $upmName = ""
    $packageVersion = ""

    if (Test-Path -LiteralPath $packageJsonPath -PathType Leaf) {
        $packageJson = Read-JsonOrFinding -Path $packageJsonPath -Findings $findings -Package $package.Name -ProjectRoot $ProjectPath
        $upmNameValue = Get-PropertyValue -Object $packageJson -Name "name"
        if ($null -ne $upmNameValue) {
            $upmName = [string]$upmNameValue
        }
        $versionValue = Get-PropertyValue -Object $packageJson -Name "version"
        if ($null -ne $versionValue) {
            $packageVersion = [string]$versionValue
        }
    }
    else {
        Add-Finding -Findings $findings -Severity "error" -Package $package.Name -Path (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageJsonPath) -Message "Missing package.json."
    }

    $manifestFiles = @()
    $manifestRoot = Join-Path $package.FullName "Documentation~\Manifest"
    if (Test-Path -LiteralPath $manifestRoot -PathType Container) {
        $manifestFiles = @(Get-ChildItem -LiteralPath $manifestRoot -File -Force -Filter "*.json")
    }

    if ($manifestFiles.Count -eq 0) {
        Add-Finding -Findings $findings -Severity "warning" -Package $package.Name -Path (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $package.FullName) -Message "No package-local manifest JSON found under Documentation~/Manifest."
    }

    $manifestReports = New-Object System.Collections.Generic.List[object]
    foreach ($manifestFile in $manifestFiles) {
        $manifest = Read-JsonOrFinding -Path $manifestFile.FullName -Findings $findings -Package $package.Name -ProjectRoot $ProjectPath
        if ($null -eq $manifest) {
            continue
        }

        $manifestPackageName = Get-PropertyValue -Object $manifest -Name "package_name"
        $manifestUpmPackageName = Get-PropertyValue -Object $manifest -Name "upm_package_name"
        $packageNames = @($upmName, [string]$manifestPackageName, [string]$manifestUpmPackageName) |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
            Select-Object -Unique

        $categoryReports = @(
            Test-ManifestCategory -ProjectRoot $ProjectPath -PackageRoot $package -PackageNames $packageNames -Manifest $manifest -ManifestPath $manifestFile.FullName -Category "generated_prefabs" -CountKeys @("generated_prefabs", "planned_prefabs") -Findings $findings
            Test-ManifestCategory -ProjectRoot $ProjectPath -PackageRoot $package -PackageNames $packageNames -Manifest $manifest -ManifestPath $manifestFile.FullName -Category "generated_materials" -CountKeys @("generated_materials", "planned_materials") -Findings $findings
            Test-ManifestCategory -ProjectRoot $ProjectPath -PackageRoot $package -PackageNames $packageNames -Manifest $manifest -ManifestPath $manifestFile.FullName -Category "generated_meshes" -CountKeys @("generated_meshes", "planned_meshes") -Findings $findings
            Test-ManifestCategory -ProjectRoot $ProjectPath -PackageRoot $package -PackageNames $packageNames -Manifest $manifest -ManifestPath $manifestFile.FullName -Category "preview_renders" -CountKeys @("preview_renders", "preview_only") -Findings $findings
        )

        $manifestReports.Add([pscustomobject]@{
            path = Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $manifestFile.FullName
            pack_id = [string](Get-PropertyValue -Object $manifest -Name "pack_id")
            display_name = [string](Get-PropertyValue -Object $manifest -Name "display_name")
            version = [string](Get-PropertyValue -Object $manifest -Name "version")
            build_id = [string](Get-PropertyValue -Object $manifest -Name "build_id")
            import_smoke_status = [string](Get-PropertyValue -Object $manifest -Name "import_smoke_status")
            clean_throwaway_import_status = [string](Get-PropertyValue -Object $manifest -Name "clean_throwaway_import_status")
            primary_quarantine_import_status = [string](Get-PropertyValue -Object $manifest -Name "primary_quarantine_import_status")
            categories = $categoryReports
        }) | Out-Null
    }

    if ($findings.Count -gt $packageFindingsStart) {
        $packageFindings = @($findings.ToArray()[$packageFindingsStart..($findings.Count - 1)] | Where-Object { $null -ne $_ })
    }
    else {
        $packageFindings = @()
    }

    $packageErrors = @($packageFindings | Where-Object { $_.severity -eq "error" }).Count
    $packageWarnings = @($packageFindings | Where-Object { $_.severity -eq "warning" }).Count
    $decision = "ready_for_primary_quarantine"
    if ($packageErrors -gt 0) {
        $decision = "blocked_static_errors"
    }
    elseif ($packageWarnings -gt 0) {
        $decision = "needs_generation_or_remediation"
    }

    $packageReports.Add([pscustomobject]@{
        package_folder = $package.Name
        package_root = Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $package.FullName
        package_json = Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageJsonPath
        upm_name = $upmName
        version = $packageVersion
        manifest_count = $manifestReports.Count
        decision = $decision
        errors = $packageErrors
        warnings = $packageWarnings
        manifests = $manifestReports.ToArray()
    }) | Out-Null
}

$findingItems = $findings.ToArray()
$errorCount = @($findingItems | Where-Object { $_.severity -eq "error" }).Count
$warningCount = @($findingItems | Where-Object { $_.severity -eq "warning" }).Count

$report = [pscustomobject]@{
    generated_at = (Get-Date).ToString("o")
    project_path = $ProjectPath
    asset_packs_path = $AssetPacksPath
    package_pattern = $PackageNamePattern
    package_count = $packages.Count
    errors = $errorCount
    warnings = $warningCount
    packages = $packageReports.ToArray()
    findings = $findingItems
}

New-Item -ItemType Directory -Force -Path (Split-Path -Parent $OutputMarkdown) | Out-Null
New-Item -ItemType Directory -Force -Path (Split-Path -Parent $OutputJson) | Out-Null

$markdown = New-MarkdownReport -Report $report
Set-Content -LiteralPath $OutputMarkdown -Value $markdown -Encoding UTF8
$report | ConvertTo-Json -Depth 16 | Set-Content -LiteralPath $OutputJson -Encoding UTF8

if ($ConsoleJson) {
    $report | ConvertTo-Json -Depth 16
}
else {
    Write-Host "Quarantine readiness report"
    Write-Host "Markdown: $(Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $OutputMarkdown)"
    Write-Host "JSON: $(Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $OutputJson)"
    Write-Host "Packages: $($report.package_count)"
    Write-Host "Errors: $($report.errors)"
    Write-Host "Warnings: $($report.warnings)"
}

exit 0
