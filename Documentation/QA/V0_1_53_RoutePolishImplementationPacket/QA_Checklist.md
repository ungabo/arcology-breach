# v0.1.53 Route Polish Implementation QA Checklist

Created: 2026-05-24 19:06:19 -04:00

Use this checklist when the main lane implements the route-polish packet. This checklist describes validation and smoke expectations only; this docs lane did not edit code or scenes.

## Entry Gate

- [ ] Only the main implementation lane edits gameplay scripts, generated scenes, validation code, runtime smoke tests, build docs, or shared status docs.
- [ ] v0.1.53 route polish is implemented as one coherent playable batch across Level02-Level04.
- [ ] New route-polish objects live under existing route roots:
  - `ROUTE_L02_PressureBypass_v0_1_50`
  - `ROUTE_L03_FoundryGantry_v0_1_50`
  - `ROUTE_L04_ObservatoryPumpworks_v0_1_50`
- [ ] Existing v0.1.51 route-expansion required names remain present and continue to pass validation.
- [ ] New P0 objects are named deterministically so runtime smoke can find them with `GameObject.Find`.

## Required Validator Additions

- [ ] Add a new validator helper, recommended name: `ValidateV0153RoutePolish(sceneName)`.
- [ ] Call the helper for `Level02`, `Level03`, and `Level04` after existing v0.1.51 route-expansion validation.
- [ ] Level02 validator requires:
  - [ ] `Label - L02 Manual Bleed`
  - [ ] `Label - L02 Mainline Rejoin`
  - [ ] `L02 Pressure Bypass Rejoin Green Gauge`
  - [ ] `L02 Pressure Bypass Secret Bleed Wheel Clue` if the P1 secret polish is included.
- [ ] Level03 validator requires:
  - [ ] `Label - L03 Foundry Floor`
  - [ ] `Label - L03 Upper Gantry`
  - [ ] `Label - L03 Control Walkway`
  - [ ] `Label - L03 Crane Return`
  - [ ] `Label - L03 High Rejoin`
  - [ ] `L03 Gantry Breath Pocket Green Gauge`
  - [ ] `L03 Crucible Shelf Coolant Leak Clue` if the P1 secret polish is included.
- [ ] Level04 validator requires:
  - [ ] `Label - L04 Intake Control`
  - [ ] `Label - L04 Pump Primer`
  - [ ] `Label - L04 Pressure Return`
  - [ ] `Label - L04 Observatory Feed`
  - [ ] `Label - L04 Pumpworks Rejoin`
  - [ ] `L04 Observatory Return Duct Gauge Clue` if the P1 secret polish is included.
- [ ] Validator checks route-order positions, not just existence:
  - [ ] Level02 manual-bleed label appears after entry label and before rejoin marker.
  - [ ] Level03 upper-gantry label appears before the high catwalk/rejoin.
  - [ ] Level04 pump labels appear in intake, primer, pressure-return, observatory-feed, rejoin order.

## Required Runtime Smoke Additions

### RuntimeMidgameFlowTest

- [ ] Extend Level02 checks to require `Label - L02 Manual Bleed`, `Label - L02 Mainline Rejoin`, and `L02 Pressure Bypass Rejoin Green Gauge`.
- [ ] Extend Level02 checks to assert rejoin marker sits north of the manual-bleed label and near the pump-room exit path.
- [ ] Extend Level03 checks to require `Label - L03 Upper Gantry`, `Label - L03 Control Walkway`, `Label - L03 High Rejoin`, and `L03 Gantry Breath Pocket Green Gauge`.
- [ ] Extend Level03 checks to assert the breath pocket appears after the Bellows Node platform and before the rejoin stair/high rejoin.
- [ ] Expected marker remains `V0_MIDGAME_FLOW_PASS`.

### RuntimeClimaxFlowTest

- [ ] Extend Level04 checks to require `Label - L04 Intake Control`, `Label - L04 Pump Primer`, `Label - L04 Pressure Return`, `Label - L04 Observatory Feed`, and `Label - L04 Pumpworks Rejoin`.
- [ ] Assert the pump chain order by position so labels cannot all be stacked at spawn.
- [ ] Assert arena warning/pump-state cue appears before `Enemy - L04 Pumpworks Arena Bulwark`.
- [ ] Expected marker remains `V0_CLIMAX_FLOW_PASS`.

