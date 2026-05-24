param(
    [string]$ProjectPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
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

function Require-Path {
    param(
        [string]$Path,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Windows issue triage packet failed: missing $Label at $Path"
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

function Get-RouteTable {
    param([string]$RouteAuditPath)

    $lines = Get-Content -LiteralPath $RouteAuditPath
    $start = [Array]::IndexOf($lines, "## Scene Route Matrix")
    $end = [Array]::IndexOf($lines, "## Findings")

    if ($start -lt 0 -or $end -le $start) {
        return @("- Route table unavailable; see route audit report.")
    }

    return $lines[($start + 2)..($end - 2)]
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$branding = Get-Branding -RootPath $ProjectPath
$version = $branding.Version
$executableStem = $branding.ExecutableStem

$exePath = Join-Path $ProjectPath "Builds\Windows\$version\${executableStem}_$version.exe"
$routeAuditPath = Join-Path $ProjectPath "Documentation\QA\RouteAudit\ROUTE_AUDIT_$version.md"
$qaPacketPath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\QA_PACKET_$version.md"
$qaPacketManifestPath = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA\QA_PACKET_$version.json"
$packageManifestPath = Join-Path $ProjectPath "Builds\WindowsPackages\$version\${executableStem}_${version}_WindowsPackageManifest.json"
$manualRoot = Join-Path $ProjectPath "Documentation\QA\ManualPlaytestV1"
$qaRoot = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA"
$triagePath = Join-Path $qaRoot "ISSUE_TRIAGE_$version.md"
$triageManifestPath = Join-Path $qaRoot "ISSUE_TRIAGE_$version.json"
$manualIndexPath = Join-Path $manualRoot "README.md"

Require-Path -Path $exePath -Label "Windows executable"
Require-Path -Path $routeAuditPath -Label "route audit report"
Require-Path -Path $qaPacketPath -Label "Windows QA packet"
Require-Path -Path $qaPacketManifestPath -Label "Windows QA packet manifest"
Require-Path -Path $packageManifestPath -Label "Windows package manifest"
Require-Path -Path $manualRoot -Label "manual playtest folder"

$requiredSheets = @(
    "01_FIRST_RUN_ROUTE.md",
    "02_COMBAT_FEEL_ROUTE.md",
    "03_SECRET_HUNT_ROUTE.md",
    "04_HAZARD_ROUTE.md",
    "05_BOSS_ROUTE.md",
    "06_ACCESSIBILITY_READABILITY_ROUTE.md",
    "SESSION_SUMMARY_TEMPLATE.md",
    "ISSUE_LOG_TEMPLATE.md"
)

$missingSheets = @()
foreach ($sheet in $requiredSheets) {
    $sheetPath = Join-Path $manualRoot $sheet
    if (-not (Test-Path -LiteralPath $sheetPath)) {
        $missingSheets += $sheet
    }
}

if ($missingSheets.Count -gt 0) {
    throw "Windows issue triage packet failed: missing manual route sheet(s): $($missingSheets -join ', ')"
}

$packageManifest = Get-Content -LiteralPath $packageManifestPath -Raw | ConvertFrom-Json
$packageZip = [string]$packageManifest.zip_path
$packageHash = [string]$packageManifest.sha256
Require-Path -Path $packageZip -Label "Windows package ZIP"

$generatedLocal = Get-Date -Format "yyyy-MM-dd HH:mm zzz"
$generatedUtc = (Get-Date).ToUniversalTime().ToString("o")
$routeTable = Get-RouteTable -RouteAuditPath $routeAuditPath

New-Item -ItemType Directory -Force -Path $qaRoot | Out-Null

$exeRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $exePath)
$routeAuditRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $routeAuditPath)
$qaPacketRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $qaPacketPath)
$packageRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageZip)
$packageHashCode = Convert-ToInlineCode -Value $packageHash
$triageRepo = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $triagePath)
$generatedCode = Convert-ToInlineCode -Value $generatedLocal

$seedRows = @(
    "| ISSUE-WIN-ROUTE | P0/P1 | First-run route blockage, unclear objective, missing lock/key/valve/lift affordance | Route sheet, level, room/object, time blocked, expected cue, observed cue |",
    "| ISSUE-WIN-COMBAT | P1/P2 | Enemy tell, weapon feedback, ammo pressure, death feedback, damage fairness | Enemy/weapon, encounter, health/ammo before/after, perceived cause of failure |",
    "| ISSUE-WIN-SECRET | P2/P3 | Secret clue, reward clarity, discovery feedback, secret-stat confusion | Secret name, entry clue, discovery route, reward expectation |",
    "| ISSUE-WIN-HAZARD | P1/P2 | Steam/furnace readability, damage timing, safe-lane clarity | Hazard type, warning cue, damage timing, safe route |",
    "| ISSUE-WIN-BOSS | P1/P2 | Warden lock, boss HUD, arena readability, final hoist clarity | Fight phase, HUD state, lock message, exit state |",
    "| ISSUE-WIN-ACCESS | P1/P2 | Settings, high contrast, flash intensity, prompt readability, text overflow | Resolution, fullscreen state, contrast state, affected UI text |",
    "| ISSUE-WIN-AUDIO | P2/P3 | Mix level, missing cue, confusing cue identity, ambience fatigue | Cue name or scene, volume setting, competing sounds |"
)

