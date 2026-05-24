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
        throw "Windows QA packet failed: missing $Label at $Path"
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

function Convert-ToInlineCode {
    param([string]$Value)

    $tick = [char]96
    return "$tick$Value$tick"
}

$ProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$branding = Get-Branding -RootPath $ProjectPath
$version = $branding.Version
$executableStem = $branding.ExecutableStem

$exePath = Join-Path $ProjectPath "Builds\Windows\$version\${executableStem}_$version.exe"
$routeAuditPath = Join-Path $ProjectPath "Documentation\QA\RouteAudit\ROUTE_AUDIT_$version.md"
$packageManifestPath = Join-Path $ProjectPath "Builds\WindowsPackages\$version\${executableStem}_${version}_WindowsPackageManifest.json"
$manualRoot = Join-Path $ProjectPath "Documentation\QA\ManualPlaytestV1"
$qaRoot = Join-Path $ProjectPath "Documentation\QA\WindowsRouteQA"

Require-Path -Path $exePath -Label "Windows executable"
Require-Path -Path $routeAuditPath -Label "route audit report"
Require-Path -Path $manualRoot -Label "manual playtest folder"

$packageZip = $null
$packageHash = $null
if (Test-Path -LiteralPath $packageManifestPath) {
    $packageManifest = Get-Content -LiteralPath $packageManifestPath -Raw | ConvertFrom-Json
    $packageZip = $packageManifest.zip_path
    $packageHash = $packageManifest.sha256
}

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
    throw "Windows QA packet failed: missing manual route sheet(s): $($missingSheets -join ', ')"
}

New-Item -ItemType Directory -Force -Path $qaRoot | Out-Null
$packetPath = Join-Path $qaRoot "QA_PACKET_$version.md"
$manifestPath = Join-Path $qaRoot "QA_PACKET_$version.json"
$manualIndexPath = Join-Path $manualRoot "README.md"

$routeTable = Get-RouteTable -RouteAuditPath $routeAuditPath
$generatedLocal = Get-Date -Format "yyyy-MM-dd HH:mm zzz"
$generatedUtc = (Get-Date).ToUniversalTime().ToString("o")
$generatedLocalCode = Convert-ToInlineCode -Value $generatedLocal
$exeRepoCode = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $exePath)
$routeAuditRepoCode = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $routeAuditPath)
$packageRepoValue = if ($packageZip) { Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packageZip } else { "package manifest not found" }
$packageHashValue = if ($packageHash) { $packageHash } else { "unavailable" }
$packageRepoCode = Convert-ToInlineCode -Value $packageRepoValue
$packageHashCode = Convert-ToInlineCode -Value $packageHashValue
$exePathCode = Convert-ToInlineCode -Value $exePath
$packetRepoCode = Convert-ToInlineCode -Value (Convert-ToRepoPath -RootPath $ProjectPath -AbsolutePath $packetPath)

$packetLines = @(
    "# Brassworks Breach - Windows Route QA Packet $version",
    "",
    "Generated: $generatedLocalCode",
    "",
    "## Verified Build",
    "",
    "- Executable: $exeRepoCode",
    "- Route audit: $routeAuditRepoCode",
    "- Package: $packageRepoCode",
    "- Package SHA-256: $packageHashCode",
    "",
    "## Route Matrix",
    ""
) + $routeTable + @(
    "",
    "## Manual Test Order",
    "",
    '1. `01_FIRST_RUN_ROUTE.md` - complete the five-level route from a fresh launch.',
    '2. `02_COMBAT_FEEL_ROUTE.md` - focus on Pressure Pistol, Steam Scattergun, machine tells, and death feedback.',
    '3. `03_SECRET_HUNT_ROUTE.md` - confirm Level01, Level02, and Level04 secrets remain discoverable.',
    '4. `04_HAZARD_ROUTE.md` - check steam/furnace readability and damage timing.',
    '5. `05_BOSS_ROUTE.md` - test the Governor Warden lock, fight, boss HUD, and final hoist.',
    '6. `06_ACCESSIBILITY_READABILITY_ROUTE.md` - check settings, contrast, prompts, objective text, and HUD readability.',
    "",
    "## Pass Criteria",
    "",
    "- The tester can explain the objective in each level without consulting automation logs.",
    "- Route-critical pickups, valves, lifts, gates, boss lock, hazards, and final exit are readable from normal play distance.",
    "- Combat failures feel attributable to player action or clear enemy tells.",
    "- The tester can pause, restart, and quit without confusion.",
    '- Any block over `2 minutes` is logged with location, objective text, and what visual/audio cue was missing.',
    "",
    "## Evidence To Record",
    "",
    "- Build path and package hash.",
    "- Route sheet name.",
    "- Settings used: resolution, fullscreen/windowed, sensitivity, volume, flash intensity, high contrast.",
    "- Timing notes per level.",
    "- Screenshots or short descriptions for confusing routes, unreadable enemies, unclear hazards, or audio balance issues.",
    "",
    "Next-step directive: continue immediately with the next highest-impact unfinished task."
)

