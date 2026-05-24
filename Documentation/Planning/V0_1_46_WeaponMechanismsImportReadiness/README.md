# V0.1.46 Weapon Mechanisms Import Readiness

Generated: 2026-05-24

This docs-only packet prepares the main-lane quarantine import gate for `BrassworksBreach.WeaponMechanismsSet04`, the completed sidecar package for modular steampunk first-person weapon mechanism visuals.

The package is intended for visual review, scale checks, material readability, and per-level quarantine showcases only. It does not grant gameplay authority. Do not promote any prefab from this packet into firing logic, damage, hitboxes, inventory, weapon selection, muzzle authority, recoil, reload timing, projectile spawning, or viewmodel animation without a later gameplay and viewmodel integration pass.

## Source Evidence

| Evidence | Path |
| --- | --- |
| Package root | `AssetPacks/BrassworksBreach.WeaponMechanismsSet04` |
| Package manifest | `AssetPacks/BrassworksBreach.WeaponMechanismsSet04/Documentation~/Manifest/WMS04_WeaponMechanismsSet04_Manifest_v0.1.45-p001.json` |
| Runtime catalog | `AssetPacks/BrassworksBreach.WeaponMechanismsSet04/Runtime/Metadata/WMS04_RuntimeCatalog_v0.1.45.json` |
| Production report | `Documentation/AssetProduction/V0_1_45_WeaponMechanismsSet04/WMS04_ProductionReport_v0.1.45.md` |
| Acceptance report | `Documentation/AssetProduction/V0_1_45_WeaponMechanismsSet04/WMS04_AcceptanceReport_v0.1.45.md` |
| Concept renders | `Documentation/ConceptRenders/V0_1_45_WeaponMechanismsSet04` |

## Package Counts

| Package | UPM name | Version | Prefabs | Materials | Meshes | Textures | Audio | VFX | Animation clips | Preview renders |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| WeaponMechanismsSet04 | `com.brassworks.sidecar.weapon-mechanisms-set04` | `0.1.45-p001` | 29 | 20 | 11 | 0 | 0 | 0 | 0 | 11 |

Production evidence also reports 357 renderer components, no gameplay scripts, no colliders, no rigidbodies, and no autonomous audio.

## Preview Evidence

Preview renders are available for:

- Pressure pistol coils.
- Gauge dial clusters.
- Wood and leather grips.
- Receiver plates.
- Muzzle crowns, tanks, and valves.
- Ammo cylinders, scattergun chambers, and bolt rails.
- Gloved hand silhouette pieces.
- Material swatch prefabs.
- Two all-component contact sheets.
- Full material swatch board.

The Unity acceptance result is `WMS04_UNITY_VALIDATION_PASS v0.1.45 prefabs=29 materials=20 meshes=11 renderers=357 previews=11`. Repository sidecar validation reports `status=pass package_count=1 errors=0 warnings=0 findings=[]`.

## Intended Main-Lane Import Gate

The current main manifest contains the v0.1.45 sidecar wave but does not reference `com.brassworks.sidecar.weapon-mechanisms-set04`. This packet proposes one additional package check and 15 representative asset checks. If the existing validator baseline is `packages=15 assets=123`, the post-v0.1.46 target should be `packages=16 assets=138`.

Import gate sequence:

1. Confirm no parallel worker is editing main manifests, package lock, validators, scene builder, level validator, generated scenes, package roots, or shared status docs.
2. Add only the local package reference for `com.brassworks.sidecar.weapon-mechanisms-set04`.
3. Let Unity resolve packages and compile before scene or validator placement work.
4. Add the `SidecarQuarantineImportValidator` entry from this packet.
5. Run `Project Tools/Validate Sidecar Quarantine Imports` and expect `packages=16 assets=138` if the 15 representative paths are used.
6. Add quarantine showcase placements only after the import validator passes.
7. Rebuild scenes through Unity and run `Project Tools/Validate v0 Levels`.
8. Keep rollback as reference removal plus validator/showcase cleanup.

## Files

- `ASSET_PATH_INVENTORY_v0.1.46.md`
- `RISK_MATRIX_AND_SHOWCASE_PLACEMENTS_v0.1.46.md`
- `VALIDATOR_ADDITIONS_AND_ROLLBACK_PLAN_v0.1.46.md`
- `../../QA/V0_1_46_WeaponMechanismsImportReadiness/MAIN_LANE_VALIDATION_CHECKLIST_v0.1.46.md`
- `../../QA/V0_1_46_WeaponMechanismsImportReadiness/SIDECAR_IMPORT_READINESS_REVIEW_v0.1.46.json`

## Boundaries

This packet does not edit package roots, `Packages/manifest.json`, `Packages/packages-lock.json`, Unity scenes, code, shared status docs, or git history. It is documentation-only and leaves import execution to the main integration lane.