$triageLines = @(
    "# Brassworks Breach - Windows Issue Triage Packet $version",
    "",
    "Generated: $generatedCode",
    "",
    "## Source Artifacts",
    "",
    "- Executable: $exeRepo",
    "- Package: $packageRepo",
    "- Package SHA-256: $packageHashCode",
    "- Route audit: $routeAuditRepo",
    "- QA packet: $qaPacketRepo",
    "",
    "## Route Matrix Snapshot",
    ""
) + $routeTable + @(
    "",
    "## Severity Rules",
    "",
    "| Severity | Meaning | Required Action |",
    "| --- | --- | --- |",
    "| P0 | A tester cannot finish the intended route, cannot quit/restart, or hits a crash/hard hang. | Fix before a candidate can be called release-ready. |",
    "| P1 | A tester can continue, but route, combat, hazard, boss, or accessibility clarity is materially confusing. | Fix or explicitly defer before v1.0. |",
    "| P2 | The issue weakens polish, readability, or feel but does not block the route. | Batch into the next polish slice. |",
    "| P3 | Cosmetic, copy, tuning preference, or future-platform note. | Track only if it supports the Windows v1 goal or deferred platform plans. |",
    "",
    "## Issue Buckets",
    "",
    "| Bucket | Default Severity | Capture When | Evidence To Record |",
    "| --- | --- | --- | --- |"
) + $seedRows + @(
    "",
    "## Intake Template",
    "",
    "Use one block per issue copied from a route sheet or playtest note.",
    "",
    '```text',
    "Issue ID: ISSUE-WIN-____-###",
    "Severity: P0/P1/P2/P3",
    "Bucket: route/combat/secret/hazard/boss/access/audio/performance/art",
    "Build: $version",
    "Route Sheet:",
    "Level / Room:",
    "Repro Steps:",
    "Expected:",
    "Actual:",
    "Tester Impact:",
    "Evidence:",
    "Suggested Fix Slice:",
    "Status: new/accepted/deferred/fixed/rejected",
    '```',
    "",
    "## Triage Flow",
    "",
    "1. Copy raw tester notes from the manual sheet into the intake template.",
    "2. Assign severity using the rules above, not personal taste.",
    "3. Assign one issue bucket so fixes can batch cleanly.",
    "4. If the issue affects route completion, candidate packaging, controls, quit/restart, or crash behavior, keep it P0 until verified fixed.",
    "5. Convert accepted issues into `WORK_LEDGER.md` tasks or a later GitHub issue once the cluster is stable.",
    "6. Re-run the full matrix after a fix changes gameplay, scene generation, settings, or distribution artifacts.",
    "",
    "## Candidate Gate",
    "",
    '- P0 count must be `0` for a release-ready Windows candidate.',
    "- P1 issues need either a fix, an explicit defer note, or a documented reason they do not block v1.0.",
    "- P2/P3 issues may batch into polish, art, audio, or platform-port follow-up lanes.",
    "- Any new issue that contradicts automated route evidence should trigger a route-audit or smoke-test update.",
    "",
    "Next-step directive: continue immediately with the next highest-impact unfinished task."
)

Set-Content -LiteralPath $triagePath -Value $triageLines -Encoding UTF8

$manualIndex = Get-Content -LiteralPath $manualIndexPath -Raw
$triageLine = "Current generated issue triage packet:"
if ($manualIndex -notmatch [regex]::Escape($triageLine)) {
    $manualIndex = $manualIndex -replace "Primary goal:", "$triageLine`r`n`r`n$triageRepo`r`n`r`nPrimary goal:"
    $manualIndex = $manualIndex.TrimEnd("`r", "`n")
    Set-Content -LiteralPath $manualIndexPath -Value $manualIndex -Encoding UTF8
}

$manifest = [ordered]@{
    version = $version
    generated_utc = $generatedUtc
    executable = $exePath
    package_zip = $packageZip
    package_sha256 = $packageHash
    route_audit = $routeAuditPath
    qa_packet = $qaPacketPath
    issue_triage_packet = $triagePath
    manual_index = $manualIndexPath
    issue_buckets = @("route", "combat", "secret", "hazard", "boss", "access", "audio")
    required_manual_sheets = $requiredSheets
}

$manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $triageManifestPath -Encoding UTF8

Write-Host "V0_WINDOWS_ISSUE_TRIAGE_PASS $version $triagePath"