Set-Content -LiteralPath $packetPath -Value $packetLines -Encoding UTF8

$manualIndexLines = @(
    "# Manual Playtest V1 Route Sheets",
    "",
    "Scope: $(Convert-ToInlineCode -Value $version) Windows build, current V1 manual-playtest path.",
    "",
    "Build to launch:",
    "",
    $exePathCode,
    "",
    "Current generated QA packet:",
    "",
    $packetRepoCode,
    "",
    "Primary goal: give a human tester enough route, control, pass/fail, timing, and note structure to evaluate the current five-level run without asking for automation or Codex help.",
    "",
    "## Windows Controls",
    "",
    "| Action | Control |",
    "| --- | --- |",
    "| Look / aim | Mouse |",
    '| Move | `W`, `A`, `S`, `D` |',
    "| Fire current weapon | Left mouse |",
    "| Alternate fire | Right mouse |",
    '| Interact with gates, lifts, valves, plaques | `E` when the prompt appears |',
    '| Equip Pressure Pistol | `1` |',
    '| Equip Steam Scattergun | `2` after the Level03 pickup |',
    '| Pause / resume menu | `Esc` |',
    '| Restart after death or win | `R` when the end-state prompt appears |',
    "",
    "No jump or crouch is expected or required in this build.",
    "",
    "## Current Route",
    "",
    '1. `Level01 - Brassworks Intake`: find the gear key, open the pressure gate, use the service lift.',
    '2. `Level02 - Pipeworks Annex`: confirm the Boilerheart lift is locked, route pipe pressure at the valve, use the lift to Level03.',
    '3. `Level03 - Boilerheart Core`: collect the Steam Scattergun, survive steam and the Bellows Node read, vent the Boilerheart pressure valve, use the foundry lift.',
    '4. `Level04 - Furnace Foundry`: read steam and furnace heat hazards, fight mixed machines including the first Bulwark, use the emergency hoist.',
    '5. `Level05 - Governor Core`: confirm the master override hoist is locked, defeat the Governor Warden, use the hoist to trigger win.',
    "",
    "Current registered secrets:",
    "",
    '- `Level01 - Secret - Intake Pressure Cache`',
    '- `Level02 - Secret - Pipeworks Cartridge Cache`',
    '- `Level04 - Secret - Foundry Coal Cache`',
    "",
    "## Route Sheets",
    "",
    "- [First-Run Route](01_FIRST_RUN_ROUTE.md)",
    "- [Combat Feel Route](02_COMBAT_FEEL_ROUTE.md)",
    "- [Secret-Hunt Route](03_SECRET_HUNT_ROUTE.md)",
    "- [Hazard Route](04_HAZARD_ROUTE.md)",
    "- [Boss Route](05_BOSS_ROUTE.md)",
    "- [Accessibility And Readability Route](06_ACCESSIBILITY_READABILITY_ROUTE.md)",
    "- [Test-Session Summary Template](SESSION_SUMMARY_TEMPLATE.md)",
    "- [Issue-Log Template](ISSUE_LOG_TEMPLATE.md)",
    "",
    "## Common Test Rules",
    "",
    "- Start from a fresh launch unless a route sheet says otherwise.",
    "- Record the build path and route sheet name at the top of every session note.",
    '- If a tester is blocked for more than `2 minutes`, log where and why, then continue if a route forward is found.',
    "- Do not turn a manual route sheet into a hard progression blocker. The goal is observation, not perfect execution.",
    "- A pass means the tester can complete the intended task and can explain why it worked.",
    "- A fail means the tester is blocked, confused for longer than the route target, misreads a threat/objective, or completes only by luck.",
    "",
    "## Expected Current Limitations",
    "",
    "- Automated smoke confirms the objective chain and regressions, but not human route readability or combat feel.",
    "- Visuals are still mostly procedural prototype art unless an asset document marks a component as promoted or final.",
    "- Enemy navigation uses simple side-steering, not a full NavMesh solution.",
    "- Balance values have automated coverage but still need human feel tuning.",
    "- AudioV1 is wired and mix-smoke-tested but still needs a human listen pass.",
    "- Settings include sensitivity, master volume, flash intensity, resolution, fullscreen, and high-contrast readability.",
    "- Windows is the only current playable target. Android, WebGL, PC VR, and Meta Quest are planned but deferred.",
    "- Health and ammo persist across level transitions, but future weapon inventory and campaign flags still need expansion.",
    "- There is no save system; boss and late-route retests require replaying the route unless a developer provides a special build later."
)

Set-Content -LiteralPath $manualIndexPath -Value $manualIndexLines -Encoding UTF8

$manifest = [ordered]@{
    version = $version
    generated_utc = $generatedUtc
    executable = $exePath
    route_audit = $routeAuditPath
    qa_packet = $packetPath
    manual_index = $manualIndexPath
    package_zip = $packageZip
    package_sha256 = $packageHash
    manual_sheets = $requiredSheets
}

$manifest | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $manifestPath -Encoding UTF8

Write-Host "V0_WINDOWS_QA_PACKET_PASS $version $packetPath"
