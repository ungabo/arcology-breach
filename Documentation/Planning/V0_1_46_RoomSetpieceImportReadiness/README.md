# V0.1.46 Room Setpiece Import Readiness

Generated: 2026-05-24

This docs-only packet prepares the main-lane quarantine import gate for the completed `BrassworksBreach.RoomSetpieceKit04` sidecar package.

Target package:

- Package root: `AssetPacks/BrassworksBreach.RoomSetpieceKit04`
- UPM name: `com.brassworks.sidecar.room-setpiece-kit04`
- Package version: `0.1.45-p001`
- Main-lane reference to add later: `file:../AssetPacks/BrassworksBreach.RoomSetpieceKit04`

## Package Counts

Counts are taken from the package-local manifest and runtime catalog:

| Count | Value |
| --- | ---: |
| Generated prefabs | 30 |
| Generated materials | 18 |
| Reusable meshes | 10 |
| Runtime catalogs | 1 |
| Package-local manifests | 1 |
| Preview renders | 12 |
| Textures | 0 |
| Audio | 0 |
| VFX systems | 0 |
| Animation clips | 0 |
| Runtime scripts | 0 |
| Colliders | 0 |
| Rigidbodies | 0 |
| Audio sources | 0 |

## Intended Main-Lane Import Gate

This packet proposes one additional sidecar package check and 12 representative asset load checks for `SidecarQuarantineImportValidator`. If the v0.1.45 readiness wave is already active, the expected validator target after this import is:

- Packages: 16
- Representative assets: 135

The import should stay quarantine-only until Unity resolves the package, the import validator passes, and every showcase placement is reviewed as visual-only. RoomSetpieceKit04 must not own collision, navigation, occlusion, gameplay scripting, route logic, lighting authority, audio, pickup, hazard, door, lift, bridge, transition, AI, damage, or objective behavior.

## Files

- `ASSET_PATH_INVENTORY_v0.1.46.md`
- `RISK_MATRIX_AND_SHOWCASE_PLACEMENTS_v0.1.46.md`
- `VALIDATOR_ADDITIONS_AND_ROLLBACK_PLAN_v0.1.46.md`
- `../../QA/V0_1_46_RoomSetpieceImportReadiness/MAIN_LANE_VALIDATION_CHECKLIST_v0.1.46.md`
- `../../QA/V0_1_46_RoomSetpieceImportReadiness/ROOM_SETPIECE_IMPORT_READINESS_REVIEW_v0.1.46.json`

## Boundaries

This packet does not edit package roots, `Packages/manifest.json`, `Packages/packages-lock.json`, scenes, runtime/editor code, shared status docs, or git history. Import, scene placement, validator implementation, scene rebuild, and build validation remain main-lane work.