### Existing Smoke Expectations

- [ ] `V0_HAZARD_PASS` still verifies steam/furnace hazards after timing/readability changes.
- [ ] `V0_SECRET_PASS` still verifies optional secrets remain optional.
- [ ] `V0_AUTO_PLAYTHROUGH_PASS` still verifies Level02 valve, Level03 valve, Level04 transition, and run-state persistence.
- [ ] `V0_RANGED_COMBAT_PASS` still verifies Level02 Lancer behavior after bypass cover/angle changes.
- [ ] `V0_BULWARK_COMBAT_PASS` still verifies Level04 Bulwark behavior after pumpworks pacing changes.
- [ ] `V0_WORLD_LABEL_READABILITY_PASS` still passes after adding new route labels.

## Level02 Human Route QA

- [ ] At normal player height, enter the pressure bypass and read `PRESSURE BYPASS` before committing.
- [ ] Reach `Label - L02 Manual Bleed` without blind steam damage.
- [ ] Observe a safe/idle state before the first pressure hazard damages the player.
- [ ] Fight or bypass `Enemy - L02 Pressure Bypass Ranged A` with visible cover after the first hazard tell.
- [ ] Reach `Label - L02 Mainline Rejoin` before fighting the exit guard.
- [ ] Optional secret clue is readable only after pressure-system language has been taught.

## Level03 Human Route QA

- [ ] Identify `Foundry Floor`, `Upper Gantry`, `Control Walkway`, `Crane Return`, and `High Rejoin` before committing to each height change.
- [ ] Narrow catwalks never stack more than one primary threat plus one ambient/flank pressure.
- [ ] Furnace-strip and slag-vent hazards are visible from a safe preview angle before damage exposure.
- [ ] Bellows Node platform has enough lateral space and a clear breath pocket before rejoin.
- [ ] If `Enemy - L03 Gantry Ranged Teacher` is promoted to Lancer, ranged pressure is tested beyond simple route-order checks.

## Level04 Human Route QA

- [ ] Read pump chain in order: `Intake Control`, `Pump Primer`, `Pressure Return`, `Observatory Feed`, `Pumpworks Rejoin`.
- [ ] First pump-state cue is visible before combat escalates.
- [ ] `Enemy - L04 Pumpworks Lancer` does not interrupt the first changed-state read.
- [ ] `Enemy - L04 Pumpworks Arena Bulwark` is seen after arena warning and observatory-feed/rejoin language.
- [ ] Secret return duct clue appears after pump-state language is established.
- [ ] Route completion does not require the secret return duct.

## Automated Exit Gate

- [ ] Run `V0LevelValidator.RunValidation` and confirm `V0_LEVEL_VALIDATION_PASS`.
- [ ] Run route audit and confirm `V0_ROUTE_AUDIT_PASS`.
- [ ] Run the full build matrix and confirm:
  - [ ] `V0_BUILD_MATRIX_PASS`
  - [ ] `V0_RUNTIME_SMOKE_PASS`
  - [ ] `V0_AUTO_PLAYTHROUGH_PASS`
  - [ ] `V0_MIDGAME_FLOW_PASS`
  - [ ] `V0_CLIMAX_FLOW_PASS`
  - [ ] `V0_HAZARD_PASS`
  - [ ] `V0_SECRET_PASS`
  - [ ] `V0_RANGED_COMBAT_PASS`
  - [ ] `V0_BULWARK_COMBAT_PASS`
  - [ ] `V0_WORLD_LABEL_READABILITY_PASS`
- [ ] Package only after automated gates pass and human route-readability screenshots are captured outside build assets.

## Deferred or Risk Notes

- [ ] If Level04 pump gameplay is not promoted to a real interactable objective, record the v0.1.53 work as pumpworks readability/pacing polish, not objective-system completion.
- [ ] If Level03 ranged teacher remains a Scrapper, update any player-facing/test wording that implies a true ranged enemy.
- [ ] If optional combined hazards or new secret counts are deferred, mark them as future route expansion rather than failed v0.1.53 scope.
