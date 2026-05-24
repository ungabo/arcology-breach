# v0.1.53 Route Polish Implementation Packet

Created: 2026-05-24 19:06:19 -04:00

Scope: implementation-ready route-polish plan for one ambitious playable batch across the accepted v0.1.51/v0.1.52 route expansion work:

- Level02 pressure bypass: `ROUTE_L02_PressureBypass_v0_1_50`
- Level03 foundry gantry: `ROUTE_L03_FoundryGantry_v0_1_50`
- Level04 observatory pumpworks: `ROUTE_L04_ObservatoryPumpworks_v0_1_50`

This packet is intentionally docs-only. It does not edit gameplay scripts, generated scenes, package manifests, shared status docs, build docs, or version docs.

## Batch Intent

Implement the v0.1.53 route-polish work as one coherent playable leap, not as individual micro-slices. The main lane should update `V0SceneBuilder`, strengthen `V0LevelValidator`, extend targeted runtime smoke checks, regenerate Level02-Level04, run route audit/build matrix, then package only if the whole cross-level route polish is passing.

The batch should improve:

- Route legibility from entrance to midpoint to rejoin.
- Hazard teach/execute/combine pacing.
- Encounter pressure on narrow versus wide spaces.
- Secret readability as route mastery, not random wall searching.
- Validator and runtime smoke evidence for the new route-polish objects.

## Source Evidence Read

