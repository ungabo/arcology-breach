# Brassworks Breach - v0.1.34 Targeted Smoke Commands

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_34_BatchValidation/`

## Purpose

Suggest targeted commands for validating the combined `v0.1.34` weapon/prop, enemy-readability, and level-density milestone before running the full V0 matrix.

These commands use the existing Unity and runtime smoke harness. They are intentionally targeted: use them after a coherent batch is integrated, not after every individual prop.

## Assumptions

- Project path: `D:\__MY APPS\Unity Doom`
- Unity editor path used by existing scripts: `C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe`
- Expected v0.1.34 log prefix: `v044`
- Expected v0.1.34 executable after Windows build: `Builds\Windows\v0.1.34\BrassworksBreach_v0.1.34.exe`

## Targeted Editor Gates

Run these first after the batch is integrated enough to compile as one slice.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
$Project = "D:\__MY APPS\Unity Doom"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.BuildV0 -quit -logFile "$Project\Logs\v044-scene.log"
Select-String -LiteralPath "$Project\Logs\v044-scene.log" -SimpleMatch "V0 scenes rebuilt"

& $Unity -batchmode -projectPath $Project -executeMethod V0LevelValidator.RunValidation -quit -logFile "$Project\Logs\v044-level-validation.log"
Select-String -LiteralPath "$Project\Logs\v044-level-validation.log" -SimpleMatch "V0_LEVEL_VALIDATION_PASS"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile "$Project\Logs\v044-smoke-test.log"
Select-String -LiteralPath "$Project\Logs\v044-smoke-test.log" -SimpleMatch "V0_SMOKE_TEST_PASS"
```

Run the route audit before targeted player smokes if the batch touches route density, transitions, hazards, boss arena, or objective framing.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
.\Tools\RunV0RouteAudit.ps1 -LogPrefix v044
```

Expected marker:

- `V0_ROUTE_AUDIT_PASS`

## Build Once For Targeted Player Smokes

The existing player smokes need a Windows build. Build once after editor validation is clean.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
$Project = "D:\__MY APPS\Unity Doom"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile "$Project\Logs\v044-windows-build.log"
Select-String -LiteralPath "$Project\Logs\v044-windows-build.log" -SimpleMatch "V0_WINDOWS_BUILD_PASS"
```

## Targeted Player Smoke Helper

Use this helper for the focused player flags below.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Project = "D:\__MY APPS\Unity Doom"
$Exe = "$Project\Builds\Windows\v0.1.34\BrassworksBreach_v0.1.34.exe"

