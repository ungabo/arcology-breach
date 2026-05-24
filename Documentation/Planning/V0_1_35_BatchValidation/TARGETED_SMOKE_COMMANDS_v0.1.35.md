# Brassworks Breach - v0.1.35 Targeted Smoke Commands

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_BatchValidation/`

## Purpose

Suggest targeted commands for validating the `v0.1.35` gameplay-systems batch before the full V0 matrix. These commands use the existing Unity editor and runtime smoke harness.

## Assumptions

- Project path: `D:\__MY APPS\Unity Doom`
- Unity editor path used by existing scripts: `C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe`
- Candidate log prefix: `v045`
- Expected executable after Windows build: `Builds\Windows\v0.1.35\BrassworksBreach_v0.1.35.exe`

If the main lane chooses a different log prefix, keep the pass markers the same and only rename log files.

## Targeted Editor Gates

Run these first after the gameplay-systems batch compiles as one coherent slice.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
$Project = "D:\__MY APPS\Unity Doom"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.BuildV0 -quit -logFile "$Project\Logs\v045-scene.log"
Select-String -LiteralPath "$Project\Logs\v045-scene.log" -SimpleMatch "V0 scenes rebuilt"

& $Unity -batchmode -projectPath $Project -executeMethod V0LevelValidator.RunValidation -quit -logFile "$Project\Logs\v045-level-validation.log"
Select-String -LiteralPath "$Project\Logs\v045-level-validation.log" -SimpleMatch "V0_LEVEL_VALIDATION_PASS"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile "$Project\Logs\v045-smoke-test.log"
Select-String -LiteralPath "$Project\Logs\v045-smoke-test.log" -SimpleMatch "V0_SMOKE_TEST_PASS"
```

Run route audit before player smokes because this batch touches feedback around route objects, secrets, pickups, hazards, and boss unlock.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
.\Tools\RunV0RouteAudit.ps1 -LogPrefix v045
```

Expected marker:

- `V0_ROUTE_AUDIT_PASS`

## Build Once For Targeted Player Smokes

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
$Project = "D:\__MY APPS\Unity Doom"

& $Unity -batchmode -projectPath $Project -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile "$Project\Logs\v045-windows-build.log"
Select-String -LiteralPath "$Project\Logs\v045-windows-build.log" -SimpleMatch "V0_WINDOWS_BUILD_PASS"
```

## Targeted Player Smoke Helper

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
$Project = "D:\__MY APPS\Unity Doom"
$Exe = "$Project\Builds\Windows\v0.1.35\BrassworksBreach_v0.1.35.exe"

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

Run this focused batch before the full matrix.

```powershell
Invoke-V0PlayerSmoke -Argument "-v0RuntimeSmoke" -LogName "v045-runtime-smoke.log" -Marker "V0_RUNTIME_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0WeaponSwitchSmoke" -LogName "v045-weapon-switch-smoke.log" -Marker "V0_WEAPON_SWITCH_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatSmoke" -LogName "v045-combat-smoke.log" -Marker "V0_COMBAT_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatEdgeSmoke" -LogName "v045-combat-edge-smoke.log" -Marker "V0_COMBAT_EDGE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0CombatScenarioSmoke" -LogName "v045-combat-scenario-smoke.log" -Marker "V0_COMBAT_SCENARIO_PASS"
Invoke-V0PlayerSmoke -Argument "-v0BellowsNodeSmoke" -LogName "v045-bellows-node-smoke.log" -Marker "V0_BELLOWS_NODE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0RangedCombatSmoke" -LogName "v045-ranged-combat-smoke.log" -Marker "V0_RANGED_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0BulwarkCombatSmoke" -LogName "v045-bulwark-combat-smoke.log" -Marker "V0_BULWARK_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0WardenCombatSmoke" -LogName "v045-warden-combat-smoke.log" -Marker "V0_WARDEN_COMBAT_PASS"
Invoke-V0PlayerSmoke -Argument "-v0InteractionSmoke" -LogName "v045-interaction-smoke.log" -Marker "V0_INTERACTION_SMOKE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0HazardSmoke" -LogName "v045-hazard-smoke.log" -Marker "V0_HAZARD_PASS"
Invoke-V0PlayerSmoke -Argument "-v0SecretSmoke" -LogName "v045-secret-smoke.log" -Marker "V0_SECRET_PASS"
Invoke-V0PlayerSmoke -Argument "-v0PauseFlow" -LogName "v045-pause-flow.log" -Marker "V0_PAUSE_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0MovementSmoke" -LogName "v045-movement-smoke.log" -Marker "V0_MOVEMENT_FEEL_PASS"
Invoke-V0PlayerSmoke -Argument "-v0BalanceSmoke" -LogName "v045-balance-smoke.log" -Marker "V0_BALANCE_ENVELOPE_PASS"
Invoke-V0PlayerSmoke -Argument "-v0Level01FlowSmoke" -LogName "v045-level01-flow-smoke.log" -Marker "V0_LEVEL01_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0MidgameFlowSmoke" -LogName "v045-midgame-flow-smoke.log" -Marker "V0_MIDGAME_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0ClimaxFlowSmoke" -LogName "v045-climax-flow-smoke.log" -Marker "V0_CLIMAX_FLOW_PASS"
Invoke-V0PlayerSmoke -Argument "-v0AudioMixSmoke" -LogName "v045-audio-mix-smoke.log" -Marker "V0_AUDIO_MIX_PASS"
Invoke-V0PlayerSmoke -Argument "-v0DisplaySettingsSmoke" -LogName "v045-display-settings-smoke.log" -Marker "V0_DISPLAY_SETTINGS_PASS"
Invoke-V0PlayerSmoke -Argument "-v0ReadabilitySmoke" -LogName "v045-readability-smoke.log" -Marker "V0_READABILITY_SETTINGS_PASS"
```

