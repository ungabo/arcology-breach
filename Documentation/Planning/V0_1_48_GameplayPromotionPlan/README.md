# V0.1.48 Gameplay Promotion Planning Packet

Generated: 2026-05-24

This docs-only packet plans how the currently imported sidecar visual packages can later graduate from quarantine review into real gameplay, collision, VFX sockets, lighting, weapon viewmodel systems, and feedback systems.

## Source Evidence

| Evidence | Path |
| --- | --- |
| Imported packages | `Packages/manifest.json` |
| Import authority gate | `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs` |
| Showcase placements | `Assets/_Project/Editor/V0SceneBuilder.cs` |
| v0.1.45 release baseline | `Documentation/Releases/RELEASE_NOTES_v0.1.45.md` |
| v0.1.46 future-lane context | `Documentation/Planning/V0_1_46_RoomSetpieceImportReadiness/` |
| v0.1.46 future-lane context | `Documentation/Planning/V0_1_46_WeaponMechanismsImportReadiness/` |

## Imported Baseline

The active project manifest imports 15 `com.brassworks.sidecar.*` packages. Filesystem counts under those imported package roots show 266 prefabs, 220 materials, 83 meshes, 15 audio clips, and 4 proxy animation clips. The v0.1.45 validator baseline reports `packages=15 assets=123`.

The v0.1.45 release notes explicitly state that imported assets remain quarantined visual-review content and do not add gameplay authority, colliders, rigidbodies, autonomous audio, AI, pickups, damage, or interactable state. `V0SceneBuilder` also instantiates showcase items under `Sidecar Quarantine Showcase - LevelXX` and strips presentation physics from sidecar instances.

## Files

- `PROMOTION_MATRIX_v0.1.48.md`
- `RISK_AUTHORITY_MATRIX_v0.1.48.md`
- `MILESTONE_BATCHES_AFTER_v0.1.47.md`
- `../../QA/V0_1_48_GameplayPromotionPlan/QA_CHECKLIST_v0.1.48.md`
- `../../QA/V0_1_48_GameplayPromotionPlan/GAMEPLAY_PROMOTION_REVIEW_v0.1.48.json`

## Non-Goals

This packet does not import packages, edit `Packages/manifest.json`, edit validators, edit scene builder code, rebuild scenes, touch package roots, alter shared status docs, or make git history changes. It is a planning bridge for future owned slices.