function Invoke-V0PlayerSmoke {
    param(
        [string]$Argument,
        [string]$LogName,
        [string]$Marker
    )

    & $Exe -batchmode -nographics -logFile "$Project\Logs\$LogName" $Argument
    if ($LASTEXITCODE -ne 0) {
        throw "Player smoke failed: $Argument"
    }

    if (-not (Select-String -LiteralPath "$Project\Logs\$LogName" -SimpleMatch $Marker -Quiet)) {
        throw "Expected marker missing: $Marker in $LogName"
    }
}
```

## Recommended Targeted Smoke Batch

Run this focused batch before the full matrix for the combined milestone.

```powershell
Invoke-V0PlayerSmoke -Argument "-v0RuntimeSmoke" -LogName "v044-runtime-smoke.log" -Marker "V0_RUNTIME_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0WeaponSwitchSmoke" -LogName "v044-weapon-switch-smoke.log" -Marker "V0_WEAPON_SWITCH_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatSmoke" -LogName "v044-combat-smoke.log" -Marker "V0_COMBAT_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatEdgeSmoke" -LogName "v044-combat-edge-smoke.log" -Marker "V0_COMBAT_EDGE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatScenarioSmoke" -LogName "v044-combat-scenario-smoke.log" -Marker "V0_COMBAT_SCENARIO_PASS"
Invoke-V0PlayerSmoke -Argument "-v0BellowsNodeSmoke" -LogName "v044-bellows-node-smoke.log" -Marker "V0_BELLOWS_NODE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0RangedCombatSmoke" -LogName "v044-ranged-combat-smoke.log" -Marker "V0_RANGED_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0BulwarkCombatSmoke" -LogName "v044-bulwark-combat-smoke.log" -Marker "V0_BULWARK_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0WardenCombatSmoke" -LogName "v044-warden-combat-smoke.log" -Marker "V0_WARDEN_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0InteractionSmoke" -LogName "v044-interaction-smoke.log" -Marker "V0_INTERACTION_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0HazardSmoke" -LogName "v044-hazard-smoke.log" -Marker "V0_HAZARD_PASS"
Invoke-V0PlayerSmoke -Argument "-v0Level01FlowSmoke" -LogName "v044-level01-flow-smoke.log" -Marker "V0_LEVEL01_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0MidgameFlowSmoke" -LogName "v044-midgame-flow-smoke.log" -Marker "V0_MIDGAME_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0ClimaxFlowSmoke" -LogName "v044-climax-flow-smoke.log" -Marker "V0_CLIMAX_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0AudioMixSmoke" -LogName "v044-audio-mix-smoke.log" -Marker "V0_AUDIO_MIX_PASS"
Invoke-V0PlayerSmoke -Argument "-v0ReadabilitySmoke" -LogName "v044-readability-smoke.log" -Marker "V0_READABILITY_SETTINGS_PASS"
```

## Smoke Coverage Map

| Batch concern | Suggested flag | Expected marker | Why it matters |
| --- | --- | --- | --- |
| Global runtime wiring | `-v0RuntimeSmoke` | `V0_RUNTIME_SMOKE_PASS` | Confirms core controllers, settings, audio bindings, HUD wiring, and runtime test components exist. |
| Weapon/prop readability | `-v0WeaponSwitchSmoke` | `V0_WEAPON_SWITCH_PASS` | Protects the Steam Scattergun pickup/switch route and weapon feedback after prop/display changes. |
| Weapon and combat feedback | `-v0CombatScenarioSmoke` | `V0_COMBAT_SCENARIO_PASS` | Protects pressure burst, combat resource behavior, and weapon feedback under active combat. |
| General enemy combat | `-v0CombatSmoke` | `V0_COMBAT_SMOKE_PASS` | Catches broad enemy/player combat regressions. |
| Attack tell readability | `-v0CombatEdgeSmoke` | `V0_COMBAT_EDGE_PASS` | Protects pre-damage enemy tell timing and close combat readability. |
| Bellows support readability | `-v0BellowsNodeSmoke` | `V0_BELLOWS_NODE_PASS` | Protects support-machine pulse and boost readability. |
| Lancer readability | `-v0RangedCombatSmoke` | `V0_RANGED_COMBAT_PASS` | Protects ranged tell/damage readability in pipe-heavy spaces. |
| Bulwark readability | `-v0BulwarkCombatSmoke` | `V0_BULWARK_COMBAT_PASS` | Protects heavy windup, movement footprint, and shutdown feedback. |
| Warden readability | `-v0WardenCombatSmoke` | `V0_WARDEN_COMBAT_PASS` | Protects boss HUD, boss attacks, Warden defeat, and final unlock expectations. |
| Prompt/pickup interaction | `-v0InteractionSmoke` | `V0_INTERACTION_SMOKE_PASS` | Protects plaques, prompts, and interaction feedback near dense prop work. |
| Hazard readability | `-v0HazardSmoke` | `V0_HAZARD_PASS` | Protects steam/furnace readability and damage expectations after density changes. |
| Level01 route | `-v0Level01FlowSmoke` | `V0_LEVEL01_FLOW_PASS` | Protects first-run key, gate, and lift flow. |
| Midgame route | `-v0MidgameFlowSmoke` | `V0_MIDGAME_FLOW_PASS` | Protects Level02/Level03 route and midgame pressure progression. |
| Climax route | `-v0ClimaxFlowSmoke` | `V0_CLIMAX_FLOW_PASS` | Protects Level04/Level05 route, boss, and final exit flow. |
| Audio readability | `-v0AudioMixSmoke` | `V0_AUDIO_MIX_PASS` | Protects new cue integration and mix presence. |
| Accessibility/readability settings | `-v0ReadabilitySmoke` | `V0_READABILITY_SETTINGS_PASS` | Protects high-contrast/readability settings after visual density and UI-adjacent changes. |

## Final Full Matrix

After targeted gates pass and any readability tuning is complete, run the existing full matrix. This remains the final automated gate before release readiness.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
.\Tools\RunV0BuildMatrix.ps1 -LogPrefix v044
```

Expected final marker:

- `V0_BUILD_MATRIX_PASS`

The full matrix also regenerates package, QA packet, issue-triage packet, and candidate-readiness evidence unless skipped by the main lane.
