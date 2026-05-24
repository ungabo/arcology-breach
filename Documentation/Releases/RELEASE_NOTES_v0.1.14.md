# Brassworks Breach - Release Notes v0.1.14

Date: 2026-05-24

## Summary

`v0.1.14` is a narrow asset-promotion slice. It promotes a single pressure-coil component into the active Pressure Pistol viewmodel with metadata and validation, continuing the component-first path instead of trying to replace the full weapon at once.

## Added

- `PressureCoilPrototype` runtime metadata component.
- `Pressure Pistol Prototype Copper Coil Pack` in the generated Pressure Pistol viewmodel.
- Coil-pack construction with blackened iron backing, aged brass rails, copper manifolds, red heat core, visible coil turns, rivets, pressure leads, and patina marks.
- `V0LevelValidator` coverage for the coil prototype's metadata, named hierarchy, detail counts, and material roles.
- Pressure-coil production notes under `Documentation/AssetProduction/PressureCoilPrototype/`.

## Verified Outputs

- Windows executable: `Builds/Windows/v0.1.14/BrassworksBreach_v0.1.14.exe`.
- Windows package: `Builds/WindowsPackages/v0.1.14/BrassworksBreach_v0.1.14_Windows.zip`.
- Package SHA-256: `5C5CE3E737CB660F8A239FCE72EE038E422BB3083ECA7302F00533CFA70AD834`.
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.14.md`.
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.14.md`.

## Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_COMBAT_EDGE_PASS`
- `V0_COMBAT_SCENARIO_PASS`
- `V0_WEAPON_SWITCH_PASS`
- `V0_BELLOWS_NODE_PASS`
- `V0_RANGED_COMBAT_PASS`
- `V0_BULWARK_COMBAT_PASS`
- `V0_WARDEN_COMBAT_PASS`
- `V0_INTERACTION_SMOKE_PASS`
- `V0_HAZARD_PASS`
- `V0_SECRET_PASS`
- `V0_PAUSE_FLOW_PASS`
- `V0_MOVEMENT_FEEL_PASS`
- `V0_BALANCE_ENVELOPE_PASS`
- `V0_LEVEL01_FLOW_PASS`
- `V0_MIDGAME_FLOW_PASS`
- `V0_CLIMAX_FLOW_PASS`
- `V0_AUDIO_MIX_PASS`
- `V0_DISPLAY_SETTINGS_PASS`
- `V0_READABILITY_SETTINGS_PASS`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_BUILD_MATRIX_PASS`

Next-step directive: continue immediately with the next highest-impact unfinished task.