## Smoke Coverage Map

| Batch concern | Suggested flag | Expected marker | Why it matters |
| --- | --- | --- | --- |
| Global runtime wiring | `-v0RuntimeSmoke` | `V0_RUNTIME_SMOKE_PASS` | Confirms core controllers, HUD/settings/audio/runtime wiring remain present. |
| Weapon pickup/switch | `-v0WeaponSwitchSmoke` | `V0_WEAPON_SWITCH_PASS` | Protects weapon acquisition and switching after new feedback hooks. |
| Weapon/combat feedback | `-v0CombatScenarioSmoke` | `V0_COMBAT_SCENARIO_PASS` | Exercises weapon use, resource behavior, and combat feedback under active scenario conditions. |
| General combat | `-v0CombatSmoke` | `V0_COMBAT_SMOKE_PASS` | Catches broad player/enemy combat regressions. |
| Combat edge readability | `-v0CombatEdgeSmoke` | `V0_COMBAT_EDGE_PASS` | Protects close-range tells, low resources, and damage/death edge cases. |
| Bellows Node state | `-v0BellowsNodeSmoke` | `V0_BELLOWS_NODE_PASS` | Protects support pulse/boost/shutdown feedback. |
| Lancer state | `-v0RangedCombatSmoke` | `V0_RANGED_COMBAT_PASS` | Protects ranged tells and projectile readability. |
| Bulwark state | `-v0BulwarkCombatSmoke` | `V0_BULWARK_COMBAT_PASS` | Protects heavy enemy windup, footprint, and shutdown feedback. |
| Warden state | `-v0WardenCombatSmoke` | `V0_WARDEN_COMBAT_PASS` | Protects boss combat, defeat, guardian lock, and final unlock expectations. |
| Pickups/interactions | `-v0InteractionSmoke` | `V0_INTERACTION_SMOKE_PASS` | Protects pickup, prompt, route interaction, and collection feedback wiring. |
| Hazards | `-v0HazardSmoke` | `V0_HAZARD_PASS` | Ensures feedback/VFX does not hide or corrupt steam/furnace hazard language. |
| Secrets | `-v0SecretSmoke` | `V0_SECRET_PASS` | Protects discovery feedback and existing secret authority. |
| Pause | `-v0PauseFlow` | `V0_PAUSE_FLOW_PASS` | Protects pause/resume input, time scale, cursor, and menu control. |
| Movement feel | `-v0MovementSmoke` | `V0_MOVEMENT_FEEL_PASS` | Catches camera/recoil/settings changes that destabilize movement feel. |
| Balance envelope | `-v0BalanceSmoke` | `V0_BALANCE_ENVELOPE_PASS` | Detects accidental damage/ammo/pickup/balance changes from feedback work. |
| Level01 route | `-v0Level01FlowSmoke` | `V0_LEVEL01_FLOW_PASS` | Protects first-run key, gate, pickup, and lift flow. |
| Midgame route | `-v0MidgameFlowSmoke` | `V0_MIDGAME_FLOW_PASS` | Protects Level02/Level03 valve, pickup, hazard, and lift flow. |
| Climax route | `-v0ClimaxFlowSmoke` | `V0_CLIMAX_FLOW_PASS` | Protects Level04/Level05 route, boss, final hoist, and final exit flow. |
| Audio mix | `-v0AudioMixSmoke` | `V0_AUDIO_MIX_PASS` | Protects new cue integration and mix presence. |
| Display settings | `-v0DisplaySettingsSmoke` | `V0_DISPLAY_SETTINGS_PASS` | Protects display settings after pause/settings polish. |
| Readability settings | `-v0ReadabilitySmoke` | `V0_READABILITY_SETTINGS_PASS` | Protects high-contrast/readability behavior after VFX/UI feedback changes. |

## Candidate Manual Review

After targeted smokes pass, capture a quick first-person review note for:

- Weapon fire/hit/miss/empty feedback on both weapons.
- Pickup confirmation for health, ammo, key, and scattergun.
- Enemy hit/death feedback for Scrapper, Lancer, Bellows Node, Bulwark, and Warden.
- Route-state feedback for every level's core objective.
- Secret discovery feedback.
- Pause/resume and settings changes during gameplay.

Manual review should reject the candidate if feedback is automated-pass but visually confusing.

## Final Full Matrix

After targeted gates pass and any readability tuning is complete, run the existing full matrix. This remains the final automated gate before release readiness.

```powershell
Set-Location "D:\__MY APPS\Unity Doom"
.\Tools\RunV0BuildMatrix.ps1 -LogPrefix v045
```

Expected final marker:

- `V0_BUILD_MATRIX_PASS`

The full matrix should also regenerate package, QA packet, issue-triage packet, and candidate-readiness evidence according to the current main-lane build workflow.