- `Documentation/Planning/V0_1_52_RouteExpansionTuning/route_expansion_tuning_packet.json`
- `Documentation/Planning/V0_1_52_RouteExpansionTuning/README.md`
- `Documentation/QA/V0_1_52_RouteExpansionTuning/QA_Checklist.md`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Editor/V0RouteAudit.cs`
- `Assets/_Project/Scripts/Utility/RuntimeMidgameFlowTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeClimaxFlowTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeHazardTest.cs`
- `Tools/RunV0BuildMatrix.ps1`

## P0 Group A - Route Labels and Rejoin Readability

Goal: make all three expansion routes read as intentional spaces before the player commits.

### Level02 Pressure Bypass

Add route polish under the existing L02 route root:

- Keep existing required label `Label - L02 Pressure Bypass`.
- Add `Label - L02 Manual Bleed` near `GEO_L02_PressureBypass_ValveRoomA`.
- Add `Label - L02 Mainline Rejoin` near `GEO_L02_PressureBypass_ExitSpine`.
- Add floor chevrons or brass sightline strips from entry to `AUTH_L02_Valve_BypassA`, then from `AUTH_L02_Valve_BypassB` to `AUTH_L02_Door_PumpRoomExit`.
- Add a green/amber rejoin lamp and gauge near the exit door so the return to the main route is readable from the pump room.

Validation target: `V0LevelValidator.ValidateV0151RouteExpansion` or a new `ValidateV0153RoutePolish` helper requires the new labels, chevrons, and rejoin signal. Runtime target: `RuntimeMidgameFlowTest` confirms entry label, manual-bleed label, and rejoin marker are ordered along the bypass route.

### Level03 Foundry Gantry

Add functional vertical-route labels:

- `Label - L03 Foundry Floor` before the furnace pit.
- `Label - L03 Upper Gantry` before `GEO_L03_FoundryGantry_EastServiceRamp`.
- `Label - L03 Control Walkway` before `GEO_L03_FoundryGantry_ControlMezzanine`.
- `Label - L03 Crane Return` at `GEO_L03_FoundryGantry_RejoinStair`.
- `Label - L03 High Rejoin` at `GEO_L03_FoundryGantry_HighRejoinBalcony`.

Add sightline rails and destination lamps so height changes communicate function before the player climbs. Keep pickups away from railing edges so pickups do not masquerade as route markers.

Validation target: required named labels and a spatial test that `Upper Gantry` precedes the raised catwalk and `High Rejoin` sits after the catwalk branch.

### Level04 Observatory Pumpworks

Make the route read as a connected pump chain:

- Keep existing required label `Label - L04 Observatory Pumpworks`.
- Add `Label - L04 Intake Control` near `GEO_L04_Pumpworks_EntryConduit`.
- Add `Label - L04 Pump Primer` near `AUTH_L04_Item_PressureKey` and `AUTH_L04_Door_KeyedMaintenance_A`.
- Add `Label - L04 Pressure Return` near `AUTH_L04_PumpReroute_A`.
- Add `Label - L04 Observatory Feed` near `GEO_L04_Pumpworks_ObservatoryOverlook`.
- Add `Label - L04 Pumpworks Rejoin` near `GEO_L04_Pumpworks_RejoinLockroom`.

Validation target: `RuntimeClimaxFlowTest` should confirm pump labels appear in route order before the hoist transition.

## P0 Group B - Hazard Readability and Safe Pockets

Goal: hazards should teach first, execute second, and combine only after the player has seen the tell.

### Level02 Pressure Bypass

Existing hazards:

- `AUTH_L02_Hazard_SteamJet_C1_A`
- `AUTH_L02_Hazard_PumpVent_R3_A`

Implementation notes:

- Treat `SteamJet_C1_A` as the teach vent. Add a visible warning gauge, steam shadow plate, and safe pocket just before the vent.
- Treat `PumpVent_R3_A` as the execute vent. Give it a clear idle/safe visual and prevent blind damage from the corner into `GEO_L02_PressureBypass_PumpRoom`.
- Add only one combined final read if needed, named `AUTH_L02_Hazard_ReturnVent_R4_A`, and place it after a visible safe pocket.

### Level03 Foundry Gantry

Existing hazards:

- `AUTH_L03_Hazard_FurnaceStrip_West`
- `AUTH_L03_Hazard_FurnaceStrip_East`
- `AUTH_L03_Hazard_SlagVent_R3_A`

Implementation notes:

- Add furnace timing preview strips before the pit, not inside the first combat view.
- Keep hot surfaces red/orange and route guidance green/amber so the route and hazard language do not collapse into one warm color field.
- Add a safe preview platform before the slag vent and a breath pocket after the Bellows Node platform.

### Level04 Observatory Pumpworks

Existing hazards:

- `AUTH_L04_Hazard_PressureJet_R4_North`
- `AUTH_L04_Hazard_PressureJet_R4_South`
- `AUTH_L04_Hazard_GearSweep_R4_Deck`
- `AUTH_L04_Hazard_Overpressure_R3`

Implementation notes:

- Add a pump-state reveal marker before `AUTH_L04_Hazard_Overpressure_R3`.
- Add safe rails and warning plates around the vertical pump arena before the Bulwark pressure starts.
- Do not trigger the arena fight until the player can see the pump-state change and route-open cue.

Validation target: `RuntimeHazardTest` continues to pass, plus named warning/safe-pocket props are required by level validation.

## P0 Group C - Encounter and Pacing Tuning

Goal: route polish should make combat feel intentional rather than stacked on top of navigation confusion.

### Level02 Pressure Bypass

Current route-expansion enemies:

- `Enemy - L02 Pressure Bypass Scout A`
- `Enemy - L02 Pressure Bypass Ranged A`
- `Enemy - L02 Pressure Bypass Exit Guard`

Implementation notes:

- Keep Scout A before the first vent as a low-pressure read.
- Move or angle Ranged A so the player has cover after the first hazard tell.
- Keep Exit Guard near rejoin, but give the player a visible rejoin marker before the fight.

### Level03 Foundry Gantry

Current route-expansion enemies:

- `Enemy - L03 Gantry Ranged Teacher`
- `Enemy - L03 Gantry Detour Scrapper`
- `Enemy - L03 Gantry Bellows Node`

Implementation notes:

- Note: `Enemy - L03 Gantry Ranged Teacher` is currently created with `CreateEnemy`, so it behaves like a Scrapper unless main lane changes it.
- Keep heavy pressure on wider platforms, not narrow catwalks.
- Add cover and one post-encounter breath pocket before `GEO_L03_FoundryGantry_RejoinStair`.
- If ranged teaching is required, promote the enemy to a real Lancer in the main code lane and add validation/smoke for that change.

### Level04 Observatory Pumpworks

Current route-expansion enemies:

- `Enemy - L04 Pumpworks Scout`
- `Enemy - L04 Pumpworks Lancer`
- `Enemy - L04 Pumpworks Arena Bulwark`

Implementation notes:

- Scout can remain early as an orientation check.
- Lancer should sit after the pump-primer read, not directly on top of the first changed-state cue.
- Bulwark remains the arena pressure, but the player should see `Observatory Feed` and the arena warning before entering its combat radius.

Validation target: `RuntimeMidgameFlowTest`, `RuntimeClimaxFlowTest`, `RuntimeRangedCombatTest`, and `RuntimeBulwarkCombatTest` continue passing, with new flow checks for route-order markers.

## P1 Group D - Secrets as Route Mastery

Goal: polish existing route-expansion secrets, do not add more secret count unless the main lane deliberately wants the campaign total to change.

### Level02

Polish existing route-expansion secret:

- `TRG_L02_Secret_BoilerNiche`
- `Pickup - L02 Secret Ammo Cache`
- `Pickup - L02 Secret Boiler Health`

Add a readable pressure-system clue after two hazard tells, for example `L02 Pressure Bypass Secret Bleed Wheel Clue`, then keep the return short.

### Level03

Polish existing route-expansion secret:

- `TRG_L03_Secret_CrucibleShelf`
- `Pickup - L03 Secret Crucible Cache`

Signal it from the upper gantry after the player understands the height language. Avoid placing the clue in the same view as a mandatory route label.

### Level04

Polish existing route-expansion secret:

- `TRG_L04_Secret_ReturnDuct`
- `Pickup - L04 Secret Return Ammo`

Signal it only after `Pressure Return` or `Observatory Feed` so it feels pump-state related. Return before the next mandatory encounter or before the rejoin lockroom.

Validation target: `V0_SECRET_PASS` continues to pass. Optional routes do not become required for campaign completion.

## P1 Group E - Screenshot and Human Review Hooks

Add a manual QA request for screenshots at:

- Level02: entry label, first safe pocket, secret clue, rejoin marker.
- Level03: furnace floor label, upper gantry label, Bellows platform breath pocket, high rejoin.
- Level04: intake control, pump primer, pressure return, observatory feed, secret return, rejoin.

These screenshots are human review evidence only; do not add screenshot artifacts to the build.

## Main-Lane Implementation Order

1. Add route-polish generation helpers in `V0SceneBuilder` and call them from the existing Level02-Level04 expansion builders.
2. Add validator coverage using a new helper such as `ValidateV0153RoutePolish(sceneName)` rather than overloading the v0.1.51 route-expansion presence check.
3. Extend `RuntimeMidgameFlowTest` for Level02 and Level03 route-order assertions.
4. Extend `RuntimeClimaxFlowTest` for Level04 pump-chain route-order assertions.
5. Keep `RuntimeAutoPlaythroughTest` focused on objective progression and transition safety unless a real pump objective is added.
6. Regenerate Level02-Level04 scenes through the existing scene builder.
7. Run level validation, route audit, midgame/climax smoke, hazard/secret smoke, then the full Windows matrix.

## Uncertainties

- Level04 observatory pumpworks currently has route-authority valves, doors, hazards, and labels, but this packet does not confirm a fully wired pump-objective gameplay state. Treat the v0.1.53 batch as visual/readability/pacing polish unless the main lane explicitly promotes `AUTH_L04_PumpReroute_A` or `AUTH_L04_OverlookSwitch_B` into real interactable gameplay.
- Level03 `Enemy - L03 Gantry Ranged Teacher` is named as ranged teaching but is created with the generic `CreateEnemy` path. Main lane should either rename/treat it as a Scrapper pressure unit or promote it to a Lancer deliberately.
- This packet assumes no new generated-scene roots are needed. New objects should live under existing route roots and be validated by stable object names.
